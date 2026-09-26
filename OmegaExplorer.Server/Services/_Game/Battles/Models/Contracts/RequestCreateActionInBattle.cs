namespace OmegaExplorer.Server.Services._Game.Battles.Models.Contracts;

public class RequestCreateActionInBattle
{
    public Guid BattleId { get; set; }

    public Guid SpaceshipId { get; set; }

    public List<Guid> TargetIds { get; set; } = new();

    public int SkillIndex { get; set; }

    public int Turn { get; set; }
}