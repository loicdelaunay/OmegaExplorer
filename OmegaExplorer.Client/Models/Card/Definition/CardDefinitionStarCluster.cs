using OmegaExplorer.Client.Models.Card.Dynamic;
using OmegaExplorer.Client.Models.Card.Dynamic.Drivers;
using OmegaExplorer.Client.Utilities.Extensions;
using OmegaExplorer.Client.Utilities.Resources;
using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Models.Card.Definition;

[DynamicDriver(typeof(DynamicDriverStarCluster))]
public class CardDefinitionStarCluster : CardDefinition
{
    public CardDefinitionStarCluster(ResponseStarCluster starCluster)
    {
        StarCluster = starCluster;

        Title = starCluster.Name;
        Subtitle = "StarCluster";

        Contents.Add(new CardContent("Position", StarCluster.SpatialLocation.Position.ToStringFormated()));

        ImageHeader = ResourceAnimation.STAR_STANDARD_1;
        ImageMode = EnumImageMode.Img;

        Actions.Add(new CardAction("Go", ResourceIcon.SEE, nameof(CallSeeStarCluster)));
    }

    public ResponseStarCluster StarCluster { get; set; }

    public void CallSeeStarCluster(Components.Card.Card card)
    {
        card.ImplNavigationManager.NavigateTo($"/map/star-cluster/{StarCluster.Id}");
    }
}