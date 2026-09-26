using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.Services._Game.Battles.Models.Classes;
using OmegaExplorer.Server.Services._Game.Cycles;
using OmegaExplorer.Server.Services._Game.Spaceships.Models.Entities;
using OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Entities;
using OmegaExplorer.Server.Services.Databases;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmegaExplorer.Server.Services._Game.Battles.Models.Entities;

[Table(nameof(DatabaseContext.Battles))]
public class Battle : SpatialObject
{
    [ActivatorUtilitiesConstructor]
    public Battle()
    {
    }

    /// <summary>
    ///     Create a battle in a universe
    /// </summary>
    /// <param name="location"></param>
    /// <param name="cycleAtBattleStart"></param>
    /// <param name="spaceshipIds"></param>
    public Battle(long cycleAtBattleStart, List<Guid> spaceshipIds)
    {
        StartAtCycle = cycleAtBattleStart;
    }

    /// <summary>
    ///     List of spaceships in the battle
    /// </summary>
    [DeleteBehavior(DeleteBehavior.SetNull)]
    public List<Spaceship> Spaceships { get; set; } = new();

    /// <summary>
    ///     List of all actions in the battle
    /// </summary>
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public List<ActionInBattle> Actions { get; set; } = new();

    /// <summary>
    ///     Cycle when the battle started
    /// </summary>
    public long StartAtCycle { get; set; }

    public long EndAtCycle { get; set; }

    public BattleResult? Result { get; set; }

    public int CurrentCycle
    {
        get
        {
            var cycleElapsed = (int)(CycleBackgroundService.CurrentCycle - StartAtCycle);
            return cycleElapsed;
        }
    }
}