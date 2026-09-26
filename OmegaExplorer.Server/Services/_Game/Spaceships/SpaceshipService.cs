using OmegaExplorer.Server.Extensions;
using OmegaExplorer.Server.Services._Core.Extensions;
using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Battles;
using OmegaExplorer.Server.Services._Game.Limits;
using OmegaExplorer.Server.Services._Game.Limits.Models.Exceptions;
using OmegaExplorer.Server.Services._Game.Origins.Models.Enums;
using OmegaExplorer.Server.Services._Game.Spaceships.Models.Entities;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint.Models.Entities;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Entities;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipSkill.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Classes;
using OmegaExplorer.Server.Services._Game.Species;
using OmegaExplorer.Server.Services._Game.StarSystems;
using OmegaExplorer.Server.Services._Game.StarSystems.Models.Entities;
using OmegaExplorer.Server.Services.Loggers.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Spaceships;

public class SpaceshipService
{
    private readonly BattleService _battleService;
    private readonly LimitService _limitService;

    private readonly SpaceshipModuleDataProvider _spaceshipModuleDataProvider;
    private readonly SpaceshipModuleService _spaceshipModuleService;

    private readonly SpaceshipRepository _spaceshipRepository;
    private readonly SpeciesService _speciesService;
    private readonly StarSystemService _starSystemService;

    public SpaceshipService(SpaceshipRepository spaceshipRepository, BattleService battleService,
        SpaceshipModuleService spaceshipModuleService, SpeciesService speciesService,
        StarSystemService starSystemService, LimitService limitService,
        SpaceshipModuleDataProvider spaceshipModuleDataProvider)
    {
        _spaceshipRepository = spaceshipRepository;
        _battleService = battleService;
        _spaceshipModuleService = spaceshipModuleService;
        _speciesService = speciesService;
        _starSystemService = starSystemService;
        _limitService = limitService;
        _spaceshipModuleDataProvider = spaceshipModuleDataProvider;
    }


    /// <summary>
    ///     Set spaceship as destroyed
    ///     Take care if the spaceship is a player spaceship, it will be teleported to the nearest station
    ///     else it will be deleted
    /// </summary>
    /// <param name="spaceshipId"></param>
    public async Task DestroySpaceship(Guid spaceshipId)
    {
        var spaceship = await GetById(spaceshipId, false);

        if (spaceship == null) throw new Exception("Spaceship not found");

        //Exit battle
        await _battleService.ExitBattle(spaceshipId);

        //If spaceship is a player spaceship, teleport it to the nearest station
        if (spaceship.OwnerType == EnumOwnerType.Player)
        {
            if (spaceship.OwnerId == null)
                throw new Exception($"Player spaceship at id {spaceship.Id} without owner ???");

            //Set spaceship as destroyed
            await _spaceshipRepository.SetDestroyed(spaceshipId);

            //Teleport to nearest station
            var nearestStation =
                await _starSystemService.GetStarSystemByUserAndNearestToPosition((Guid)spaceship.OwnerId,
                    spaceship.SpatialLocation);

            if (nearestStation == null)
            {
                Log.Logger.Warning($"No station found for player spaceship {spaceship.Id}");
                return;
            }

            await _spaceshipRepository.TeleportTo(spaceshipId, nearestStation.SpatialLocation);
        }
        //if not a player spaceship, just delete it
        else
        {
            await _spaceshipRepository.SetDestroyed(spaceshipId);
        }
    }

    public async Task<bool> IsOwner(Guid spaceshipId, Guid userId)
    {
        var isOwner = await _spaceshipRepository.IsOwner(spaceshipId, userId);

        return isOwner;
    }

    public async Task SubtractHealthFromModule(Spaceship target,
        SpaceshipModule moduleToTarget, int amount)
    {
        await _spaceshipRepository.SubtractHealthFromModule(target, moduleToTarget, amount);
    }

    public async Task<SpaceshipModule> SubtractHealthFromRandomModule(Guid spaceshipId, int amount)
    {
        var res = await _spaceshipRepository.SubtractHealthFromRandomModule(spaceshipId, amount);
        return res;
    }

    public async Task OrderMoveTo(Guid userId, Guid spaceshipId, SpatialLocation location)
    {
        await _spaceshipRepository.OrderMove(userId, spaceshipId, location);
    }

    public async Task OrderAttack(Guid userId, Guid spaceshipId, Guid targetId)
    {
        await _spaceshipRepository.OrderAttack(userId, spaceshipId, targetId);
    }

    public async Task<IEnumerable<SpaceshipModule>> SubtractHealthFromAllModules(Guid spaceshipId, int amount)
    {
        var res = await _spaceshipRepository.SubtractHealthFromAllModules(spaceshipId, amount);
        return res;
    }

    public async Task<List<Spaceship>> GetSpaceshipsInFriendlyStarSystem()
    {
        var res = await _spaceshipRepository.GetSpaceshipsInFriendlyStarSystem();
        return res;
    }

    public async Task RepairSpaceship(Guid spaceshipId)
    {
        await _spaceshipRepository.RepairSpaceship(spaceshipId);
    }

    public async Task RemoveSpaceship(Guid spaceshipId)
    {
        await _spaceshipRepository.Delete(spaceshipId);
    }

    public async Task RenameSpaceship(Guid spaceshipId, Guid userId, string name)
    {
        await _spaceshipRepository.RenameSpaceship(spaceshipId, userId, name);
    }

    public async Task OrderColonize(Guid spaceshipId, Guid userId, Guid targetId)
    {
        //Check if user can colonize with current LIMIT
        var limit = await _limitService.GetLimitStarSystemByUser(userId);

        if (limit.IsLimitReached()) throw new ExceptionLimitReached(limit);

        await _spaceshipRepository.OrderColonize(spaceshipId, userId, targetId);
    }

    public async Task<List<IDynamicItemSpaceshipSkill>> GetSkills(Spaceship spaceship)
    {
        List<IDynamicItemSpaceshipSkill> res = new();

        foreach (var module in spaceship.Modules)
        {
            var skills = await _spaceshipModuleService.GetSkill(module);

            res.AddRange(skills);
        }

        return res;
    }

    #region GET

    public async Task<Spaceship?> GetById(Guid id, bool efcTracking = true)
    {
        var spaceship = await _spaceshipRepository.GetById(id);

        return spaceship;
    }

    public async Task<List<Spaceship>> GetByIds(List<Guid> spaceshipIds)
    {
        var spaceships = await _spaceshipRepository.GetByIds(spaceshipIds);

        return spaceships;
    }

    #endregion

    #region CREATE

    public async Task<Spaceship> CreateForPlayer(BlueprintSpaceship blueprint,
        StarSystem starSystem, Guid userId)
    {
        try
        {
            if (starSystem.SpatialLocation.StarClusterId.IsNullOrEmpty())
            {
                throw new Exception("Try to create a spaceship on a system, but the system is not in a star cluster ???");
            }


            var spaceship = new Spaceship(blueprint, EnumOwnerType.Player, EnumFaction.Neutral)
            {
                Name = $"Spaceship {Guid.NewGuid().ToString().Substring(0, 5)}",
                OwnerId = userId,
                SpatialLocation = new SpatialLocation
                {
                    Position = starSystem.SpatialLocation.Position.Copy(),
                    UniverseId = starSystem.SpatialLocation.UniverseId,
                    GalaxyId = starSystem.SpatialLocation.GalaxyId,
                    StarClusterId = starSystem.SpatialLocation.StarClusterId
                }
            };

            spaceship = await Create(spaceship, blueprint);

            return spaceship;
        }
        catch (Exception e)
        {
            Log.Logger.Error(e, "Error while creating spaceship");
            throw;
        }
    }

    public async Task<Spaceship> CreatePirate(BlueprintSpaceship blueprint,
        SpatialLocation location)
    {
        try
        {
            Spaceship spaceship = new(blueprint, EnumOwnerType.Ai, EnumFaction.Pirate)
            {
                Name = $"Spaceship {Guid.NewGuid().ToString().Substring(0, 5)}",
                SpatialLocation = new SpatialLocation
                {
                    Position = location.Position,
                    UniverseId = location.UniverseId,
                    GalaxyId = location.GalaxyId,
                    StarClusterId = location.StarClusterId
                }
            };

            spaceship = await Create(spaceship, blueprint);

            return spaceship;
        }
        catch (Exception e)
        {
            Log.Logger.Error($"Error while creating spaceship {Environment.NewLine}{e}", EnumLogSeverity.Error);
            throw;
        }
    }

    private async Task<Spaceship> Create(
        Spaceship spaceship, BlueprintSpaceship blueprint)
    {
        // Load modules from blueprint to final spaceship module
        await _spaceshipModuleService.AssignModulesToSpaceship(spaceship, blueprint);

        // Save the raw blueprint
        spaceship.RawBlueprint = blueprint.JsonSerialize();

        var newSpaceship = await _spaceshipRepository.Create(spaceship);

        return newSpaceship;
    }

    #endregion

    #region DELETE

    /// <summary>
    ///     ADMIN METHOD
    ///     Delete all pirates from the game
    /// </summary>
    public async Task DeleteAllPirateSpaceships()
    {
        await _spaceshipRepository.DeletePirates();
    }

    public async Task DeleteSpaceship(Guid spaceshipId)
    {
        await _spaceshipRepository.Delete(spaceshipId);
    }

    #endregion
}