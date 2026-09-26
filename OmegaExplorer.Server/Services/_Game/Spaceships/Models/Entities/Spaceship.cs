using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.Extensions;
using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Battles.Models.Entities;
using OmegaExplorer.Server.Services._Game.Origins.Models.Enums;
using OmegaExplorer.Server.Services._Game.Spaceships.Generator;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint.Models.Entities;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Entities;
using OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Entities;
using OmegaExplorer.Server.Services.Databases;
using OmegaExplorer.Server.Services.Databases.Attributes;
using OmegaExplorer.Server.Services.Users.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmegaExplorer.Server.Services._Game.Spaceships.Models.Entities;

[Table(nameof(DatabaseContext.Spaceships))]
public class Spaceship : SpatialObject
{
    public enum SpaceshipSize
    {
        Cruiser = 0,
        Corvette = 1,
        Battleship = 2,
        Dreadnought = 3,
        Capital = 4
    }

    [ActivatorUtilitiesConstructor]
    public Spaceship()
    {
    }

    public Spaceship(BlueprintSpaceship blueprint, EnumOwnerType ownerType, EnumFaction faction)
    {
        Name = SpaceshipGenerator.GetRandomName();
        OwnerId = blueprint.OwnerId;
        Size = blueprint.Size;
        Rarity = SpaceshipGenerator.GetRandomRarity();
        OwnerType = ownerType;
        Faction = faction;
    }

    [ForeignKey(nameof(Owner))] public Guid? OwnerId { get; set; }

    [InverseProperty(nameof(User.Spaceships))]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public User? Owner { get; set; }

    [InverseProperty(nameof(SpaceshipModule.Spaceship))]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public List<SpaceshipModule> Modules { get; set; } = new();

    /// <summary>
    ///     Set the quantity of damage to repair for set the spaceship as not destroyed
    /// </summary>
    public int DestroyedQuantityDamageToRepair { get; set; } = 0;

    public EnumOwnerType OwnerType { get; set; } = EnumOwnerType.Null;

    public EnumFaction Faction { get; set; } = EnumFaction.Neutral;

    public SpaceshipSize Size { get; set; }

    public EnumRarity Rarity { get; set; }

    public Guid? BattleId { get; set; }

    [JsonColumn] public Dictionary<int, int> Species { get; set; } = new();

    public bool AutoBattle { get; set; } = true;

    /// <summary>
    ///     Current battle of the spaceship
    /// </summary>
    [ForeignKey(nameof(BattleId))]
    public Battle? Battle { get; set; }

    [InverseProperty(nameof(ActionInBattle.Actor))]
    public List<ActionInBattle> ActionsAsActor { get; set; } = new();

    [InverseProperty(nameof(ActionInBattle.Targets))]
    public List<ActionInBattle> ActionsAsTarget { get; set; } = new();

    public bool IsDestroyed => DestroyedQuantityDamageToRepair > 0;

    public int Dodge => GetDodge();

    public int Critical => GetCritical();

    public int Health => GetHealth();

    public int ActionPoints => GetActionPoints();

    /// <summary>
    ///     Based on <see cref="RawBlueprint" />
    /// </summary>
    [NotMapped]
    public BlueprintSpaceship? Blueprint => RawBlueprint.JsonDeserialize<BlueprintSpaceship>();

    /// <summary>
    ///     Data of the original blueprint
    /// </summary>
    // ReSharper disable once EntityFramework.ModelValidation.UnlimitedStringLength
    // due to the fact that the string can be very long because of the JSON serialization of
    // the blueprint
    public string? RawBlueprint { get; set; } = string.Empty;

    /// <summary>
    ///     Get the dodge number to hit when throwing a die
    /// </summary>
    /// <returns></returns>
    public int GetDodge()
    {
        return 15;
    }

    /// <summary>
    ///     Get the critical number to hit when throwing a die
    /// </summary>
    /// <returns></returns>
    public int GetCritical()
    {
        return 15;
    }

    /// <summary>
    ///     Get the health of the spaceship based on the modules
    /// </summary>
    /// <returns></returns>
    public int GetHealth()
    {
        if (Modules.Count == 0) return 0;

        return Modules.Sum(module => module.Health);
    }

    /// <summary>
    ///     Get the number of action points for the spaceship
    /// </summary>
    /// <returns></returns>
    public int GetActionPoints()
    {
        return 3;
    }

    public override string ToString()
    {
        return $"Spaceship {Name} - {Id} | {OwnerType} {Faction}";
    }
}