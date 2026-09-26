using OmegaExplorer.Server.Services._Game.Celebrities.Models.Enums;
using OmegaExplorer.Server.Services._Game.Celebrities.Models.Interfaces;

namespace OmegaExplorer.Server.Services._Game.Brands.Models._classes;

public class ContributionCelebrity
{
    public IDynamicItemCelebrity Celebrity { get; set; }
    public EnumCelebrityRole Role { get; set; }
}