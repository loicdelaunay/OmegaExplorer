using OmegaExplorer.Server.Services._Core.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Orders;
using OmegaExplorer.Shared.Models.Classes;

namespace OmegaExplorer.Server.Services._Game.Cycles.Modules.CycleProcess.Tasks.Spaceship;

public class CycleTaskExecuteSpaceshipOrders : ICycleTask
{
    public int Priority { get; }

    public IEnumerable<Type>? Dependencies { get; }

    public async Task Run(IServiceProvider serviceProvider, IProgressReporter reporter)
    {
        reporter.ReportProgress("Executing spaceship orders...", ProgressInfo.ProcessState.Running, 1, 0, 0);

        await using var scope = serviceProvider.CreateAsyncScope();
        var orderableService = scope.ServiceProvider.GetRequiredService<OrderExecuteService>();

        //Execute all things that need to be done at the end of a cycle
        await orderableService.Execute();

        reporter.ReportProgress("Spaceship orders executed", ProgressInfo.ProcessState.Completed, 1, 1, 100);
    }

    public int? ExecuteEachXCycle { get; }
}