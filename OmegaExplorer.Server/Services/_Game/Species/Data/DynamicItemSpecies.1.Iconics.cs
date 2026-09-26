using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Species.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Species.Data;

public class DynamicItemSpeciesIconics : IDynamicItemSpecies
{
    public const int INDEX = 1;

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "Iconics";

    public string? Description { get; set; } =
        "This species is not fully recognize. = Work in progress.";

    public EnumDataKnowledge Knowledge { get; set; } = EnumDataKnowledge.Locked;

    public EnumRarity Rarity { get; set; } = EnumRarity.Uncommon;
}