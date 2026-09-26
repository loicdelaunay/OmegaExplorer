using OmegaExplorer.Server.Services._Core.Models.Contracts._inherits;

namespace OmegaExplorer.Server.Services._Game.GameResources.Models.Contracts;

public class ResponseGameResource : ResponseIdentifiable
{
    public int Index { get; set; }

    public int Amount { get; set; }

    public int Balance { get; set; } = 0;
}