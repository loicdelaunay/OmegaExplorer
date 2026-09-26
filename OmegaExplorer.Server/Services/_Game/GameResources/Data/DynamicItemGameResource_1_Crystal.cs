using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.GameResources.Data;

public class DynamicItemGameResource_1_Crystal : IDynamicItemGameResource
{
    public const int INDEX = 1;
    public static readonly DynamicItemGameResource_1_Crystal Instance = new();

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "Crystal";

    public string? Description { get; set; } =
        "Crystal is a rare and valuable resource that is used to power starships and space stations. It is highly sought after by explorers and traders for its unique properties and ability to enhance the performance of starship systems.";

    public EnumDataKnowledge Knowledge { get; set; } = EnumDataKnowledge.Unlocked;
    public EnumRarity Rarity { get; set; } = EnumRarity.Common;
}