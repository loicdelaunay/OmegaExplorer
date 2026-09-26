using OmegaExplorer.Server.Services._Core.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Spaceships;
using OmegaExplorer.Server.Services.Loggers.Models.Enums;
using OmegaExplorer.Shared.Models.Classes;

namespace OmegaExplorer.Server.Services._Game.Cycles.Modules.CycleProcess.Tasks.StarSystem;

public class CycleTaskRepairSpaceshipInStarSystem : ICycleTask
{
    public int Priority { get; }

    public IEnumerable<Type>? Dependencies { get; }

    public async Task Run(IServiceProvider serviceProvider, IProgressReporter reporter)
    {
        try
        {
            await using var scope = serviceProvider.CreateAsyncScope();
            var spaceshipService = scope.ServiceProvider.GetRequiredService<SpaceshipService>();

            reporter.ReportProgress("Repairing spaceship in star system...", ProgressInfo.ProcessState.Running, 1, 0,
                0);

            //Get all spaceship in friendly star system
            var spaceshipsInFriendlyStarSystem = await spaceshipService.GetSpaceshipsInFriendlyStarSystem();
            reporter.ReportProgress("Spaceships listed", ProgressInfo.ProcessState.Running,
                spaceshipsInFriendlyStarSystem.Count, 0, 0);

            var count = 0;
            foreach (var spaceship in spaceshipsInFriendlyStarSystem)
            {
                //Repair spaceship
                await spaceshipService.RepairSpaceship(spaceship.Id);

                count++;
                reporter.ReportProgress("Spaceships repaired", ProgressInfo.ProcessState.Running,
                    spaceshipsInFriendlyStarSystem.Count, count, 0);
            }

            reporter.ReportProgress("Spaceship repaired in star system", ProgressInfo.ProcessState.Completed, 1, 1,
                100);
        }
        catch (Exception e)
        {
            Log.Logger.Error($"Error in CycleProcessRepairSpaceshipInStarSystem : {e}", EnumLogSeverity.Error);
        }
    }

    public int? ExecuteEachXCycle { get; }
}