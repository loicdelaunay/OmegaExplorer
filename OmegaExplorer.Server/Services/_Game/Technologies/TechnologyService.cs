using OmegaExplorer.Server.Services._Core.Extensions;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Entities;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Technologies.Services.TechnologyKnowledge;
using OmegaExplorer.Server.Services.Users.Models.Entities;

namespace OmegaExplorer.Server.Services._Game.Technologies;

public class TechnologyService
{
    private readonly TechnologyDataProvider _technologyDataProvider;
    private readonly TechnologyKnowledgeRepository _technologyKnowledgeRepository;

    public TechnologyService(TechnologyDataProvider technologyDataProvider,
        TechnologyKnowledgeRepository technologyKnowledgeRepository)
    {
        _technologyDataProvider = technologyDataProvider;
        _technologyKnowledgeRepository = technologyKnowledgeRepository;
    }

    /// <summary>
    ///     Get all user technologies with their progress
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<List<IDynamicItemTechnology>> GetAllTechnologies(Guid userId)
    {
        var allTechnologies = _technologyDataProvider.GetAll();
        var progress = await _technologyKnowledgeRepository.GetTechnologyKnowledges(userId);

        allTechnologies.GetTechnologiesWithProgress(progress);

        return allTechnologies;
    }

    public async Task<TechnologyKnowledge?> GetCurrentTechnologyProgress(User user)
    {
        var res = await _technologyKnowledgeRepository.GetCurrentTechnologyProgress(user);

        return res;
    }
}