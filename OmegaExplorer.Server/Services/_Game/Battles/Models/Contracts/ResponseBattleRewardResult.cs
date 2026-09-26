using OmegaExplorer.Server.Services._Core.Models.Contracts._inherits;

namespace OmegaExplorer.Server.Services._Game.Battles.Models.Contracts;

public class ResponseBattleRewardResult : ResponseIdentifiable
{
    public Guid PlayerId { get; set; }
    public int IndexResource { get; set; }
    public int Amount { get; set; }
}