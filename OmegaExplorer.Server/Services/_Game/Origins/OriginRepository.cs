using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.Services._Game.Origins.Models.Entities;
using OmegaExplorer.Server.Services.Databases;

namespace OmegaExplorer.Server.Services._Game.Origins;

public class OriginRepository
{
    private readonly DatabaseContext _databaseContext;

    public OriginRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Origin?> GetOriginByUserId(Guid idUser)
    {
        var user = await _databaseContext.Users
            .Include(x => x.Origin)
            .FirstOrDefaultAsync(x => x.Id == idUser);

        return user?.Origin;
    }

    public async Task<Origin> SetOrigin(Guid idUser, int indexSpecies, int indexHistory)
    {
        var user = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Id == idUser);
        if (user == null) throw new Exception("User not found");

        Origin origin = new()
        {
            IndexSpecies = indexSpecies,
            IndexHistory = indexHistory
        };

        user.Origin = origin;

        await _databaseContext.SaveChangesAsync();

        return origin;
    }
}