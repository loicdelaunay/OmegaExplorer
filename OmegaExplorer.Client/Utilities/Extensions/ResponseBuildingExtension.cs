#region

using OmegaExplorer.Client.Utilities.API;
using OmegaExplorer.Toolkit.API;

#endregion

namespace OmegaExplorer.Client.Utilities.Extensions;

public static class ResponseBuildingExtension
{
    public static string GetFormatted(this List<ResponseBuilding> buildings, string spliter = ", ")
    {
        IEnumerable<string> names = buildings.Select(building => building.Name);

        string result = string.Join(spliter, names);

        return result;
    }

    public static string GetImage(this ResponseBuilding responseBuilding)
    {
        return $"/res/img/dynamic/building/{responseBuilding.Data.Index}.webp";
    }

    public static string GetImage(this IDynamicItemBuilding buildingData)
    {
        return $"/res/img/dynamic/building/{buildingData.Index}.webp";
    }
}