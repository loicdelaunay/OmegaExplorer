using OmegaExplorer.Client.Models.Card.Definition;
using OmegaExplorer.Client.Utilities.API;
using OmegaExplorer.Shared.Filters;
using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Models.Card.Dynamic.Drivers;

/// <summary>
///     Manage access to the contact element from the server
/// </summary>
public class DynamicDriverContact : IDynamicCardStackDriver
{
    public async Task<ResultPagined> Get(Pagination pagination, DynamicDriverFilter filter)
    {
        ResultPagined result = new ResultPagined();
        List<CardDefinitionUser> resultDefinitions = new List<CardDefinitionUser>();
        List<ResponseUser>? resultApi = await ApiManager.Client.GetUserContactsAsync();

        foreach (ResponseUser responseElement in resultApi)
        {
            resultDefinitions.Add(item: new CardDefinitionUser(user: responseElement));
        }

        result.Pagination = pagination;
        result.Data = resultDefinitions;

        return result;
    }
}
