using OmegaExplorer.Client.Models.Card.Definition;
using OmegaExplorer.Client.Utilities.API;
using OmegaExplorer.Shared.Filters;
using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Models.Card.Dynamic.Drivers;

public class DynamicDriverQuestAvailable : IDynamicCardStackDriver
{
    public async Task<ResultPagined> Get(Pagination pagination, DynamicDriverFilter filter)
    {
        ResultPagined result = new();
        List<CardDefinitionQuest> resultDefinitions = new();
        List<IDynamicItemQuest> resultApi;

        resultApi = await ApiManager.Client.GetAllQuestsAvailableByUserConnectedAsync();

        foreach (IDynamicItemQuest res in resultApi)
        {
            resultDefinitions.Add(new CardDefinitionQuest(res));
        }

        result.Pagination = pagination;
        result.Data = resultDefinitions;

        return result;
    }
}
