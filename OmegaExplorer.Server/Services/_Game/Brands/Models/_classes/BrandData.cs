using OmegaExplorer.Server.Services._Game._core.Models.Old;
using OmegaExplorer.Server.Services._Game.Brands.Models._enums;

namespace OmegaExplorer.Server.Services._Game.Brands.Models._classes;

public class BrandData : DataElement
{
    public List<ContributionCelebrity> ContributionCelebrities = new();
    public EnumBrandType Type { get; set; }
}