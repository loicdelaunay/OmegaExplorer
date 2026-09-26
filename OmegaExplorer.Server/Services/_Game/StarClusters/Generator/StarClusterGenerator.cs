using OmegaExplorer.Server.Extensions;
using OmegaExplorer.Server.OmegaExplorer.Shared.Utilities.Log;
using OmegaExplorer.Server.Services._Game._core.Models;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Galaxies.Models.Entities;
using OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Classes;
using OmegaExplorer.Server.Services._Game.StarClusters.Models.Entities;
using OmegaExplorer.Server.Services.ProjectFiles;

namespace OmegaExplorer.Server.Services._Game.StarClusters.Generator;

public static class StarClusterGenerator
{
    public static NamingContent? NamingContent { get; private set; }

    public static bool IsInitialized { get; set; }

    public static void Initialize()
    {
        Log.Logger.Information($"Initializing {nameof(StarClusterGenerator)}");

        var fileNamingGalaxy =
            FileProjectManager.GetProjectFile("Services/_Game/StarClusters/Data/starcluster.naming.json");
        NamingContent = fileNamingGalaxy.ReadJson<NamingContent>();

        IsInitialized = true;

        Log.Logger.Success($"Initialized {nameof(StarClusterGenerator)}");
    }

    public static async Task<StarCluster> Generate(Vector2 position, Galaxy galaxy)
    {
        if (!IsInitialized) Initialize();

        if (galaxy.UniverseId == null) throw new Exception("Galaxy must have a universe");

        if (NamingContent == null)
            throw new Exception("StarClusterGenerator is not wekk initialized, NamingContent is null");

        var name = NamingContent.Names.GetRandom(true);
        var adjective = NamingContent.Adjectives.GetRandom(true);

        StarCluster starCluster = new()
        {
            Name = $"{name} {adjective}",
            SpatialLocation = new SpatialLocation(position.X, position.Y, galaxy.UniverseId, galaxy.Id, null)
        };

        return starCluster;
    }
}