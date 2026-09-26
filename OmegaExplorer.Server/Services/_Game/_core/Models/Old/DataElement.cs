using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game._core.Models.Old;

/// <summary>
///     Data element is a data model of .json used to add data to the game.
/// </summary>
public class DataElement
{
    protected DataElement()
    {
    }

    protected DataElement(int index, string name, string fileName)
    {
        Index = index;
        Name = name;
        FileName = fileName;
    }

    public int Index { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }
    public string FileName { get; set; }

    public EnumDataKnowledge Knowledge { get; set; } = EnumDataKnowledge.Locked;

    public EnumRarity Rarity { get; set; } = EnumRarity.Common;

    public override string ToString()
    {
        return $"{Index}:{Name}";
    }
}