using OmegaExplorer.Server.Services._Core.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Galaxies.Models.Contracts;
using OmegaExplorer.Server.Services._Game.StarClusters.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Universes.Models.Contracts.Responses;

namespace OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Contracts;

/// <summary>
///     Position of an object in space
/// </summary>
public class ResponseSpatialLocation
{
    public string Name { get; set; }

    public Guid? UniverseId { get; set; }
    public ResponseUniverse? Universe { get; set; }

    public Guid? GalaxyId { get; set; }

    public ResponseGalaxy? Galaxy { get; set; }

    public Guid? StarClusterId { get; set; }
    public ResponseStarCluster? StarCluster { get; set; }

    public ResponseVector2 Position { get; set; }
}