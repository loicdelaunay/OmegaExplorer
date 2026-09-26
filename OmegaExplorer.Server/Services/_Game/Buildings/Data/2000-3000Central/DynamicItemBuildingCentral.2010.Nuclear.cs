using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Brands.Data;
using OmegaExplorer.Server.Services._Game.Brands.Models._interfaces;
using OmegaExplorer.Server.Services._Game.Buildings.Models.Enums;
using OmegaExplorer.Server.Services._Game.Buildings.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.GameResources.Data;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Classes;
using OmegaExplorer.Server.Services._Game.StarSystems.Models.Enums;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Buildings.Data._2000_3000Central;

public class DynamicItemBuildingCentralNuclear : IDynamicItemBuilding
{
    public const int INDEX = 2010;

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "Nuclear electricity central";

    public string? Description { get; set; } =
        "Nuclear electric central, produce electricity from nuclear, what a surprise! Everything is fine here, no need to worry about anything. Pinky promise.";

    public EnumDataKnowledge Knowledge { get; set; } = EnumDataKnowledge.Locked;
    public EnumRarity Rarity { get; set; } = EnumRarity.Legendary;

    public int Size { get; set; } = 8;

    public EnumStarSystemType StarSystemCompatibility { get; set; } = EnumStarSystemType.Planet;

    public List<ModifierResource> Cost { get; set; } = new()
    {
        new ModifierResource
        {
            Index = DynamicItemGameResource_0_Credit.INDEX,
            Amount = -10000
        }
    };

    public List<ModifierResource> Modifiers { get; set; } = new()
    {
        new ModifierResource
        {
            Index = DynamicItemGameResource_2_Electricity.INDEX,
            Amount = 1000
        }
    };

    public List<EnumBuildingAction> Actions { get; set; } = new();

    public IDynamicItemBrand? Brand { get; set; } = DynamicItemBrandStellarDynamics.Instance;
}