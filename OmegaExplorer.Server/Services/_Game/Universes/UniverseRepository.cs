using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.Services._Game.Universes.Models.Entities;
using OmegaExplorer.Server.Services._Game.Universes.Models.Enums;
using OmegaExplorer.Server.Services.Databases;

namespace OmegaExplorer.Server.Services._Game.Universes;

public class UniverseRepository
{
    private readonly DatabaseContext _databaseContext;

    public UniverseRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Universe?> GetUniverseByType(EnumUniverseType type)
    {
        var universe = await _databaseContext.Universes.FirstOrDefaultAsync(universe => universe.Type == type);

        return universe;
    }

    public async Task Create(Universe universe)
    {
        //Check if the universe already exists
        var exist = await GetUniverseByType(universe.Type);
        if (exist != null) return;

        _databaseContext.Universes.Add(universe);

        await _databaseContext.SaveChangesAsync();
    }

    public async Task<List<Universe>> GetAll()
    {
        var universes = await _databaseContext.Universes.ToListAsync();

        return universes;
    }

    public async Task<Universe?> GetById(Guid universeId)
    {
        var universe = await _databaseContext.Universes
            .FirstOrDefaultAsync(universe => universe.Id == universeId);

        return universe;
    }
}