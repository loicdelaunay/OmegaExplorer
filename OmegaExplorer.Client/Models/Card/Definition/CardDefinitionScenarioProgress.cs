using Microsoft.AspNetCore.Components;
using MudBlazor;
using OmegaExplorer.Client.Components.Blocks;
using OmegaExplorer.Client.Dialogs;
using OmegaExplorer.Client.Models.Card.Dynamic;
using OmegaExplorer.Client.Models.Card.Dynamic.Drivers;
using OmegaExplorer.Client.Utilities.Dialog;
using OmegaExplorer.Client.Utilities.Resources;
using OmegaExplorer.Toolkit.API;
using Serilog;

namespace OmegaExplorer.Client.Models.Card.Definition;

[DynamicDriver(typeof(DynamicDriverScenarioProgress))]
public class CardDefinitionScenarioProgress : CardDefinition
{
    public CardDefinitionScenarioProgress(ResponseUserScenarioProgress scenarioProgress)
    {
        ScenarioProgress = scenarioProgress;

        Title = ScenarioProgress.ScenarioData.Name;
        Subtitle = ScenarioProgress.ScenarioData.Description;
        Rarity = ScenarioProgress.ScenarioData.Rarity;

        ImageHeader = ResourceImage.MESSAGE;

        Text = ScenarioProgress.ScenarioData.Dialogs.FirstOrDefault()?.Message;

        if (ScenarioProgress.IsResolved)
        {
            ShowAsResolved();
        }

        Actions.Add(new CardAction("Open message", ResourceIcon.ACTIONS, nameof(OpenScenario)));
    }

    private void ShowAsResolved()
    {
        RenderFragment blockProgressFragment = builder =>
        {
            builder.OpenComponent<BlockIsResolved>(0);
            builder.CloseComponent();
        };

        Fragments.Add(blockProgressFragment);
    }

    public ResponseUserScenarioProgress ScenarioProgress { get; set; }

    public async Task OpenScenario(Components.Card.Card card)
    {
        DialogOptions options = DialogOptionTemplates.DefaultDialogOptions();
        DialogParameters parameters = new() { [nameof(DialogScenario.ScenarioProgress)] = ScenarioProgress };
        IDialogReference dialog = await card.ImplDialogService.ShowAsync<DialogScenario>("Scenario", parameters, options);

        DialogResult? result = await dialog.Result;

        if (result.Canceled)
        {
            return;
        }

        Log.Logger.Warning($"[DEBUG] : scenario {ScenarioProgress.ScenarioData.Name} completed");
        card.CallUpdateCardsStack();
    }
}
