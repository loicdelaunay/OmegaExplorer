namespace OmegaExplorer.Server.Services._Game.Battles.Models.Contracts;

public class ResponseActionInBattleResult
{
    public int DiceActor { get; set; }
    public int DiceTarget { get; set; }

    public bool Dodge { get; set; }

    public bool Absorb { get; set; }

    public bool Bounce { get; set; }

    public bool Critical { get; set; }

    public int Value { get; set; }

    /// <summary>
    ///     List of all modules that were impacted
    /// </summary>
    public List<Guid> ModuleIds { get; set; } = new();
}