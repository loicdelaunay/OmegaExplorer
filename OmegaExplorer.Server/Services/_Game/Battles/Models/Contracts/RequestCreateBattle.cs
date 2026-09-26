using OmegaExplorer.Server.Services._Game.Battles.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Battles.Models.Contracts;

public class RequestCreateBattle
{
    /// <summary>
    ///     Initial spaceship of the battle
    /// </summary>
    public Guid SpaceshipId { get; set; }

    /// <summary>
    ///     List of spaceships in the battle
    /// </summary>
    public Guid SpaceshipTargetId { get; set; }

    /// <summary>
    ///     Type of the battle
    /// </summary>
    public EnumBattleType Type { get; set; }
}