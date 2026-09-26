using OmegaExplorer.Client.Models.Card.Dynamic;
using OmegaExplorer.Client.Models.Card.Dynamic.Drivers;
using OmegaExplorer.Client.Utilities.Extensions;
using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Models.Card.Definition;

[DynamicDriver(typeof(DynamicDriverBlueprintBuilding))]
public class CardDefinitionBlueprintBuilding : CardDefinition
{
    public CardDefinitionBlueprintBuilding(IDynamicItemBuilding buildingData)
    {
        BlueprintBuilding = buildingData;
        Value = buildingData;

        Title = buildingData.Name;
        Subtitle = buildingData.Description;

        ImageHeader = buildingData.GetImage();
        ImageMode = EnumImageMode.Img;

        Rarity = buildingData.Rarity;
    }

    public IDynamicItemBuilding BlueprintBuilding { get; set; }
}