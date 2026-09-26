using OmegaExplorer.Server.Extensions;
using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services.ProjectFiles;

namespace OmegaExplorer.Server.Services._Game.Spaceships.Generator;

public static class SpaceshipGenerator
{
    private static SpaceshipGeneratorData _spaceshipGeneratorData = new();

    private static readonly bool IsInitialized = false;

    public static void Initialize()
    {
        Log.Logger.Information("Initializing GeneratorSpaceship");


        var file = FileProjectManager.GetProjectFile("Services/_Game/Spaceships/Data/spaceship.naming.json");

        _spaceshipGeneratorData = file.ReadJson<SpaceshipGeneratorData>();

        Log.Logger.Information("Initialized GeneratorPlanet");
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

    public static string GetRandomName()
    {
        if (!IsInitialized) Initialize();

        var name = _spaceshipGeneratorData.Names.GetRandom();
        var adjective = _spaceshipGeneratorData.Adjectives.GetRandom();

        var generatedName = $"{name} {adjective}";

        return generatedName;
    }
}