using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Battles.Models.Classes;
using OmegaExplorer.Server.Services._Game.Spaceships.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmegaExplorer.Server.Services._Game.Battles.Models.Entities;

public class ActionInBattle : Identifiable
{
    [ActivatorUtilitiesConstructor]
    public ActionInBattle()
    {
    }

    public ActionInBattle(Guid actorSpaceshipId, List<Spaceship> spaceships, int turn,
        int skillIndex)
    {
        ActorSpaceshipId = actorSpaceshipId;
        Targets = spaceships;
        Turn = turn;
        SkillIndex = skillIndex;
    }

    public Guid BattleId { get; set; }

    [ForeignKey(nameof(BattleId))] public Battle Battle { get; set; }

    public Guid ActorSpaceshipId { get; set; }

    [ForeignKey(nameof(ActorSpaceshipId))]
    [InverseProperty(nameof(Spaceship.ActionsAsActor))]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public Spaceship? Actor { get; set; }

    [InverseProperty(nameof(Spaceship.ActionsAsTarget))]
    [DeleteBehavior(DeleteBehavior.SetNull)]
    public List<Spaceship> Targets { get; set; } = new();

    /// <summary>
    ///     Turn of the action in the battle at a cycle
    /// </summary>
    public int Turn { get; set; }

    public int SkillIndex { get; set; }

    /// <summary>
    ///     Cycle of the battle action is played
    /// </summary>
    public int Cycle { get; set; }

    public ActionInBattleResult? Result { get; set; }
}