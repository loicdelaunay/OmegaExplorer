using OmegaExplorer.Client.Models.Card.Definition;
using OmegaExplorer.Client.Utilities.API;
using OmegaExplorer.Server.Extensions;
using OmegaExplorer.Server.OmegaExplorer.Shared.Utilities.Log;
using OmegaExplorer.Shared.Filters;
using OmegaExplorer.Toolkit.API;
using Serilog;

namespace OmegaExplorer.Client.Models.Card.Dynamic.Drivers;

/// <summary>
///     Manage access to the planet element from the server
/// </summary>
public class DynamicDriverSpaceship : IDynamicCardStackDriver
{
    public async Task<ResultPagined> Get(Pagination pagination, DynamicDriverFilter? filter)
    {
        Log.Information($"[{nameof(DynamicDriverSpaceship)}] : Getting all spaceships with filter {filter} and pagination {pagination}...");

        ResultPagined result = new();
        List<CardDefinitionSpaceship> resultDefinitions = new();

        List<ResponseSpaceship>? spaceships = await ApiManager.Client.GetFilteredSpaceshipsAsync(filter.JsonSerialize());

        foreach (ResponseSpaceship res in spaceships)
        {
            resultDefinitions.Add(new CardDefinitionSpaceship(res));
        }

        result.Pagination = pagination;
        result.Data = resultDefinitions;

        Log.Logger.Success($"[{nameof(DynamicDriverSpaceship)}] : Getting all spaceships done with {resultDefinitions.Count} spaceships");
        return result;
    }
}
