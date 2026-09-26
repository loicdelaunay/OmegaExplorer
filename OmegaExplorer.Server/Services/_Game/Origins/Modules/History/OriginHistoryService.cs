using OmegaExplorer.Server.Services._Game.Origins.Models.Entities;
using OmegaExplorer.Server.Services._Game.Scenarios;
using OmegaExplorer.Server.Services._Game.Scenarios.Dynamic._1001_2000_Human_main_quests;
using OmegaExplorer.Server.Services._Game.Species;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Origins.Modules.History;

public class OriginHistoryService
{
    private readonly OriginHistoryDataProvider _originHistoryDataProvider;
    private readonly OriginRepository _originRepository;

    private readonly ScenarioService _scenarioService;
    private readonly SpeciesDataProvider _speciesDataProvider;

    public OriginHistoryService(OriginHistoryDataProvider originHistoryDataProvider,
        SpeciesDataProvider speciesDataProvider, ScenarioService scenarioService, OriginRepository originRepository)
    {
        _originHistoryDataProvider = originHistoryDataProvider;
        _speciesDataProvider = speciesDataProvider;
        _scenarioService = scenarioService;
        _originRepository = originRepository;
    }

    public async Task<Origin> UpdateUserOrigin(int indexSpecies, int indexHistory, Guid userId)
    {
        //Check if the user already has an origin
        var existingOrigin = await _originRepository.GetOriginByUserId(userId);
        if (existingOrigin != null) throw new Exception("User already has an origin. Cannot set a new one.");

        #region Check if the user have rights to set this origin

        var origin = _originHistoryDataProvider.GetByIndex(indexHistory);
        if (origin is not { Knowledge: EnumDataKnowledge.Unlocked })
            throw new Exception($"Origin history with index {indexHistory} not available.");

        var species = _speciesDataProvider.GetByIndex(indexSpecies);
        if (species is not { Knowledge: EnumDataKnowledge.Unlocked })
            throw new Exception($"Species with index {indexSpecies} not available.");

        #endregion

        var newOrigin = await _originRepository.SetOrigin(userId, indexSpecies, indexHistory);

        //Give the first scenario for each origin
        //TODO currently only for humanity
        await _scenarioService.AssignScenarioToAPlayer(DynamicItemScenario_1001_HumanMainQuestsStarting.INDEX, userId);

        return newOrigin;
    }
}