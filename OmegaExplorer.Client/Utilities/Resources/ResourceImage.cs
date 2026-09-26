namespace OmegaExplorer.Client.Utilities.Resources;

public static class ResourceImage
{
    public const string BUILD_SPACESHIP = "/res/img/other/spaceship-build.png";

    public const string QUEST = "/res/img/icons/quest.png";

    public const string D20_BACKGROUND = "/res/img/icons/d20-background.png";

    public const string FIGHT = "/res/img/icons/fight.png";

    public const string LIMIT_SYSTEM = "/res/img/icons/limit-system.png";

    public const string PROTECT = "/res/img/icons/shield.png";
    public const string REPAIR = "/res/img/icons/repair.png";

    public const string COLONIZE = "/res/img/icons/colonize.png";

    public const string ATTACK = "/res/img/icons/attack.png";
    public const string DAMAGE = "/res/img/icons/attack.png";

    public const string BLUEPRINT_SPACESHIP = "/res/img/icons/blueprint-spaceship.png";

    public const string USER = "/res/img/icons/user.svg";
    public const string LOGO = "/res/img/logo.png";
    public const string LOGO_GOOGLE = "/res/img/icons/google.png";

    public const string UNKNOWN_RESOURCE = "res/img/dynamic/game-resources/unknown-resource.png";
    public const string MESSAGE = "res/img/icons/message.png";

    public static string GetImagePersonality(string race = "human", int index = 0)
    {
        string path = $"/res/img/dynamic/personality/{race}-{index}.png";

        return path;
    }

    public static string GetImageGameResource(int index)
    {
        string path = $"/res/img/dynamic/game-resource/{index}.png";

        return path;
    }

    public static string GetImageBuilding(int index)
    {
        string path = $"/res/img/dynamic/building/{index}.webp";

        return path;
    }

    public static string GetImageTechnology(int index)
    {
        string path = $"/res/img/dynamic/technology/{index}.webp";

        return path;
    }

    public static string GetImageSpaceshipModule(int index)
    {
        string path = $"/res/img/dynamic/spaceship/module/{index}.webp";

        return path;
    }

    public static string GetImageSpeciesIllustration(int index)
    {
        string path = $"/res/img/dynamic/species/illustrations/{index}.webp";

        return path;
    }

    public static string GetImageSpeciesPortrait(int indexSpecies, int index)
    {
        string path = $"/res/img/dynamic/species/portraits/{indexSpecies}/{index}.webp";

        return path;
    }

    public static string GetImageOriginHistory(int index)
    {
        string path = $"/res/img/dynamic/origin-history/{index}.webp";

        return path;
    }

    public static string GetImageEvent(int index)
    {
        string path = $"/res/img/dynamic/event/{index}.webp";

        return path;
    }

    public static string GetImageScenario(int index)
    {
        string path = $"res/img/dynamic/event/{index}.webp";

        return path;
    }
}