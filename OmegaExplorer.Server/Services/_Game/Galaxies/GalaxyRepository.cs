using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.Services._Game.Galaxies.Models.Entities;
using OmegaExplorer.Server.Services._Game.Galaxies.Models.Enums;
using OmegaExplorer.Server.Services.Databases;
using OmegaExplorer.Server.Services.Loggers.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Galaxies;

public class GalaxyRepository
{
    private readonly DatabaseContext _databaseContext;

    public GalaxyRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Galaxy?> GetGalaxyByType(EnumGalaxyType type)
    {
        var galaxy = await _databaseContext.Galaxies.FirstOrDefaultAsync(galaxy => galaxy.Type == type);

        return galaxy;
    }

    public async Task<Galaxy?> GetFirstGalaxy()
    {
        var galaxy = await _databaseContext.Galaxies.FirstOrDefaultAsync();

        return galaxy;
    }

    public async Task Create(Galaxy newGalaxy)
    {
        try
        {
            _databaseContext.Galaxies.Add(newGalaxy);
            await _databaseContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Log.Logger.Information($"Error while adding galaxy {Environment.NewLine}{e}", EnumLogSeverity.Error);
            throw;
        }
    }

    public async Task<List<Galaxy>> GetGalaxiesInUniverse(Guid universeId)
    {
        var galaxies = await _databaseContext.Galaxies.Where(galaxy => galaxy.UniverseId == universeId).ToListAsync();

        return galaxies;
    }

    public async Task<Galaxy?> GetById(Guid galaxyId)
    {
        var galaxy = await _databaseContext.Galaxies.FirstOrDefaultAsync(galaxy => galaxy.Id == galaxyId);

        return galaxy;
    }
}