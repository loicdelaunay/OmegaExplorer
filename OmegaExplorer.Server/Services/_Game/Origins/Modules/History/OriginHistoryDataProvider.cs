using OmegaExplorer.Server.Services._Game._core;
using OmegaExplorer.Server.Services._Game.Origins.Modules.History.Models._interfaces;

namespace OmegaExplorer.Server.Services._Game.Origins.Modules.History;

public class OriginHistoryDataProvider : GameDataProvider<IDynamicItemOriginHistory>
{
    public OriginHistoryDataProvider(ILogger<OriginHistoryDataProvider> logger) : base(logger)
    {
    }
}