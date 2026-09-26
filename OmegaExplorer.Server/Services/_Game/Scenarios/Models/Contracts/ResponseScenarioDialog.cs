using OmegaExplorer.Server.Services._Game.Celebrities.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Recompenses.Models.Classes;
using OmegaExplorer.Server.Services._Game.Scenarios.Services.ScenarioChoice.Models.Contracts;

namespace OmegaExplorer.Server.Services._Game.Scenarios.Models.Contracts;

public class ResponseScenarioDialog
{
    public int Index { get; set; }

    public IDynamicItemCelebrity? Celebrity { get; set; }

    public string Message { get; set; }

    /// <summary>
    ///     Mapped in after mapper
    /// </summary>
    public List<ResponseDynamicItemScenarioChoice> ScenarioChoices { get; set; } = new();

    public ResponseDynamicItemScenarioChoice PreviousChoice { get; set; }

    public List<Recompense> Recompenses { get; set; }
}