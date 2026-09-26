using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.Extensions;
using OmegaExplorer.Server.Services._Core.Models.Classes;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Cycles;
using OmegaExplorer.Server.Services._Game.Orders.Models.Entities;
using OmegaExplorer.Server.Services._Game.Origins.Models.Enums;
using OmegaExplorer.Server.Services._Game.Spaceships.Models.Entities;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Entities;
using OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Classes;
using OmegaExplorer.Server.Services._Game.StarSystems;
using OmegaExplorer.Server.Services.Databases;
using OmegaExplorer.Shared.Filters;

namespace OmegaExplorer.Server.Services._Game.Spaceships;

public class SpaceshipRepository : RepositoryCustom
{
    private readonly CycleBackgroundService _cycleBackgroundService;
    private readonly DatabaseContext _databaseContext;

    private readonly StarSystemService _starSystemService;

    public SpaceshipRepository(StarSystemService starSystemService, CycleBackgroundService cycleBackgroundService,
        DatabaseContext databaseContext)
    {
        _starSystemService = starSystemService;
        _cycleBackgroundService = cycleBackgroundService;
        _databaseContext = databaseContext;
    }

    #region CREATE

    /// <summary>
    ///     Create a spaceship
    /// </summary>
    /// <param name="spaceship"></param>
    /// <exception cref="Exception"></exception>
    public async Task<Spaceship> Create(
        Spaceship spaceship)
    {
        try
        {
            if (spaceship.Modules.Count == 0) throw new Exception("A spaceship must have at least one module");

            var res = _databaseContext.Spaceships.Add(spaceship);

            await _databaseContext.SaveChangesAsync();

            return res.Entity;
        }
        catch (Exception e)
        {
            Log.Error(e, "Error while creating spaceship");
            throw;
        }
    }

    #endregion

    /// <summary>
    ///     Delete a spaceship
    /// </summary>
    /// <param name="spaceshipId"></param>
    /// <param name="userId"></param>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task DeleteAsOwner(Guid spaceshipId, Guid userId)
    {
        var spaceship =
            await _databaseContext.Spaceships.FirstOrDefaultAsync(s => s.Id == spaceshipId && s.OwnerId == userId);

        if (spaceship == null) throw new KeyNotFoundException();

        _databaseContext.Spaceships.Remove(spaceship);

        await _databaseContext.SaveChangesAsync();
    }

    public async Task DeleteAsServer(Guid spaceshipId)
    {
        var spaceship = await _databaseContext.Spaceships.FirstOrDefaultAsync(s => s.Id == spaceshipId);

        if (spaceship == null) throw new KeyNotFoundException();

        _databaseContext.Spaceships.Remove(spaceship);

        await _databaseContext.SaveChangesAsync();
    }

    /// <summary>
    ///     Get all orders for a spaceship
    /// </summary>
    /// <param name="spaceshipId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<List<Order>> GetOrders(Guid spaceshipId, Guid userId)
    {
        var res = await _databaseContext.Orders
            .Where(order => order.OrderableId == spaceshipId)
            .ToListAsync();

        return res;
    }

    /// <summary>
    ///     Update the position of a spaceship
    /// </summary>
    /// <param name="spaceshipId"></param>
    /// <param name="newPosition"></param>
    public async Task UpdatePosition(Guid spaceshipId, Vector2 newPosition)
    {
        var spaceship = await _databaseContext.Spaceships
            .Include(spaceship => spaceship.SpatialLocation)
            .FirstOrDefaultAsync(spaceship => spaceship.Id == spaceshipId);

        if (spaceship != null)
        {
            spaceship.SpatialLocation.Position.X = newPosition.X;
            spaceship.SpatialLocation.Position.Y = newPosition.Y;

            await _databaseContext.SaveChangesAsync();
        }
    }

    /// <summary>
    ///     Count the number of pirates in a galaxy
    /// </summary>
    /// <param name="galaxyId"></param>
    /// <returns></returns>
    public async Task<int> CountPirateInGalaxy(Guid galaxyId)
    {
        var res = await _databaseContext.Spaceships
            .Where(spaceship =>
                spaceship.SpatialLocation.GalaxyId == galaxyId && spaceship.Faction == EnumFaction.Pirate)
            .CountAsync();

        return res;
    }

    /// <summary>
    ///     Count the number of pirates in a star cluster
    /// </summary>
    /// <param name="starClusterId"></param>
    /// <returns></returns>
    public async Task<int> CountPirateInStarCluster(Guid starClusterId)
    {
        var res = await _databaseContext.Spaceships
            .Where(spaceship => spaceship.SpatialLocation.StarClusterId == starClusterId &&
                                spaceship.Faction == EnumFaction.Pirate)
            .CountAsync();

        return res;
    }

    /// <summary>
    ///     Delete all pirates from the game
    /// </summary>
    public async Task DeletePirates()
    {
        var pirates = await _databaseContext.Spaceships
            .Where(spaceship => spaceship.Faction == EnumFaction.Pirate)
            .ToListAsync();

        _databaseContext.Spaceships.RemoveRange(pirates);

        await _databaseContext.SaveChangesAsync();
    }

    /// <summary>
    ///     Check if the user is the owner of the spaceship
    /// </summary>
    /// <param name="spaceshipId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<bool> IsOwner(Guid spaceshipId, Guid userId)
    {
        var res = await _databaseContext.Spaceships
            .AnyAsync(spaceship => spaceship.Id == spaceshipId && spaceship.OwnerId == userId);

        return res;
    }

    public async Task<List<Spaceship>> GetByIds(List<Guid> spaceshipIds)
    {
        var res = await _databaseContext.Spaceships
            .Where(spaceship => spaceshipIds.Contains(spaceship.Id))
            .ToListAsync();

        return res;
    }

    public async Task SubtractHealthFromModule(Spaceship target,
        SpaceshipModule moduleToTarget, int amount)
    {
        var module = await _databaseContext.SpaceshipModules
            .FirstOrDefaultAsync(m => m.Id == moduleToTarget.Id);

        if (module != null)
        {
            module.Health -= amount;
            if (module.Health <= 0) module.Health = 0;

            await _databaseContext.SaveChangesAsync();
        }
    }

    public async Task<SpaceshipModule> SubtractHealthFromRandomModule(Guid spaceshipId, int amount)
    {
        // Re-fetch the spaceship from the database, including modules
        var spaceship = await _databaseContext.Spaceships
            .Include(s => s.Modules)
            .FirstOrDefaultAsync(s => s.Id == spaceshipId);

        if (spaceship == null)
            throw new Exception("Spaceship not found in the database");

        var module = spaceship.Modules
            .Where(m => m.Health > 0)
            .ToList()
            .GetRandom();

        if (module == null) throw new Exception("No module found");

        module.Health -= amount;
        if (module.Health < 0) module.Health = 0;

        await _databaseContext.SaveChangesAsync();

        return module;
    }

    public async Task SetDestroyed(Guid spaceshipId)
    {
        var spaceship = await _databaseContext.Spaceships
            .FirstOrDefaultAsync(s => s.Id == spaceshipId);

        if (spaceship == null) throw new KeyNotFoundException();

        spaceship.DestroyedQuantityDamageToRepair = 100;

        //Set spaceship destruction 100 cycle after
        spaceship.CycleWhenRemove = CycleBackgroundService.CurrentCycle + 100;

        await _databaseContext.SaveChangesAsync();
    }

    public async Task TeleportTo(Guid spaceshipId, SpatialLocation target)
    {
        var spaceship = await _databaseContext.Spaceships
            .FirstOrDefaultAsync(s => s.Id == spaceshipId);

        if (spaceship == null) throw new KeyNotFoundException();

        spaceship.SpatialLocation = target;

        await _databaseContext.SaveChangesAsync();
    }

    public async Task ExitBattle(Guid spaceshipId)
    {
        var spaceship = await _databaseContext.Spaceships
            .FirstOrDefaultAsync(s => s.Id == spaceshipId);

        if (spaceship == null) throw new KeyNotFoundException();

        spaceship.BattleId = null;

        await _databaseContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<SpaceshipModule>> SubtractHealthFromAllModules(Guid spaceshipId, int amount)
    {
        var spaceship = await _databaseContext.Spaceships
            .Include(s => s.Modules)
            .FirstOrDefaultAsync(s => s.Id == spaceshipId);

        if (spaceship == null) throw new KeyNotFoundException();

        foreach (var module in spaceship.Modules)
        {
            module.Health -= amount;
            if (module.Health < 0) module.Health = 0;
        }

        await _databaseContext.SaveChangesAsync();

        return spaceship.Modules;
    }

    /// <summary>
    ///     Return the list of all spaceships in a friendly star system
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<List<Spaceship>> GetSpaceshipsInFriendlyStarSystem()
    {
        var spaceships = await _databaseContext.Spaceships
            .Include(spaceshp => spaceshp.SpatialLocation)
            .ToListAsync();

        foreach (var spaceship in spaceships.ToArray())
        {
            //Check if spaceship is on friendly planet or station else remove it
            var starSystem = await _starSystemService.GetStarSystemByLocation(spaceship.SpatialLocation);

            if (starSystem == null) spaceships.Remove(spaceship);

            if (starSystem?.OwnerId != spaceship.OwnerId) spaceships.Remove(spaceship);
        }

        return spaceships;
    }

    public async Task RepairSpaceship(Guid spaceshipId)
    {
        var spaceship = await _databaseContext.Spaceships
            .Include(spaceship => spaceship.Modules)
            .FirstOrDefaultAsync(s => s.Id == spaceshipId);

        if (spaceship == null) throw new KeyNotFoundException();

        spaceship.DestroyedQuantityDamageToRepair = 0;

        foreach (var module in spaceship.Modules) module.Health = 100;

        await _databaseContext.SaveChangesAsync();
    }

    /// <summary>
    ///     Get all spaceship destroyed in the game (all modules health = 0)
    ///     and where RepairQuantityDamageToRepair = 0
    /// </summary>
    /// <returns></returns>
    public async Task<List<Spaceship>> GetSpaceshipsDestroyed()
    {
        var res = await _databaseContext.Spaceships
            .Include(spaceship => spaceship.Modules)
            .Where(spaceship => spaceship.Modules.Sum(module => module.Health) == 0 &&
                                spaceship.DestroyedQuantityDamageToRepair == 0)
            .ToListAsync();

        return res;
    }

    public async Task Delete(Guid spaceshipId)
    {
        var spaceship = await _databaseContext.Spaceships
            .FirstOrDefaultAsync(s => s.Id == spaceshipId);

        if (spaceship == null) throw new KeyNotFoundException();

        _databaseContext.Spaceships.Remove(spaceship);

        await _databaseContext.SaveChangesAsync();
    }

    public async Task RenameSpaceship(Guid spaceshipId, Guid userId, string name)
    {
        var spaceship = await _databaseContext.Spaceships
            .FirstOrDefaultAsync(s => s.Id == spaceshipId && s.OwnerId == userId);

        if (spaceship == null) throw new KeyNotFoundException();

        spaceship.Name = name;

        await _databaseContext.SaveChangesAsync();
    }

    #region GET

    public async Task<List<Spaceship>> GetAllFiltered(Guid userId,
        DynamicDriverSpaceshipFilter? filter)
    {
        try
        {
            var query = _databaseContext.Spaceships
                .Include(spaceship => spaceship.Modules)
                .Include(spaceship => spaceship.SpatialLocation.Galaxy)
                .Include(spaceship => spaceship.SpatialLocation.StarCluster)
                .Include(spaceship => spaceship.Owner)
                .Include(spaceship => spaceship.SpatialLocation.Universe)
                .Include(spaceship => spaceship.Battle)
                .AsQueryable();

            if (filter != null)
            {
                //filter by owner
                var searchByOwner = filter.GetFilterBySpaceshipOwner();
                if (searchByOwner != null)
                    switch (searchByOwner)
                    {
                        case DynamicDriverSpaceshipFilter.SpaceshipOwner.All:
                            break;
                        case DynamicDriverSpaceshipFilter.SpaceshipOwner.ConnectedPlayer:
                            query = query.Where(spaceship => spaceship.OwnerId == userId);
                            break;
                        case DynamicDriverSpaceshipFilter.SpaceshipOwner.Ally:
                            break;
                        case DynamicDriverSpaceshipFilter.SpaceshipOwner.Enemy:
                            query = query.Where(spaceship => spaceship.OwnerId != userId);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                //filter by type
                var searchByType = filter.GetFilterBySpaceshipType();
                if (searchByType != null)
                    switch (searchByType)
                    {
                        case DynamicDriverSpaceshipFilter.SpaceshipType.Fighter:
                            break;
                        case DynamicDriverSpaceshipFilter.SpaceshipType.Colonizer:
                            //Search if spaceship has a colonizer module
                            query = query.Where(spaceship =>
                                spaceship.Modules.Any(module => module.Index >= 4001 && module.Index <= 5000));
                            break;
                        case DynamicDriverSpaceshipFilter.SpaceshipType.Transporter:
                            break;
                        case null:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                var searchByStarClusterId = filter.GetFilterByStarClusterId();
                if (!searchByStarClusterId.IsNullOrEmpty())
                    query = query.Where(spaceship => spaceship.SpatialLocation.StarClusterId == searchByStarClusterId);
            }


            var data = await query.ToListAsync();

            return data;
        }
        catch (Exception e)
        {
            Log.Error(e, "Error while getting all spaceships");
            throw;
        }
    }

    public async Task<List<Spaceship>> GetAll(Guid userId)
    {
        try
        {
            var res = await _databaseContext.Spaceships
                .Where(spaceship => spaceship.OwnerId == userId)
                .Include(spaceship => spaceship.SpatialLocation.Galaxy)
                .Include(spaceship => spaceship.SpatialLocation.StarCluster)
                .ToListAsync();

            return res;
        }
        catch (Exception e)
        {
            Log.Error(e, "Error while getting all spaceships");
            throw;
        }
    }

    public async Task<Spaceship?> GetById(Guid spaceshipId)
    {
        try
        {
            var res = await _databaseContext.Spaceships
                .Include(spaceship => spaceship.SpatialLocation.Universe)
                .Include(spaceship => spaceship.SpatialLocation.Galaxy)
                .Include(spaceship => spaceship.SpatialLocation.StarCluster)
                .Include(x => x.Modules)
                .FirstOrDefaultAsync(x => x.Id == spaceshipId);

            return res;
        }
        catch (Exception e)
        {
            Log.Error(e, "Error while getting spaceship by id {spaceshipId}", spaceshipId);
            throw;
        }
    }

    public async Task<List<Spaceship>> GetAllByPosition(Guid userId, int xStart,
        int xEnd, int yStart,
        int yEnd)
    {
        var res = await _databaseContext.Spaceships
            .Include(spaceship => spaceship.Owner)
            .Where(spaceship => spaceship.SpatialLocation.Position.X >= xStart &&
                                spaceship.SpatialLocation.Position.X <= xEnd &&
                                spaceship.SpatialLocation.Position.Y >= yStart &&
                                spaceship.SpatialLocation.Position.Y <= yEnd)
            .ToListAsync();
        return res;
    }

    /// <summary>
    ///     Get all spaceships in a star cluster
    /// </summary>
    /// <param name="starClusterId"></param>
    /// <returns></returns>
    public async Task<List<Spaceship>> GetAllInStarCluster(Guid starClusterId)
    {
        var res = await _databaseContext.Spaceships
            .Where(spaceship => spaceship.SpatialLocation.StarClusterId == starClusterId)
            .ToListAsync();

        return res;
    }

    #endregion

    #region ORDER

    public async Task OrderMove(Guid userId, Guid spaceshipId, SpatialLocation locationTarget)
    {
        var spaceship = await _databaseContext.Spaceships.FindAsync(spaceshipId);

        if (spaceship == null) throw new KeyNotFoundException();

        var moveOrder = new Order(userId, spaceship.Id)
            .SetMoveLocation(locationTarget);

        _databaseContext.Orders.Add(moveOrder);

        await _databaseContext.SaveChangesAsync();
    }

    public async Task OrderAttack(Guid userId, Guid spaceshipId, Guid spaceshipTargetId)
    {
        var spaceship = await _databaseContext.Spaceships.FindAsync(spaceshipId);

        if (spaceship == null) throw new Exception($"Spaceship not found at id {spaceshipId}");

        var attackOrder = new Order(userId, spaceship.Id)
            .SetAttackSpaceship(spaceshipTargetId);

        _databaseContext.Orders.Add(attackOrder);

        await _databaseContext.SaveChangesAsync();
    }

    public async Task OrderColonize(Guid spaceshipId, Guid userId, Guid starSystemId)
    {
        var spaceship = await _databaseContext.Spaceships.FindAsync(spaceshipId);

        if (spaceship == null) throw new KeyNotFoundException();

        var starSystem = await _starSystemService.GetStarSystemById(userId, starSystemId);

        if (starSystem is null)
            throw new Exception($"Star system {starSystemId} not found for user {userId} to colonize");

        var colonizeOrder = new Order(userId, spaceship.Id)
            .SetColonizeTarget(starSystem);

        _databaseContext.Orders.Add(colonizeOrder);

        await _databaseContext.SaveChangesAsync();
    }

    #endregion
}