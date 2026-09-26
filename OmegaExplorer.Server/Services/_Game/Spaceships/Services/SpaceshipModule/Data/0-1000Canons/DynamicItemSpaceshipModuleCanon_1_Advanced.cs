using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Brands.Data;
using OmegaExplorer.Server.Services._Game.Brands.Models._interfaces;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Classes;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Enums;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipSkill.Data._0_1000Ballistic;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipSkill.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Data._0_1000Canons;

public class DynamicItemSpaceshipModuleCanon_1_Pirate : IDynamicItemSpaceshipModule
{
    public const int INDEX = 1;

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "Canon pirate";

    public string? Description { get; set; } =
        "A small canon for a small Spaceship but pirate try to update it to be more powerful with a good old gears system.";

    public EnumDataKnowledge Knowledge { get; set; } = EnumDataKnowledge.Unlocked;
    public EnumRarity Rarity { get; set; } = EnumRarity.Common;

    public List<IDynamicItemSpaceshipSkill> Skills { get; set; } = new()
    {
        DynamicItemSpaceshipSkillBallisticShoot.Instance
    };

    public EnumSpaceshipModuleType Type { get; set; } = EnumSpaceshipModuleType.Weapon;
    public IDynamicItemBrand Brand { get; set; } = DynamicItemBrandStellarDynamics.Instance;
    public int Health { get; set; } = 80;
    public List<SpaceshipModuleModifier> Modifiers { get; set; } = new();
}