namespace OmegaExplorer.Server.Services._Core.Models.Interfaces;

/// <summary>
///     Class that can be initialized and uninitialized
/// </summary>
public interface IInitializable : IInitializableState
{
    public void Initialize();
    public void Uninitialize();
}