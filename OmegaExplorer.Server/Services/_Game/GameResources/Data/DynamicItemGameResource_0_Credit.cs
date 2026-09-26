using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.GameResources.Data;

public class DynamicItemGameResource_0_Credit : IDynamicItemGameResource
{
    public const int INDEX = 0;
    public static readonly DynamicItemGameResource_0_Credit Instance = new();

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "Credit";

    public string? Description { get; set; } =
        "Credit is the primary currency used in the game. It is used to purchase starship parts, hire crew members, and upgrade your starship.";

    public EnumDataKnowledge Knowledge { get; set; } = EnumDataKnowledge.Unlocked;
    public EnumRarity Rarity { get; set; } = EnumRarity.Common;
}