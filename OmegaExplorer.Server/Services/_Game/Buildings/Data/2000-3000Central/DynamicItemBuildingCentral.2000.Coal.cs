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

public class DynamicItemBuildingCentralCoal : IDynamicItemBuilding
{
    public const int INDEX = 2000;

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "Central Coal";

    public string? Description { get; set; } = "Coal electricity central, produce electricity from coal, what a shame";

    public EnumDataKnowledge Knowledge { get; set; } = EnumDataKnowledge.Locked;
    public EnumRarity Rarity { get; set; } = EnumRarity.Common;

    public int Size { get; set; } = 2;

    public EnumStarSystemType StarSystemCompatibility { get; set; } = EnumStarSystemType.Planet;

    public List<ModifierResource> Cost { get; set; } = new()
    {
        new ModifierResource
        {
            Index = DynamicItemGameResource_0_Credit.INDEX,
            Amount = -1000
        }
    };

    public List<ModifierResource> Modifiers { get; set; } = new()
    {
        new ModifierResource
        {
            Index = DynamicItemGameResource_2_Electricity.INDEX,
            Amount = 100
        }
    };

    public List<EnumBuildingAction> Actions { get; set; } = new();

    public IDynamicItemBrand? Brand { get; set; } = DynamicItemBrandStellarDynamics.Instance;
}