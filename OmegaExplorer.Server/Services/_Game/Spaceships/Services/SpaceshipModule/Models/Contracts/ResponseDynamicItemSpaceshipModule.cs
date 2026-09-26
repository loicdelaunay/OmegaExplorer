using OmegaExplorer.Server.Services._Core.Models.Contracts._inherits;
using OmegaExplorer.Server.Services._Game.Brands.Models._interfaces;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Enums;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipSkill.Models.Contracts;

namespace OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Contracts;

public class ResponseDynamicItemSpaceshipModule : ResponseDynamicItem
{
    public EnumSpaceshipModuleType Type { get; set; }

    public List<ResponseDynamicItemSpaceshipSkill> Skills { get; set; } = new();

    public IDynamicItemBrand? Brand { get; set; }

    public int X { get; set; }
    public int Y { get; set; }

    public int Health { get; set; }

    public List<ResponseSpaceshipModuleModifier> Modifiers { get; set; }
}