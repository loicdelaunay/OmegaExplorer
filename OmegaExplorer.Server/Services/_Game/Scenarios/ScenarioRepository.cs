using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.Services._Core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Scenarios.Models.Entities;
using OmegaExplorer.Server.Services._Game.Scenarios.Models.Enums;
using OmegaExplorer.Server.Services.Databases;
using OmegaExplorer.Server.Services.Users.Models.Entities;
using Xunit;

namespace OmegaExplorer.Server.Services._Game.Scenarios;

public class ScenarioRepository : RepositoryCustom
{
    private readonly DatabaseContext _databaseContext;
    private readonly ScenarioDataProvider _scenarioDataProvider;

    public ScenarioRepository(ScenarioDataProvider scenarioDataProvider, DatabaseContext databaseContext)
    {
        _scenarioDataProvider = scenarioDataProvider;
        _databaseContext = databaseContext;
    }

    public async Task AddScenarioToPlayer(int indexScenario, Guid userId)
    {
        var scenario = _scenarioDataProvider.GetByIndex(indexScenario);
        if (scenario == null) throw new Exception("Scenario not found");


        UserScenarioProgress userScenario = new()
        {
            Index = scenario.Index,
            UserId = userId,
            IsResolved = false
        };

        _databaseContext.ScenarioProgresses.Add(userScenario);

        await _databaseContext.SaveChangesAsync();
    }

    public async Task<List<UserScenarioProgress>> GetScenariosByUser(Guid userId, bool includeComplete = false)
    {
        var query = _databaseContext.ScenarioProgresses
            .Where(x => x.UserId == userId);

        if (!includeComplete) query = query.Where(x => !x.IsResolved);

        var scenarios = await query.ToListAsync();

        return scenarios;
    }

    public async ValueTask<Dictionary<User, List<int>>> GetUsersWithMandatoryQuestNotDone()
    {
        var users = await _databaseContext.Users.ToListAsync();
        var quests = _scenarioDataProvider
            .GetAll()
            .Where(scenario => scenario.ScenarioDistribution == EnumScenarioDistribution.Mandatory)
            .ToList();

        Dictionary<User, List<int>> usersWithMandatoryQuestNotDone = new();

        foreach (var user in users)
        {
            List<int> listScenario = new();

            foreach (var quest in quests)
                //If not exist in user progress add it
                if (!_databaseContext.ScenarioProgresses.Any(x => x.Index == quest.Index && x.UserId == user.Id))
                    listScenario.Add(quest.Index);

            usersWithMandatoryQuestNotDone.Add(user, listScenario);
        }

        return usersWithMandatoryQuestNotDone;
    }

    public async Task<UserScenarioProgress> SetScenarioChoice(Guid userId, Guid scenarioId, int dialogIndex,
        int choiceIndex)
    {
        var scenarioProgress =
            _databaseContext.ScenarioProgresses.FirstOrDefault(x => x.UserId == userId && x.Id == scenarioId);

        if (scenarioProgress == null) throw new Exception("Scenario not found");

        scenarioProgress.Choices.Add(dialogIndex, choiceIndex);

        #region Check if the scenario is resolved

        var scenario = _scenarioDataProvider.GetByIndex(scenarioProgress.Index);

        Assert.NotNull(scenario);

        //-2 because -1 + -1 because last dialog can be from an IA so it's automatically resolved
        // I'm sorry moi du future si c'est pas clair :'(
        if (dialogIndex >= scenario.Dialogs.Count - 2) scenarioProgress.IsResolved = true;

        #endregion

        await _databaseContext.SaveChangesAsync();

        return scenarioProgress;
    }
}