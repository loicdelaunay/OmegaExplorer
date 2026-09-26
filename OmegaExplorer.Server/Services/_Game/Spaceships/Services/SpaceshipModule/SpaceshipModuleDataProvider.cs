using OmegaExplorer.Server.Services._Game._core;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Interfaces;

namespace OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule;

public class SpaceshipModuleDataProvider : GameDataProvider<IDynamicItemSpaceshipModule>
{
    public SpaceshipModuleDataProvider(ILogger<SpaceshipModuleDataProvider> logger) : base(logger)
    {
    }
}