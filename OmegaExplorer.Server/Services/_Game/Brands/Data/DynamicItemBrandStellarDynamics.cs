using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Brands.Models._classes;
using OmegaExplorer.Server.Services._Game.Brands.Models._enums;
using OmegaExplorer.Server.Services._Game.Brands.Models._interfaces;
using OmegaExplorer.Server.Services._Game.Celebrities.Data._10001_11000_Specials;
using OmegaExplorer.Server.Services._Game.Celebrities.Models.Enums;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Brands.Data;

public class DynamicItemBrandStellarDynamics : IDynamicItemBrand
{
    public const int INDEX = 0;
    public static readonly DynamicItemBrandStellarDynamics Instance = new();

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "Stellar Dynamics";

    public string? Description { get; set; } =
        "Stellar Dynamics is a brand that specializes in the production of high-quality starships and space stations. They are known for their innovative designs and cutting-edge technology.";

    public EnumDataKnowledge Knowledge { get; set; } = EnumDataKnowledge.Locked;

    public EnumRarity Rarity { get; set; } = EnumRarity.Common;

    public EnumBrandType Type { get; set; } = EnumBrandType.MakeSpaceship | EnumBrandType.MakeSpaceshipModule;

    public List<ContributionCelebrity> ContributionCelebrities { get; set; } = new()
    {
        new ContributionCelebrity
        {
            Celebrity = DynamicItemCelebrity_10001_Dream.Instance,
            Role = EnumCelebrityRole.Founder
        }
    };
}