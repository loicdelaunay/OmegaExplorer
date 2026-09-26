using OmegaExplorer.Client.Utilities.API;
using OmegaExplorer.Toolkit.API;
using System.Collections.Concurrent;

namespace OmegaExplorer.Client.Utilities.Caching;

public static class FromCacheStarCluster
{
    private static readonly ConcurrentDictionary<Guid, CacheEntry<ResponseStarCluster>> _cache = new();

    public static async Task<ResponseStarCluster> Get(Guid? id, TimeSpan expiration = default)
    {
        if (id == null)
        {
            return null;
        }

        if (expiration == TimeSpan.Zero)
        {
            expiration = TimeSpan.FromMinutes(1);
        }

        Guid idToSearch = id.Value;

        CacheEntry<ResponseStarCluster> cacheEntry;
        if (_cache.TryGetValue(idToSearch, out cacheEntry))
        {
            // Check if the cache entry has not expired (for example, expired after 10 minutes)
            if (DateTime.UtcNow < cacheEntry.Expiration)
            {
                // Return cached data
                return cacheEntry.Value;
            }

            // Delete cache entry if it has expired
            _cache.TryRemove(idToSearch, out _);
        }

        ResponseStarCluster? res = await ApiManager.Client.GetStarClusterByIdAsync(id);
        if (res == null)
        {
            return null;
        }

        // Store response in cache with current timestamp
        _cache[idToSearch] = new CacheEntry<ResponseStarCluster> { Value = res, Expiration = DateTime.UtcNow.Add(expiration) };

        return res;
    }
}