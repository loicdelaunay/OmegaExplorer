using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Brands.Data;
using OmegaExplorer.Server.Services._Game.Brands.Models._interfaces;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Classes;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Enums;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipSkill.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Data._2001_3000Reactors;

public class DynamicItemSpaceshipModuleReactor_2999_ADM : IDynamicItemSpaceshipModule
{
    public const int INDEX = 2999;
    public static readonly DynamicItemSpaceshipModuleReactor_2999_ADM Instance = new();

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "ADM Reactor";

    public string? Description { get; set; } =
        "We use a lot of fuel to push a little. But it's not enough so I add my secret babidi magically oupaloupa sugar cheat code lines in the fuel";

    public EnumDataKnowledge Knowledge { get; set; } = EnumDataKnowledge.Unlocked;
    public EnumRarity Rarity { get; set; } = EnumRarity.Origin;
    public List<IDynamicItemSpaceshipSkill> Skills { get; set; } = new();
    public EnumSpaceshipModuleType Type { get; set; } = EnumSpaceshipModuleType.Reactor;
    public IDynamicItemBrand Brand { get; set; } = DynamicItemBrandStellarDynamics.Instance;
    public int Health { get; set; } = 100;

    public List<SpaceshipModuleModifier> Modifiers { get; set; } = new()
    {
        new SpaceshipModuleModifier
        {
            ModifierSpaceshipModuleModifierType = SpaceshipModuleModifier.SpaceshipModuleModifierType.Velocity,
            Value = 1_000_000
        }
    };
}