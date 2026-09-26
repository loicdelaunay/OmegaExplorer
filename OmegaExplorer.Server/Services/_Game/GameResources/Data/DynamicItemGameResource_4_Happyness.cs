using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.GameResources.Data;

public class DynamicItemGameResource_4_Happiness : IDynamicItemGameResource
{
    public const int INDEX = 4;
    public static readonly DynamicItemGameResource_4_Happiness Instance = new();

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "Happiness";
    public string? Description { get; set; } = "Yes i decided happiness is a resource, sorry about that.";
    public EnumDataKnowledge Knowledge { get; set; } = EnumDataKnowledge.Researched;
    public EnumRarity Rarity { get; set; } = EnumRarity.Common;
}