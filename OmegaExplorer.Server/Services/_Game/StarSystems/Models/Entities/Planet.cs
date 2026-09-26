using OmegaExplorer.Server.Services._Game.StarSystems.Generators;
using OmegaExplorer.Server.Services._Game.StarSystems.Models.Enums;
using OmegaExplorer.Server.Services.Databases;
using OmegaExplorer.Shared.Utilities.Random;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmegaExplorer.Server.Services._Game.StarSystems.Models.Entities;

[Table(nameof(DatabaseContext.Planets))]
public class Planet : StarSystem
{
    public Planet()
        : base(StarSystemGenerator.GetRandomPlanetName(), GetRandomSize())
    {
        //Randomize planet type
        PlanetType = GetRandomPlanetType();
    }


    private static int GetRandomSize()
    {
        var size = RandomWithSeed.Shared.Next(1, 10);

        return size;
    }

    private static EnumPlanetType GetRandomPlanetType()
    {
        var rdm = Random.Shared.Next(0, 10000); // Use 10,000 for more precision

        return rdm switch
        {
            > 9999 => EnumPlanetType.Special,
            > 9800 => EnumPlanetType.Water,
            > 9500 => EnumPlanetType.Forest,
            > 9000 => EnumPlanetType.Gaz,
            > 7500 => EnumPlanetType.Volcanic,
            > 4000 => EnumPlanetType.Ice,
            > 2000 => EnumPlanetType.Dead,
            _ => EnumPlanetType.Desert
        };
    }
}