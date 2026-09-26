namespace OmegaExplorer.Client.Utilities.Caching;

public class CacheEntry<T>
{
    public T Value { get; set; }
    public DateTime Expiration { get; set; }
}