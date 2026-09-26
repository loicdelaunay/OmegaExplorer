using OmegaExplorer.Server.Services._Game._core.Models.Enums;

namespace OmegaExplorer.Server.Services._Core.Models.Contracts._inherits;

public class ResponseDynamicItem
{
    public int Index { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }
    public string FileName { get; set; }

    public EnumRarity Rarity { get; set; }
}