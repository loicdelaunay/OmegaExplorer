using OmegaExplorer.Server.Services._Game._core.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Effects;

namespace OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipSkill.Models.Interfaces;

public interface IDynamicItemSpaceshipSkill : IDynamicItem
{
    public List<Effect> Effects { get; set; }
}