#region

using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.Services._Game.Buildings.Models.Entities;
using OmegaExplorer.Server.Services.Databases;
using OmegaExplorer.Server.Services.Loggers.Models.Enums;

#endregion

namespace OmegaExplorer.Server.Services._Game.Buildings;

public class BuildingRepository
{
    private readonly DatabaseContext _databaseContext;

    public BuildingRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    /// <summary>
    ///     Create a building
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="systemId"></param>
    /// <param name="indexBuilding"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<Building> Create(Guid userId, Guid systemId, int indexBuilding)
    {
        var starSystem = await _databaseContext.StarSystems
            .Include(system => system.Buildings)
            .FirstOrDefaultAsync(system => system.Id == systemId);

        if (starSystem == null) throw new Exception("System not found.");


        Building newBuilding = new(indexBuilding, starSystem);

        _databaseContext.Buildings.Add(newBuilding);

        try
        {
            await _databaseContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Log.Logger.Error("Error while saving changes on Repository.BuildOnSystem method : " + e,
                EnumLogSeverity.Error);
            throw;
        }

        return newBuilding;
    }

    public List<Building> GetAllBuildingsBySystem(Guid systemId)
    {
        var buildings = _databaseContext.Buildings
            .Where(building => building.SystemId == systemId)
            .ToList();

        return buildings;
    }

    public async Task Delete(Guid buildingId, Guid userId)
    {
        var debug = await _databaseContext.Buildings
            .Include(building => building.System)
            .ThenInclude(system => system.Owner)
            .ToListAsync();

        var building = await _databaseContext.Buildings
            .Include(building => building.System)
            .ThenInclude(system => system.Owner)
            .FirstOrDefaultAsync(building => building.Id == buildingId && building.System.Owner.Id == userId);

        if (building == null) throw new Exception("Building not found.");

        _databaseContext.Buildings.Remove(building);
        await _databaseContext.SaveChangesAsync();
    }
}