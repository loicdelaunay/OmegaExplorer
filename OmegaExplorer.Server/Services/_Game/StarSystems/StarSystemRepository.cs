#region

using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.Extensions;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Cycles;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Classes;
using OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Classes;
using OmegaExplorer.Server.Services.Databases;
using OmegaExplorer.Server.Services.Loggers.Models.Enums;

#endregion

namespace OmegaExplorer.Server.Services._Game.StarSystems;

public class StarSystemRepository
{
    private readonly CycleBackgroundService _cycleBackgroundService;
    private readonly DatabaseContext _databaseContext;

    public StarSystemRepository(CycleBackgroundService cycleBackgroundService, DatabaseContext databaseContext)
    {
        _cycleBackgroundService = cycleBackgroundService;
        _databaseContext = databaseContext;
    }

    /// <summary> The GetAllByUser function returns a list of all planets owned by the user with the given id.</summary>
    /// <param name="Guid idUser">
    ///     /// the id of the user who owns the planet.
    /// </param>
    /// <param name="userId"></param>
    /// <returns> A list of planets owned by the user.</returns>
    public async Task<List<Models.Entities.StarSystem>> GetAllOwnerByUser(Guid userId)
    {
        var res = await _databaseContext.StarSystems
            .Include(system => system.Owner)
            .Include(system => system.SpatialLocation.StarCluster)
            .Include(system => system.SpatialLocation.Galaxy)
            .Include(system => system.SpatialLocation.Universe)
            .Include(system => system.Buildings)
            .Where(system => system.OwnerId == userId)
            .ToListAsync();
        return res;
    }


    /// <summary> The GetById function returns a planet with the given idPlanet and owned by the user with idUser.</summary>
    /// <param name="starSystemId"> The id of the planet to get</param>
    /// <param name="userId">
    ///     /// the id of the user who owns the planet.
    /// </param>
    /// <returns> A planet object.</returns>
    public async Task<Models.Entities.StarSystem?> GetById(Guid starSystemId, Guid userId)
    {
        var res = await _databaseContext.StarSystems
            .Include(starSystem => starSystem.Owner)
            .Include(starSystem => starSystem.SpatialLocation.StarCluster)
            .Include(starSystem => starSystem.SpatialLocation.Galaxy)
            .Include(starSystem => starSystem.SpatialLocation.Universe)
            .Include(starSystem => starSystem.Buildings)
            .FirstOrDefaultAsync(planet => planet.Id == starSystemId);
        return res;
    }

    /// <summary> The Add function adds a planet to the database.</summary>
    /// <param name="starSystem"> System to add</param>
    /// <returns> A void</returns>
    public async Task Add(Models.Entities.StarSystem? starSystem)
    {
        try
        {
            _databaseContext.StarSystems.Add(starSystem);
            await _databaseContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Log.Logger.Information($"Error while adding system {Environment.NewLine}{e}", EnumLogSeverity.Error);
            throw;
        }
    }

    /// <summary> The Remove function removes a planet from the database.</summary>
    /// <param name="systemId"> The id of the planet to be removed</param>
    /// <param name="userId"> The id of the user</param>
    /// <returns> A task.</returns>
    public async Task Delete(Guid systemId, Guid userId)
    {
        var system =
            await _databaseContext.StarSystems.FirstOrDefaultAsync(system =>
                system.OwnerId == userId && system.Id == systemId);

        if (system != null)
        {
            _databaseContext.StarSystems.Remove(system);
            await _databaseContext.SaveChangesAsync();
        }
    }

    public async Task<List<Models.Entities.StarSystem>> GetSystemsInStarCluster(Guid starClusterId)
    {
        var res = await _databaseContext.StarSystems
            .Include(system => system.Owner)
            .Include(system => system.SpatialLocation.StarCluster)
            .Include(system => system.SpatialLocation.Galaxy)
            .Include(system => system.SpatialLocation.Universe)
            .Include(system => system.Buildings)
            .Where(system => system.SpatialLocation.StarClusterId == starClusterId)
            .ToListAsync();

        return res;
    }

    public async Task<Models.Entities.StarSystem> Colonize(Guid systemId, Guid userId)
    {
        var system = await _databaseContext.StarSystems.FirstOrDefaultAsync(system => system.Id == systemId);

        //System not found
        if (system is null) throw new Exception("System not found");

        //Check if the planet is already colonized
        if (!system.OwnerId.IsNullOrEmpty()) throw new Exception("System already colonized");

        system.OwnerId = userId;

        await _databaseContext.SaveChangesAsync();

        return system;
    }

    public async Task Decolonize(Guid systemId, Guid userId)
    {
        var system = await _databaseContext.StarSystems
            .Include(starSystem => starSystem.Buildings)
            .FirstOrDefaultAsync(system => system.Id == systemId);

        //System not found
        if (system is null) throw new Exception("System not found");

        //Check if the planet is already colonized
        if (system.OwnerId.IsNullOrEmpty()) throw new Exception("System not colonized");

        if (system.OwnerId != userId) throw new Exception("You are not the owner of this system");

        system.OwnerId = null;

        //Destroy all buildings on the planet
        var building = system.Buildings;
        _databaseContext.Buildings.RemoveRange(building);

        await _databaseContext.SaveChangesAsync();
    }

    public async Task<int> CountColonizedByUser(Guid userId)
    {
        var count = await _databaseContext.StarSystems.CountAsync(planet => planet.OwnerId == userId);

        return count;
    }

    public async Task<Models.Entities.StarSystem?> GetByPosition(Vector2 position, Guid id)
    {
        var res = await _databaseContext.StarSystems
            .FirstOrDefaultAsync(system =>
                system.SpatialLocation.Position.X == position.X && system.SpatialLocation.Position.Y == position.Y &&
                system.SpatialLocation.StarClusterId == id);

        return res;
    }

    public async Task<List<Models.Entities.StarSystem>> GetAllStarSystems()
    {
        var res = await _databaseContext.StarSystems.ToListAsync();

        return res;
    }

    public async Task AddModifierToPlanet(Guid userId, Guid? starSystemId, ModifierResource modifier)
    {
        var starSystem = _databaseContext.StarSystems.FirstOrDefault(system => system.Id == starSystemId);

        if (starSystem is null) throw new Exception($"System not found at id {starSystemId}");

        modifier.StartCycle = CycleBackgroundService.CurrentCycle;
        starSystem.Modifiers.Add(modifier);

        await _databaseContext.SaveChangesAsync();
    }

    public async Task<Models.Entities.StarSystem?> GetByLocation(SpatialLocation getLocation)
    {
        var res = _databaseContext.StarSystems
            .FirstOrDefault(system =>
                system.SpatialLocation.Position.X == getLocation.Position.X &&
                system.SpatialLocation.Position.Y == getLocation.Position.Y &&
                system.SpatialLocation.StarClusterId == getLocation.StarClusterId);

        return res;
    }

    public async Task<List<Models.Entities.StarSystem>> GetAllWithSpecies()
    {
        // First recover the IDS of systems with an unused dictionary
        var systemIds = await _databaseContext.Database
            .SqlQuery<Guid>($"SELECT Id FROM StarSystems WHERE Species != '{{}}' AND Species IS NOT NULL")
            .ToListAsync();

        // Then load complete systems from filtered IDS
        var res = await _databaseContext.StarSystems
            .Where(s => systemIds.Contains(s.Id))
            .ToListAsync();
        return res;
    }

    public async Task<Models.Entities.StarSystem> GetRandomStarSystem(bool isColonized, bool? canBeColonized)
    {
        var query = _databaseContext.StarSystems.AsQueryable();

        // Filter by colonization status
        if (isColonized)
            query = query.Where(s => s.OwnerId != null);
        else
            query = query.Where(s => s.OwnerId == null);

        // Apply the colonization filter if specified
        if (canBeColonized.HasValue)
        {
            //TODO filter star system by colonization
        }

        // Include linked entities as in other methods
        query = query.Include(system => system.Owner)
            .Include(system => system.SpatialLocation.StarCluster)
            .Include(system => system.SpatialLocation.Galaxy)
            .Include(system => system.SpatialLocation.Universe)
            .Include(system => system.Buildings);

        // Recover all the corresponding systems
        var systems = await query.ToListAsync();

        if (systems.Count == 0) throw new Exception("No star system corresponds to the specified criteria");

        // Select a system randomly
        var randomIndex = Random.Shared.Next(0, systems.Count);
        var randomSystem = systems[randomIndex];

        return randomSystem;
    }
}