using OmegaExplorer.Client.Models.Card.Definition;
using OmegaExplorer.Client.Utilities.API;
using OmegaExplorer.Shared.Filters;
using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Models.Card.Dynamic.Drivers;

/// <summary>
///     Manage access to the systems element from the server
/// </summary>
public class DynamicDriverStarSystem : IDynamicCardStackDriver
{
    public async Task<ResultPagined> Get(Pagination pagination, DynamicDriverFilter? filter)
    {
        Guid? filterByStarClusterId = filter?.GetFilterById();
        DynamicDriverStarSystemFilter? advancedFilter = filter as DynamicDriverStarSystemFilter;

        ResultPagined result = new();
        List<CardDefinitionStarSystem> resultDefinitions = new();
        List<ResponseStarSystem>? resultApi = new();

        //Get all star systems in star cluster
        if (filterByStarClusterId != null)
        {
            resultApi = await ApiManager.Client.GetAllStarSystemsByStarClusterAsync(filterByStarClusterId);
        }
        //Get all star systems visible to the user
        else if (advancedFilter is { IsGetAllStarSystems: true })
        {
            resultApi = await ApiManager.Client.GetAllStarSystemsAsync();
        }
        //Get all star systems owned by the user
        else
        {
            resultApi = await ApiManager.Client.GetAllStarSystemsOwnedByUserAsync();
        }

        foreach (ResponseStarSystem responsePlanet in resultApi)
        {
            resultDefinitions.Add(new CardDefinitionStarSystem(responsePlanet));
        }

        result.Pagination = pagination;
        result.Data = resultDefinitions;

        return result;
    }
}
