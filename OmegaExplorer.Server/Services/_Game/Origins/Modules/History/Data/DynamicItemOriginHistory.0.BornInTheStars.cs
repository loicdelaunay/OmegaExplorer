using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Origins.Modules.History.Models._interfaces;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Origins.Modules.History.Data;

public class DynamicItemOriginHistoryBornInTheStars : IDynamicItemOriginHistory
{
    public const int INDEX = 0;

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "Born in the Stars";

    public string? Description { get; set; } =
        "Born in the Stars is a brand that specializes in the production of high-quality starships and space stations. They are known for their innovative designs and cutting-edge technology.";

    public EnumDataKnowledge Knowledge { get; set; } = EnumDataKnowledge.Unlocked;
    public EnumRarity Rarity { get; set; } = EnumRarity.Common;
}