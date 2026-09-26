using OmegaExplorer.Client.Models.Card.Definition;
using OmegaExplorer.Client.Utilities.API;
using OmegaExplorer.Shared.Filters;
using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Models.Card.Dynamic.Drivers;

/// <summary>
///     Manage access to the user element from the server
/// </summary>
public class DynamicDriverBlueprintSpaceship : IDynamicCardStackDriver
{
    public async Task<ResultPagined> Get(Pagination pagination, DynamicDriverFilter filter)
    {
        ResultPagined result = new ResultPagined();
        List<CardDefinitionBlueprintSpaceship> resultDefinitions = new List<CardDefinitionBlueprintSpaceship>();
        List<ResponseBlueprintSpaceship>? resultApi = await ApiManager.Client.GetSpaceshipBlueprintsByUserConnectedAsync();

        foreach (ResponseBlueprintSpaceship responseElement in resultApi)
        {
            resultDefinitions.Add(item: new CardDefinitionBlueprintSpaceship(responseElement));
        }

        result.Pagination = pagination;
        result.Data = resultDefinitions;

        return result;
    }
}
