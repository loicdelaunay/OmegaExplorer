using Microsoft.AspNetCore.Components;
using OmegaExplorer.Client.Components.Blocks;
using OmegaExplorer.Client.Components.MudExtension;
using OmegaExplorer.Client.Models.Card.Dynamic;
using OmegaExplorer.Client.Models.Card.Dynamic.Drivers;
using Serilog;

namespace OmegaExplorer.Client.Models.Card.Definition;

using Components;
using Dialogs;
using MudBlazor;
using Toolkit.API;
using Utilities.API;
using Utilities.Configuration;
using Utilities.Dialog;
using Utilities.Extensions;
using Utilities.Resources;

[DynamicDriver(typeof(DynamicDriverSpaceship))]
public class CardDefinitionSpaceship : CardDefinition
{
    public CardDefinitionSpaceship(ResponseSpaceship spaceship)
    {
        Spaceship = spaceship;
        Value = spaceship;

        Title = spaceship.Name;
        ComputeSubtitle();

        ImageHeader = GetThumbnailUrl();
        ImageMode = EnumImageMode.Img;

        Rarity = spaceship.Rarity;

        Contents.Add(new CardContent("Size", spaceship.Size.ToString()));
        Contents.Add(new CardContent("Position", Spaceship.SpatialLocation.Position.ToStringFormated()));

        if (Spaceship.SpatialLocation.StarCluster != null)
        {
            Contents.Add(new CardContent("Located", value: Spaceship.SpatialLocation.StarCluster.Name, color: Color.Primary, targetUrl: spaceship.SpatialLocation.StarCluster.GetMapUrl()));
        }
        else if (Spaceship.SpatialLocation.Galaxy != null)
        {
            Contents.Add(new CardContent("Located", Spaceship.SpatialLocation.Galaxy.Name));
        }
        else if (Spaceship.SpatialLocation.Universe != null)
        {
            Contents.Add(new CardContent("Located", Spaceship.SpatialLocation.Universe.Name));
        }

        ComputeIsDestroyed();
        ComputeIsInBattle();

        Actions.Add(new CardAction("See", ResourceIcon.SEE, nameof(CallGoTo)));
        Actions.Add(new CardAction("Info", ResourceIcon.INFO, nameof(CallInfo)));

        if (!Spaceship.IsDestroyed && !Spaceship.IsOwner())
        {
            Actions.Add(new CardAction("Attack", ResourceIcon.ATTACK, nameof(CallAttack)));
        }

        if (Spaceship.IsOwner())
        {
            Actions.Add(new CardAction("Orders", ResourceIcon.ORDERS, nameof(CallOpenOrders)));
            Actions.Add(new CardAction("Move", ResourceIcon.MOVE, nameof(CallMove)));

            if (Spaceship.CanColonize())
            {
                Actions.Add(new CardAction("Colonize", ResourceIcon.COLONIZE, nameof(CallColonize)));
            }

            Actions.Add(new CardAction("Abandon", ResourceIcon.DELETE, nameof(CallDelete), Color.Error));
        }
    }

    private void ComputeIsDestroyed()
    {
        if (Spaceship.DestroyedQuantityDamageToRepair > 0)
        {
            RenderFragment blockProgressFragment = builder =>
            {
                builder.OpenComponent<BlockSpaceshipDestoyed>(0);
                builder.AddAttribute(1, nameof(BlockSpaceshipDestoyed.Spaceship), Spaceship);
                builder.CloseComponent();
            };

            Fragments.Add(blockProgressFragment);
        }
    }

    private void ComputeIsInBattle()
    {
        if (Spaceship.Battle != null)
        {
            var link = Spaceship.Battle.GetUrl();

            Fragments = new List<RenderFragment>
            {
                builder =>
                {
                    builder.OpenComponent<MudSimpleChipWithIcon>(0);
                    builder.AddAttribute(1, nameof(MudSimpleChipWithIcon.Color), Color.Warning);
                    builder.AddAttribute(2, nameof(MudSimpleChipWithIcon.Title), "In Battle");
                    builder.AddAttribute(3, nameof(MudSimpleChipWithIcon.Icon), ResourceIcon.BATTLE);
                    builder.AddAttribute(4, nameof(MudSimpleChipWithIcon.Href), link);
                    builder.CloseComponent();
                }
            };
        }
    }

    private void ComputeSubtitle()
    {
        if (Spaceship.IsOwner())
        {
            Subtitle = "Owned";
        }
        else if (Spaceship.OwnerType == EnumOwnerType.Player)
        {
            Subtitle = $"Owned by {Spaceship.Owner?.Name}";
        }
        else if (Spaceship.OwnerType == EnumOwnerType.Null)
        {
            Subtitle = Spaceship.Faction.ToString();
        }
        else
        {
            Subtitle = $"{Spaceship.OwnerType} {Spaceship.Faction}";
        }
    }

    public ResponseSpaceship Spaceship { get; set; }

    public List<ResponseOrder> Orders { get; set; } = new();

    public override async Task<bool> InitializeAsync(bool needRefresh = false)
    {
        if (Spaceship.IsOwner())
        {
            bool refresh = false;

            // If the spaceship is owned by the user, we can fetch orders
            Orders = await ApiManager.Client.GetOrdersSpaceshipAsync(Spaceship.Id);

            if (Orders.Any())
            {
                ResponseOrder nextOrder = Orders.FirstOrDefault();
                Contents.Add(new CardContent("Order", nextOrder.ToStringHumanized(), nextOrder.TargetRaw));
                refresh = true;
            }

            return await base.InitializeAsync(refresh);
        }

        return await base.InitializeAsync();
    }

    public string GetThumbnailUrl()
    {
        string target = ConfigurationApplication.ConfigurationApi.Url + "/Static/Spaceship/Thumbnail/" + Spaceship.Thumbnail + ".png";
        Console.WriteLine("Target is : " + target);
        return target;
    }

    public void CallGoTo(Components.Card.Card card)
    {
        card.ImplNavigationManager.NavigateTo($"/spaceship/{Spaceship.Id}");
    }

    public async Task CallOpenOrders(Components.Card.Card card)
    {
        DialogParameters parameters = new()
        {
            {nameof(DialogOrders.Spaceship), Spaceship}
        };

        await card.ImplDialogService.ShowAsync<DialogOrders>("Orders", parameters: parameters, DialogOptionTemplates.DefaultDialogOptions());
    }

    public async void CallInfo(Components.Card.Card card)
    {
    }

    public async Task CallDelete(Components.Card.Card card)
    {
        await ApiManager.Client.DeleteSpaceshipAsync(Spaceship.Id);
        card.ImplSnackbar.Add($"Abandon spaceship {Spaceship.Name}", Severity.Success);
        card.CallDelete();
    }

    public async Task CallMove(Components.Card.Card card)
    {
        DialogParameters parameters = new()
        {
            {nameof(DialogCreateOrder.OrderType), OrderType.Move},
            {nameof(DialogCreateOrder.Actor), Spaceship}
        };

        await card.ImplDialogService.ShowAsync<DialogCreateOrder>("Move", parameters: parameters, DialogOptionTemplates.ExtraLargeDialogOptions());
    }

    public async Task CallColonize(Components.Card.Card card)
    {
        DialogParameters parameters = new()
        {
            {nameof(DialogCreateOrder.OrderType), OrderType.Colonize},
            {nameof(DialogCreateOrder.Actor), Spaceship}
        };

        await card.ImplDialogService.ShowAsync<DialogCreateOrder>("Colonize", parameters: parameters, DialogOptionTemplates.ExtraLargeDialogOptions());
    }

    public async Task CallAttack(Components.Card.Card card)
    {
        DialogParameters parameters = new()
        {
            {nameof(DialogCreateOrder.OrderType), OrderType.Attack},
            {nameof(DialogCreateOrder.Target), Spaceship}
        };

        await card.ImplDialogService.ShowAsync<DialogCreateOrder>("Attack", parameters: parameters, DialogOptionTemplates.ExtraLargeDialogOptions());

        return;
    }
}
