using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.Services._Game.Battles;
using OmegaExplorer.Server.Services._Game.Orders.Models.Entities;
using OmegaExplorer.Server.Services._Game.Spaceships;
using OmegaExplorer.Server.Services._Game.Spaceships.Models.Entities;
using OmegaExplorer.Server.Services._Game.SpatialObjects;
using OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Classes;
using OmegaExplorer.Server.Services._Game.StarSystems;
using OmegaExplorer.Server.Services.Databases;
using OmegaExplorer.Server.Services.Loggers.Models.Enums;
using OmegaExplorer.Server.Services.Users;

namespace OmegaExplorer.Server.Services._Game.Orders;

/// <summary>
///     Manage orders for spaceship
/// </summary>
public class OrderExecuteService
{
    private readonly BattleService _battleService;
    private readonly DatabaseContext _databaseContext;

    private readonly IServiceProvider _serviceProvider;
    private readonly SpaceshipRepository _spaceshipRepository;
    private readonly SpaceshipService _spaceshipService;
    private readonly SpatialTravelService _spatialTravelService;
    private readonly StarSystemService _starSystemService;

    public OrderExecuteService(IServiceProvider serviceProvider, BattleService battleService,
        SpaceshipService spaceshipService, StarSystemService starSystemService, SpaceshipRepository spaceshipRepository,
        SpatialTravelService spatialTravelService, DatabaseContext databaseContext)
    {
        _serviceProvider = serviceProvider;
        _battleService = battleService;
        _spaceshipService = spaceshipService;
        _starSystemService = starSystemService;
        _spaceshipRepository = spaceshipRepository;
        _spatialTravelService = spatialTravelService;
        _databaseContext = databaseContext;
    }


    public async Task Execute()
    {
        var spaceships = await _databaseContext.Spaceships
            .Where(spaceship => spaceship.Orders.Any())
            .Include(s => s.Orders)
            .ToListAsync();

        foreach (var spaceship in spaceships)
        {
            var order = spaceship.Orders.FirstOrDefault();

            // if no order skip
            if (order == null) continue;

            try
            {
                var orderCompleted = false;

                switch (order.Type)
                {
                    case Order.OrderType.Attack:
                        orderCompleted = await ComputeAttack(spaceship, order);
                        break;
                    case Order.OrderType.Defend:
                        break;
                    case Order.OrderType.Move:
                        orderCompleted = await ComputeMove(spaceship, order);
                        break;
                    case Order.OrderType.Wait:
                        break;
                    case Order.OrderType.Colonize:
                        orderCompleted = await ComputeColonize(spaceship, order);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"{nameof(Order.Type)}",
                            "Order type not found.");
                }

                if (orderCompleted) spaceship.Orders.Remove(order);
            }
            catch (Exception e)
            {
                Log.Logger.Error($"Not able to execute order : {e}", EnumLogSeverity.Error);

                //Remove order if not able to execute
                spaceship.Orders.Remove(order);
            }
        }

        await _databaseContext.SaveChangesAsync();
    }

    private async Task<bool> ComputeColonize(Spaceship spaceship, Order order)
    {
        if (order.TargetLocation is null) throw new Exception("Target location is null");

        //Check if the target is in range
        var target = order.TargetLocation;
        var locationActor = spaceship.SpatialLocation;

        SpatialDistance spatialDistance = new(locationActor, target);
        var totalDistance = spatialDistance.ComputeTotalDistance();

        if (totalDistance > 1)
        {
            // Travel to the target
            await ComputeMove(spaceship, order);
        }
        else
        {
            //Spaceship owner is not owner anymore ???
            if (spaceship.OwnerId == null) return true;

            using var scope = _serviceProvider.CreateScope();
            var userService = scope.ServiceProvider.GetRequiredService<UserService>();

            var user = await userService.GetUserById((Guid)spaceship.OwnerId);

            if (user == null)
                throw new Exception(
                    $"User not found for spaceship {spaceship.Id} | ownerId {spaceship.OwnerId}, owner id is not null but user not found");

            if (!Guid.TryParse(order.TargetRaw, out var starSystemId))
                throw new Exception($"Target raw is not a valid guid {order.TargetRaw}");

            //Delete the spaceship
            await _spaceshipService.DeleteSpaceship(spaceship.Id);

            //Colonize the star system
            await _starSystemService.ColonizeStarSystem(user, starSystemId);

            return true;
        }

        return false;
    }

    /// <summary>
    ///     Compute the move of the spaceship
    /// </summary>
    /// <param name="spaceship"></param>
    /// <param name="order"></param>
    /// <returns>Return completion of the movement order</returns>
    private async Task<bool> ComputeMove(Spaceship spaceship, Order order)
    {
        if (order.TargetLocation is null) throw new Exception("Target location is null");

        //TODO : Implement the move of the spaceship
        var target = order.TargetLocation;

        SpatialTravel travel = new(spaceship, target);
        await _spatialTravelService.Compute(travel);

        var isComplete = _spatialTravelService.IsCompleted(travel);

        if (!isComplete) await _spatialTravelService.MoveToNextPosition(travel);

        isComplete = _spatialTravelService.IsCompleted(travel);

        return isComplete;
    }

    private async Task<bool> ComputeAttack(Spaceship spaceship, Order order)
    {
        //Check if the target is in range
        var spaceshipTarget = order.TargetSpaceship;

        //If not more target set the order as completed
        if (order.TargetSpaceshipId == null) return true;

        //Retrieve the target
        if (spaceshipTarget == null)
            spaceshipTarget = await _spaceshipRepository.GetById(order.TargetSpaceshipId.Value);

        if (spaceshipTarget == null)
            throw new Exception(
                $"Target not found for order {order.Id} | spaceship {spaceship.Id} exist but not able to retrieve the spaceship linked");

        var locationActor = spaceship.SpatialLocation;
        var locationTarget = spaceshipTarget.SpatialLocation;

        var distance = locationActor.Distance(locationTarget);

        //Target in not in range
        if (distance.ComputeTotalDistance() > 1)
        {
            //Travel to the target
            SpatialTravel travel = new(spaceship, locationTarget);

            await _spatialTravelService.Compute(travel);
            await _spatialTravelService.MoveToNextPosition(travel);

            return false;
        }

        //Target is in range

        //Start battle
        List<Guid> spaceshipIds =
        [
            spaceship.Id,
            spaceshipTarget.Id
        ];

        await _battleService.CreateBattle(locationTarget, spaceshipIds);
        return true;
    }
}