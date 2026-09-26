using OmegaExplorer.Shared.Models.Classes;

namespace OmegaExplorer.Server.Services._Core.Models.Interfaces;

public interface IProgressReporter
{
    /// <summary>
    ///
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="state"></param>
    /// <param name="totalSteps"></param>
    /// <param name="currentStep"></param>
    /// <param name="stepProgress">between 0-1 progress of the current step</param>
    void ReportProgress(string message, ProgressInfo.ProcessState state, int totalSteps, int currentStep, int stepProgress);

}