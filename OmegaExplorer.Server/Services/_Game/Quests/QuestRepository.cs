using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.Services._Core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Quests.Models.Classes;
using OmegaExplorer.Server.Services._Game.Quests.Models.Entities;
using OmegaExplorer.Server.Services._Game.Quests.Models.Enums;
using OmegaExplorer.Server.Services._Game.Quests.Models.Interfaces;
using OmegaExplorer.Server.Services.Databases;

namespace OmegaExplorer.Server.Services._Game.Quests;

public class QuestRepository : RepositoryCustom
{
    private readonly DatabaseContext _databaseContext;
    private readonly ILogger<QuestRepository> _logger;

    private readonly QuestDataProvider _questDataProvider;

    public QuestRepository(QuestDataProvider questDataProvider, DatabaseContext databaseContext,
        ILogger<QuestRepository> logger)
    {
        _questDataProvider = questDataProvider;
        _databaseContext = databaseContext;
        _logger = logger;
    }

    public async Task<List<QuestProgress>> GetAllQuestsInProgress(Guid userId)
    {
        var quests = await _databaseContext.QuestProgresses
            .Where(x => x.UserId == userId && x.Progress == EnumQuestProgress.Started)
            .ToListAsync();

        return quests;
    }

    public async Task<List<QuestProgress>> GetAllQuestsDone(Guid userId)
    {
        var quests = await _databaseContext.QuestProgresses
            .Where(x => x.UserId == userId && x.Finished)
            .ToListAsync();

        return quests;
    }

    public async Task<QuestProgress> AcceptQuest(Guid userId, IDynamicItemQuest? quest)
    {
        ArgumentNullException.ThrowIfNull(quest);


        QuestProgress questProgress = new()
        {
            UserId = userId,
            Index = quest.Index,
            Progress = EnumQuestProgress.Started
        };

        _databaseContext.QuestProgresses.Add(questProgress);
        await _databaseContext.SaveChangesAsync();

        return questProgress;
    }

    public async Task CancelQuest(Guid userId, Guid questId)
    {
        var quest = await _databaseContext.QuestProgresses
            .FirstOrDefaultAsync(x => x.UserId == userId && x.Id == questId);

        if (quest is null) return;

        _databaseContext.QuestProgresses.Remove(quest);
        await _databaseContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<QuestProgress>> GetQuestsByObjective(EnumQuestObjectiveType attackSpaceship,
        Guid userId)
    {
        var quests = await _databaseContext.QuestProgresses
            .Where(questProgress => questProgress.UserId == userId)
            .ToListAsync();

        foreach (var quest in quests) quest.Data = _questDataProvider.GetByIndex(quest.Index);

        quests = quests.Where(quest => quest.Data != null && quest.Data.Objectives.Any(x => x.Type == attackSpaceship))
            .ToList();

        return quests;
    }

    public async Task IncrementQuestProgress(Guid questInProgressId, IEnumerable<QuestObjective> objectives)
    {
        var questInProgress = await _databaseContext.QuestProgresses
            .Include(x => x.QuestObjectiveProgresses)
            .FirstOrDefaultAsync(x => x.Id == questInProgressId);

        if (questInProgress is null)
        {
            _logger.LogWarning($"QuestProgress with id {questInProgressId} not found.");
            return;
        }

        //For each objective progress in questInProgress create a new progress if null else increment
        foreach (var objective in objectives)
        {
            var progress = questInProgress.QuestObjectiveProgresses
                .FirstOrDefault(x => x.Index == objective.Index);

            if (progress is null)
            {
                progress = new QuestObjectiveProgress
                {
                    Index = objective.Index,
                    Amount = 1
                };

                questInProgress.QuestObjectiveProgresses.Add(progress);
            }
            else
            {
                progress.Amount++;
            }
        }

        await _databaseContext.SaveChangesAsync();
    }

    public async Task CompleteQuest(Guid userId, Guid questId)
    {
        var quest = await _databaseContext.QuestProgresses
            .FirstOrDefaultAsync(x => x.UserId == userId && x.Id == questId);

        if (quest is null) return;

        quest.Progress = EnumQuestProgress.Completed;

        await _databaseContext.SaveChangesAsync();
    }

    public async Task<QuestProgress?> GetById(Guid userId, Guid questId)
    {
        var quest = await _databaseContext.QuestProgresses
            .FirstOrDefaultAsync(x => x.UserId == userId && x.Id == questId);

        return quest;
    }
}