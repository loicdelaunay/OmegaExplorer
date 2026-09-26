using OmegaExplorer.Server.Services._Game._core;
using OmegaExplorer.Server.Services._Game.Scenarios.Models.Interfaces;

namespace OmegaExplorer.Server.Services._Game.Scenarios;

public class ScenarioDataProvider : GameDataProvider<IDynamicItemScenario>
{
    public ScenarioDataProvider(ILogger<ScenarioDataProvider> logger) : base(logger)
    {
    }
}