using OmegaExplorer.Server.Services._Core.Models.Contracts._inherits;
using OmegaExplorer.Server.Services._Game.Origins.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Battles.Models.Contracts;

public class ResponseBattleResult : ResponseIdentifiable
{
    public List<Guid?> PlayerWinnerIds { get; set; } = new();
    public List<Guid?> PlayerLoserIds { get; set; } = new();

    public List<EnumFaction> FactionWinners { get; set; } = new();
    public List<EnumFaction> FactionLosers { get; set; } = new();

    public List<ResponseBattleRewardResult> Rewards { get; set; } = new();
}