using OmegaExplorer.Server.Services._Game._core;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint.Models.Interfaces;

namespace OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint;

public class SpaceshipBlueprintDataProvider : GameDataProvider<IDynamicItemSpaceshipBlueprint>
{
    public SpaceshipBlueprintDataProvider(ILogger<SpaceshipBlueprintDataProvider> logger) : base(logger)
    {
    }
}