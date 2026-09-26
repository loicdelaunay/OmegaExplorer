using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Buildings.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Interfaces;

namespace OmegaExplorer.Server.Services._Game.Technologies.Data._2000_3000Planetary;

public class DynamicItemTechnology_2000_PlanetaryOptimized : IDynamicItemTechnology
{
    public const int INDEX = 2000;
    public static readonly DynamicItemTechnology_2000_PlanetaryOptimized Instance = new();

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "Planetary Optimized";

    public string? Description { get; set; } =
        "You know sometime you need to optimize your planetary resources, fire some of your workers, and make them work more efficiently.";

    public EnumDataKnowledge Knowledge { get; set; } = EnumDataKnowledge.Unlocked;

    public EnumRarity Rarity { get; set; } = EnumRarity.Common;
    public List<IDynamicItemTechnology> RequiredTechnologies { get; set; } = new();

    public List<IDynamicItemBuilding> UnlockBuildings { get; set; } = new();

    public int Complexity { get; set; } = 10;

    public int X { get; set; } = 10;
    public int Y { get; set; } = 0;
}