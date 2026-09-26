using OmegaExplorer.Server.Services._Core.Models.Contracts._inherits;
using OmegaExplorer.Server.Services._Game.Scenarios.Models.Entities;
using OmegaExplorer.Server.Services._Game.Scenarios.Models.Interfaces;
using OmegaExplorer.Server.Services.Users.Models.Contracts;

namespace OmegaExplorer.Server.Services._Game.Scenarios.Models.Contracts;

public class ResponseUserScenarioProgress : ResponseMetadata
{
    /// <summary>
    ///     Index of the <see cref="IDynamicItemScenario" />
    /// </summary>
    public int Index { get; set; }

    public ResponseDynamicItemScenario ScenarioData { get; private set; }

    public Guid UserId { get; set; }

    public ResponseUser User { get; set; }

    public bool IsResolved { get; set; }

    /// <summary>
    ///     Look at the scenario data to see if the scenario is resolved and current progress to hide choices
    /// </summary>
    /// <param name="scenarioProgress"></param>
    /// <param name="scenarioData"></param>
    public void SetScenarioData(UserScenarioProgress? scenarioProgress, ResponseDynamicItemScenario scenarioData)
    {
        //If no progress get only first element
        if (scenarioProgress == null)
        {
            scenarioData.Dialogs = ScenarioData.Dialogs.Take(1).ToList();
            return;
        }

        //Get the choice + 1 of the current choice
        var choiceToTake = scenarioProgress.Choices.Count + 1;
        scenarioData.Dialogs = scenarioData.Dialogs.Take(choiceToTake).ToList();

        ScenarioData = scenarioData;
    }
}