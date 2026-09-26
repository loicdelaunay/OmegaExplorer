using OmegaExplorer.Client.Models.Card.Definition;
using OmegaExplorer.Client.Utilities.API;
using OmegaExplorer.Shared.Filters;
using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Models.Card.Dynamic.Drivers;

/// <summary>
///     Manage access to the planet element from the server
/// </summary>
public class DynamicDriverBuilding : IDynamicCardStackDriver
{
    public async Task<ResultPagined> Get(Pagination pagination, DynamicDriverFilter filter)
    {
        DynamicDriverBuildingFilter? filterCasted = filter as DynamicDriverBuildingFilter;
        Guid? filterByStarSystemId = filterCasted?.GetFilterByStarSystemId();

        ResultPagined result = new ResultPagined();
        List<CardDefinitionBuilding> resultDefinitions = new List<CardDefinitionBuilding>();
        List<ResponseBuilding> items;

        items = await ApiManager.Client.GetAllBuildingsBySystemAsync(filter.GetFilterById());


        foreach (ResponseBuilding item in items)
        {
            resultDefinitions.Add(new CardDefinitionBuilding(item));
        }

        result.Pagination = pagination;
        result.Data = resultDefinitions;

        return result;
    }
}
