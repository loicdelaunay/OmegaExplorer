using OmegaExplorer.Client.Models.Card.Definition;
using OmegaExplorer.Client.Utilities.API;
using OmegaExplorer.Shared.Filters;
using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Models.Card.Dynamic.Drivers;

/// <summary>
///     Manage access to the user element from the server
/// </summary>
public class DynamicDriverSpaceshipModuleData : IDynamicCardStackDriver
{
    public async Task<ResultPagined> Get(Pagination pagination, DynamicDriverFilter filter)
    {
        ResultPagined result = new();
        List<CardDefinitionSpaceshipModuleData> resultDefinitions = new();
        List<ResponseDynamicItemSpaceshipModule>? resultApi = await ApiManager.Client.GetAllSpaceshipModuleDataByUserAsync();

        foreach (ResponseDynamicItemSpaceshipModule responseElement in resultApi)
        {
            resultDefinitions.Add(item: new CardDefinitionSpaceshipModuleData(responseElement));
        }

        result.Pagination = pagination;
        result.Data = resultDefinitions;

        return result;
    }
}
