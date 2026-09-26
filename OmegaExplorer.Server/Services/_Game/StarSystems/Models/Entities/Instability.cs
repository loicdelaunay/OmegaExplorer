using OmegaExplorer.Server.Services._Game.StarSystems.Generators;
using OmegaExplorer.Server.Services.Databases;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmegaExplorer.Server.Services._Game.StarSystems.Models.Entities;

/// <summary>
///     In the game, an instability is a spatial object that can be found on the map
///     create enemies
/// </summary>
[Table(nameof(DatabaseContext.Instabilities))]
public class Instability : StarSystem
{
    public enum EnumInstabilityType
    {
        //Keep difference to add more instabilities level in future
        S = 60,
        A = 50,
        B = 40,
        C = 30,
        D = 20,
        E = 10,
        F = 0
    }

    public Instability(string name, int size)
        : base(name, size)
    {
        Rarity = StarSystemGenerator.GetRandomInstabilityRarity();
        TypeInstability = StarSystemGenerator.GetRandomInstabilityType();
    }

    public EnumInstabilityType TypeInstability { get; set; } = EnumInstabilityType.F;
}