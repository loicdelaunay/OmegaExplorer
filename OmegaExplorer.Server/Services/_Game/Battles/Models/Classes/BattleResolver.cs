using OmegaExplorer.Server.Extensions;
using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Battles.Models.Entities;
using OmegaExplorer.Server.Services._Game.Effects;
using OmegaExplorer.Server.Services._Game.Effects.Models.Enums;
using OmegaExplorer.Server.Services._Game.GameResources;
using OmegaExplorer.Server.Services._Game.GameResources.Data;
using OmegaExplorer.Server.Services._Game.Origins.Models.Enums;
using OmegaExplorer.Server.Services._Game.Quests;
using OmegaExplorer.Server.Services._Game.Spaceships;
using OmegaExplorer.Server.Services._Game.Spaceships.Models.Entities;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Entities;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipSkill;
using OmegaExplorer.Server.Services.Loggers.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Battles.Models.Classes;

/// <summary>
///     Resolve the state of the battle
/// </summary>
public class BattleResolver
{
    private bool _isEnded;
    public Battle Battle;

    public List<Guid> Players = new();
    public List<Guid> PlayersWinnerIds = new();
    public List<Spaceship> SpaceshipsWinners = new();


    public BattleResolver(Battle battle, IServiceProvider serviceProvider)
    {
        Battle = battle;

        Players = battle.Spaceships
            .Where(spaceship => spaceship is { OwnerType: EnumOwnerType.Player, OwnerId: not null })
            .Select(spaceship => spaceship.OwnerId)
            .DistinctBy(playerId => playerId)
            .Cast<Guid>()
            .ToList();

        ServiceProvider = serviceProvider;
    }

    public List<BattleRewardResult> Rewards { get; set; } = new();

    private IServiceProvider ServiceProvider { get; }


    public async Task Resolve()
    {
        try
        {
            await using var scope = ServiceProvider.CreateAsyncScope();

            var spaceshipService = scope.ServiceProvider.GetRequiredService<SpaceshipService>();
            var questService = scope.ServiceProvider.GetRequiredService<QuestService>();
            var battleService = scope.ServiceProvider.GetRequiredService<BattleService>();
            var spaceshipSkillService = scope.ServiceProvider.GetRequiredService<SpaceshipSkillService>();
            var gameResourceService = scope.ServiceProvider.GetRequiredService<GameResourceService>();

            var spaceshipSkillDataProvider = scope.ServiceProvider.GetRequiredService<SpaceshipSkillDataProvider>();

            //For each spaceship check if auto use skill and add action
            foreach (var spaceship in Battle.Spaceships)
                await ResolveAutoBattle(spaceship, battleService, spaceshipSkillService);

            //Get all actions in battle
            var actions = await battleService.GetAllActionsInBattleByCycle(Battle.Id, Battle.CurrentCycle);

            //Order actions by spaceship velocity and skill priority 
            actions = actions
                .Where(action => action.Actor is not null)
                .OrderBy(action => action.Turn)
                .ThenBy(action => action.Actor!.SpeedInStarCluster)
                .ToList();


            foreach (var action in actions)
                //Resolve action
                try
                {
                    await ResolveAction(action, spaceshipSkillDataProvider, spaceshipService, battleService);
                }
                catch (Exception e)
                {
                    Log.Logger.Error(
                        $"[{nameof(BattleResolver)}] Error while resolving action {action.Id} : {e.Message}",
                        EnumLogSeverity.Error);
                }

            await RetrieveStateUpdated(battleService);
            await CheckSpaceshipState(spaceshipService, questService, battleService);
            await CheckBattleState(battleService, gameResourceService);
        }
        catch (Exception e)
        {
            Log.Logger.Error($"[{nameof(BattleResolver)}] Error while resolving battle {Battle.Id} : {e.Message}",
                EnumLogSeverity.Error);
        }
    }

    private async Task ResolveAutoBattle(Spaceship spaceship, BattleService battleService,
        SpaceshipSkillService spaceshipSkillService)
    {
        await ResolveAutoUseSkills(spaceship, battleService, spaceshipSkillService);
    }

    /// <summary>
    ///     Check if spaceship can use skills automatically and add actions for each action points available
    /// </summary>
    /// <param name="spaceship"></param>
    private async Task ResolveAutoUseSkills(Spaceship spaceship, BattleService battleService,
        SpaceshipSkillService spaceshipSkillService)
    {
        //Get current spaceship actions
        var actions = await battleService.GetAllActionsInBattleByCurrentCycleAndSpaceship(spaceship.Id, Battle.Id);

        //How many actions the spaceship can do more
        var actionPointsRemaining = spaceship.ActionPoints - actions.Count();

        for (var i = 0; i < actionPointsRemaining; i++)
            await ResolveAutoUseSkill(spaceship, i, battleService, spaceshipSkillService);
    }

    /// <summary>
    ///     Add an action automatically for the spaceship
    /// </summary>
    /// <param name="spaceship"></param>
    /// <param name="turn"></param>
    private async Task ResolveAutoUseSkill(Spaceship spaceship, int turn,
        BattleService battleService, SpaceshipSkillService spaceshipSkillService)
    {
        //Get a random target coherent with the spaceship faction and user relation

        var targets = Battle.Spaceships
            .Where(spaceshipTarget => spaceshipTarget.Id != spaceship.Id &&

                                      //Check if spaceship is not in the same faction or if spaceship is not in the same faction is not the same player
                                      (spaceshipTarget.OwnerType != spaceship.OwnerType ||
                                       (spaceshipTarget.OwnerType == EnumOwnerType.Player &&
                                        spaceshipTarget.OwnerId != spaceship.OwnerId)))
            .ToList()
            .GetRandomList(1);

        //Get a random skill
        var skills = await spaceshipSkillService.GetSkills(spaceship);
        var skill = skills.GetRandom();

        if (skill == null) return;

        ActionInBattle actionInBattle = new(spaceship.Id, targets, turn, skill.Index);

        await battleService.CreateActionInBattle(Battle.Id, actionInBattle);
    }

    /// <summary>
    ///     Update all spaceships state after all actions are resolved
    /// </summary>
    private async Task RetrieveStateUpdated(BattleService battleService)
    {
        Battle = await battleService.GetBattleById(Battle.Id, true);
    }

    /// <summary>
    ///     Check if spaceship is destroyed and destroy it
    /// </summary>
    private async Task CheckSpaceshipState(SpaceshipService spaceshipService, QuestService questService,
        BattleService battleService)
    {
        foreach (var spaceship in Battle.Spaceships)
            if (spaceship.GetHealth() <= 0)
            {
                await spaceshipService.DestroySpaceship(spaceship.Id);

                //Check quest completion
                foreach (var player in Players)
                {
                    if (player == null) continue;

                    try
                    {
                        try
                        {
                            await questService.CheckQuestSpaceshipDestroyed(player, spaceship);
                        }
                        catch (Exception e)
                        {
                            Log.Logger.Information(
                                $"[{nameof(BattleResolver)}] : error while checking quest for player {player} and spaceship {spaceship.Id} : {e.Message}",
                                EnumLogSeverity.Error);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
            }
    }

    /// <summary>
    ///     Check if battle is ended
    /// </summary>
    private async Task CheckBattleState(BattleService battleService, GameResourceService gameResourceService)
    {
        var remainingSpaceshipsByFaction = Battle.Spaceships
            .Where(spaceship => spaceship.OwnerType != EnumOwnerType.Player)
            .GroupBy(spaceship => spaceship.Faction)
            .ToDictionary(group => group.Key, group => group.Count(spaceship => spaceship.GetHealth() > 0));

        foreach (var faction in remainingSpaceshipsByFaction)
            if (faction.Value == 0)
                // All ships of this faction are destroyed
                await ExitBattleForFaction(faction.Key);

        var remainingSpaceshipsByPlayer = Battle.Spaceships
            .Where(spaceship => spaceship is { OwnerType: EnumOwnerType.Player, OwnerId: not null })
            .GroupBy(spaceship => spaceship.OwnerId!.Value)
            .ToDictionary(group => group.Key, group => group.Count(spaceship => spaceship.GetHealth() > 0));

        foreach (var player in remainingSpaceshipsByPlayer)
            if (player.Value == 0)
                // All ships of this player are destroyed
                await ExitBattleForPlayer(player.Key);

        //Check if battle is ended
        var factionRemaining = remainingSpaceshipsByFaction.Count(faction => faction.Value > 0);
        var playerRemaining = remainingSpaceshipsByPlayer.Count(player => player.Value > 0);

        // If all faction are not destroyed and all player are destroyed then end battle
        if (factionRemaining >= 0 && playerRemaining == 0)
        {
            await EndBattle([], [], battleService, gameResourceService);
            return;
        }

        // If all faction are destroyed and all player are not destroyed then end battle
        if (factionRemaining == 0 && playerRemaining > 0)
        {
            //Get a list of player and spaceship remaining
            var players = remainingSpaceshipsByPlayer
                .Where(player => player.Value > 0)
                .Select(player => player.Key)
                .ToList();

            var spaceships = Battle.Spaceships.Where(spaceship => players.Contains(spaceship.OwnerId!.Value)).ToList();

            await EndBattle(players, spaceships, battleService, gameResourceService);
        }
    }

    /// <summary>
    ///     Check if player is totally destroyed and exit battle
    /// </summary>
    /// <param name="playerId"></param>
    private async Task ExitBattleForPlayer(Guid? playerId)
    {
        //Nothing to do special for now
        //TODO send notification ?
    }

    /// <summary>
    ///     Check if faction is totally destroyed and exit battle
    /// </summary>
    /// <param name="faction"></param>
    private async Task ExitBattleForFaction(EnumFaction faction)
    {
        //Nothing to do special for now
    }

    private async Task EndBattle(List<Guid> playerWinnerIds, List<Spaceship> spaceships,
        BattleService battleService, GameResourceService gameResourceService)
    {
        PlayersWinnerIds = playerWinnerIds;
        SpaceshipsWinners = spaceships;

        await CheckRewards(gameResourceService);

        await battleService.EndBattle(this);
        _isEnded = true;
    }

    #region ACTION RESOLUTION

    private async Task ResolveAction(ActionInBattle action, SpaceshipSkillDataProvider spaceshipSkillDataProvider
        , SpaceshipService spaceshipService, BattleService battleService)
    {
        var skill = spaceshipSkillDataProvider.GetByIndex(action.SkillIndex);

        if (skill == null) return;

        foreach (var effect in skill.Effects) await ResolveEffect(action, effect, spaceshipService, battleService);
    }

    private async Task ResolveEffect(ActionInBattle action, Effect effect,
        SpaceshipService spaceshipService, BattleService battleService)
    {
        switch (effect.Type)
        {
            case EnumEffectType.Damage:
                await ResolveEffectDamage(action, effect, spaceshipService, battleService);
                break;
            case EnumEffectType.Undamage:
                await ResolveEffectUndamage(action, effect);
                break;
            case EnumEffectType.Protect:
                await ResolveEffectProtect(action, effect);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private async Task ResolveEffectDamage(ActionInBattle action, Effect effect,
        SpaceshipService spaceshipService, BattleService battleService)
    {
        //Get spaceships targets
        var targets = Battle.Spaceships
            .Where(spaceship => action.Targets.Exists(target => target.Id == spaceship.Id)).ToList();

        foreach (var target in targets)
            await ResolveEffectDamageOnSpaceship(target, action, effect, spaceshipService, battleService);
    }

    private async Task ResolveEffectUndamage(ActionInBattle action, Effect effect)
    {
        throw new NotImplementedException();
    }

    private async Task ResolveEffectProtect(ActionInBattle action, Effect effect)
    {
    }

    private async Task ResolveEffectDamageOnSpaceship(Spaceship spaceship,
        ActionInBattle action,
        Effect effect, SpaceshipService spaceshipService, BattleService battleService)
    {
        ActionInBattleResult actionResolution = new();
        var amount = effect.Value;

        //Throw dice for actor 
        actionResolution.DiceActor = Random.Shared.Next(1, 20);
        actionResolution.DiceTarget = Random.Shared.Next(1, 20);

        //Check if absorb
        //TODO

        //Check if bounce 
        //TODO

        //Check if critical
        if (actionResolution.DiceActor > spaceship.GetCritical())
        {
            actionResolution.Critical = true;
            amount = (int)Math.Round((double)(amount * 2));
        }

        //Check if dodge
        if (actionResolution.DiceTarget > spaceship.GetDodge())
        {
            actionResolution.Dodge = true;
            amount = 0;
        }

        actionResolution.Value = amount;

        //Check number of module hit
        List<SpaceshipModule> modulesHit = new();

        if (effect.ModuleTargetType == EnumEffectModuleTargetType.All)
        {
            var modules = await spaceshipService.SubtractHealthFromAllModules(spaceship.Id, amount);
            modulesHit.AddRange(modules);
        }
        else
        {
            for (var i = 0; i < effect.ModuleTargetCount; i++)
            {
                var module = await spaceshipService.SubtractHealthFromRandomModule(spaceship.Id, amount);
                modulesHit.Add(module);
            }
        }

        actionResolution.ModuleIds = modulesHit.Select(module => module.Id).ToList();

        //Register the action result
        await battleService.CreateActionResult(action.Id, actionResolution);
    }

    #endregion

    #region REWARDS

    private async Task CheckRewards(GameResourceService gameResourceService)
    {
        //Retrieve only player winners in the battle
        var players = Battle.Spaceships
            .Where(spaceship => PlayersWinnerIds.Contains(spaceship.OwnerId!.Value))
            .Select(spaceship => spaceship.OwnerId)
            .ToList();

        foreach (var playerId in players)
        {
            if (playerId == null) continue;

            await GiveReward((Guid)playerId, gameResourceService);
        }
    }

    private async Task GiveReward(Guid playerId, GameResourceService gameResourceService)
    {
        //Give 1000 credits to the player
        var newValue =
            await gameResourceService.AddResourceToPlayer(DynamicItemGameResource_0_Credit.INDEX, 1000, playerId);
        Rewards.Add(new BattleRewardResult(playerId, DynamicItemGameResource_0_Credit.INDEX, 1000));
    }

    #endregion
}