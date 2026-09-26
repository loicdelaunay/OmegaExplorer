#region

using OmegaExplorer.Client.Models.Card.Dynamic;
using OmegaExplorer.Client.Models.Card.Dynamic.Drivers;
using OmegaExplorer.Client.Utilities.Resources;
using OmegaExplorer.Toolkit.API;

#endregion

namespace OmegaExplorer.Client.Models.Card.Definition;

[DynamicDriver(typeof(DynamicDriverSpaceshipModuleData))]
public class CardDefinitionSpaceshipModuleData : CardDefinition
{
    public CardDefinitionSpaceshipModuleData(ResponseDynamicItemSpaceshipModule moduleSpaceshipData)
    {
        ModuleSpaceshipData = moduleSpaceshipData;
        Value = moduleSpaceshipData;

        Title = moduleSpaceshipData.Name;
        Subtitle = string.Empty;

        ImageHeader = ResourceImage.GetImageSpaceshipModule(moduleSpaceshipData.Index);
        ImageMode = EnumImageMode.Img;
    }

    public ResponseDynamicItemSpaceshipModule ModuleSpaceshipData { get; set; }
}