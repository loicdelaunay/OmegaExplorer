using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Brands.Data;
using OmegaExplorer.Server.Services._Game.Brands.Models._interfaces;
using OmegaExplorer.Server.Services._Game.Buildings.Models.Enums;
using OmegaExplorer.Server.Services._Game.Buildings.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Classes;
using OmegaExplorer.Server.Services._Game.StarSystems.Models.Enums;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Buildings.Data._100_200Shipyard;

public class DynamicItemBuildingShipyard_101_Small : IDynamicItemBuilding
{
    public const int INDEX = 101;

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "Small Shipyard";
    public string? Description { get; set; } = "A standard shipyard, capable of building and repairing small ships.";
    public EnumDataKnowledge Knowledge { get; set; } = EnumDataKnowledge.Unlocked;
    public EnumRarity Rarity { get; set; } = EnumRarity.Common;
    public int Size { get; set; } = 1;
    public EnumStarSystemType StarSystemCompatibility { get; set; } = EnumStarSystemType.Planet;
    public List<ModifierResource> Cost { get; set; } = new();
    public List<ModifierResource> Modifiers { get; set; } = new();

    public List<EnumBuildingAction> Actions { get; set; } = new()
    {
        EnumBuildingAction.BuildSpaceship
    };

    public IDynamicItemBrand? Brand { get; set; } = DynamicItemBrandStellarDynamics.Instance;
}