using OmegaExplorer.Client.Models.Card.Definition;
using OmegaExplorer.Client.Utilities.API;
using OmegaExplorer.Shared.Filters;
using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Models.Card.Dynamic.Drivers;

public class DynamicDriverBattle : IDynamicCardStackDriver
{
    public async Task<ResultPagined> Get(Pagination pagination, DynamicDriverFilter filter)
    {
        ResultPagined result = new ResultPagined();
        List<CardDefinitionBattle> resultDefinitions = new List<CardDefinitionBattle>();
        List<ResponseBattle> resultApi;

        resultApi = await ApiManager.Client.GetAllBattlesByConnectedAsync();

        foreach (ResponseBattle res in resultApi)
        {
            resultDefinitions.Add(new CardDefinitionBattle(res));
        }

        result.Pagination = pagination;
        result.Data = resultDefinitions;

        return result;
    }
}
