namespace OmegaExplorer.Shared.Filters;

public class DynamicDriverSpaceshipFilter : DynamicDriverFilter
{
    private const string FILTER_BY_STAR_CLUSTER_ID_KEY = "starcluster-id";

    private const string FILTER_BY_SPACESHIP_TYPE_KEY = "spaceship-type";

    private const string FILTER_BY_SPACESHIP_OWNER_KEY = "spaceship-owner";

    public enum SpaceshipType
    {
        Fighter,
        Colonizer,
        Transporter,
    }

    public enum SpaceshipOwner
    {
        All,
        ConnectedPlayer,
        Ally,
        Enemy,
    }

    public DynamicDriverSpaceshipFilter FilterByStarClusterId(Guid id)
    {
        Data.Add(FILTER_BY_STAR_CLUSTER_ID_KEY, id.ToString());

        return this;
    }

    public Guid? GetFilterByStarClusterId()
    {
        if (Data.TryGetValue(FILTER_BY_STAR_CLUSTER_ID_KEY, out string? raw))
        {
            if (Guid.TryParse(raw, out Guid id))
            {
                return id;
            }
            else
            {
                return null;
            }
        }

        return null;
    }

    public DynamicDriverSpaceshipFilter AddFilterBySpaceshipType(SpaceshipType type)
    {
        Data.Add(FILTER_BY_SPACESHIP_TYPE_KEY, type.ToString());

        return this;
    }

    public SpaceshipType? GetFilterBySpaceshipType()
    {
        if (Data.TryGetValue(FILTER_BY_SPACESHIP_TYPE_KEY, out string? raw))
        {
            if (Enum.TryParse(raw, out SpaceshipType type))
            {
                return type;
            }

            return null;
        }

        return null;
    }

    public DynamicDriverSpaceshipFilter AddFilterBySpaceshipOwner(SpaceshipOwner owner)
    {
        Data[FILTER_BY_SPACESHIP_OWNER_KEY] = owner.ToString();

        return this;
    }

    public SpaceshipOwner? GetFilterBySpaceshipOwner()
    {
        if (Data.TryGetValue(FILTER_BY_SPACESHIP_OWNER_KEY, out string? raw))
        {
            if (Enum.TryParse(raw, out SpaceshipOwner owner))
            {
                return owner;
            }

            return null;
        }

        return null;
    }


}
