using OmegaExplorer.Server.Services._Core.Models.Contracts._inherits;
using OmegaExplorer.Server.Services._Game.Effects.Models.Contracts;

namespace OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipSkill.Models.Contracts;

public class ResponseDynamicItemSpaceshipSkill : ResponseDynamicItem
{
    public List<ResponseEffect> Effects { get; set; } = new();
}