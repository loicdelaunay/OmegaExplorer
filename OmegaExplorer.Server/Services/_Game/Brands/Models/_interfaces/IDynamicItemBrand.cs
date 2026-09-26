using OmegaExplorer.Server.Services._Game._core.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Brands.Models._classes;
using OmegaExplorer.Server.Services._Game.Brands.Models._enums;

namespace OmegaExplorer.Server.Services._Game.Brands.Models._interfaces;

public interface IDynamicItemBrand : IDynamicItem
{
    public EnumBrandType Type { get; set; }
    public List<ContributionCelebrity> ContributionCelebrities { get; set; }
}