using OmegaExplorer.Server.Services._Game.Notifications;
using OmegaExplorer.Server.Services._Game.Recompenses;
using OmegaExplorer.Server.Services._Game.Scenarios.Models.Entities;
using OmegaExplorer.Server.Services.Users.Models.Entities;

namespace OmegaExplorer.Server.Services._Game.Scenarios;

public class ScenarioService
{
    private readonly NotificationService _notificationService;
    private readonly RecompenseService _recompenseService;

    private readonly ScenarioRepository _scenarioRepository;

    private readonly IServiceProvider _serviceProvider;

    public ScenarioService(NotificationService notificationService, RecompenseService recompenseService,
        ScenarioRepository scenarioRepository, IServiceProvider serviceProvider)
    {
        _notificationService = notificationService;
        _recompenseService = recompenseService;
        _scenarioRepository = scenarioRepository;
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    ///     Assign a scenario to a player
    /// </summary>
    /// <param name="indexScenario"></param>
    /// <param name="userId"></param>
    public async Task AssignScenarioToAPlayer(int indexScenario, Guid userId)
    {
        await _scenarioRepository.AddScenarioToPlayer(indexScenario, userId);

        //Notify the player that a new scenario is available

        var notification = await _notificationService.GetNotificationSummaryByUser(userId);

        await _notificationService.HubNotifyNotificationSummaryChanged(userId, notification);
    }

    /// <summary>
    ///     Return the list of user that have not done the mandatory quest
    /// </summary>
    /// <returns></returns>
    public async Task<Dictionary<User, List<int>>> GetUsersWithMandatoryQuestNotDone()
    {
        var usersWithMandatoryQuestNotDone = await _scenarioRepository.GetUsersWithMandatoryQuestNotDone();

        return usersWithMandatoryQuestNotDone;
    }

    public async Task<List<UserScenarioProgress>> GetScenariosByUser(Guid userId, bool includeCompleted = false)
    {
        var scenarioProgresses = await _scenarioRepository.GetScenariosByUser(userId, includeCompleted);

        return scenarioProgresses;
    }

    public async Task<UserScenarioProgress> SetScenarioChoice(User user, Guid scenarioId,
        int dialogIndex, int choiceIndex)
    {
        var scenarioProgress =
            await _scenarioRepository.SetScenarioChoice(user.Id, scenarioId, dialogIndex, choiceIndex);

        //Get the current dialog
        var currentDialog = await scenarioProgress.GetCurrentDialog(_serviceProvider);

        if (currentDialog.Recompenses.Any())
            foreach (var recompense in currentDialog.Recompenses)
                try
                {
                    await _recompenseService.GiveRecompenseToPlayer(user.Id, recompense);
                }
                catch (Exception e)
                {
                    Log.Error(e, "Error in SetScenarioChoice for user {User} and recompense {Recompense}", user,
                        recompense);
                }

        return scenarioProgress;
    }
}