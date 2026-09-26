using MudBlazor;
using OmegaExplorer.Client.Models.Card.Dynamic;
using OmegaExplorer.Client.Models.Card.Dynamic.Drivers;
using OmegaExplorer.Client.Utilities.API;
using OmegaExplorer.Client.Utilities.Extensions;
using OmegaExplorer.Client.Utilities.Resources;
using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Models.Card.Definition;

[DynamicDriver(typeof(DynamicDriverBuilding))]
public class CardDefinitionBuilding : CardDefinition
{
    public CardDefinitionBuilding(ResponseBuilding building)
    {
        if (building.Data == null)
        {
            Exception = new Exception("Not able to find building definition for this building");
            return;
        }

        Building = building;
        Data = building.Data;

        Title = Data.Name;
        Subtitle = Data.Description;

        ImageHeader = building.GetImage();
        ImageMode = EnumImageMode.Img;

        foreach (EnumBuildingAction action in Data.Actions)
        {
            Text += action + "\n";
        }

        Rarity = Data.Rarity;

        Actions.Add(new CardAction("Delete", ResourceIcon.DELETE, nameof(CallDelete), Color.Error));
    }

    public ResponseBuilding Building { get; set; }
    public IDynamicItemBuilding? Data { get; set; }

    public async void CallDelete(Components.Card.Card card)
    {
        try
        {
            await ApiManager.Client.DeleteBuildingAsync(Building.Id);
            await card.OnDeleted.InvokeAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            card.ImplSnackbar.Add($"Error while deleting building : {e.Message}", Severity.Error);
        }
        card.ImplSnackbar.Add("Building deleted", Severity.Success);
    }
}