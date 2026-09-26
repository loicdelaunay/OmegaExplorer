using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Brands.Data;
using OmegaExplorer.Server.Services._Game.Brands.Models._interfaces;
using OmegaExplorer.Server.Services._Game.Buildings.Models.Enums;
using OmegaExplorer.Server.Services._Game.Buildings.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Classes;
using OmegaExplorer.Server.Services._Game.StarSystems.Models.Enums;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Buildings.Data._1000_2000Mine;

public class DynamicItemBuildingMine_1500_Core : IDynamicItemBuilding
{
    public const int INDEX = 1500;
    public static readonly DynamicItemBuildingMine_1500_Core Instance = new();

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "Mine Core";

    public string? Description { get; set; } =
        "A technology requiring a lot of effort to tap into the core energy of the planet. It has never been a good idea and it never will be.";

    public EnumDataKnowledge Knowledge { get; set; } = EnumDataKnowledge.Locked;
    public EnumRarity Rarity { get; set; } = EnumRarity.Origin;
    public int Size { get; set; } = 1;
    public EnumStarSystemType StarSystemCompatibility { get; set; } = EnumStarSystemType.Planet;
    public List<ModifierResource> Cost { get; set; } = new();
    public List<ModifierResource> Modifiers { get; set; } = new();
    public List<EnumBuildingAction> Actions { get; set; } = new();
    public IDynamicItemBrand? Brand { get; set; } = DynamicItemBrandStellarDynamics.Instance;
}