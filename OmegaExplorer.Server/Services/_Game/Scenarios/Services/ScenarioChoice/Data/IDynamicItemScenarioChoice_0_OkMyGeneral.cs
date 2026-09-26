using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Scenarios.Services.ScenarioChoice.Interfaces;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Scenarios.Services.ScenarioChoice.Data;

public class IDynamicItemScenarioChoice_0_OkMyGeneral : IDynamicItemScenarioChoice
{
    public const int INDEX = 0;
    public static readonly IDynamicItemScenarioChoice_0_OkMyGeneral Instance = new();

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "Ok my general";
    public string? Description { get; set; } = "Sure thing, General. Whatever you say.";
    public EnumDataKnowledge Knowledge { get; set; }
    public EnumRarity Rarity { get; set; }
}