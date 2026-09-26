using OmegaExplorer.Client.Models.Card.Definition;
using OmegaExplorer.Client.Utilities.API;
using OmegaExplorer.Shared.Filters;
using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Models.Card.Dynamic.Drivers;

public class DynamicDriverEvent : IDynamicCardStackDriver
{
    public async Task<ResultPagined> Get(Pagination pagination, DynamicDriverFilter filter)
    {
        ResultPagined result = new ResultPagined();
        List<CardDefinitionEvent> resultDefinitions = new List<CardDefinitionEvent>();
        List<ResponseEvent> resultApi;

        resultApi = await ApiManager.Client.GetAllEventsByConnectedUserAsync();

        foreach (ResponseEvent res in resultApi)
        {
            resultDefinitions.Add(new CardDefinitionEvent(res));
        }

        result.Pagination = pagination;
        result.Data = resultDefinitions;

        return result;
    }
}
