using OmegaExplorer.Server.Services._Game._core.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Classes;

namespace OmegaExplorer.Server.Services._Game.Modificators._classes._interfaces;

public interface IDynamicItemModificator : IDynamicItem
{
    public List<ModifierResource> ModifierResources { get; set; }
}