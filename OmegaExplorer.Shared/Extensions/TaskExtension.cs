namespace OmegaExplorer.Server.Extensions;

/// <summary>
///     Extend task capability
/// </summary>
public static class TaskExtension
{
    /// <summary>
    ///     Implement cancellation system into any task just pass a cancellation token
    /// </summary>
    /// <param name="task"> </param>
    /// <param name="cancellationToken"> </param>
    /// <typeparam name="T"> </typeparam>
    /// <returns> </returns>
    public static Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
    {
        return task.IsCompleted // fast-path optimization
            ? task
            : task.ContinueWith(
                completedTask => completedTask.GetAwaiter().GetResult(),
                cancellationToken,
                TaskContinuationOptions.ExecuteSynchronously,
                TaskScheduler.Default);
    }
}