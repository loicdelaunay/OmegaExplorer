using OmegaExplorer.Server.Services._Core.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Battles;
using OmegaExplorer.Shared.Models.Classes;

namespace OmegaExplorer.Server.Services._Game.Cycles.Modules.CycleProcess.Tasks.Battle;

public class CycleTaskExecuteBattleResolution : ICycleTask
{
    public int Priority => 100;

    public IEnumerable<Type>? Dependencies { get; }

    public async Task Run(IServiceProvider serviceProvider, IProgressReporter reporter)
    {
        reporter.ReportProgress("Executing battle resolution...", ProgressInfo.ProcessState.Running, 1, 0, 0);

        await using var scope = serviceProvider.CreateAsyncScope();
        var battleService = scope.ServiceProvider.GetRequiredService<BattleService>();

        //Get all battle 
        var battles = await battleService.GetBattlesNotFinished();

        var count = 0;
        //Resolve all battles
        foreach (var battle in battles)
        {
            await battleService.ResolveBattle(battle);
            count++;
            reporter.ReportProgress("Battle resolution in progress", ProgressInfo.ProcessState.Running, battles.Count,
                count, 0);
        }

        reporter.ReportProgress("Battle resolution executed", ProgressInfo.ProcessState.Completed, 1, 1, 100);
    }

    public int? ExecuteEachXCycle { get; }
}