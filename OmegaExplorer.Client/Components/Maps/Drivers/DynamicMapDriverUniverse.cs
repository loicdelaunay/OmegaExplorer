using OmegaExplorer.Client.Components.Diagram.Nodes.Models;
using OmegaExplorer.Client.Utilities.API;
using OmegaExplorer.Server.Extensions;
using OmegaExplorer.Toolkit.API;
using Serilog;

namespace OmegaExplorer.Client.Components.Maps.Drivers;

/// <summary>
/// Get all elements into a specific Universe id
/// </summary>
public class DynamicMapDriverUniverse : DynamicMapDriver
{
    public DynamicMapDriverUniverse()
    {
    }

    public override async Task Load()
    {
        try
        {
            Diagram.Nodes.Clear();

            Log.Logger.Information($"[{nameof(DynamicMapDriverUniverse)}] : loading with filter {Filter} ...");

            Guid? universeId = Filter.GetFilterById();

            if (universeId.IsNullOrEmpty())
            {
                Log.Logger.Error($"[{nameof(DynamicMapDriverUniverse)}] : invalid filter by universe id - {universeId}");
                Exception = new Exception($"[{nameof(DynamicMapDriverUniverse)}] : invalid filter by universe id - {universeId}");

                StateHasChanged();
                return;
            }

            var search = new ResponseMapSearch();

            try
            {
                Log.Logger.Information($"[{nameof(DynamicMapDriverUniverse)}] : requesting search ...");
                search = await ApiManager.Client.SearchByUniverseAsync(universeId);
            }
            catch (Exception e)
            {
                Log.Logger.Error($"[{nameof(DynamicMapDriverUniverse)}] : not able to search in Universe : {e.Message}");
            }

            Log.Logger.Information($"[{nameof(DynamicMapDriverUniverse)}] : searching done with {search.Galaxies.Count} galaxies found in {universeId}");

            foreach (ResponseGalaxy? galaxy in search.Galaxies)
            {
                AddGalaxy(galaxy);
            }

            StateHasChanged();
        }
        catch (Exception e)
        {
            Log.Logger.Error(e, $"[{nameof(DynamicMapDriverUniverse)}] : not able to load universe : {e.Message}");
            throw;
        }
    }

    private void AddGalaxy(ResponseGalaxy galaxy)
    {
        NodeModelMapUniverseSearch newNode = new(galaxy);

        Diagram.Nodes.Add(newNode);
    }
}