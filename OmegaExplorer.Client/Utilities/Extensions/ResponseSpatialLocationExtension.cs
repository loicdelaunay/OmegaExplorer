using OmegaExplorer.Client.Utilities.Caching;
using OmegaExplorer.Server.Extensions;
using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Utilities.Extensions;

public static class ResponseSpatialLocationExtension
{
    public static string ToStringFormated(this ResponseSpatialLocation location, bool simple = false)
    {
        if (location == null)
        {
            return "Unknown";
        }

        string res = string.Empty;

        if (simple)
        {
            res = $"{location.Name}";
        }
        else
        {
            res = $"{location.Name} | {location.Universe?.Name} >  {location.Galaxy?.Name} > {location.StarCluster?.Name} > {location.Name} > {location.Position?.ToStringFormated()}";
        }

        return res;
    }

    public static async Task Feed(this ResponseSpatialLocation location)
    {
        if (!location.UniverseId.IsNullOrEmpty())
        {
            location.Universe = await FromCacheUniverse.Get(location.UniverseId);
        }

        if (!location.GalaxyId.IsNullOrEmpty())
        {
            location.Galaxy = await FromCacheGalaxy.Get(location.GalaxyId);
        }

        if (!location.StarClusterId.IsNullOrEmpty())
        {
            location.StarCluster = await FromCacheStarCluster.Get(location.StarClusterId);
        }
    }
}
