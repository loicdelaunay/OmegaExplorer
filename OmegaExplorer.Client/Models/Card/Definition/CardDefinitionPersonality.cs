using MudBlazor;
using OmegaExplorer.Client.Models.Card.Dynamic;
using OmegaExplorer.Client.Models.Card.Dynamic.Drivers;
using OmegaExplorer.Client.Utilities.API;
using OmegaExplorer.Client.Utilities.Resources;
using OmegaExplorer.Shared.Extensions;
using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Models.Card.Definition;

[DynamicDriver(typeof(DynamicDriverPersonality))]
public class CardDefinitionPersonality : CardDefinition
{
    public CardDefinitionPersonality(ResponsePersonality personality)
    {
        Personality = personality;

        Title = Personality.Name;
        Subtitle = string.Empty;
        ImageHeader = ResourceImage.GetImageSpeciesPortrait(0, index: personality.PortraitIndex);
        Rarity = Personality.Rarity;

        Contents.Add(new CardContent("Age", Personality.Age.ToString()));

        Actions.Add(new CardAction("Delete", ResourceIcon.DELETE, nameof(CallDelete), Color.Error));
        Actions.Add(new CardAction("Add virtual XP", ResourceIcon.EXPERIENCE, nameof(CallAddVirtualXP), Color.Warning));

        UpdateProgress();
    }

    private void UpdateProgress()
    {
        Progress = new Progress
        {
            Value = Personality.Experience.GetExperienceCurrentLevel(),
            MaxValue = Personality.Experience.GetExperienceTotalToNextLevel(),
            Title = $"Level {Personality.Experience.GetLevel()}"
        };
    }

    public bool AdminView { get; set; }

    public ResponsePersonality Personality { get; set; }

    public async void CallDelete(Components.Card.Card card)
    {
        try
        {
            await ApiManager.Client.DeletePersonalityAsync(Personality.Id);
            card.OnDeleted.InvokeAsync();
            card.ImplSnackbar.Add("Personality deleted", Severity.Success);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            card.ImplSnackbar.Add($"Error while deleting personality : {e.Message}", Severity.Error);
        }
    }

    public async void CallAddVirtualXP(Components.Card.Card card)
    {
        try
        {
            Personality.Experience += 50;
            card.ImplSnackbar.Add("50 XP added", Severity.Success);

            UpdateProgress();
            card.ForceStateHasChanged();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            card.ImplSnackbar.Add($"Error while adding XP : {e.Message}", Severity.Error);
        }
    }
}