using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Buildings.Data._1000_2000Mine;
using OmegaExplorer.Server.Services._Game.Buildings.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Extensions;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Interfaces;

namespace OmegaExplorer.Server.Services._Game.Technologies.Data._1000_2000Mine;

public class DynamicItemTechnologyMine_1500_Core : IDynamicItemTechnology
{
    public const int INDEX = 1500;

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = DynamicItemTechnologyExtension.GetTechnologyNameAuto();
    public string? Description { get; set; } = DynamicItemTechnologyExtension.GetTechnologyDescriptionAuto();
    public EnumDataKnowledge Knowledge { get; set; }
    public EnumRarity Rarity { get; set; } = EnumRarity.Origin;

    public List<IDynamicItemTechnology> RequiredTechnologies { get; set; } = new()
    {
        DynamicItemTechnologyMine_1400_AI.Instance
    };

    public List<IDynamicItemBuilding> UnlockBuildings { get; set; }
        = new()
        {
            DynamicItemBuildingMine_1500_Core.Instance
        };

    public int Complexity { get; set; } = 100000;

    public int X { get; set; } = 0;
    public int Y { get; set; } = 5;
}