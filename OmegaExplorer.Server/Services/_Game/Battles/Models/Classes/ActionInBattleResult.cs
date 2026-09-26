using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.Services._Game.Battles.Models.Contracts;

namespace OmegaExplorer.Server.Services._Game.Battles.Models.Classes;

[Owned]
public class ActionInBattleResult
{
    public int? DiceActor { get; set; }
    public int? DiceTarget { get; set; }

    public bool Dodge { get; set; }

    public bool Absorb { get; set; }

    public bool Bounce { get; set; }

    public bool Critical { get; set; }

    public int Value { get; set; }

    /// <summary>
    ///     <inheritdoc cref="ResponseActionInBattleResult" />
    /// </summary>
    public List<Guid> ModuleIds { get; set; } = new();
}