using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;
using OmegaExplorer.Server.Services.Databases;
using OmegaExplorer.Server.Services.Users.Models.Entities;

namespace OmegaExplorer.Server.Services._Game.Technologies.Services.TechnologyKnowledge;

public class TechnologyKnowledgeRepository
{
    private readonly DatabaseContext _databaseContext;

    private readonly TechnologyDataProvider _technologyDataProvider;

    public TechnologyKnowledgeRepository(TechnologyDataProvider technologyDataProvider, DatabaseContext databaseContext)
    {
        _technologyDataProvider = technologyDataProvider;
        _databaseContext = databaseContext;
    }

    public async Task<List<Technologies.Models.Entities.TechnologyKnowledge>> GetTechnologyKnowledges(Guid userId)
    {
        var techKnowledge = await _databaseContext.TechnologyKnowledges
                                                  .Where(x => x.UserId == userId)
                                                  .ToListAsync();

        return techKnowledge;
    }

    public async Task<Technologies.Models.Entities.TechnologyKnowledge> StartSearchTechnology(Guid userId,
        int technologyIndex)
    {
        var user = await _databaseContext.Users.SingleOrDefaultAsync(x => x.Id == userId);

        if (user == null) throw new KeyNotFoundException();

        //If already researching this technology, return it
        var techKnowledge = await _databaseContext.TechnologyKnowledges
                                                  .SingleOrDefaultAsync(x => x.UserId == userId && x.TechnologyIndex == technologyIndex);

        //If not, create a new one
        if (techKnowledge == null)
        {
            techKnowledge = new Technologies.Models.Entities.TechnologyKnowledge
            {
                UserId = userId,
                TechnologyIndex = technologyIndex,
                Progress = 0
            };

            _databaseContext.TechnologyKnowledges.Add(techKnowledge);
        }

        user.CurrentTechnologyResearchIndex = technologyIndex;

        await _databaseContext.SaveChangesAsync();

        return techKnowledge;
    }

    public async Task StopSearchTechnology(Guid userId)
    {
        var user = await _databaseContext.Users.SingleOrDefaultAsync(x => x.Id == userId);

        if (user == null) throw new KeyNotFoundException();

        user.CurrentTechnologyResearchIndex = null;

        await _databaseContext.SaveChangesAsync();
    }

    public async Task<List<User>> GetUsersResearchingTechnology()
    {
        var users = await _databaseContext.Users.Where(x => x.CurrentTechnologyResearchIndex != null)
            .ToListAsync();

        return users;
    }

    public async Task ProgressTechnologySearch(Guid userId, int indexTechnology, int technologySearchCapacity)
    {
        var techKnowledge = await _databaseContext.TechnologyKnowledges
                                                  .SingleOrDefaultAsync(x => x.UserId == userId && x.TechnologyIndex == indexTechnology);

        var techData = _technologyDataProvider.GetByIndex(indexTechnology);

        if (techData == null) throw new Exception("Technology not found at index {indexTechnology}");

        if (techKnowledge == null) throw new KeyNotFoundException();

        techKnowledge.Progress += technologySearchCapacity;

        if (techKnowledge.Progress > techData.Complexity)
        {
            techKnowledge.Knowledge = EnumDataKnowledge.Researched;

            //Notification to user end search ( TODO ) 

            //Stop search of this tech
            await StopSearchTechnology(userId);
        }

        await _databaseContext.SaveChangesAsync();
    }

    public async Task<Technologies.Models.Entities.TechnologyKnowledge?> GetCurrentTechnologyProgress(User user)
    {
        var currentTechnologyProgress = await _databaseContext.TechnologyKnowledges.Where(te =>
                                                                  te.UserId == user.Id && te.TechnologyIndex == user.CurrentTechnologyResearchIndex)
                                                              .SingleOrDefaultAsync();

        return currentTechnologyProgress;
    }
}