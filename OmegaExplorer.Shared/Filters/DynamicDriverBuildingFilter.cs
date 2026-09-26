namespace OmegaExplorer.Shared.Filters;

public class DynamicDriverBuildingFilter : DynamicDriverFilter
{
    private const string FILTER_BY_GALAXY_ID_KEY = "id";

    public void AddFilterByStarSystemId(Guid id)
    {
        Data.Add(FILTER_BY_GALAXY_ID_KEY, id.ToString());
    }

    public Guid? GetFilterByStarSystemId()
    {
        string raw = Data[FILTER_BY_GALAXY_ID_KEY];
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
