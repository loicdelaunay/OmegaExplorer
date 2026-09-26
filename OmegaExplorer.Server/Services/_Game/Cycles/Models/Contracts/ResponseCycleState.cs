using OmegaExplorer.Server.Services._Core.Models.Contracts._inherits;

namespace OmegaExplorer.Server.Services._Game.Cycles.Models.Contracts;

public class ResponseCycleState : ResponseIdentifiable
{
    /// <summary>
    ///     Number of cycles
    /// </summary>
    public long CycleCount { get; set; }

    /// <summary>
    ///     Current cycle start at
    /// </summary>
    public DateTime CurrentCycle { get; set; }

    /// <summary>
    ///     Next cycle start at
    /// </summary>
    public DateTime NextCycle { get; set; }
}