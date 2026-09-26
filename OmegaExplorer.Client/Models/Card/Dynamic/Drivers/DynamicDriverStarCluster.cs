using OmegaExplorer.Client.Models.Card.Definition;
using OmegaExplorer.Client.Utilities.API;
using OmegaExplorer.Server.Extensions;
using OmegaExplorer.Shared.Filters;
using OmegaExplorer.Toolkit.API;
using Serilog;

namespace OmegaExplorer.Client.Models.Card.Dynamic.Drivers;

/// <summary>
///     Get all starcluster or all starcluster by galaxy id
/// </summary>
public class DynamicDriverStarCluster : IDynamicCardStackDriver
{
    public async Task<ResultPagined> Get(Pagination pagination, DynamicDriverFilter filter)
    {
        Guid? filterByGalaxyId = filter.GetFilterById();

        ResultPagined result = new();
        List<CardDefinitionStarCluster> resultDefinitions = new();
        List<ResponseStarCluster>? resultApi = await ApiManager.Client.GetAllStarClustersByGalaxyAsync(filterByGalaxyId);

        foreach (ResponseStarCluster res in resultApi)
        {
            resultDefinitions.Add(new CardDefinitionStarCluster(res));
        }

        result.Pagination = pagination;
        result.Data = resultDefinitions;

        return result;
    }
}
