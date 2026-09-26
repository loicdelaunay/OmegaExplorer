using OmegaExplorer.Server.Services._Game._core.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Celebrities.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Scenarios.Models.Classes;
using OmegaExplorer.Server.Services._Game.Scenarios.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Scenarios.Models.Interfaces;

public interface IDynamicItemScenario : IDynamicItem
{
    public List<ScenarioDialog> Dialogs { get; set; }

    public IDynamicItemCelebrity? MainContact { get; set; }

    /// <summary>
    ///     If the scenario have a priority force dialog on client to be displayed
    /// </summary>
    public bool Priority { get; set; }

    public EnumScenarioRepetition ScenarioRepetition { get; set; }

    public EnumScenarioDistribution ScenarioDistribution { get; set; }

    /// <summary>
    ///     When the dialog is fully resolved
    /// </summary>
    /// <returns></returns>
    public Task OnResolved();
}