using OmegaExplorer.Server.Extensions;
using OmegaExplorer.Server.OmegaExplorer.Shared.Utilities.Log;
using OmegaExplorer.Server.Services._Game._core.Models;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Classes;
using OmegaExplorer.Server.Services._Game.StarClusters.Models.Entities;
using OmegaExplorer.Server.Services._Game.StarSystems.Models.Entities;
using OmegaExplorer.Server.Services.Databases;
using OmegaExplorer.Server.Services.ProjectFiles;
using OmegaExplorer.Shared.Utilities.Random;

namespace OmegaExplorer.Server.Services._Game.StarSystems.Generators;

public static class StarSystemGenerator
{
    private static NamingContent? _namingContent;

    private static StarSystemContentNaming? _starSystemContentNaming;

    public static NamingContent NamingContent
    {
        get
        {
            if (_namingContent == null) Initialize();

            return _namingContent;
        }
    }

    public static StarSystemContentNaming StarSystemContentNaming
    {
        get
        {
            if (_starSystemContentNaming == null) Initialize();

            return _starSystemContentNaming;
        }
    }

    public static bool IsInitialized { get; set; }

    public static void Initialize()
    {
        Log.Logger.Information("Initializing GeneratorPlanet");


        var filePlanetData = FileProjectManager.GetProjectFile("Services/_Game/StarSystems/Data/planet.naming.json");
        _namingContent = filePlanetData.ReadJson<NamingContent>();

        var fileInstabilityData =
            FileProjectManager.GetProjectFile("Services/_Game/StarSystems/Data/instability.naming.json");
        _starSystemContentNaming = fileInstabilityData.ReadJson<StarSystemContentNaming>();

        IsInitialized = true;

        Log.Logger.Success($"Initialized {nameof(StarSystemGenerator)}");
    }


    public static async Task<Planet?> GeneratePlanet(Vector2 position, StarCluster starCluster)
    {
        if (!IsInitialized) Initialize();

        if (starCluster.SpatialLocation.UniverseId == null) throw new Exception("StarCluster must have a universe");

        if (starCluster.SpatialLocation.GalaxyId == null) throw new Exception("StarCluster must have a galaxy");

        Planet planet = new()
        {
            SpatialLocation = new SpatialLocation
            {
                Position = position,
                UniverseId = starCluster.SpatialLocation.UniverseId,
                GalaxyId = starCluster.SpatialLocation.GalaxyId,
                StarClusterId = starCluster.Id
            }
        };

        return planet;
    }

    public static async Task<Models.Entities.Instability?> GenerateInstability(Vector2 position,
        StarCluster starCluster)
    {
        if (!IsInitialized) Initialize();

        if (starCluster.SpatialLocation.UniverseId == null) throw new Exception("StarCluster must have a universe");

        if (starCluster.SpatialLocation.GalaxyId == null) throw new Exception("StarCluster must have a galaxy");

        Models.Entities.Instability instability =
            new(GetRandomInstabilityName(), GetRandomInstabilitySize())
            {
                SpatialLocation = new SpatialLocation
                {
                    Position = position,
                    UniverseId = starCluster.SpatialLocation.UniverseId,
                    GalaxyId = starCluster.SpatialLocation.GalaxyId,
                    StarClusterId = starCluster.Id
                }
            };

        return instability;
    }

    public static async Task<Star?> GenerateStar(Vector2 position, StarCluster starCluster)
    {
        if (!IsInitialized) Initialize();

        if (starCluster.SpatialLocation.UniverseId == null) throw new Exception("StarCluster must have a universe");

        if (starCluster.SpatialLocation.GalaxyId == null) throw new Exception("StarCluster must have a galaxy");

        Star star = new(GetRandomStarName(), GetRandomStarSize())
        {
            SpatialLocation = new SpatialLocation
            {
                Position = position,
                UniverseId = starCluster.SpatialLocation.UniverseId,
                GalaxyId = starCluster.SpatialLocation.GalaxyId,
                StarClusterId = starCluster.Id
            }
        };

        return star;
    }

    public static EnumRarity GetRandomRarity()
    {
        var rdm = Random.Shared.Next(0, 100);

        return rdm switch
        {
            > 98 => EnumRarity.Origin,
            > 95 => EnumRarity.Legendary,
            > 90 => EnumRarity.Epic,
            > 75 => EnumRarity.Rare,
            > 40 => EnumRarity.Uncommon,
            _ => EnumRarity.Common
        };
    }

    public static string GetRandomStarName()
    {
        var name = StarSystemContentNaming.Names.GetRandom(true);
        var adjective = StarSystemContentNaming.Adjectives.GetRandom(true);

        var generatedName = $"{name} {adjective}";


        return generatedName;
    }

    public static int GetRandomStarSize()
    {
        var size = RandomWithSeed.Shared.Next(20, 40);

        return size;
    }

    public static string GetRandomPlanetName()
    {
        var content = NamingContent;

        var name = content.Names.GetRandom(true);
        var adjective = content.Adjectives.GetRandom(true);

        var generatedName = $"{name} {adjective}";


        return generatedName;
    }

    #region Instability generator

    public static Models.Entities.Instability.EnumInstabilityType GetRandomInstabilityType()
    {
        var rdm = Random.Shared.Next(0, 100);

        return rdm switch
        {
            > 99 => Models.Entities.Instability.EnumInstabilityType.S,
            > 90 => Models.Entities.Instability.EnumInstabilityType.A,
            > 80 => Models.Entities.Instability.EnumInstabilityType.B,
            > 70 => Models.Entities.Instability.EnumInstabilityType.C,
            > 50 => Models.Entities.Instability.EnumInstabilityType.D,
            > 30 => Models.Entities.Instability.EnumInstabilityType.E,
            _ => Models.Entities.Instability.EnumInstabilityType.F
        };
    }

    public static EnumRarity GetRandomInstabilityRarity()
    {
        var rdm = RandomWithSeed.Shared.Next(0, 100);

        return rdm switch
        {
            > 98 => EnumRarity.Origin,
            > 95 => EnumRarity.Legendary,
            > 90 => EnumRarity.Epic,
            > 75 => EnumRarity.Rare,
            > 40 => EnumRarity.Uncommon,
            _ => EnumRarity.Common
        };
    }

    public static int GetRandomInstabilitySize()
    {
        var size = RandomWithSeed.Shared.Next(1, 10);

        return size;
    }

    public static string GetRandomInstabilityName()
    {
        var content = NamingContent;

        var name = content.Names.GetRandom();
        var adjective = content.Adjectives.GetRandom();

        var generatedName = $"{name} {adjective}";

        //Check if name exist in database and add number if exist
        using DatabaseContext ctx = new();
        var count = ctx.Planets.Count(planet => planet.Name == generatedName);

        if (count > 0) generatedName += $" {count + 1}";

        return generatedName;
    }

    #endregion
}