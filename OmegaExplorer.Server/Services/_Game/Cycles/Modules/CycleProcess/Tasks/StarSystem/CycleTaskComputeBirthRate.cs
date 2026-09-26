using OmegaExplorer.Server.Services._Core.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.StarSystems;

namespace OmegaExplorer.Server.Services._Game.Cycles.Modules.CycleProcess.Tasks.StarSystem;

public class CycleTaskComputeBirthRate : ICycleTask
{
    public int Priority { get; }
    public IEnumerable<Type>? Dependencies { get; }

    public async Task Run(IServiceProvider serviceProvider, IProgressReporter reporter)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var starSystemManager = scope.ServiceProvider.GetRequiredService<StarSystemService>();

        await starSystemManager.ComputeBirthRateForAllStarSystems();
    }

    public int? ExecuteEachXCycle { get; } = 5;
}