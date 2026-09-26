using AutoMapper;
using OmegaExplorer.Server.Services._Game.Scenarios;
using OmegaExplorer.Server.Services._Game.Scenarios.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Scenarios.Models.Entities;

namespace OmegaExplorer.Server.Services.Mappers.AfterMapper;

public class AfterMapUserScenarioProgress : IMappingAction<UserScenarioProgress, ResponseUserScenarioProgress>
{
    private readonly ScenarioDataProvider _scenarioDataProvider;

    public AfterMapUserScenarioProgress(ScenarioDataProvider scenarioDataProvider)
    {
        _scenarioDataProvider = scenarioDataProvider;
    }

    public void Process(UserScenarioProgress src, ResponseUserScenarioProgress dest, ResolutionContext ctx)
    {
        var data = _scenarioDataProvider.GetByIndex(src.Index);
        if (data == null)
        {
            return;
        }

        var dataMapped = ctx.Mapper.Map<ResponseDynamicItemScenario>(data);

        dest.SetScenarioData(src, dataMapped);
    }
}