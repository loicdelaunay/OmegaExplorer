using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game._core.Models.Interfaces;

public interface IDynamicItem
{
    public int Index { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public EnumDataKnowledge Knowledge { get; set; }

    public EnumRarity Rarity { get; set; }

    /// <summary>
    ///     Feed all data possible dynamically
    /// </summary>
    public void Feed()
    {
    }
}