using Microsoft.AspNetCore.Components;
using OmegaExplorer.Client.Components.Blocks.Battle;
using OmegaExplorer.Client.Models.Card.Dynamic;
using OmegaExplorer.Client.Models.Card.Dynamic.Drivers;
using OmegaExplorer.Client.Utilities.Resources;
using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Models.Card.Definition;

[DynamicDriver(typeof(DynamicDriverBattle))]
public class CardDefinitionBattle : CardDefinition
{
    public ResponseBattle Battle { get; set; }

    public CardDefinitionBattle(ResponseBattle battle)
    {
        Battle = battle;
        Value = battle;

        Title = Battle.Name;
        Subtitle = "Battle";

        ImageHeader = ResourceImage.FIGHT;
        ImageMode = EnumImageMode.Img;

        if (Battle.Result != null)
        {
            Fragments = new List<RenderFragment>
            {
                builder =>
                {
                    builder.OpenComponent<BlockBattleChipResult>(0);
                    builder.AddAttribute(1, nameof(BlockBattleChipResult.BattleResult), Battle.Result);
                    builder.CloseComponent();
                }
            };

            Actions.Add(new CardAction("See", ResourceIcon.SEE, nameof(CallGoToBattleResult)));
        }
        else
        {
            Actions.Add(new CardAction("See", ResourceIcon.SEE, nameof(CallGoToBattle)));
        }
    }

    public void CallGoToBattle(Components.Card.Card card)
    {
        card.ImplNavigationManager.NavigateTo($"/battle/{Battle.Id}");
    }

    public void CallGoToBattleResult(Components.Card.Card card)
    {
        card.ImplNavigationManager.NavigateTo($"/battle/result/{Battle.Id}");
    }
}
