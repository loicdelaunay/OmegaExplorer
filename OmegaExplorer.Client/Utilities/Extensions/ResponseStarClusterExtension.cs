using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Utilities.Extensions;

public static class ResponseStarClusterExtension
{
    public static string GetMapUrl(this ResponseStarCluster starCluster)
    {
        string url = "/map/star-cluster/" + starCluster.Id;

        return url;
    }
}
