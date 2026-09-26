using OmegaExplorer.Server.Services._Game._core.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Brands.Models._interfaces;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Classes;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Enums;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipSkill.Models.Interfaces;

namespace OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Interfaces;

public interface IDynamicItemSpaceshipModule : IDynamicItem
{
    public List<IDynamicItemSpaceshipSkill> Skills { get; set; }

    public EnumSpaceshipModuleType Type { get; set; }

    public IDynamicItemBrand Brand { get; set; }

    /// <summary>
    ///     How much damage the module can take before being destroyed
    /// </summary>
    public int Health { get; set; }

    public List<SpaceshipModuleModifier> Modifiers { get; set; }
}