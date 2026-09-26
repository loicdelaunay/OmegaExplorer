namespace OmegaExplorer.Server.Services._Game.GameResources.Models.Classes;

/// <summary>
///     Useful for a cost of something
///     or
///     for modify a resource each planet cycle
/// </summary>
public class ModifierResource
{
    public string Name { get; set; }
    public int Index { get; set; }
    public int Amount { get; set; }

    /// <summary>
    ///     If 0 = infinite
    /// </summary>
    public int Duration { get; set; }

    /// <summary>
    ///     When the modifier start
    ///     If null not started yet
    /// </summary>
    public long? StartCycle { get; set; }
}