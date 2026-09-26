using OmegaExplorer.Server.Services._Game._core;
using OmegaExplorer.Server.Services._Game.Buildings.Models.Interfaces;

namespace OmegaExplorer.Server.Services._Game.Buildings;

public class BuildingDataProvider : GameDataProvider<IDynamicItemBuilding>
{
    public BuildingDataProvider(ILogger<BuildingDataProvider> logger) : base(logger)
    {
    }
}