using MudBlazor;
using OmegaExplorer.Client.Dialogs.Update;
using OmegaExplorer.Client.Models.Card.Dynamic;
using OmegaExplorer.Client.Models.Card.Dynamic.Drivers;
using OmegaExplorer.Client.Utilities.Resources;
using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Models.Card.Definition;

[DynamicDriver(typeof(DynamicDriverUser))]
public class CardDefinitionUser : CardDefinition
{
    public CardDefinitionUser(ResponseUser user, bool adminView = false)
    {
        User = user;
        Value = user;

        Title = user.Name;
        Subtitle = string.Empty;
        ImageHeader = ResourceImage.USER;
        AdminView = adminView;

        Contents.Add(new CardContent("Email", User.Email));
        Contents.Add(new CardContent("Name", User.Name));
        Contents.Add(new CardContent("Role", User.AccessLevel.ToString()));
        Contents.Add(new CardContent("Disabled", User.Disabled.ToString()));

        if (AdminView)
        {
            Actions.Add(new CardAction("Edit password", ResourceIcon.PASSWORD, nameof(CallUpdateUserPassword)));
            Actions.Add(new CardAction("Edit values", ResourceIcon.EDIT, nameof(CallUpdateUser)));
        }
    }

    public bool AdminView { get; set; }

    public ResponseUser User { get; set; }

    public async void CallUpdateUserPassword(Components.Card.Card card)
    {
        if (card.ImplDialogService == null)
        {
            Console.WriteLine("Card implementation dialog service is null :( ?");
            return;
        }

        DialogOptions options = new() { CloseOnEscapeKey = true };
        DialogParameters parameters = new() { [nameof(DialogUpdateUserPassword.User)] = User };
        IDialogReference dialog = await card.ImplDialogService.ShowAsync<DialogUpdateUserPassword>("Update user password", parameters, options);
        DialogResult? result = await dialog.Result;
    }

    public async void CallUpdateUser(Components.Card.Card card)
    {
        if (card.ImplDialogService == null)
        {
            Console.WriteLine("Card implementation dialog service is null :( ?");
            return;
        }

        DialogOptions options = new() { CloseOnEscapeKey = true };
        DialogParameters parameters = new() { [nameof(DialogUpdateUser.userToEditId)] = User.Id };
        IDialogReference dialog = await card.ImplDialogService.ShowAsync<DialogUpdateUser>("Update user", parameters, options);
        DialogResult? result = await dialog.Result;
    }
}