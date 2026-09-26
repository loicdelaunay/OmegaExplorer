using OmegaExplorer.Client.Models.Card.Definition;
using OmegaExplorer.Client.Utilities.API;
using OmegaExplorer.Shared.Filters;
using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Models.Card.Dynamic.Drivers;

public class DynamicDriverQuestProgress : IDynamicCardStackDriver
{
    public async Task<ResultPagined> Get(Pagination pagination, DynamicDriverFilter filter)
    {
        ResultPagined result = new();
        List<CardDefinitionQuestProgress> resultDefinitions = new();
        List<ResponseQuestProgress> resultApi;

        resultApi = await ApiManager.Client.GetAllQuestsInProgressByUserConnectedAsync();

        foreach (ResponseQuestProgress res in resultApi)
        {
            resultDefinitions.Add(new CardDefinitionQuestProgress(res));
        }

        result.Pagination = pagination;
        result.Data = resultDefinitions;

        return result;
    }
}
