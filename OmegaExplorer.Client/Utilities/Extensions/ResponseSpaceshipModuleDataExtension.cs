using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Utilities.Extensions;

public static class ResponseSpaceshipModuleDataExtension
{
    public static ResponseSpaceshipModule ToSpaceshipModule(this ResponseDynamicItemSpaceshipModule moduleData)
    {
        return new ResponseSpaceshipModule
        {
            Name = moduleData.Name,
            Index = moduleData.Index,
            X = moduleData.X,
            Y = moduleData.Y,
            Data = moduleData
        };
    }
}