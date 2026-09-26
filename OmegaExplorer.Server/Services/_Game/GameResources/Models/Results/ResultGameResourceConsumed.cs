namespace OmegaExplorer.Server.Services._Game.GameResources.Models.Results;

/// <summary>
///     Return the amount remaining after operation on game
///     resource
/// </summary>
public class ResultGameResourceConsumed
{
    /// <summary>
    ///     Resource asked
    /// </summary>
    public Entities.GameResource? GameResource { get; set; }

    /// <summary>
    ///     User ask amount
    /// </summary>
    public int AmountRemaining { get; set; }

    public bool Success { get; set; }
}