namespace OmegaExplorer.Shared.Filters;

public class DynamicDriverFilter
{
    private const string FILTER_BY_ID_KEY = "id";

    /// <summary>
    /// Dictionary of data behind the filter
    /// </summary>
    public Dictionary<string, string> Data { get; private set; } = new();

    public DynamicDriverFilter AddFilterById(Guid id)
    {
        Data[FILTER_BY_ID_KEY] = id.ToString();
        return this;
    }

    public Guid? GetFilterById()
    {
        if (!Data.TryGetValue(FILTER_BY_ID_KEY, out string? raw) || string.IsNullOrEmpty(raw))
        {
            return null;
        }

        if (Guid.TryParse(raw, out Guid id))
        {
            return id;
        }

        return null;
    }

    public override string ToString()
    {
        return $"Filter | data is {string.Join(", ", Data.Select(kvp => $"{kvp.Key}={kvp.Value}"))}";
    }

    public void Concat(DynamicDriverFilter filter)
    {
        foreach (KeyValuePair<string, string> kvp in filter.Data)
        {
            Data[kvp.Key] = kvp.Value;
        }
    }
}
