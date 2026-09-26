using OmegaExplorer.Client.Models.Card.Dynamic;
using OmegaExplorer.Client.Models.Card.Dynamic.Drivers;
using OmegaExplorer.Client.Utilities.Resources;
using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Models.Card.Definition;

[DynamicDriver(typeof(DynamicDriverOriginHistory))]
public class CardDefinitionOriginHistory : CardDefinition
{
    public CardDefinitionOriginHistory(IDynamicItemOriginHistory originHistory)
    {
        OriginHistory = originHistory;

        Title = originHistory.Name;
        Subtitle = string.Empty;
        Text = originHistory.Description;
        ImageHeader = GetImage();
    }

    public IDynamicItemOriginHistory OriginHistory { get; set; }

    private string GetImage()
    {
        return ResourceImage.GetImageOriginHistory(OriginHistory.Index);
    }
}