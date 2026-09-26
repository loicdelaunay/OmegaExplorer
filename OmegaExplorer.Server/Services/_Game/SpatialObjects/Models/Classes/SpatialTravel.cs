namespace OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Classes;

/// <summary>
///     Class to compute the travel of a spaceship to reach a target location
/// </summary>
public class SpatialTravel
{
    public SpatialTravel(Entities.SpatialObject actor, SpatialLocation target)
    {
        Actor = actor;
        Target = target;
    }

    public Entities.SpatialObject Actor { get; set; }
    public SpatialLocation Target { get; set; }
    public SpatialDistance? Distance { get; set; }

    public int DelayExitStarCluster { get; set; }
    public int DelayExitGalaxy { get; set; }
    public int DelayToExitUniverse { get; set; }
    public int DelayInUniverseToReachGalaxy { get; set; }
    public int DelayInGalaxyToReachStarCluster { get; set; }
    public int DelayInStarClusterToReachPosition { get; set; }
}