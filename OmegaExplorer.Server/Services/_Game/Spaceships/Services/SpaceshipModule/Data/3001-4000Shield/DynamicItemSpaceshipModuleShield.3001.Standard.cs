using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Brands.Data;
using OmegaExplorer.Server.Services._Game.Brands.Models._interfaces;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Classes;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Enums;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipSkill.Data._1001_2000Shield;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipSkill.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Data._3001_4000Shield;

public class DynamicItemSpaceshipModuleShieldStandard : IDynamicItemSpaceshipModule
{
    public const int INDEX = 3001;
    public static readonly DynamicItemSpaceshipModuleShieldStandard Instance = new();

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "Standard Shield";

    public string? Description { get; set; } =
        "A small tesla coil with a basic technology to modular energy around a target shape.";

    public EnumDataKnowledge Knowledge { get; set; } = EnumDataKnowledge.Unlocked;
    public EnumRarity Rarity { get; set; } = EnumRarity.Common;

    public List<IDynamicItemSpaceshipSkill> Skills { get; set; } = new()
    {
        DynamicItemSpaceshipSkillShield.Instance
    };

    public EnumSpaceshipModuleType Type { get; set; } = EnumSpaceshipModuleType.Module;
    public IDynamicItemBrand Brand { get; set; } = DynamicItemBrandStellarDynamics.Instance;
    public int Health { get; set; } = 150;
    public List<SpaceshipModuleModifier> Modifiers { get; set; } = new();
}