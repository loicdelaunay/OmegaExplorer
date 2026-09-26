namespace OmegaExplorer.Shared.Filters;

public class DynamicDriverStarSystemFilter : DynamicDriverFilter
{
    private const string FILTER_BY_STAR_CLUSTER_ID_KEY = "id";

    public bool IsGetAllStarSystems { get; set; }

    public DynamicDriverStarSystemFilter FilterByStarClusterId(Guid id)
    {
        Data.Add(FILTER_BY_STAR_CLUSTER_ID_KEY, id.ToString());
        return this;
    }

    public Guid? GetFilterByStarClusterId()
    {
        string raw = Data[FILTER_BY_STAR_CLUSTER_ID_KEY];
        if (string.IsNullOrEmpty(raw))
        {
            return null;
        }
        if (Guid.TryParse(raw, out Guid id))
        {
            return id;
        }
        else
        {
            return null;
        }
    }

    public void SetFilterToSearchAllStarSystems(bool value)
    {
        IsGetAllStarSystems = value;
    }
}
