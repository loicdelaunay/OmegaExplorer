using Microsoft.AspNetCore.Components;
using MudBlazor;
using OmegaExplorer.Client.Components;
using OmegaExplorer.Client.Components.Blocks;
using OmegaExplorer.Client.Models.Card.Dynamic;
using OmegaExplorer.Client.Models.Card.Dynamic.Drivers;
using OmegaExplorer.Client.Utilities.API;
using OmegaExplorer.Client.Utilities.Resources;
using OmegaExplorer.Toolkit.API;
using Serilog;

namespace OmegaExplorer.Client.Models.Card.Definition;

[DynamicDriver(typeof(DynamicDriverQuestProgress))]
public class CardDefinitionQuestProgress : CardDefinition
{
    public CardDefinitionQuestProgress(ResponseQuestProgress questProgress)
    {
        if (questProgress.QuestData == null)
        {
            Exception = new Exception("No data for quest progress");
            return;
        }

        QuestProgress = questProgress;
        Quest = questProgress.QuestData;

        Title = Quest.Name;
        Subtitle = Quest.Description;

        AddSubComponents();

        ImageHeader = ResourceImage.QUEST;

        Progress = new Progress
        {
            Title = "Completion",
            Value = GetProgressCompletion(),
            MaxValue = 100,
        };

        SubtitleMultiline = true;

        if (GetProgressCompletion() == 100)
        {
            Actions.Add(new CardAction(title: "Claim Reward", icon: ResourceIcon.REWARD, color: Color.Success, function: nameof(ClaimReward)));
        }

        Actions.Add(new CardAction(title: "Cancel Quest", icon: ResourceIcon.DELETE, color: Color.Error, function: nameof(CancelQuest)));
    }

    /// <summary>
    /// Get the % of completion of the quest
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private int GetProgressCompletion()
    {
        int total = Quest.Objectives.Sum(objective => objective.Amount);
        int progress = QuestProgress.QuestObjectiveProgresses.Sum(objective => objective.Amount);

        int completion = (int)((float)progress / total * 100);
        return completion;
    }

    /// <summary>
    /// Add the subcomponents to the card objectives and modifiers
    /// </summary>
    private void AddSubComponents()
    {
        string res = string.Empty;

        foreach (QuestObjective? objective in Quest.Objectives)
        {
            ResponseQuestObjectiveProgress? progress = QuestProgress.QuestObjectiveProgresses.FirstOrDefault(x => x.Index == objective.Index);

            // Example: Creation of a fragment that makes the block progress component with a given value
            RenderFragment blockProgressFragment = builder =>
            {
                builder.OpenComponent<BlockQuestObjectiveProgress>(0);
                builder.AddAttribute(1, nameof(BlockQuestObjectiveProgress.Objective), objective);
                builder.AddAttribute(2, nameof(BlockQuestObjectiveProgress.Progress), progress);
                builder.CloseComponent();
            };

            // This fragment is added to the list of card content
            Fragments.Add(blockProgressFragment);
        }

        foreach (ResponseModifierResource? oneTimeModifier in Quest.OneTimeModifiers)
        {
            // Example: Creation of a fragment that makes the block progress component with a given value
            RenderFragment blockProgressFragment = builder =>
            {
                builder.OpenComponent<BlockModifier>(0);
                builder.AddAttribute(1, nameof(BlockModifier.Modifier), oneTimeModifier);
                builder.CloseComponent();
            };

            // This fragment is added to the list of card content
            Fragments.Add(blockProgressFragment);
        }

        Text = res;
    }

    public ResponseQuestProgress QuestProgress { get; set; }

    public ResponseDynamicItemQuest Quest { get; set; }

    public async Task CancelQuest(Components.Card.Card card)
    {
        bool confirm = await MainLayout.Instance.DialogTemplates.OpenDialogConfirm(
                                                                                   subject: "Are you sure you want to cancel this quest?",
                                                                                   title: "Cancel Quest"
                                                                                  );

        if (!confirm)
        {
            return;
        }

        try
        {
            await ApiManager.Client.CancelQuestAsync(QuestProgress.Id);
            card.CallDelete();
            card.ImplSnackbar.Add("Quest canceled", Severity.Success);
        }
        catch (Exception e)
        {
            card.ImplSnackbar.Add("Not able to select quest", Severity.Error);
            Log.Logger.Error(e, "Error while selecting quest");
        }
    }

    public async Task ClaimReward(Components.Card.Card card)
    {
        try
        {
            await ApiManager.Client.CompleteQuestAsync(QuestProgress.Id);
            MainLayout.Instance.CallUpdateResources();
            card.CallDelete();
            card.ImplSnackbar.Add("Quest reward claimed", Severity.Success);
        }
        catch (Exception e)
        {
            card.ImplSnackbar.Add("Not able to claim reward", Severity.Error);
            Log.Logger.Error(e, "Error while claiming reward");
        }
    }
}