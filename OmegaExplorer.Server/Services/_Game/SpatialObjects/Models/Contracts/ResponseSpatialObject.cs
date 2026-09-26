using OmegaExplorer.Server.Services._Game.Orders.Models.Contracts;

namespace OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Contracts;

/// <summary>
///     Define an object in space with position velocity and possibility to be ordered
/// </summary>
public class ResponseSpatialObject : ResponseOrderable
{
    public ResponseSpatialLocation SpatialLocation { get; set; }

    public int SpeedInUniverse { get; set; } = 0;

    public int SpeedInGalaxy { get; set; } = 0;

    public int SpeedInStarCluster { get; set; } = 0;
}