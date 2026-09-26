using OmegaExplorer.Client.Models.Card.Dynamic;
using OmegaExplorer.Client.Models.Card.Dynamic.Drivers;
using OmegaExplorer.Client.Utilities.Resources;
using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Models.Card.Definition;

[DynamicDriver(typeof(DynamicDriverBrand))]
public class CardDefinitionBrand : CardDefinition
{
    public IDynamicItemBrand Brand { get; set; }

    public CardDefinitionBrand(IDynamicItemBrand brand)
    {
        Brand = brand;
        Value = brand;

        Title = Brand.Name;
        Subtitle = "Brand";

        ImageHeader = ResourceImage.FIGHT;
        ImageMode = EnumImageMode.Img;
    }
}
