using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.Services._Core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Spaceships.Models.Entities;
using OmegaExplorer.Server.Services._Game.StarSystems.Models.Entities;
using OmegaExplorer.Server.Services.Databases;

namespace OmegaExplorer.Server.Services._Game.Species;

public class SpeciesRepository : RepositoryCustom
{
    private readonly DatabaseContext _databaseContext;

    public SpeciesRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Spaceship> UpdateAmountOfASpeciesOnSpaceship(Guid userId,
        Guid spaceshipId,
        int speciesIndex, int amount)
    {
        var spaceship = await _databaseContext.Spaceships
            .FirstOrDefaultAsync(s => s.OwnerId == userId && s.Id == spaceshipId);

        if (spaceship == null) throw new Exception($"Spaceship at id {spaceshipId} not found");

        var species = spaceship.Species;

        var currentValue = species.GetValueOrDefault(speciesIndex, 0);
        species[speciesIndex] = currentValue + amount;

        spaceship.Species = species;

        await _databaseContext.SaveChangesAsync();

        return spaceship;
    }

    public async Task<StarSystem> UpdateAmountOfASpeciesOnStarSystem(Guid? userId,
        Guid starsystemId, int speciesIndex, long amount, bool asAdmin = false)
    {
        var query = _databaseContext.StarSystems.AsQueryable();

        if (!asAdmin) query = query.Where(s => s.OwnerId == userId);

        var starSystem = await query
            .FirstOrDefaultAsync(s => s.Id == starsystemId);

        if (starSystem == null) throw new Exception($"Starsystem at id {starsystemId} not found");

        starSystem.Species[speciesIndex] = amount;

        _databaseContext.Entry(starSystem).Property(s => s.Species).IsModified = true;

        await _databaseContext.SaveChangesAsync();

        return starSystem;
    }
}