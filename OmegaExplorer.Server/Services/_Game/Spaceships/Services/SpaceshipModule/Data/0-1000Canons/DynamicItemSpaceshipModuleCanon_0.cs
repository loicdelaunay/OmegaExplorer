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

public class DynamicItemSpaceshipModuleCanon_0 : IDynamicItemSpaceshipModule
{
    public const int INDEX = 0;
    public static readonly DynamicItemSpaceshipModuleCanon_0 Instance = new();

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "Canon";

    public string? Description { get; set; } =
        "Basic canon system, you put a huge thing inside, and send it to the face of your enemies.";

    public EnumDataKnowledge Knowledge { get; set; } = EnumDataKnowledge.Unlocked;
    public EnumRarity Rarity { get; set; } = EnumRarity.Common;

    public List<IDynamicItemSpaceshipSkill> Skills { get; set; } = new()
    {
        DynamicItemSpaceshipSkillBallisticShoot.Instance
    };

    public EnumSpaceshipModuleType Type { get; set; } = EnumSpaceshipModuleType.Weapon;

    public IDynamicItemBrand Brand { get; set; } = DynamicItemBrandStellarDynamics.Instance;
    public int Health { get; set; } = 100;
    public List<SpaceshipModuleModifier> Modifiers { get; set; } = new();
}