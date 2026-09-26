namespace OmegaExplorer.Server.Services._Game.GameResources.Models.Contracts;

public class ResponseModifierResource
{
    public string? Name { get; set; }

    public int Index { get; set; }
    public int Amount { get; set; }

    /// <summary>
    ///     If 0 = infinite
    /// </summary>
    public int Duration { get; set; }

    public ResponseDynamicItemGameResource Data { get; set; }

    public long? StartCycle { get; set; }
}