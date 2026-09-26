using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Enums;
using OmegaExplorer.Server.Services.Databases;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmegaExplorer.Server.Services._Game.Battles.Models.Classes;

[Table(nameof(DatabaseContext.BattleResultRewards))]
public class BattleRewardResult : Identifiable
{
    [ActivatorUtilitiesConstructor]
    public BattleRewardResult()
    {
    }

    public BattleRewardResult(Guid playerId, EnumGameResourceIndex gameResourceIndex, int amount)
    {
        PlayerId = playerId;
        IndexResource = (int)gameResourceIndex;
        Amount = amount;
    }

    public BattleRewardResult(Guid playerId, int indexResource, int amount)
    {
        PlayerId = playerId;
        IndexResource = indexResource;
        Amount = amount;
    }

    public Guid PlayerId { get; set; }
    public int IndexResource { get; set; }
    public int Amount { get; set; }
}