namespace OmegaExplorer.Server.Services._Core.Models.Interfaces;

/// <summary>
///     Class that can be initialized and uninitialized
/// </summary>
public interface IInitializableAsync : IInitializableState
{
    public Task InitializeAsync();
    public Task UninitializeAsync();
}