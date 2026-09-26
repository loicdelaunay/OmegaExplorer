using OmegaExplorer.Server.Services._Game.Celebrities.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Recompenses.Models.Classes;
using OmegaExplorer.Server.Services._Game.Scenarios.Services.ScenarioChoice.Interfaces;

namespace OmegaExplorer.Server.Services._Game.Scenarios.Models.Classes;

public class ScenarioDialog
{
    public int Index { get; set; }

    public IDynamicItemCelebrity? Celebrity { get; set; }

    public string Message { get; set; }

    public List<IDynamicItemScenarioChoice> ScenarioChoices { get; set; }

    public IDynamicItemScenarioChoice PreviousChoice { get; set; }

    public List<Recompense> Recompenses { get; set; } = new();
}