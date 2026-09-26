using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Brands.Models._classes;
using OmegaExplorer.Server.Services._Game.Brands.Models._enums;
using OmegaExplorer.Server.Services._Game.Brands.Models._interfaces;
using OmegaExplorer.Server.Services._Game.Celebrities.Data._10001_11000_Specials;
using OmegaExplorer.Server.Services._Game.Celebrities.Models.Enums;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Brands.Data;

public class DynamicItemBrandStellarCorsair : IDynamicItemBrand
{
    public const int INDEX = 1;
    public static readonly DynamicItemBrandStellarCorsair Instance = new();

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "Stellar Corsair";

    public string? Description { get; set; } = "TODO";

    public EnumDataKnowledge Knowledge { get; set; } = EnumDataKnowledge.Unlocked;

    public EnumRarity Rarity { get; set; } = EnumRarity.Common;

    public EnumBrandType Type { get; set; } = EnumBrandType.MakeSpaceship | EnumBrandType.MakeSpaceshipModule;

    public List<ContributionCelebrity> ContributionCelebrities { get; set; } = new()
    {
        new ContributionCelebrity
        {
            Celebrity = DynamicItemCelebrity_10001_Dream.Instance,
            Role = EnumCelebrityRole.Designer
        }
    };
}