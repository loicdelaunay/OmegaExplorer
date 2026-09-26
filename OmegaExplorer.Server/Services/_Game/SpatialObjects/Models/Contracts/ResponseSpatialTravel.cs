namespace OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Contracts;

public class ResponseSpatialTravel
{
    public ResponseSpatialObject Actor { get; set; }

    public ResponseSpatialLocation Target { get; set; }

    public ResponseSpatialDistance? Distance { get; set; }

    public int DelayExitStarCluster { get; set; }

    public int DelayExitGalaxy { get; set; }

    public int DelayToExitUniverse { get; set; }

    public int DelayInUniverseToReachGalaxy { get; set; }

    public int DelayInGalaxyToReachStarCluster { get; set; }

    public int DelayInStarClusterToReachPosition { get; set; }
}