using OmegaExplorer.Server.Services._Core.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Cycles.Hubs;
using OmegaExplorer.Shared.Models.Classes;
using System.Reflection;

namespace OmegaExplorer.Server.Services._Game.Cycles.Modules.CycleProcess;

public class CycleTaskService : IProgressReporter
{
    private readonly CycleBackgroundService _cycleBackgroundService;

    private readonly IServiceProvider _serviceProvider;

    private ProgressInfo? _currentProgress;
    private List<ICycleTask> _tasks = new();

    // Timer to call the cycle event
    private Timer? _timerReportProgress;

    public bool IsComputing;

    public CycleTaskService(IServiceProvider serviceProvider, CycleBackgroundService cycleBackgroundService)
    {
        _serviceProvider = serviceProvider;
        _cycleBackgroundService = cycleBackgroundService;

        Initialize();
    }


    public void ReportProgress(string message, ProgressInfo.ProcessState state, int totalSteps, int currentStep,
        int stepProgress)
    {
        _currentProgress = new ProgressInfo
        {
            Message = message,
            State = state,
            TotalSteps = totalSteps,
            CurrentStep = currentStep,
            StepProgress = stepProgress
        };
    }

    public void Initialize()
    {
        InitializeTasks();
        _timerReportProgress = new Timer(TimerElapsed, null, 2000, Timeout.Infinite);
    }

    private void InitializeTasks()
    {
        // Use reflection to find all implementations of <see cref="ICycleTask"/>
        var taskTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(ICycleTask).IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false });

        foreach (var type in taskTypes)
            if (Activator.CreateInstance(type) is ICycleTask task)
                _tasks.Add(task);

        // Manage priorities and get only task need to be run at this cycle
        _tasks = _tasks
            .OrderByDescending(p => p.Priority)
            .ToList();
    }

    public async Task RunTasks()
    {
        //Important to throw fatal exception because a turn can be corrupted if a new turn is started while the previous one is still running <!>
        if (IsComputing) throw new Exception("Cycle task is already running ???");

        IsComputing = true;


        if (!_cycleBackgroundService.IsPaused)
        {
            HashSet<Type> executedProcesses = new();

            var currentCycle = CycleBackgroundService.CurrentCycle;

            foreach (var task in _tasks)
            {
                if (task.ExecuteEachXCycle != null && currentCycle % task.ExecuteEachXCycle != 0) continue;

                await RunTaskWithDependencies(task, executedProcesses);
            }
        }

        IsComputing = false;
    }

    private async Task RunTaskWithDependencies(ICycleTask task, HashSet<Type> executedProcesses)
    {
        if (task.Dependencies != null)
            // Run dependencies first
            foreach (var dependencyType in task.Dependencies)
                if (!executedProcesses.Contains(dependencyType))
                {
                    var dependencyProcess = _tasks.FirstOrDefault(p => p.GetType() == dependencyType);
                    if (dependencyProcess != null)
                        await RunTaskWithDependencies(dependencyProcess, executedProcesses);
                    else
                        ReportProgress($"Missing dependency : {dependencyType.Name}", ProgressInfo.ProcessState.Failed,
                            0, 0, 0);
                }

        // Run the task if not already done
        if (!executedProcesses.Contains(task.GetType()))
        {
            ReportProgress($"Running the task : {task.GetType().Name}", ProgressInfo.ProcessState.Running, 0, 0, 0);
            try
            {
                await task.Run(_serviceProvider, this);
                ReportProgress($"Task completed : {task.GetType().Name}", ProgressInfo.ProcessState.Completed, 0, 0,
                    100);
            }
            catch (Exception ex)
            {
                ReportProgress($"Task failure {task.GetType().Name} : {ex.Message}", ProgressInfo.ProcessState.Failed,
                    0, 0, 0);
            }

            executedProcesses.Add(task.GetType());
        }
    }

    private async void TimerElapsed(object? state)
    {
        try
        {
            await NotifyClientsProgress();
        }
        catch (Exception e)
        {
            Log.Logger.Error(e, "Error while notifying clients about cycle progress");
        }
    }

    private async Task NotifyClientsProgress()
    {
        if (!IsComputing) return;

        if (_currentProgress == null) return;

        await CycleProcessHub.NotifyCycleProgress(_currentProgress);
    }
}