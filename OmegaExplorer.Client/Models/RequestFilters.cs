namespace OmegaExplorer.Client.Models;

public class RequestFilters
{
    private readonly Dictionary<string, string> _filters = new();

    public void AddFilter(string key, string value)
    {
        _filters[key] = value;
    }
}