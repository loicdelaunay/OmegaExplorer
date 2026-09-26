namespace OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Contracts;

public class ResponseSpatialDistance
{
    public int DistanceExitStarCluster { get; set; }
    public int DistanceExitGalaxy { get; set; }
    public int DistanceToExitUniverse { get; set; }
    public int DistanceInUniverseToReachGalaxy { get; set; }

    public int DistanceInGalaxyToReachStarCluster { get; set; }
    public int DistanceInStarClusterToReachPosition { get; set; }

    public bool ExitStarCluster { get; set; }
    public bool ExitGalaxy { get; set; }
    public bool ExitUniverse { get; set; }

    public bool EnterGalaxy { get; set; }
    public bool EnterStarSystem { get; set; }
    public bool EnterUniverse { get; set; }
    public bool Computed { get; set; }

    private ResponseSpatialLocation Actor { get; set; }
    private ResponseSpatialLocation Target { get; set; }
}