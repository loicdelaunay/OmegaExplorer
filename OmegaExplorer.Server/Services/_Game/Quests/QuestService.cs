using OmegaExplorer.Server.Services._Game.GameResources;
using OmegaExplorer.Server.Services._Game.Quests.Models.Entities;
using OmegaExplorer.Server.Services._Game.Quests.Models.Enums;
using OmegaExplorer.Server.Services._Game.Quests.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Spaceships.Models.Entities;

namespace OmegaExplorer.Server.Services._Game.Quests;

public class QuestService
{
    private readonly GameResourceService _gameResourceService;
    private readonly QuestDataProvider _questDataProvider;
    private readonly QuestRepository _questRepository;

    public QuestService(QuestDataProvider questDataProvider, GameResourceService gameResourceService,
        QuestRepository questRepository)
    {
        _questDataProvider = questDataProvider;
        _gameResourceService = gameResourceService;
        _questRepository = questRepository;
    }

    public async Task<List<IDynamicItemQuest>> GetAllQuestsAvailable(Guid userId)
    {
        //Return quest 1
        var res = _questDataProvider.GetAll();

        return res;
    }

    public async Task<List<QuestProgress>> GetAllQuestsInProgress(Guid userId)
    {
        var res = await _questRepository.GetAllQuestsInProgress(userId);
        return res;
    }

    public async Task<List<QuestProgress>> GetAllQuestsDone(Guid userId)
    {
        var res = await _questRepository.GetAllQuestsDone(userId);
        return res;
    }

    public async Task<QuestProgress> AcceptQuest(Guid userId, int questIndex)
    {
        var quest = _questDataProvider.GetByIndex(questIndex);

        var res = await _questRepository.AcceptQuest(userId, quest);

        return res;
    }

    public async Task CancelQuest(Guid userId, Guid questProgressId)
    {
        await _questRepository.CancelQuest(userId, questProgressId);
    }

    /// <summary>
    ///     Check if a player at id progress in quest if he kill a spaceship
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="spaceship"></param>
    /// <exception cref="Exception"></exception>
    public async Task CheckQuestSpaceshipDestroyed(Guid userId, Spaceship spaceship)
    {
        var questsInProgress =
            await _questRepository.GetQuestsByObjective(EnumQuestObjectiveType.AttackSpaceship, userId);

        foreach (var questInProgress in questsInProgress)
        {
            if (questInProgress.Data == null) throw new Exception($"[{nameof(QuestService)}] : quest data is null");

            //Get all objectives of type AttackSpaceship with the same owner type and faction as the destroyed spaceship
            var objectives = questInProgress.Data.Objectives
                .Where(objective => objective.OwnerType == spaceship.OwnerType &&
                                    objective.Faction == spaceship.Faction &&
                                    objective.Type == EnumQuestObjectiveType.AttackSpaceship);

            //Increment the progress of the objective
            await _questRepository.IncrementQuestProgress(questInProgress.Id, objectives);
        }
    }

    public async Task CompleteQuest(Guid userId, Guid questId)
    {
        var quest = await _questRepository.GetById(userId, questId);

        if (quest is null) throw new Exception($"[{nameof(QuestService)}] : quest is null");

        quest.Data = _questDataProvider.GetByIndex(quest.Index);

        if (quest.Data is null) throw new Exception($"[{nameof(QuestService)}] : quest data is null");

        //Set quest finished
        await _questRepository.CompleteQuest(userId, questId);

        //Get rewards
        var rewards = quest.Data.OneTimeModifiers;

        //Apply rewards
        foreach (var reward in rewards)
            await _gameResourceService.AddResourceToPlayer(reward.Index, reward.Amount, userId);
    }
}