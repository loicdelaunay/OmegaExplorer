using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.Services._Game.Battles.Models.Classes;
using OmegaExplorer.Server.Services._Game.Battles.Models.Entities;
using OmegaExplorer.Server.Services._Game.Cycles;
using OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Classes;
using OmegaExplorer.Server.Services.Databases;
using OmegaExplorer.Server.Services.Loggers.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Battles;

public class BattleRepository
{
    private readonly DatabaseContext _databaseContext;
    private readonly ILogger<BattleRepository> _logger;

    public BattleRepository(DatabaseContext databaseContext, ILogger<BattleRepository> logger)
    {
        _databaseContext = databaseContext;
        _logger = logger;
    }

    public async Task<Battle?> GetBattleById(Guid id, bool asNoTracking = false)
    {
        var query = _databaseContext.Battles
            .Include(b => b.Spaceships)
            .ThenInclude(s => s.Modules)
            .Include(b => b.Result)
            .ThenInclude(br => br.Rewards);

        if (asNoTracking) query.AsNoTracking();

        var battle = await query.FirstOrDefaultAsync(b => b.Id == id);

        return battle;
    }

    public async Task<List<ActionInBattle?>> GetActionsInBattleByUser(Guid idBattle, Guid idUser)
    {
        var actions = await _databaseContext.BattleActions
            .Where(action => action.BattleId == idBattle && action.Actor.OwnerId == idUser)
            .ToListAsync();

        return actions;
    }

    public async Task<List<ActionInBattle>> GetActionsInBattleByCycleAndByUser(Battle battle,
        long cycle, Guid idUser)
    {
        var queryActions = _databaseContext.BattleActions
            .Where(action => action.BattleId == battle.Id && action.Cycle == cycle);

        //Check if cycle is current cycle of the battle, so hide the others players action
        if (cycle == battle.CurrentCycle)
            queryActions = queryActions
                .Where(action => action.Actor != null && action.Actor.OwnerId == idUser);

        queryActions = queryActions
            .Include(ba => ba.Actor)
            .Include(ba => ba.Targets)
            .ThenInclude(s => s.Modules);


        var actions = await queryActions.ToListAsync();
        return actions;
    }


    /// <summary>
    ///     Get all battles in a star cluster
    /// </summary>
    /// <param name="starClusterId"></param>
    /// <returns></returns>
    public async Task<List<Battle>> GetBattlesInStarCluster(Guid starClusterId)
    {
        var battles = await _databaseContext.Battles
            .Where(battle => battle.SpatialLocation.StarClusterId == starClusterId)
            .ToListAsync();

        return battles;
    }

    /// <summary>
    ///     Get all battles in a galaxy
    /// </summary>
    /// <param name="galaxyId"></param>
    /// <returns></returns>
    public async Task<List<Battle>> GetBattlesInGalaxy(Guid galaxyId)
    {
        var battles = await _databaseContext.Battles
            .Where(battle =>
                battle.SpatialLocation.GalaxyId == galaxyId && battle.SpatialLocation.StarClusterId == null)
            .ToListAsync();

        return battles;
    }

    /// <summary>
    ///     Get all battles in a universe
    /// </summary>
    /// <param name="idUniverse"></param>
    /// <returns></returns>
    public async Task<List<Battle>> GetBattlesInUniverse(Guid idUniverse)
    {
        var battles = await _databaseContext.Battles
            .Where(battle => battle.SpatialLocation.UniverseId == idUniverse &&
                             battle.SpatialLocation.GalaxyId == null && battle.SpatialLocation.StarClusterId == null)
            .ToListAsync();

        return battles;
    }

    public async Task<Battle> Create(SpatialLocation location, List<Guid> spaceshipIds)
    {
        Battle newBattle = new(CycleBackgroundService.CurrentCycle, spaceshipIds)
        {
            //Generate battle name
            Name = $"Battle at {location.Position} in {location.Name}",

            SpatialLocation = location
        };


        _databaseContext.Battles.Add(newBattle);

        foreach (var spaceshipId in spaceshipIds)
        {
            var spaceship = _databaseContext.Spaceships.FirstOrDefault(s => s.Id == spaceshipId);
            if (spaceship != null)
            {
                // Link the spaceship to this battle
                spaceship.Battle = newBattle;

                // Mark spaceship as modified if it's tracked,
                // or you can attach it if it's detached, etc.
                _databaseContext.Spaceships.Update(spaceship);
            }
        }

        await _databaseContext.SaveChangesAsync();

        return newBattle;
    }

    public async Task<List<Battle>> GetAllBattlesByUser(Guid userId)
    {
        var battles = await _databaseContext.Battles
            .Include(battle => battle.Result)
            .ThenInclude(battleResult => battleResult.Rewards)
            .Where(battle => battle.Spaceships.Any(spaceship => spaceship.OwnerId == userId))
            .ToListAsync();

        return battles;
    }

    public async Task Update(Battle battle)
    {
        _databaseContext.Battles.Update(battle);

        await _databaseContext.SaveChangesAsync();
    }

    public async Task<ActionInBattle?> AddActionInBattle(Battle battle, ActionInBattle actionInBattle)
    {
        try
        {
            //Attach the Targets as well
            _databaseContext.Spaceships.AttachRange(actionInBattle.Targets);

            _databaseContext.BattleActions.Add(actionInBattle);

            await _databaseContext.SaveChangesAsync();

            // Load the Actor
            if (actionInBattle.Actor == null)
                await _databaseContext.Entry(actionInBattle).Reference(a => a.Actor).LoadAsync();

            return actionInBattle;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while adding action in battle : {e}");
        }

        return null;
    }

    public async Task RemoveActionInBattle(Battle battle, Guid idActionInBattle)
    {
        try
        {
            var action = await _databaseContext.BattleActions.FirstOrDefaultAsync(a => a.Id == idActionInBattle);
            if (action == null) throw new Exception("Action not found");

            _databaseContext.BattleActions.Remove(action);

            await _databaseContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Log.Logger.Error($"Error while removing action in battle : {e}", EnumLogSeverity.Error);
        }
    }

    public async Task DeleteBattle(Guid id)
    {
        try
        {
            var battle = await _databaseContext.Battles.FirstOrDefaultAsync(b => b.Id == id);
            if (battle == null) throw new Exception("Battle not found");

            _databaseContext.Battles.Remove(battle);

            await _databaseContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Log.Logger.Error($"Error while deleting battle : {e}", EnumLogSeverity.Error);
        }
    }

    public async Task<List<Battle>> GetAllBattles()
    {
        var battles = await _databaseContext.Battles
            .Include(battle => battle.Spaceships)
            .ThenInclude(spaceship => spaceship.Modules)
            .ToListAsync();

        return battles;
    }

    public async Task<List<Battle>> GetAllBattlesNotFinished(bool asNoTracking = false)
    {
        var query = _databaseContext.Battles
            .Where(battle => battle.Result == null)
            .Include(battle => battle.Result)
            .Include(battle => battle.Spaceships)
            .ThenInclude(spaceship => spaceship.Modules);

        if (asNoTracking) query.AsNoTracking();

        var battles = await query.ToListAsync();

        return battles;
    }

    public async Task<List<ActionInBattle>> GetActionsInBattleByCycle(Guid battleId, long cycle)
    {
        var actions = await _databaseContext.BattleActions
            .Where(action => action.BattleId == battleId && action.Cycle == cycle)
            .Include(ba => ba.Actor)
            .Include(ba => ba.Targets)
            .ToListAsync();

        return actions;
    }

    public async Task CreateActionResult(Guid actionId, ActionInBattleResult result)
    {
        try
        {
            var action = await _databaseContext.BattleActions.FirstOrDefaultAsync(a => a.Id == actionId);
            if (action == null) throw new Exception("Action not found");

            action.Result = result;

            await _databaseContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Log.Logger.Error($"Error while creating action result : {e}", EnumLogSeverity.Error);
        }
    }

    public async Task CreateBattleResult(BattleResult battleResult)
    {
        try
        {
            var battle = await _databaseContext.Battles.FirstOrDefaultAsync(b => b.Id == battleResult.BattleId);
            if (battle == null) throw new Exception("Battle not found");

            battle.EndAtCycle = battle.CurrentCycle;

            _databaseContext.BattleResults.Add(battleResult);

            await _databaseContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Log.Logger.Error($"Error while creating battle result : {e}", EnumLogSeverity.Error);
        }
    }

    public async Task<List<ActionInBattle>> GetActionsInBattleByCycleAndBySpaceship(Guid battleId,
        int battleCycleOfTheBattle, Guid spaceshipId)
    {
        var actions = await _databaseContext.BattleActions
            .Where(action => action != null && action.BattleId == battleId && action.Cycle == battleCycleOfTheBattle &&
                             action.Actor != null && action.Actor.Id == spaceshipId)
            .Include(actionInBattle => actionInBattle.Actor)
            .Include(actionInBattle => actionInBattle.Targets)
            .ToListAsync();

        return actions;
    }

    public async Task<Battle?> GetBattleBySpaceshipId(Guid spaceshipId, Guid userId)
    {
        // Get the spaceship and include its battle
        var spaceship = await _databaseContext.Spaceships
            .Where(s => s.OwnerId == userId)
            .Include(s => s.Battle)
            .FirstOrDefaultAsync(s => s.Id == spaceshipId);

        if (spaceship == null) return null; // No spaceship found with the given ID

        return spaceship.Battle; // Return the battle associated with the spaceship
    }
}