namespace OmegaExplorer.Shared.Filters;

public class DynamicDriverGalaxyFilter : DynamicDriverFilter
{
    private const string FILTER_BY_UNIVERSE_ID_KEY = "id";

    public void AddFilterByUniverseId(Guid id)
    {
        Data.Add(FILTER_BY_UNIVERSE_ID_KEY, id.ToString());
    }

    public Guid? GetFilterByUniverseId()
    {
        string raw = Data[FILTER_BY_UNIVERSE_ID_KEY];
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
}
