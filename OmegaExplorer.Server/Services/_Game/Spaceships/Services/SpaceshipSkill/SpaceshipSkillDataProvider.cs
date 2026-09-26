using OmegaExplorer.Server.Services._Game._core;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipSkill.Models.Interfaces;

namespace OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipSkill;

public class SpaceshipSkillDataProvider : GameDataProvider<IDynamicItemSpaceshipSkill>
{
    public SpaceshipSkillDataProvider(ILogger<SpaceshipSkillDataProvider> logger) : base(logger)
    {
    }
}