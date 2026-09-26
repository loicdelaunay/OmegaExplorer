using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Battles.Models.Entities;
using OmegaExplorer.Server.Services._Game.Origins.Models.Enums;
using OmegaExplorer.Server.Services.Databases;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmegaExplorer.Server.Services._Game.Battles.Models.Classes;

[Table(nameof(DatabaseContext.BattleResults))]
public class BattleResult : Identifiable
{
    [ActivatorUtilitiesConstructor]
    public BattleResult()
    {
    }

    public BattleResult(Guid battleId)
    {
        BattleId = battleId;
    }

    public Battle Battle { get; set; }

    [ForeignKey(nameof(Battle))] public Guid BattleId { get; set; }

    public List<Guid?> PlayerWinnerIds { get; set; } = new();
    public List<Guid?> PlayerLoserIds { get; set; } = new();

    public List<EnumFaction> FactionWinners { get; set; } = new();
    public List<EnumFaction> FactionLosers { get; set; } = new();

    public List<BattleRewardResult> Rewards { get; set; } = new();
}