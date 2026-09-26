using OmegaExplorer.Server.Services._Core.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Spaceships;
using OmegaExplorer.Shared.Models.Classes;

namespace OmegaExplorer.Server.Services._Game.Cycles.Modules.CycleProcess.Tasks.Spaceship;

public class CycleTaskExecuteSpaceshipIsDestroyed : ICycleTask
{
    public int Priority { get; }

    public IEnumerable<Type>? Dependencies { get; }

    public int? ExecuteEachXCycle { get; }

    public async Task Run(IServiceProvider serviceProvider, IProgressReporter reporter)
    {
        reporter.ReportProgress("Executing spaceship orders...", ProgressInfo.ProcessState.Running, 1, 0, 0);

        await using var scope = serviceProvider.CreateAsyncScope();
        var spaceshipRepository = scope.ServiceProvider.GetRequiredService<SpaceshipRepository>();
        var cycleBackgroundService = scope.ServiceProvider.GetRequiredService<CycleBackgroundService>();

        reporter.ReportProgress("Listing spaceship...", ProgressInfo.ProcessState.Running, 1, 0, 0);
        var spaceshipsDestroyed = await spaceshipRepository.GetSpaceshipsDestroyed();
        reporter.ReportProgress("Spaceships listed", ProgressInfo.ProcessState.Running, spaceshipsDestroyed.Count, 0,
            0);

        var spaceshipService = scope.ServiceProvider.GetRequiredService<SpaceshipService>();

        var count = 0;
        foreach (var spaceship in spaceshipsDestroyed)
            if (spaceship.CycleWhenRemove <= CycleBackgroundService.CurrentCycle)
            {
                await spaceshipService.RemoveSpaceship(spaceship.Id);

                count++;
                reporter.ReportProgress("Spaceships listed", ProgressInfo.ProcessState.Running,
                    spaceshipsDestroyed.Count, count, 0);
            }

        reporter.ReportProgress("Spaceship orders executed", ProgressInfo.ProcessState.Completed, 1, 1, 100);
    }
}