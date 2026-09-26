using OmegaExplorer.Server.Services._Core.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Scenarios;
using OmegaExplorer.Shared.Models.Classes;

namespace OmegaExplorer.Server.Services._Game.Cycles.Modules.CycleProcess.Tasks.Scenario;

public class CycleTaskExecuteScenarioCheckMandatoryQuest : ICycleTask
{
    public int Priority { get; }

    public IEnumerable<Type>? Dependencies { get; }

    public async Task Run(IServiceProvider serviceProvider, IProgressReporter reporter)
    {
        reporter.ReportProgress("Executing checking mandatory quests...", ProgressInfo.ProcessState.Running, 1, 0, 0);

        await using var scope = serviceProvider.CreateAsyncScope();
        var scenarioManager = scope.ServiceProvider.GetRequiredService<ScenarioService>();

        reporter.ReportProgress("Getting users with mandatory quests not done...", ProgressInfo.ProcessState.Running, 1,
            0, 0);
        var usersWithMandatoryQuestNotDone = await scenarioManager.GetUsersWithMandatoryQuestNotDone();

        reporter.ReportProgress("Assigning mandatory quests to users...", ProgressInfo.ProcessState.Running, 1, 0, 0);

        var countUser = 0;
        foreach (var userWithMandatoryQuestNotDone in usersWithMandatoryQuestNotDone)
        {
            var countQuest = 0;
            foreach (var indexQuestMandatoryNotDone in userWithMandatoryQuestNotDone.Value)
            {
                await scenarioManager.AssignScenarioToAPlayer(indexQuestMandatoryNotDone,
                    userWithMandatoryQuestNotDone.Key.Id);
                countQuest++;
            }

            countUser++;
            reporter.ReportProgress(
                $"Assigned {countQuest} mandatory quests to user {userWithMandatoryQuestNotDone.Key.Id}",
                ProgressInfo.ProcessState.Running, usersWithMandatoryQuestNotDone.Count, countUser,
                countQuest / usersWithMandatoryQuestNotDone.Count);
        }

        reporter.ReportProgress("Executed", ProgressInfo.ProcessState.Completed, 1, 1, 100);
    }

    public int? ExecuteEachXCycle { get; } = 2;
}