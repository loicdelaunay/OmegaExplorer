using OmegaExplorer.Server.Services._Core.Models.Interfaces;

namespace OmegaExplorer.Server.Services._Game.Cycles.Modules.CycleProcess;

public interface ICycleTask
{
    /// <summary>
    ///     Higher number = higher priority
    /// </summary>
    int Priority { get; }

    IEnumerable<Type>? Dependencies { get; }

    public int? ExecuteEachXCycle { get; }
    Task Run(IServiceProvider serviceProvider, IProgressReporter reporter);
}