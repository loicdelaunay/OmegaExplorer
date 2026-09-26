using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Utilities.Extensions;

public static class ResponseSpatialTravelExtension
{
    public static int GetTotalCycles(this ResponseSpatialTravel travel)
    {
        int total = 0;

        total += travel.DelayExitGalaxy;
        total += travel.DelayExitStarCluster;
        total += travel.DelayToExitUniverse;
        total += travel.DelayInUniverseToReachGalaxy;
        total += travel.DelayInGalaxyToReachStarCluster;
        total += travel.DelayInStarClusterToReachPosition;

        return total;
    }
}