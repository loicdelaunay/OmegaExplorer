using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.GameResources.Data;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Classes;
using OmegaExplorer.Server.Services._Game.Modificators._classes._interfaces;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Modificators._dynamic;

public class DynamicItemModificatorProsperity : IDynamicItemModificator
{
    public const int INDEX = 0;

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "Prosperity";

    public string? Description { get; set; } =
        "This modificator will increase the prosperity of your planet. Because when everyone is happy, everyone is productive.";

    public EnumDataKnowledge Knowledge { get; set; }
    public EnumRarity Rarity { get; set; } = EnumRarity.Common;

    public List<ModifierResource> ModifierResources { get; set; } = new()
    {
        new ModifierResource
        {
            Index = DynamicItemGameResource_4_Happiness.INDEX,
            Amount = 50
        }
    };
}