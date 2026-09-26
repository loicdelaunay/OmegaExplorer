using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Species.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Species.Data;

public class DynamicItemSpeciesPlantis : IDynamicItemSpecies
{
    public const int INDEX = 2;

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "Plantis";

    public string? Description { get; set; } =
        "A plant-like species. They are known for their ability to photosynthesize and their ability to grow in a variety of environments, but they are also known for their slow movement and the capability to burn really easily.";

    public EnumDataKnowledge Knowledge { get; set; } = EnumDataKnowledge.Locked;

    public EnumRarity Rarity { get; set; } = EnumRarity.Common;
}