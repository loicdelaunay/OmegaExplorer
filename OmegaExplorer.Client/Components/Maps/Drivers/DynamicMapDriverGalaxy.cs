using OmegaExplorer.Client.Components.Diagram.Nodes.Models;
using OmegaExplorer.Client.Utilities.API;
using OmegaExplorer.Server.Extensions;
using OmegaExplorer.Toolkit.API;
using Serilog;

namespace OmegaExplorer.Client.Components.Maps.Drivers;

/// <summary>
/// Get all elements from the Galaxy
/// </summary>
public class DynamicMapDriverGalaxy : DynamicMapDriver
{
    public DynamicMapDriverGalaxy()
    {
    }

    public override async Task Load()
    {
        try
        {
            if (Diagram == null)
            {
                Log.Logger.Error("Diagram is null, cannot load Galaxy data.");
                return;
            }

            if (Filter == null)
            {
                Log.Logger.Error("Filter is null, cannot load Galaxy data.");
                return;
            }

            Diagram.Nodes.Clear();

            Log.Logger.Information($"[{nameof(DynamicMapDriverGalaxy)}] : loading with filter {Filter} ...");

            Guid? galaxyId = Filter.GetFilterById();

            if (galaxyId.IsNullOrEmpty())
            {
                Log.Logger.Error($"[{nameof(DynamicMapDriverGalaxy)}] : invalid filter by galaxy id - {galaxyId}");
                Exception = new Exception($"[{nameof(DynamicMapDriverGalaxy)}] : invalid filter by galaxy id - {galaxyId}");

                StateHasChanged();
                return;
            }

            ResponseMapSearch search = new();

            try
            {
                Log.Logger.Information($"[{nameof(DynamicMapDriverGalaxy)}] : requesting search ...");
                search = await ApiManager.Client.SearchByGalaxyAsync(galaxyId);
            }
            catch (Exception e)
            {
                Log.Logger.Error($"[{nameof(DynamicMapDriverGalaxy)}] : not able to search in Galaxy : {e.Message}");
            }

            Log.Logger.Information($"[{nameof(DynamicMapDriverGalaxy)}] : searching done with {search.Galaxies.Count} galaxies found in {galaxyId}");

            foreach (ResponseStarCluster? starCluster in search.StarClusters)
            {
                AddStarCluster(starCluster);
            }

            StateHasChanged();
        }
        catch (Exception e)
        {
            Log.Logger.Error(e, $"[{nameof(DynamicMapDriverGalaxy)}] : not able to load galaxy : {e}");
            throw;
        }
    }

    private void AddStarCluster(ResponseStarCluster starCluster)
    {
        NodeModelMapGalaxySearch newNode = new NodeModelMapGalaxySearch(starCluster);

        Diagram.Nodes.Add(newNode);
    }
}
