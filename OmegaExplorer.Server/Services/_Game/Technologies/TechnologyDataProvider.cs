using OmegaExplorer.Server.Services._Game._core;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Interfaces;

namespace OmegaExplorer.Server.Services._Game.Technologies;

public class TechnologyDataProvider : GameDataProvider<IDynamicItemTechnology>
{
    public TechnologyDataProvider(ILogger<TechnologyDataProvider> logger) : base(logger)
    {
        PostInitialize();
    }

    public void PostInitialize()
    {
        foreach (var tech in Data.Values) tech.Feed();
    }
}