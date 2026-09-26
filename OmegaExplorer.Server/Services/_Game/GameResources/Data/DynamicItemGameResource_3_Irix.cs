using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.GameResources.Data;

public class DynamicItemGameResource_3_Irix : IDynamicItemGameResource
{
    public const int INDEX = 3;
    public static readonly DynamicItemGameResource_3_Irix Instance = new();

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "Irix";

    public string? Description { get; set; } =
        "Irix is a rare and valuable resource that is used to power starships and space stations.";

    public EnumDataKnowledge Knowledge { get; set; } = EnumDataKnowledge.Researched;
    public EnumRarity Rarity { get; set; } = EnumRarity.Rare;
}