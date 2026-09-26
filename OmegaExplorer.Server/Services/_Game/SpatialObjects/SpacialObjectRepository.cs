using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.Services._Core.Models.Classes;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Entities;
using OmegaExplorer.Server.Services.Databases;
using OmegaExplorer.Server.Services.Databases.Extensions;

namespace OmegaExplorer.Server.Services._Game.SpatialObjects;

public class SpacialObjectRepository : RepositoryCustom
{
    private readonly DatabaseContext _databaseContext;

    public SpacialObjectRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    /// <summary>
    ///     Get the spatial object at specific id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<SpatialObject?> GetById(Guid id)
    {
        var res = await _databaseContext.SpatialObjects.FindAsync(id);

        return res;
    }

    public async Task<SpatialObject?> GetByIdWithInclude(Guid id)
    {
        try
        {
            var res = await _databaseContext.SpatialObjects
                                            .Include(spatialObject => spatialObject.SpatialLocation)
                                            .ThenInclude(location => location.Universe)
                                            .Include(spatialObject => spatialObject.SpatialLocation)
                                            .ThenInclude(location => location.Galaxy)
                                            .Include(spatialObject => spatialObject.SpatialLocation)
                                            .ThenInclude(location => location.StarCluster)
                                            .FirstOrDefaultAsync(spatialLocatedObject => spatialLocatedObject.Id == id);

            return res;
        }
        catch (Exception e)
        {
            throw;
        }
    }

    /// <summary>
    ///     Check if a spatial object exist at the given position
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public async Task<bool> ObjectExistAtPosition(Vector2 position)
    {
        var exist = await _databaseContext.SpatialObjects.AnyAsync(o => o.SpatialLocation.Position == position);

        return exist;
    }

    /// <summary>
    ///     Check if a spatial object exist near the given position with a given distance
    /// </summary>
    /// <param name="position"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    public async Task<bool> ObjectExistClosePosition(Vector2 position, int distance)
    {
        var exist = await _databaseContext.SpatialObjects.WithinDistance(position, distance).AnyAsync();

        return exist;
    }
}