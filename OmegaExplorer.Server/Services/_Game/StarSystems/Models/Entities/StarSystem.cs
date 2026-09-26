using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.Extensions;
using OmegaExplorer.Server.Services._Game._core.Models;
using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Buildings.Models.Entities;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Classes;
using OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Entities;
using OmegaExplorer.Server.Services._Game.StarSystems.Models.Enums;
using OmegaExplorer.Server.Services.Databases;
using OmegaExplorer.Server.Services.Databases.Attributes;
using OmegaExplorer.Server.Services.Users.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmegaExplorer.Server.Services._Game.StarSystems.Models.Entities;

/// <summary>
///     Can be a planet, a solar, an instability, an asteroid
/// </summary>
[Table(nameof(DatabaseContext.StarSystems))]
public class StarSystem : SpatialObject
{
    [ActivatorUtilitiesConstructor]
    public StarSystem()
    {
    }

    public StarSystem(string name, int size)
    {
        Name = name;
        Size = size;
        Rarity = GetRandomRarity();
    }

    public int Size { get; set; }

    public int Slots => 10 + Size * 2;

    public EnumRarity Rarity { get; set; }

    public EnumPlanetType PlanetType { get; set; }

    [JsonColumn] public List<ModifierResource> Modifiers { get; set; } = new();

    [JsonColumn] public Dictionary<int, long> Species { get; set; } = new();

    [ForeignKey(nameof(Owner))] public Guid? OwnerId { get; set; }

    /// <summary>
    ///     Owner of the planet
    ///     If null, the planet is not owned by a user
    /// </summary>
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public User? Owner { get; set; }

    public long MaxLivingSpecies => GetMaxLivingSpecies();

    [InverseProperty(nameof(Building.System))]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public List<Building> Buildings { get; set; } = new();

    private string ComposeName(NamingContent content)
    {
        var name = content.Names.GetRandom();
        var adjective = content.Adjectives.GetRandom();

        var generatedName = $"{name} {adjective}";

        //Check if name exist in database and add number if exist
        using DatabaseContext ctx = new();
        var count = ctx.Planets.Count(planet => planet.Name == generatedName);

        if (count > 0) generatedName += $" {count + 1}";

        return generatedName;
    }

    public EnumRarity GetRandomRarity()
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

    /// <summary>
    ///     Compute the max living species on the planet
    /// </summary>
    /// <returns></returns>
    private long GetMaxLivingSpecies()
    {
        return (long)Size * 1_000_000_000;
    }

    public override string ToString()
    {
        var type = GetType().Name;

        return $"{type}|{Name} - {Rarity} - {Size} - {SpatialLocation}";
    }
}