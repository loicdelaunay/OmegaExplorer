using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Brands.Data;
using OmegaExplorer.Server.Services._Game.Brands.Models._interfaces;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Classes;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Enums;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipSkill.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Data._1001_2000Cockpits;

public class DynamicItemSpaceshipModuleCockpit_1002_StandardCorsair : IDynamicItemSpaceshipModule
{
    public const int INDEX = 1002;
    public static readonly DynamicItemSpaceshipModuleCockpit_1002_StandardCorsair Instance = new();
    private List<IDynamicItemSpaceshipSkill> _skills;

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "Standard Corsair Cockpit";
    public string? Description { get; set; } = "A beautiful cockpit in one word.";
    public EnumDataKnowledge Knowledge { get; set; } = EnumDataKnowledge.Unlocked;
    public EnumRarity Rarity { get; set; } = EnumRarity.Common;
    public List<IDynamicItemSpaceshipSkill> Skills { get; set; } = new();

    public EnumSpaceshipModuleType Type { get; set; } = EnumSpaceshipModuleType.Cockpit;
    public IDynamicItemBrand Brand { get; set; } = DynamicItemBrandStellarCorsair.Instance;
    public int Health { get; set; } = 80;
    public List<SpaceshipModuleModifier> Modifiers { get; set; } = new();
}