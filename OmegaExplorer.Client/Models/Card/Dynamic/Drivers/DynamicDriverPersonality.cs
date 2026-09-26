using OmegaExplorer.Client.Models.Card.Definition;
using OmegaExplorer.Client.Utilities.API;
using OmegaExplorer.Shared.Filters;
using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Models.Card.Dynamic.Drivers;

/// <summary>
///     Manage access to the user element from the server
/// </summary>
public class DynamicDriverPersonality : IDynamicCardStackDriver
{
    public async Task<ResultPagined> Get(Pagination pagination, DynamicDriverFilter filter)
    {
        ResultPagined result = new();
        List<CardDefinitionPersonality> resultDefinitions = new();
        List<ResponsePersonality>? resultApi = await ApiManager.Client.GetAllPersonalitiesAsync();

        foreach (ResponsePersonality responseElement in resultApi)
        {
            resultDefinitions.Add(item: new CardDefinitionPersonality(responseElement));
        }

        result.Pagination = pagination;
        result.Data = resultDefinitions;

        return result;
    }
}
