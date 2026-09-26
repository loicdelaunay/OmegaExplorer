using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Buildings.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Interfaces;

namespace OmegaExplorer.Server.Services._Game.Technologies.Data._2000_3000Planetary;

public class DynamicItemTechnology_2100_PlanetarySuperOptimization : IDynamicItemTechnology
{
    public const int INDEX = 2100;

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "Planetary Super Optimization";

    public string? Description { get; set; } =
        "You know sometime you need to fire even more workers, include some leaders and replace them by huge computer. This is the way to make your planetary resources work more efficiently.";

    public EnumDataKnowledge Knowledge { get; set; }

    public EnumRarity Rarity { get; set; } = EnumRarity.Uncommon;

    public List<IDynamicItemTechnology> RequiredTechnologies { get; set; } = new()
    {
        DynamicItemTechnology_2000_PlanetaryOptimized.Instance
    };

    public List<IDynamicItemBuilding> UnlockBuildings { get; set; } = new();

    public int Complexity { get; set; } = 100;

    public int X { get; set; } = 10;
    public int Y { get; set; } = 1;
}