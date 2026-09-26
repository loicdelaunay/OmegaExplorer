using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Scenarios.Models.Classes;
using OmegaExplorer.Server.Services._Game.Scenarios.Models.Interfaces;
using OmegaExplorer.Server.Services.Databases.Attributes;
using OmegaExplorer.Server.Services.Users.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmegaExplorer.Server.Services._Game.Scenarios.Models.Entities;

public class UserScenarioProgress : Metadata
{
    [NotMapped] private IDynamicItemScenario? _currentData;

    /// <summary>
    ///     Index of the <see cref="IDynamicItemScenario" />
    /// </summary>
    public int Index { get; set; }

    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }

    [DeleteBehavior(DeleteBehavior.Cascade)]
    public User? User { get; set; }

    /// <summary>
    ///     First key is the index of the <see cref="IDynamicItemScenario" /> and value is choice index
    /// </summary>
    [JsonColumn]
    public Dictionary<int, int> Choices { get; set; } = new();

    /// <summary>
    ///     If the scenario is resolved or not
    /// </summary>
    public bool IsResolved { get; set; }

    /// <summary>
    ///     Get the last dialog set of the scenario progress
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<ScenarioDialog> GetCurrentDialog(IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var scenarioDataProvider = scope.ServiceProvider.GetRequiredService<ScenarioDataProvider>();

        //Need to feed the current data if null
        if (_currentData == null) _currentData = scenarioDataProvider.GetByIndex(Index);

        if (_currentData == null) throw new Exception("No data found");

        var lastDialog = Choices.Count;

        return _currentData.Dialogs[lastDialog];
    }
}