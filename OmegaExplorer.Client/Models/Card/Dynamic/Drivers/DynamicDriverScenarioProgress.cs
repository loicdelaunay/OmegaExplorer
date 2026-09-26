using OmegaExplorer.Client.Models.Card.Definition;
using OmegaExplorer.Client.Utilities.API;
using OmegaExplorer.Shared.Filters;
using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Models.Card.Dynamic.Drivers;

public class DynamicDriverScenarioProgress : IDynamicCardStackDriver
{
    public async Task<ResultPagined> Get(Pagination pagination, DynamicDriverFilter filter)
    {
        DynamicDriverScenarioProgressFilter filterCasted = filter as DynamicDriverScenarioProgressFilter;

        ResultPagined result = new();
        List<CardDefinitionScenarioProgress> resultDefinitions = new();
        List<ResponseUserScenarioProgress> resultApi;

        resultApi = await ApiManager.Client.GetAllScenariosByConnectedUserAsync();

        if (filterCasted != null && (bool)filterCasted.GetFilterByFinished())
        {
            resultApi = resultApi.Where(x => x.IsResolved).ToList();
        }
        else
        {
            resultApi = resultApi.Where(x => !x.IsResolved).ToList();
        }

        foreach (ResponseUserScenarioProgress res in resultApi)
        {
            resultDefinitions.Add(new CardDefinitionScenarioProgress(res));
        }

        result.Pagination = pagination;
        result.Data = resultDefinitions;

        return result;
    }
}
