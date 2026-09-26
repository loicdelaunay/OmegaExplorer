using OmegaExplorer.Client.Utilities.Resources;
using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Utilities.Extensions;

public static class SystemTypeExtension
{
    public static string GetImage(this EnumStarSystemType systemType, EnumPlanetType? planetType)
    {
        switch (systemType)
        {
            case EnumStarSystemType.Star:
                return ResourceAnimation.STAR_STANDARD_1;

            case EnumStarSystemType.Planet:
                return GetImagePlanet(planetType);

            case EnumStarSystemType.Instability:

            case EnumStarSystemType.Unknown:

            default:
                return ResourceAnimation.ERROR_UNKNOWN;
        }
    }

    private static string GetImagePlanet(EnumPlanetType? planetType)
    {
        string target = ResourceAnimation.GetPlanetType(planetType);

        return target;
    }
}