using OmegaExplorer.Server.Services._Core.Extensions;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Buildings;
using OmegaExplorer.Server.Services._Game.Buildings.Data._0_100Administration;
using OmegaExplorer.Server.Services._Game.Buildings.Data._100_200Shipyard;
using OmegaExplorer.Server.Services._Game.Events;
using OmegaExplorer.Server.Services._Game.Events.Data;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Classes;
using OmegaExplorer.Server.Services._Game.Limits;
using OmegaExplorer.Server.Services._Game.Limits.Models.Exceptions;
using OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Classes;
using OmegaExplorer.Server.Services._Game.Species;
using OmegaExplorer.Server.Services._Game.Species.Data;
using OmegaExplorer.Server.Services._Game.StarClusters.Models.Entities;
using OmegaExplorer.Server.Services._Game.StarSystems.Generators;
using OmegaExplorer.Server.Services._Game.StarSystems.Models.Entities;
using OmegaExplorer.Server.Services.Loggers.Models.Enums;
using OmegaExplorer.Server.Services.Users.Models.Entities;

namespace OmegaExplorer.Server.Services._Game.StarSystems;

public class StarSystemService
{
    private readonly BuildingService _buildingService;
    private readonly EventService _eventService;
    private readonly LimitService _limitService;
    private readonly SpeciesService _speciesService;
    private readonly StarSystemRepository _starSystemRepository;

    public StarSystemService(LimitService limitService, StarSystemRepository starSystemRepository,
        SpeciesService speciesService, BuildingService buildingService, EventService eventService)
    {
        _limitService = limitService;
        _starSystemRepository = starSystemRepository;
        _speciesService = speciesService;
        _buildingService = buildingService;
        _eventService = eventService;
    }

    public async Task<Models.Entities.StarSystem> ColonizeStarSystem(User user, Guid starSystemId)
    {
        // Check limit
        var limit = await _limitService.GetLimitStarSystemByUser(user.Id);

        if (limit.IsLimitReached()) throw new ExceptionLimitReached(limit);

        // Colonize planet
        var newStarSystem = await _starSystemRepository.Colonize(starSystemId, user.Id);

        // Add species on star system
        newStarSystem = await _speciesService.UpdateAmountOfASpeciesOnStarSystem(user.Id, starSystemId,
            DynamicItemSpecies_0_Humanity.INDEX, 1_000_000_000);

        // Build first building
        await _buildingService.CreateBuildingOnSystem(user.Id, newStarSystem.Id,
            DynamicItemBuildingAdministration.INDEX);
        await _buildingService.CreateBuildingOnSystem(user.Id, newStarSystem.Id,
            DynamicItemBuildingShipyard_100_.INDEX);

        // Throw event
        await _eventService.CreateEvent(user.Id, DynamicItemEvent_0_NewPlanetColonize.INDEX, newStarSystem.Id,
            nameof(Models.Entities.StarSystem));


        return newStarSystem;
    }

    /// <summary>
    ///     Count the total number of species living on the planet
    /// </summary>
    /// <returns></returns>
    public long CountTotalSpeciesLiving(Models.Entities.StarSystem starSystem)
    {
        var res = starSystem.Species.Sum(s => s.Value);

        return res;
    }

    /// <summary>
    ///     Get the nearest star system owned by user to a specific position
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="location"></param>
    /// <returns></returns>
    public async Task<Models.Entities.StarSystem?> GetStarSystemByUserAndNearestToPosition(Guid userId,
        SpatialLocation location)
    {
        var starSystems = await _starSystemRepository.GetAllOwnerByUser(userId);

        if (!starSystems.Any()) return null;

        var nearestSystem = starSystems
            .OrderBy(s => s.SpatialLocation.StarClusterId == location.StarClusterId ? 0 : 1)
            .ThenBy(s => s.SpatialLocation.GalaxyId == location.GalaxyId ? 0 : 1)
            .ThenBy(s => s.SpatialLocation.UniverseId == location.UniverseId ? 0 : 1)
            .ThenBy(s => s.SpatialLocation.Position.Distance(location.Position))
            .FirstOrDefault();

        return nearestSystem;
    }

    public async Task<Planet?> CreatePlanet(Vector2 position, StarCluster starCluster)
    {
        var newPlanet = await StarSystemGenerator.GeneratePlanet(position, starCluster);

        if (newPlanet == null)
        {
            Log.Logger.Error("Planet could not be generated", EnumLogSeverity.Warning);
            return null;
        }

        if (newPlanet.SpatialLocation.StarClusterId == null)
        {
            Log.Logger.Error("Planet could not be generated, id is null ???", EnumLogSeverity.Error);
            return null;
        }

        //Check if system not existing at element position
        var existingSystem = await _starSystemRepository.GetByPosition(newPlanet.SpatialLocation.Position,
            (Guid)newPlanet.SpatialLocation.StarClusterId);
        if (existingSystem != null)
        {
            Log.Logger.Error($"System already exists at position {newPlanet.SpatialLocation.Position} ignoring it",
                EnumLogSeverity.Warning);
            return null;
        }


        await _starSystemRepository.Add(newPlanet);

        return newPlanet;
    }

    public async Task<Models.Entities.Instability?> CreateInstability(Vector2 position,
        StarCluster starCluster)
    {
        var newInstability = await StarSystemGenerator.GenerateInstability(position, starCluster);

        await _starSystemRepository.Add(newInstability);

        return newInstability;
    }

    public async Task<Star?> CreateStar(Vector2 position, StarCluster starCluster)
    {
        var newStar = await StarSystemGenerator.GenerateStar(position, starCluster);

        await _starSystemRepository.Add(newStar);

        return newStar;
    }

    public async Task AddModifierToPlanet(Guid userId, Guid? starSystemId, ModifierResource modifier)
    {
        await _starSystemRepository.AddModifierToPlanet(userId, starSystemId, modifier);
    }

    public async Task<Models.Entities.StarSystem?> GetStarSystemByLocation(SpatialLocation getLocation)
    {
        var res = await _starSystemRepository.GetByLocation(getLocation);
        return res;
    }

    public async Task<List<Models.Entities.StarSystem>> GetAllStarSystems()
    {
        var starSystems = await _starSystemRepository.GetAllStarSystems();
        return starSystems;
    }

    public async Task ComputeBirthRateForAllStarSystems()
    {
        var starSystems = await _starSystemRepository.GetAllWithSpecies();

        //remove all star systems reaching max species on the planet
        starSystems = starSystems
            .Where(starSystem => CountTotalSpeciesLiving(starSystem) <= starSystem.MaxLivingSpecies)
            .ToList();

        foreach (var starSystem in starSystems)
            //add for each species the birth rate of 3% 
            foreach (var species in starSystem.Species)
            {
                const decimal BIRTH_RATE_IN_PERCENTAGE = 3m;

                var birthIncrement = species.Value * BIRTH_RATE_IN_PERCENTAGE / 100m;
                var newAmount = species.Value + (long)birthIncrement;

                await _speciesService.UpdateAmountOfASpeciesOnStarSystem(null, starSystem.Id, species.Key, newAmount, true);
            }
    }

    public async Task<Models.Entities.StarSystem> GetRandomStarSystem(bool isColonized = false,
        bool? canBeColonized = null)
    {
        var starSystem = await _starSystemRepository.GetRandomStarSystem(isColonized, canBeColonized);

        return starSystem;
    }

    public async Task<Models.Entities.StarSystem?> GetStarSystemById(Guid userId, Guid starSystemId)
    {
        var starSystem = await _starSystemRepository.GetById(starSystemId, userId);

        return starSystem;
    }
}