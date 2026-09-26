using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Buildings.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Interfaces;

namespace OmegaExplorer.Server.Services._Game.Technologies.Data._3000_4000Quests;

public class DynamicItemTechnology_3000_QuestPostItManagement : IDynamicItemTechnology
{
    public const int INDEX = 3000;

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "Quest Post-It Management";

    public string? Description { get; set; } =
        "You discover the utility of the post-it, and you start to use it to manage your quest. It's a good way to remember what you need to do! So now you can manage 3 quests in same time!";

    public EnumDataKnowledge Knowledge { get; set; }

    public EnumRarity Rarity { get; set; } = EnumRarity.Common;
    public List<IDynamicItemTechnology> RequiredTechnologies { get; set; } = new();

    public List<IDynamicItemBuilding> UnlockBuildings { get; set; } = new();

    public int Complexity { get; set; } = 10;

    public int X { get; set; } = 20;
    public int Y { get; set; } = 0;
}