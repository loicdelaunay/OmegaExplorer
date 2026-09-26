using OmegaExplorer.Client.Models.Card.Definition;
using OmegaExplorer.Client.Utilities.API;
using OmegaExplorer.Shared.Filters;
using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Models.Card.Dynamic.Drivers;

public class DynamicDriverIDynamicItemScenario : IDynamicCardStackDriver
{
    public async Task<ResultPagined> Get(Pagination pagination, DynamicDriverFilter filter)
    {
        ResultPagined result = new();
        List<CardDefinitionIDynamicItemScenario> resultDefinitions = new();
        List<IDynamicItemScenario> resultApi;

        resultApi = await ApiManager.Client.GetAllDataScenarioAsAdminAsync();

        foreach (IDynamicItemScenario res in resultApi)
        {
            resultDefinitions.Add(new CardDefinitionIDynamicItemScenario(res));
        }

        result.Pagination = pagination;
        result.Data = resultDefinitions;

        return result;
    }
}
