using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Galaxies.Models.Entities;
using OmegaExplorer.Server.Services._Game.StarClusters.Models.Entities;
using OmegaExplorer.Server.Services._Game.Universes.Models.Entities;
using OmegaExplorer.Server.Services.Databases;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Classes;

/// <summary>
///     Represents a location in the universe (Universe, Galaxy, StarSystem)
///     the element is located at a specific position in referential system
/// </summary>
[Table(nameof(DatabaseContext.SpatialLocations))]
public class SpatialLocation : Identifiable
{
    [ActivatorUtilitiesConstructor]
    public SpatialLocation()
    {
    }

    public SpatialLocation(Vector2 position, Guid? universeId, Guid? galaxyId, Guid? starClusterId, Universe? universe,
        Galaxy? galaxy, StarCluster? starCluster)
    {
        Universe = universe;
        Galaxy = galaxy;
        StarCluster = starCluster;
        Position = position;

        UniverseId = universeId;
        GalaxyId = galaxyId;
        StarClusterId = starClusterId;
    }

    public SpatialLocation(int x, int y, Guid? universeId, Guid? galaxyId, Guid? starClusterId)
    {
        Position = new Vector2(x, y);
        UniverseId = universeId;
        GalaxyId = galaxyId;
        StarClusterId = starClusterId;
    }

    [ForeignKey(nameof(Universe))]
    public Guid? UniverseId { get; set; }

    public Universe? Universe { get; set; }

    [ForeignKey(nameof(Galaxy))]
    public Guid? GalaxyId { get; set; }

    public Galaxy? Galaxy { get; set; }

    [ForeignKey(nameof(StarCluster))]
    public Guid? StarClusterId { get; set; }

    public StarCluster? StarCluster { get; set; }

    public Vector2 Position { get; set; } = new();

    public bool SameLocation(SpatialLocation? locationToCompare)
    {
        if (locationToCompare == null) return false;

        if (locationToCompare.Position.X != Position.X) return false;

        if (locationToCompare.Position.Y != Position.Y) return false;

        if (locationToCompare.UniverseId != UniverseId) return false;

        if (locationToCompare.GalaxyId != GalaxyId) return false;

        if (locationToCompare.StarClusterId != StarClusterId) return false;

        return true;
    }

    public SpatialDistance Distance(SpatialLocation locationTarget)
    {
        SpatialDistance distance = new(this, locationTarget);

        return distance;
    }
}