using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Celebrities.Models.Enums;
using OmegaExplorer.Server.Services._Game.Celebrities.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Species.Data;
using OmegaExplorer.Server.Services._Game.Species.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Celebrities.Data._0_1000_Humanity;

public class DynamicItemCelebrity_0_SuperPresident : IDynamicItemCelebrity
{
    public const int INDEX = 0;

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "Super President";

    public string? Description { get; set; } = "";
    public EnumDataKnowledge Knowledge { get; set; } = EnumDataKnowledge.Unlocked;
    public EnumRarity Rarity { get; set; } = EnumRarity.Legendary;

    public bool IsReal { get; set; } = false;

    public EnumCelebrityRole Role { get; set; }

    public IDynamicItemSpecies? Species { get; set; } = DynamicItemSpecies_0_Humanity.Instance;
}