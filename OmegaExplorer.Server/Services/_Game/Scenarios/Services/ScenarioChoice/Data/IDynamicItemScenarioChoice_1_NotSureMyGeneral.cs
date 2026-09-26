using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Scenarios.Services.ScenarioChoice.Interfaces;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Scenarios.Services.ScenarioChoice.Data;

public class IDynamicItemScenarioChoice_1_NotSureMyGeneral : IDynamicItemScenarioChoice
{
    public const int INDEX = 1;
    public static readonly IDynamicItemScenarioChoice_1_NotSureMyGeneral Instance = new();

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "Not sure about this, General";
    public string? Description { get; set; } = "I have some doubts about this plan...";
    public EnumDataKnowledge Knowledge { get; set; }
    public EnumRarity Rarity { get; set; }
}