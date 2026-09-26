using OmegaExplorer.Server.Services._Core.Models.Contracts._inherits;

namespace OmegaExplorer.Server.Services._Game.Orders.Models.Contracts;

public class ResponseOrderable : ResponseMetadata
{
    public List<ResponseOrder> Orders { get; set; } = new();
}