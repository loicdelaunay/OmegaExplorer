using OmegaExplorer.Server.Services._Core.Models.Contracts._inherits;
using OmegaExplorer.Server.Services._Game.Celebrities.Models.Interfaces;

namespace OmegaExplorer.Server.Services._Game.Scenarios.Models.Contracts;

public class ResponseDynamicItemScenario : ResponseDynamicItem
{
    public IDynamicItemCelebrity? MainContact { get; set; }

    public List<ResponseScenarioDialog> Dialogs { get; set; } = new();

    /// <summary>
    ///     If the scenario have a priority force dialog on client to be displayed
    /// </summary>
    public bool Priority { get; set; }
}