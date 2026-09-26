using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Brands.Data;
using OmegaExplorer.Server.Services._Game.Brands.Models._interfaces;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Classes;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Enums;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipSkill.Data._4001_5001Colonize;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipSkill.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Data._4001_5000Colonizer;

public class DynamicItemSpaceshipModuleColonizer_4002_TsunamiColonizer : IDynamicItemSpaceshipModule
{
    public const int INDEX = 4002;

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "Tsunami Colonizer";
    public string? Description { get; set; } = "Flood a star system with a complete population instant.";
    public EnumDataKnowledge Knowledge { get; set; } = EnumDataKnowledge.Unlocked;
    public EnumRarity Rarity { get; set; } = EnumRarity.Epic;

    public List<IDynamicItemSpaceshipSkill> Skills { get; set; } = new()
    {
        DynamicItemSpaceshipSkillColonize_4002_PopulationTsunami.Instance
    };

    public EnumSpaceshipModuleType Type { get; set; } = EnumSpaceshipModuleType.Module;
    public IDynamicItemBrand Brand { get; set; } = DynamicItemBrandStellarDynamics.Instance;
    public int Health { get; set; } = 1000;
    public List<SpaceshipModuleModifier> Modifiers { get; set; } = new();
}