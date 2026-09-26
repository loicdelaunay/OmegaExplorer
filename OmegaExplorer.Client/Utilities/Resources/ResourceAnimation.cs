using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Utilities.Resources;

public static class ResourceAnimation
{
    public const string ERROR_UNKNOWN = "/res/anim/other/404.gif";
    public const string DAMAGE_FIRE = "/res/anim/spaceship/damage-fire.gif";
    public const string STAR_STANDARD_1 = "/res/anim/star/star-1.gif";
    public const string GALAXY_1 = "/res/anim/galaxy/galaxy-1.gif";
    public const string PLANET_STANDARD_1 = "/res/anim/planet/standard/planet-1.gif";
    public const string INSTABILITY_STANDARD_1 = "/res/anim/instability/instability-1.gif";

    public const string BACKGROUND_LOTTIE_SPACE = "/res/anim/background/space.lottie.json";

    public static string GetPlanetType(EnumPlanetType? planetType)
    {
        if (planetType == null)
        {
            return PLANET_STANDARD_1;
        }

        string path = $"/res/anim/planet/standard/planet-{planetType.ToString().ToLower()}.gif";

        return path;
    }
}