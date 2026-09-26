using OmegaExplorer.Server.Services._Game.Cycles.Hubs;
using OmegaExplorer.Server.Services._Game.Cycles.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Cycles.Models.Entities;
using OmegaExplorer.Server.Services._Game.Cycles.Modules.CycleProcess;
using OmegaExplorer.Server.Services.Databases;

namespace OmegaExplorer.Server.Services._Game.Cycles;

public class CycleBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public CycleBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    ///     Current cycle count
    /// </summary>
    public static long CurrentCycle { get; private set; } = -1;

    /// <summary>
    ///     If the cycle processing is paused ( skip the cycle processing when true )
    /// </summary>
    public bool IsPaused { get; private set; }

    /// <summary>
    ///     Current cycle start date
    /// </summary>
    public static DateTime DateCurrentCycle { get; private set; }

    /// <summary>
    ///     Duration of a cycle
    /// </summary>
    public TimeSpan CycleDuration { get; set; } = TimeSpan.FromMinutes(2);

    /// <summary>
    ///     Next cycle start date
    /// </summary>
    public static DateTime DateNextCycle { get; private set; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested) await ExecuteCycle(stoppingToken);
    }

    private async Task ExecuteCycle(CancellationToken stoppingToken = default)
    {
        await StartCycle(stoppingToken);

        // Wait until cycle ends, unless it's a forced skip
        await Task.Delay(CycleDuration, stoppingToken);

        await using var scope = _serviceProvider.CreateAsyncScope();
        var hub = scope.ServiceProvider.GetRequiredService<CycleHub>();
        var cycleTaskService = scope.ServiceProvider.GetRequiredService<CycleTaskService>();

        await hub.NotifyCycleEnded();
        await cycleTaskService.RunTasks();
    }

    /// <summary>
    ///     Start a new cycle
    /// </summary>
    private async Task StartCycle(CancellationToken stoppingToken = default)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        var cycleHub = scope.ServiceProvider.GetRequiredService<CycleHub>();

        DateCurrentCycle = DateTime.Now;
        DateNextCycle = DateCurrentCycle.AddMinutes(CycleDuration.TotalMinutes);

        // Update cycle count in database
        var cycleCountEntity = db.CycleStates.FirstOrDefault();
        if (cycleCountEntity == null)
        {
            cycleCountEntity = new CycleState
            {
                CycleCount = 0
            };

            db.CycleStates.Add(cycleCountEntity);
            await db.SaveChangesAsync(stoppingToken);
        }
        else
        {
            cycleCountEntity.CycleCount++;
        }

        await db.SaveChangesAsync(stoppingToken);

        // Update internal cycle count
        if (CurrentCycle == -1)
            CurrentCycle = cycleCountEntity.CycleCount;
        else
            CurrentCycle++;

        await cycleHub.NotifyCycleStarted();
    }


    public ResponseCycleState GetCycleState()
    {
        return new ResponseCycleState
        {
            CycleCount = CurrentCycle,
            CurrentCycle = DateCurrentCycle,
            NextCycle = DateNextCycle
        };
    }

    public async Task ForceSkipCycle()
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var hub = scope.ServiceProvider.GetRequiredService<CycleHub>();
        var cycleTaskService = scope.ServiceProvider.GetRequiredService<CycleTaskService>();

        await hub.NotifyCycleEnded();
        await cycleTaskService.RunTasks();

        await StartCycle();
    }


    public long GetElapsedTurns(long turnToCompare)
    {
        return CurrentCycle - turnToCompare;
    }

    public bool IsCyclePaused()
    {
        return IsPaused;
    }

    public void PauseCycle()
    {
        IsPaused = true;
    }

    public void ResumeCycle()
    {
        IsPaused = false;
    }
}