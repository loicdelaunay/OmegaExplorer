namespace OmegaExplorer.Shared.Models.Classes;

public class ProgressInfo
{
    public string? Message { get; set; }

    public ProcessState State { get; set; }

    public int TotalSteps { get; set; }

    public int CurrentStep { get; set; }

    public double StepProgress { get; set; }

    public enum ProcessState
    {
        NotStarted,
        Running,
        Completed,
        Failed
    }
}