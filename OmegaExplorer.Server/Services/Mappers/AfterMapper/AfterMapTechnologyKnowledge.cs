using AutoMapper;
using OmegaExplorer.Server.Services._Game.Technologies;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Entities;
using OmegaExplorer.Server.Services._Game.Technologies.Services.TechnologyKnowledge.Models.Contracts;

namespace OmegaExplorer.Server.Services.Mappers.AfterMapper;

public class AfterMapTechnologyKnowledge : IMappingAction<TechnologyKnowledge, ResponseTechnologyKnowledge>
{
    private readonly TechnologyDataProvider _technologyDataProvider;

    public AfterMapTechnologyKnowledge(TechnologyDataProvider technologyDataProvider)
    {
        _technologyDataProvider = technologyDataProvider;
    }

    public void Process(TechnologyKnowledge src, ResponseTechnologyKnowledge dest, ResolutionContext ctx)
    {
        var tech = _technologyDataProvider.GetByIndex(src.TechnologyIndex);

        dest.TechnologyData = tech;
    }
}
