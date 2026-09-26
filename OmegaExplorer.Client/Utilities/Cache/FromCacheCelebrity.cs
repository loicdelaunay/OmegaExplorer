using OmegaExplorer.Client.Utilities.API;
using OmegaExplorer.Toolkit.API;
using System.Collections.Concurrent;

namespace OmegaExplorer.Client.Utilities.Caching;

public static class FromCacheCelebrity
{
    private static readonly ConcurrentDictionary<int, CacheEntry<IDynamicItemCelebrity>> _cache = new();

    public static async Task<IDynamicItemCelebrity?> Get(int index, TimeSpan expiration = default)
    {
        if (index == null)
        {
            return null;
        }

        if (expiration == TimeSpan.Zero)
        {
            expiration = TimeSpan.FromMinutes(1);
        }

        int indexToSearch = index;

        CacheEntry<IDynamicItemCelebrity> cacheEntry;
        if (_cache.TryGetValue(indexToSearch, out cacheEntry))
        {
            // Check if the cache entry has not expired (for example, expired after 10 minutes)
            if (DateTime.UtcNow < cacheEntry.Expiration)
            {
                // Return cached data
                return cacheEntry.Value;
            }

            // Delete cache entry if it has expired
            _cache.TryRemove(indexToSearch, out _);
        }

        IDynamicItemCelebrity? res = await ApiManager.Client.GetCelebrityByIdAsync(indexToSearch);
        if (res == null)
        {
            return null;
        }

        // Store response in cache with current timestamp
        _cache[indexToSearch] = new CacheEntry<IDynamicItemCelebrity> { Value = res, Expiration = DateTime.UtcNow.Add(expiration) };

        return res;
    }
}