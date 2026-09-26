using OmegaExplorer.Client.Components.Diagram.Nodes.Models;
using OmegaExplorer.Client.Utilities.API;
using OmegaExplorer.Client.Utilities.Extensions;
using OmegaExplorer.Toolkit.API;
using Serilog;

namespace OmegaExplorer.Client.Components.Maps.Drivers;

/// <summary>
/// Get all element into a specifi star cluser id
/// </summary>
public class DynamicMapDriverStarCluster : DynamicMapDriver
{
    public override async Task Load()
    {
        try
        {
            Diagram.Nodes.Clear();

            Log.Logger.Information("Loading Solar System map driver ...");

            Guid? starClusterId = Filter.GetFilterById();

            if (starClusterId == Guid.Empty)
            {
                Log.Logger.Error($"[{nameof(DynamicMapDriverGalaxy)}] : invalid filter by star cluster id - {starClusterId}");
                Exception = new Exception($"[{nameof(DynamicMapDriverGalaxy)}] : invalid filter by star cluster id - {starClusterId}");

                StateHasChanged();
                return;
            }

            var search = new ResponseMapSearch();
            try
            {
                Log.Logger.Information($"[{nameof(DynamicMapDriverStarCluster)}] : requesting search ...");
                search = await ApiManager.Client.SearchByStarClusterAsync(starClusterId);
            }
            catch (Exception e)
            {
                Log.Logger.Information($"[{nameof(DynamicMapDriverStarCluster)}] : not able to search in Solar System : {e.Message}");
            }

            Log.Logger.Information($"[{nameof(DynamicMapDriverStarCluster)}] : searching done with {search.StarSystems.Count} planets and {search.Spaceships.Count} spaceships for {starClusterId}");

            Dictionary<ResponseVector2, (ResponseStarSystem? Planet, List<ResponseSpaceship> Spaceships)>
                groupsByPosition = search.GetGroupedByPosition();

            // Add each group to AddElementAtPosition
            foreach (var groupByPosition in groupsByPosition)
            {
                AddElementAtPosition(groupByPosition.Value.Planet, groupByPosition.Value.Spaceships);
            }

            StateHasChanged();
        }
        catch (Exception e)
        {
            Log.Logger.Information($"[{nameof(DynamicMapDriverStarCluster)}] : error when loading Solar System map driver : " + e);
            throw;
        }
    }

    public void AddElementAtPosition(ResponseStarSystem? system, List<ResponseSpaceship> spaceships)
    {
        NodeModelMapStarClusterSearch newNode = new(system, spaceships);

        Diagram.Nodes.Add(newNode);
    }
}
