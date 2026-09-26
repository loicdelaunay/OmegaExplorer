using OmegaExplorer.Server.Services._Game._core;
using OmegaExplorer.Server.Services._Game.Celebrities.Models.Interfaces;

namespace OmegaExplorer.Server.Services._Game.Celebrities;

public class CelebrityDataProvider : GameDataProvider<IDynamicItemCelebrity>
{
    public CelebrityDataProvider(ILogger<CelebrityDataProvider> logger) : base(logger)
    {
    }
}