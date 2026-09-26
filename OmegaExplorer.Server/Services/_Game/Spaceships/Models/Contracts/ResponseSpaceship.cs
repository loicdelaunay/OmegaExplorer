using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Battles.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Origins.Models.Enums;
using OmegaExplorer.Server.Services._Game.Spaceships.Models.Entities;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Contracts;
using OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Species.Models.Contracts;
using OmegaExplorer.Server.Services.Users.Models.Entities;

namespace OmegaExplorer.Server.Services._Game.Spaceships.Models.Contracts;

public class ResponseSpaceship : ResponseSpatialObject
{
    public Guid OwnerId { get; set; }

    public User? Owner { get; set; }

    public string Thumbnail { get; set; }

    public EnumFaction Faction { get; set; } = EnumFaction.Neutral;

    public EnumOwnerType OwnerType { get; set; } = EnumOwnerType.Null;

    public List<ResponseSpaceshipModule> Modules { get; set; } = new();

    public Spaceship.SpaceshipSize Size { get; set; }

    public EnumRarity Rarity { get; set; }

    public string? RawBlueprint { get; set; } = string.Empty;

    public ResponseBlueprintSpaceship? Blueprint { get; set; }

    public Dictionary<int, int> Species { get; set; } = new();

    public List<ResponseSpeciesAmount> SpeciesData { get; set; } = new();

    public Guid? BattleId { get; set; }

    public ResponseBattle? Battle { get; set; } = null;

    /// <summary>
    ///     <inheritdoc cref="Spaceship.DestroyedQuantityDamageToRepair" />
    /// </summary>
    public int DestroyedQuantityDamageToRepair { get; set; } = 0;

    public bool IsDestroyed { get; set; }

    public bool AutoBattle { get; set; }

    public int Dodge { get; set; }

    public int Critical { get; set; }

    public int Health { get; set; }

    public int ActionPoints { get; set; }
}