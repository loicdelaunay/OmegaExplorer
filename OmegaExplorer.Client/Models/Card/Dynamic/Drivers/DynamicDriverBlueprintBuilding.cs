using OmegaExplorer.Client.Models.Card.Definition;
using OmegaExplorer.Client.Utilities.API;
using OmegaExplorer.Shared.Filters;
using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Models.Card.Dynamic.Drivers;

/// <summary>
///     Manage access to the planet element from the server
/// </summary>
public class DynamicDriverBlueprintBuilding : IDynamicCardStackDriver
{
    public async Task<ResultPagined> Get(Pagination pagination, DynamicDriverFilter filter)
    {
        ResultPagined result = new ResultPagined();
        List<CardDefinitionBlueprintBuilding> resultDefinitions = new List<CardDefinitionBlueprintBuilding>();
        List<IDynamicItemBuilding> items;

        items = await ApiManager.Client.GetAllBuildingsDataAsync();


        foreach (IDynamicItemBuilding item in items)
        {
            resultDefinitions.Add(new CardDefinitionBlueprintBuilding(item));
        }

        result.Pagination = pagination;
        result.Data = resultDefinitions;

        return result;
    }
}
