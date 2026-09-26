using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.Services._Game.StarSystems.Models.Entities;
using OmegaExplorer.Server.Services.Databases;
using OmegaExplorer.Server.Services.Loggers.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Instabilities;

public class InstabilityRepository
{
    private readonly DatabaseContext _databaseContext;

    public InstabilityRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<List<Instability>> GetAllByPosition(Guid userId, int xStart, int xEnd,
        int yStart,
        int yEnd)
    {
        var res = await _databaseContext.Instabilities
            .Where(instability => instability.SpatialLocation.Position.X >= xStart &&
                                  instability.SpatialLocation.Position.X <= xEnd &&
                                  instability.SpatialLocation.Position.Y >= yStart &&
                                  instability.SpatialLocation.Position.Y <= yEnd)
            .ToListAsync();
        return res;
    }

    public async Task Add(Instability instability)
    {
        try
        {
            _databaseContext.Instabilities.Add(instability);
            await _databaseContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Log.Logger.Information($"Error while adding instability {Environment.NewLine}{e}", EnumLogSeverity.Error);
            throw;
        }
    }
}