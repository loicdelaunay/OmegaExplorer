using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Brands.Data;
using OmegaExplorer.Server.Services._Game.Brands.Models._interfaces;
using OmegaExplorer.Server.Services._Game.Buildings.Models.Enums;
using OmegaExplorer.Server.Services._Game.Buildings.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Classes;
using OmegaExplorer.Server.Services._Game.StarSystems.Models.Enums;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Buildings.Data._1000_2000Mine;

public class DynamicItemBuildingMine_1400_AI : IDynamicItemBuilding
{
    public const int INDEX = 1400;
    public static readonly DynamicItemBuildingMine_1400_AI Instance = new();

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "Mine AI self-driven";

    public string? Description { get; set; } =
        "Now there is no longer even the need to think about it and manage anything, more productivity for fewer meetings!";

    public EnumDataKnowledge Knowledge { get; set; } = EnumDataKnowledge.Locked;
    public EnumRarity Rarity { get; set; } = EnumRarity.Rare;
    public int Size { get; set; } = 1;
    public EnumStarSystemType StarSystemCompatibility { get; set; } = EnumStarSystemType.Planet;
    public List<ModifierResource> Cost { get; set; } = new();
    public List<ModifierResource> Modifiers { get; set; } = new();
    public List<EnumBuildingAction> Actions { get; set; } = new();
    public IDynamicItemBrand? Brand { get; set; } = DynamicItemBrandStellarDynamics.Instance;
}