using OmegaExplorer.Server.Services._Game._core;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Interfaces;

namespace OmegaExplorer.Server.Services._Game.GameResources;

public class GameResourceDataProvider : GameDataProvider<IDynamicItemGameResource>
{
    public GameResourceDataProvider(ILogger<GameResourceDataProvider> logger) : base(logger)
    {
    }
}