using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Species.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Species.Data;

public class DynamicItemSpecies_0_Humanity : IDynamicItemSpecies
{
    public const int INDEX = 0;
    public static readonly DynamicItemSpecies_0_Humanity Instance = new();

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "Humanity";
    public string? Description { get; set; } = "The most advanced species in the universe? Not really.";
    public EnumDataKnowledge Knowledge { get; set; } = EnumDataKnowledge.Unlocked;
    public EnumRarity Rarity { get; set; } = EnumRarity.Common;
}