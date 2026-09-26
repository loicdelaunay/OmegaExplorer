using OmegaExplorer.Server.Services._Game.Battles.Models.Classes;
using OmegaExplorer.Server.Services._Game.Battles.Models.Entities;
using OmegaExplorer.Server.Services._Game.Cycles;
using OmegaExplorer.Server.Services._Game.Spaceships;
using OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Classes;

namespace OmegaExplorer.Server.Services._Game.Battles;

public class BattleService
{
    private readonly BattleRepository _battleRepository;
    private readonly IServiceProvider _serviceProvider;

    private readonly SpaceshipRepository _spaceshipRepository;

    public BattleService(SpaceshipRepository spaceshipRepository, BattleRepository battleRepository,
        IServiceProvider serviceProvider)
    {
        _spaceshipRepository = spaceshipRepository;
        _battleRepository = battleRepository;
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    ///     Create a new battle
    /// </summary>
    /// <param name="location"></param>
    /// <param name="spaceshipIds"></param>
    /// <returns></returns>
    public async Task<Battle> CreateBattle(SpatialLocation location, List<Guid> spaceshipIds)
    {
        try
        {
            var newBattle = await _battleRepository.Create(location, spaceshipIds);
            return newBattle;
        }
        catch (Exception e)
        {
            Log.Logger.Error(e, "Not able to create battle");
            throw;
        }
    }

    /// <summary>
    ///     Get all battles by specific user
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<List<Battle>> GetAllBattlesByUser(Guid userId)
    {
        try
        {
            var battles = await _battleRepository.GetAllBattlesByUser(userId);
            return battles;
        }
        catch (Exception e)
        {
            Log.Logger.Error(e, "Not able to get battles");
            throw;
        }
    }

    public async Task<ActionInBattle?> CreateActionInBattle(Guid battleId, ActionInBattle actionInBattle)
    {
        try
        {
            var battle = await _battleRepository.GetBattleById(battleId);

            if (battle == null) throw new Exception("Battle not found");

            //Set the turn of the action

            //Get the difference between the current cycle and the turn when the battle started
            var cycleDiff = CycleBackgroundService.CurrentCycle - battle.StartAtCycle;

            actionInBattle.Cycle = (int)cycleDiff;
            actionInBattle.BattleId = battleId;

            var newActionInBattle = await _battleRepository.AddActionInBattle(battle, actionInBattle);

            return newActionInBattle;
        }
        catch (Exception e)
        {
            Log.Logger.Error(e, "Not able to create action in battle");
            throw;
        }
    }

    public async Task RemoveActionInBattle(Guid battleId, Guid actionInBattleId)
    {
        try
        {
            var battle = await _battleRepository.GetBattleById(battleId);
            if (battle == null) throw new Exception("Battle not found");

            await _battleRepository.RemoveActionInBattle(battle, actionInBattleId);
        }
        catch (Exception e)
        {
            Log.Logger.Error(e, "Not able to remove action in battle");
            throw;
        }
    }

    public async Task<List<ActionInBattle?>> GetAllActionsInBattle(Guid battleId, Guid userId)
    {
        try
        {
            var actions = await _battleRepository.GetActionsInBattleByUser(battleId, userId);

            return actions;
        }
        catch (Exception e)
        {
            Log.Logger.Error(e, "Not able to get actions in battle");
            throw;
        }
    }

    public async Task<List<ActionInBattle>> GetAllActionsInBattleByCycleAndByUser(Battle battle, long cycle,
        Guid currentUserId)
    {
        try
        {
            var actions = await _battleRepository.GetActionsInBattleByCycleAndByUser(battle, cycle, currentUserId);

            return actions;
        }
        catch (Exception e)
        {
            Log.Logger.Error(e, "Not able to get actions in battle");
            throw;
        }
    }

    public async Task<List<ActionInBattle>> GetAllActionsInBattleByCycle(Guid battleId, long turn)
    {
        try
        {
            var actions = await _battleRepository.GetActionsInBattleByCycle(battleId, turn);

            return actions;
        }
        catch (Exception e)
        {
            Log.Logger.Error(e, "Not able to get actions in battle");
            throw;
        }
    }

    public async Task<object> GetAllActionsInBattleByCurrentCycle(Guid battleId, Guid currentUserId)
    {
        try
        {
            var battle = await _battleRepository.GetBattleById(battleId);

            if (battle == null) throw new Exception("Battle not found");

            var actions =
                await _battleRepository.GetActionsInBattleByCycleAndByUser(battle, battle.CurrentCycle, currentUserId);

            return actions;
        }
        catch (Exception e)
        {
            Log.Logger.Error(e, "Not able to get all actions");
            throw;
        }
    }

    public async Task DeleteBattle(Guid id)
    {
        try
        {
            await _battleRepository.DeleteBattle(id);
        }
        catch (Exception e)
        {
            Log.Logger.Error(e, "Not able to delete battle");
            throw;
        }
    }

    public async Task<List<Battle>> GetBattlesNotFinished()
    {
        try
        {
            var battles = await _battleRepository.GetAllBattlesNotFinished(true);
            return battles;
        }
        catch (Exception e)
        {
            Log.Logger.Error(e, "Not able to get battles");
            throw;
        }
    }

    public async Task ResolveBattle(Battle battle)
    {
        BattleResolver battleResolver = new(
            battle,
            _serviceProvider);
        await battleResolver.Resolve();
    }

    public async Task CreateActionResult(Guid actionId, ActionInBattleResult result)
    {
        try
        {
            await _battleRepository.CreateActionResult(actionId, result);
        }
        catch (Exception e)
        {
            Log.Logger.Error(e, "Not able to create action result");
            throw;
        }
    }

    public async Task ExitBattle(Guid spaceshipId)
    {
        await _spaceshipRepository.ExitBattle(spaceshipId);
    }

    public async Task EndBattle(BattleResolver battleResolver)
    {
        BattleResult battleResult = new(battleResolver.Battle.Id)
        {
            Rewards = battleResolver.Rewards
        };

        await _battleRepository.CreateBattleResult(battleResult);
    }

    public async Task<Battle?> GetBattleById(Guid battleId, bool asNoTracking = false)
    {
        var battle = await _battleRepository.GetBattleById(battleId, asNoTracking);

        return battle;
    }

    public async Task<List<ActionInBattle>> GetAllActionsInBattleByCurrentCycleAndSpaceship(Guid spaceshipId,
        Guid battleId)
    {
        try
        {
            var battle = await _battleRepository.GetBattleById(battleId);

            if (battle == null) throw new Exception("Battle not found");

            var actions =
                await _battleRepository.GetActionsInBattleByCycleAndBySpaceship(battleId, battle.CurrentCycle,
                    spaceshipId);

            return actions;
        }
        catch (Exception e)
        {
            Log.Logger.Error(e, "Not able to get all actions");
            throw;
        }
    }
}