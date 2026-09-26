using OmegaExplorer.Client.Models.Card.Definition;
using OmegaExplorer.Client.Utilities.API;
using OmegaExplorer.Server.Extensions;
using OmegaExplorer.Shared.Filters;
using OmegaExplorer.Toolkit.API;
using Serilog;

namespace OmegaExplorer.Client.Models.Card.Dynamic.Drivers;

/// <summary>
///     Manage access to the planet element from the server
/// </summary>
public class DynamicDriverGalaxy : IDynamicCardStackDriver
{
    public async Task<ResultPagined> Get(Pagination pagination, DynamicDriverFilter filter)
    {
        Log.Logger.Information($"[{nameof(DynamicDriverGalaxy)}] : loading with {filter} ...");

        Guid? filterByUniverseId = filter.GetFilterById();

        if (filterByUniverseId.IsNullOrEmpty())
        {
            Log.Logger.Error($"[{nameof(DynamicDriverGalaxy)}] : Universe Id is empty, disabling driver ...");
            return null;
        }

        ResultPagined result = new();
        List<CardDefinitionGalaxy> resultDefinitions = new();
        List<ResponseGalaxy>? resultApi = await ApiManager.Client.GetAllGalaxiesByUniverseAsync(filterByUniverseId);

        foreach (ResponseGalaxy res in resultApi)
        {
            resultDefinitions.Add(new CardDefinitionGalaxy(res));
        }

        result.Pagination = pagination;
        result.Data = resultDefinitions;

        Log.Logger.Information($"[{nameof(DynamicDriverGalaxy)}] : loaded with {resultDefinitions.Count} galaxies found in {filterByUniverseId}");

        return result;
    }
}
