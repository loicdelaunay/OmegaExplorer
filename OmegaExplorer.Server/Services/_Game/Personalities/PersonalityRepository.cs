using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.Services._Game.Personalities.Models.Entities;
using OmegaExplorer.Server.Services.Databases;

namespace OmegaExplorer.Server.Services._Game.Personalities;

public class PersonalityRepository
{
    private readonly DatabaseContext _databaseContext;

    public PersonalityRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<List<Personality>> GetAll(Guid userId)
    {
        var personalities = await _databaseContext.Personalities
            .Where(p => p.OwnerId == userId)
            .ToListAsync();

        return personalities;
    }

    public async Task<Personality?> GetById(Guid personalityId, Guid userId)
    {
        var personality = await _databaseContext.Personalities
            .Where(personality => personality.OwnerId == userId)
            .FirstOrDefaultAsync(p => p.Id == personalityId);

        return personality;
    }


    public async Task Add(Personality newPersonality)
    {
        _databaseContext.Personalities.Add(newPersonality);

        await _databaseContext.SaveChangesAsync();
    }

    public async Task Delete(Guid personalityId, Guid userId)
    {
        var personality = await _databaseContext.Personalities
            .Where(personality => personality.OwnerId == userId)
            .FirstOrDefaultAsync(personality => personality.Id == personalityId);

        if (personality == null) throw new KeyNotFoundException();

        _databaseContext.Personalities.Remove(personality);
        await _databaseContext.SaveChangesAsync();
    }
}