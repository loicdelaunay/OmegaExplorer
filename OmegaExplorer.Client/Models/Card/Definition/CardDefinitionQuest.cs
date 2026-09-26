using MudBlazor;
using OmegaExplorer.Client.Models.Card.Dynamic;
using OmegaExplorer.Client.Models.Card.Dynamic.Drivers;
using OmegaExplorer.Client.Utilities.API;
using OmegaExplorer.Client.Utilities.Resources;
using OmegaExplorer.Toolkit.API;
using Serilog;

namespace OmegaExplorer.Client.Models.Card.Definition;

[DynamicDriver(typeof(DynamicDriverQuestAvailable))]
public class CardDefinitionQuest : CardDefinition
{
    public CardDefinitionQuest(IDynamicItemQuest quest)
    {
        Quest = quest;
        Value = quest;

        Title = quest.Name;
        Subtitle = quest.Description;

        ComputeObjectives();

        Rarity = quest.Rarity;

        ImageHeader = ResourceImage.QUEST;

        Actions.Add(new CardAction("Select Quest", ResourceIcon.SELECT, nameof(SelectQuest)));

    }

    private void ComputeObjectives()
    {
        string res = string.Empty;

        foreach (QuestObjective? objective in Quest.Objectives)
        {
            res += "&nbsp;";
            res += $"{objective.Type.ToString()} {objective.Amount} {objective.Faction} {objective.OwnerType}";
        }

        Text = res;
    }

    public IDynamicItemQuest Quest { get; set; }

    public async Task SelectQuest(Components.Card.Card card)
    {
        try
        {
            ResponseQuestProgress? res = await ApiManager.Client.AcceptQuestAsync(Quest.Index);
            card.CallCardClicked();
            card.ImplSnackbar.Add("Quest selected", Severity.Success);
        }
        catch (Exception e)
        {
            card.ImplSnackbar.Add("Not able to select quest", Severity.Error);
            Log.Logger.Error(e, "Error while selecting quest");
        }
    }
}