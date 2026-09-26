using OmegaExplorer.Client.Models.Card.Dynamic;
using OmegaExplorer.Client.Models.Card.Dynamic.Drivers;
using OmegaExplorer.Client.Utilities.Extensions;
using OmegaExplorer.Client.Utilities.Resources;
using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Models.Card.Definition;

[DynamicDriver(typeof(DynamicDriverGalaxy))]
public class CardDefinitionGalaxy : CardDefinition
{
    public CardDefinitionGalaxy(ResponseGalaxy galaxy)
    {
        Galaxy = galaxy;

        Title = galaxy.Name;
        Subtitle = "Galaxy";

        Contents.Add(new CardContent("Position", Galaxy.Position.ToStringFormatted()));

        ImageHeader = ResourceAnimation.GALAXY_1;
        ImageMode = EnumImageMode.Img;

        Actions.Add(new CardAction("Go", ResourceIcon.SEE, nameof(CallSeeGalaxy)));
    }

    public ResponseGalaxy Galaxy { get; set; }

    public void CallSeeGalaxy(Components.Card.Card card)
    {
        card.ImplNavigationManager.NavigateTo($"/map/galaxy/{Galaxy.Id}");
    }
}