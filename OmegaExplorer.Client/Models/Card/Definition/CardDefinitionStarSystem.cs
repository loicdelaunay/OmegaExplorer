using MudBlazor;
using OmegaExplorer.Client.Dialogs;
using OmegaExplorer.Client.Models.Card.Dynamic;
using OmegaExplorer.Client.Models.Card.Dynamic.Drivers;
using OmegaExplorer.Client.Utilities.Dialog;
using OmegaExplorer.Client.Utilities.Extensions;
using OmegaExplorer.Client.Utilities.Resources;
using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Models.Card.Definition;

[DynamicDriver(typeof(DynamicDriverStarSystem))]
public class CardDefinitionStarSystem : CardDefinition
{
    public CardDefinitionStarSystem(ResponseStarSystem starSystem)
    {
        StarSystem = starSystem;
        Value = starSystem;

        Title = StarSystem.Name;
        Subtitle = string.Empty;

        Owner = StarSystem.Owner;

        ImageHeader = StarSystem.StarSystemType.GetImage(StarSystem.PlanetType);

        ImageMode = EnumImageMode.Img;

        if (StarSystem.PlanetType != null)
        {
            Contents.Add(new CardContent("Type", StarSystem.PlanetType.ToString()));
        }

        Contents.Add(new CardContent("Size", StarSystem.Size.ToString()));
        Contents.Add(new CardContent("Slots", StarSystem.Slots.ToString()));
        Contents.Add(new CardContent("Position", StarSystem.SpatialLocation.Position.ToStringFormated()));

        CardContent buildingContent = new("Buildings", StarSystem.Buildings.Count.ToString());
        buildingContent.ValueDetailed = StarSystem.Buildings.GetFormatted(Environment.NewLine);
        Contents.Add(buildingContent);

        if (StarSystem.SpatialLocation.StarCluster != null)
        {
            string location =
                $"Galaxy : {StarSystem.SpatialLocation.Galaxy?.Name} {StarSystem.SpatialLocation.Galaxy?.Position.ToStringFormatted()} | Star Cluster : {StarSystem.SpatialLocation.StarCluster?.Name} {StarSystem.SpatialLocation.Galaxy?.Position.ToStringFormatted()}";
            Contents.Add(new CardContent("Located", StarSystem.SpatialLocation.StarCluster.Name, location,
                color: Color.Primary, targetUrl: StarSystem.SpatialLocation.StarCluster.GetMapUrl()));
        }
        else if (StarSystem.SpatialLocation.Galaxy != null)
        {
            Contents.Add(new CardContent("Located", StarSystem.SpatialLocation.Galaxy.Name));
        }
        else if (StarSystem.SpatialLocation.Universe != null)
        {
            Contents.Add(new CardContent("Located", StarSystem.SpatialLocation.Universe.Name));
        }

        Actions.Add(new CardAction("See", ResourceIcon.SEE, nameof(CallGoTo)));
        Actions.Add(new CardAction("Info", ResourceIcon.INFO, nameof(CallInfo)));

        if (!starSystem.IsOwner())
        {
            Actions.Add(new CardAction("Colonize", ResourceIcon.COLONIZE, nameof(CallColonize)));
        }

        Actions.Add(new CardAction("Move ship", ResourceIcon.SPACESHIP, nameof(CallMoveSpaceships)));

        Settings.HeaderImageSizeFactor = StarSystem.Size;

        Rarity = StarSystem.Rarity;
    }

    public ResponseStarSystem StarSystem { get; set; }

    public void CallGoTo(Components.Card.Card card)
    {
        card.ImplNavigationManager.NavigateTo($"/star-system/{StarSystem.Id}");
    }

    public async void CallInfo(Components.Card.Card card)
    {

    }

    public async Task CallMoveSpaceships(Components.Card.Card card)
    {
        DialogParameters parameters = new() { [nameof(DialogCreateOrder.Target)] = StarSystem };
        DialogOptions options = DialogOptionTemplates.ExtraLargeDialogOptions();
        await card.ImplDialogService.ShowAsync<DialogCreateOrder>(string.Empty, options: options, parameters: parameters);
    }

    public async Task CallColonize(Components.Card.Card card)
    {
        DialogParameters parameters = new()
        {
            {nameof(DialogCreateOrder.OrderType), OrderType.Colonize},
            {nameof(DialogCreateOrder.Target), StarSystem}
        };

        await card.ImplDialogService.ShowAsync<DialogCreateOrder>("Colonize", parameters: parameters, DialogOptionTemplates.ExtraLargeDialogOptions());
    }
}
