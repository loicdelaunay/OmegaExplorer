using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.Services._Game.StarClusters.Models.Entities;
using OmegaExplorer.Server.Services.Databases;

namespace OmegaExplorer.Server.Services._Game.StarClusters;

public class StarClusterRepository
{
    private readonly DatabaseContext _databaseContext;

    public StarClusterRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task Add(StarCluster starCluster)
    {
        _databaseContext.StarClusters.Add(starCluster);

        await _databaseContext.SaveChangesAsync();
    }

    public async Task<List<StarCluster>> GetStarClustersInGalaxy(Guid galaxyId)
    {
        var starClusters = await _databaseContext.StarClusters
            .Where(x => x.SpatialLocation.GalaxyId == galaxyId)
            .ToListAsync();

        return starClusters;
    }

    public async Task<StarCluster?> GetById(Guid starClusterId)
    {
        var starCluster = await _databaseContext.StarClusters
            .FirstOrDefaultAsync(x => x.Id == starClusterId);

        return starCluster;
    }

    public async Task<List<StarCluster>> GetAll()
    {
        var starClusters = await _databaseContext.StarClusters
            .ToListAsync();

        return starClusters;
    }
}