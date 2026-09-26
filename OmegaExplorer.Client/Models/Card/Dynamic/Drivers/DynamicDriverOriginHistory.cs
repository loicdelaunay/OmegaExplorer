using OmegaExplorer.Client.Models.Card.Definition;
using OmegaExplorer.Client.Utilities.API;
using OmegaExplorer.Shared.Filters;
using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Models.Card.Dynamic.Drivers;

/// <summary>
///     Manage access to the user element from the server
/// </summary>
public class DynamicDriverOriginHistory : IDynamicCardStackDriver
{
    public async Task<ResultPagined> Get(Pagination pagination, DynamicDriverFilter filter)
    {
        ResultPagined result = new();
        List<CardDefinitionOriginHistory> resultDefinitions = new();
        List<IDynamicItemOriginHistory>? resultApi = await ApiManager.Client.GetAllOriginHistoriesAsync();

        foreach (IDynamicItemOriginHistory responseElement in resultApi)
        {
            resultDefinitions.Add(item: new CardDefinitionOriginHistory(responseElement));
        }

        result.Pagination = pagination;
        result.Data = resultDefinitions;

        return result;
    }
}
