using OmegaExplorer.Server.Services._Core.Models.Interfaces;

namespace OmegaExplorer.Server.Services._Game._core.Models.Interfaces;

public interface IGameManager : IInitializableAsync
{
    int Priority { get; set; }

    public Task PostInitialize();
}