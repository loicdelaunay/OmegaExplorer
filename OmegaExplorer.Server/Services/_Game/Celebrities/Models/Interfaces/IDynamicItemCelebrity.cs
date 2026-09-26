using OmegaExplorer.Server.Services._Game._core.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Celebrities.Models.Enums;
using OmegaExplorer.Server.Services._Game.Species.Models.Interfaces;

namespace OmegaExplorer.Server.Services._Game.Celebrities.Models.Interfaces;

public interface IDynamicItemCelebrity : IDynamicItem
{
    public bool IsReal { get; set; }

    public EnumCelebrityRole Role { get; set; }

    public IDynamicItemSpecies? Species { get; set; }
}