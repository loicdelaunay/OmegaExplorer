using OmegaExplorer.Server.Extensions;
using OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Classes;
using OmegaExplorer.Server.Services.Databases;

namespace OmegaExplorer.Server.Services._Game.SpatialObjects;

public class SpatialLocationService
{
    private readonly DatabaseContext _databaseContext;

    public SpatialLocationService(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    private string GeneratedName(SpatialLocation location)
    {
        if (!location.StarClusterId.IsNullOrEmpty())
        {
            if (location.StarCluster == null)
                location.StarCluster =
                    _databaseContext.StarClusters.FirstOrDefault(x => x.Id == location.StarClusterId);

            return location.StarCluster?.Name;
        }

        if (!location.GalaxyId.IsNullOrEmpty())
        {
            if (location.Galaxy == null)
                location.Galaxy = _databaseContext.Galaxies.FirstOrDefault(x => x.Id == location.GalaxyId);

            return location.Galaxy?.Name;
        }

        if (!location.UniverseId.IsNullOrEmpty())
        {
            if (location.Universe == null)
                location.Universe = _databaseContext.Universes.FirstOrDefault(x => x.Id == location.UniverseId);

            return location.Universe?.Name;
        }

        return $"Void {location.Position.X}:{location.Position.Y}";
    }
}