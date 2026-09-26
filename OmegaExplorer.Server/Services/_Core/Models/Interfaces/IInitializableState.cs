namespace OmegaExplorer.Server.Services._Core.Models.Interfaces;

/// <summary>
///     Class that can be initialized and uninitialized
/// </summary>
public interface IInitializableState
{
    public bool IsInitialized { get; protected set; }
}