using MudBlazor;
using OmegaExplorer.Client.Dialogs;
using OmegaExplorer.Client.Models.Card.Dynamic;
using OmegaExplorer.Client.Models.Card.Dynamic.Drivers;
using OmegaExplorer.Client.Utilities.Dialog;
using OmegaExplorer.Client.Utilities.Resources;
using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Models.Card.Definition;

[DynamicDriver(typeof(DynamicDriverEvent))]
public class CardDefinitionEvent : CardDefinition
{
    public CardDefinitionEvent(ResponseEvent @event)
    {
        Event = @event;

        if (Event.Data == null)
        {
            Exception = new Exception($"Event data is null for {Event.EventItemIndex}");
            return;
        }

        Title = Event.Data.Name;
        Subtitle = Event.Data.Description;
        Rarity = Event.Data.Rarity;

        Text = Event.Data.Text;

        ImageHeader = ResourceImage.USER;

        Actions.Add(new CardAction("Open event", ResourceIcon.ACTIONS, nameof(OpenEvent)));
    }
    public ResponseEvent Event { get; set; }

    public async Task OpenEvent(Components.Card.Card card)
    {
        DialogOptions options = DialogOptionTemplates.DefaultDialogOptions();
        DialogParameters parameters = new() { [nameof(DialogEvent.Event)] = Event };
        IDialogReference dialog = await card.ImplDialogService.ShowAsync<DialogEvent>("Update user", parameters, options);

        DialogResult? result = await dialog.Result;

        if (result.Canceled)
        {
            return;
        }

        card.CallDelete();
    }
}