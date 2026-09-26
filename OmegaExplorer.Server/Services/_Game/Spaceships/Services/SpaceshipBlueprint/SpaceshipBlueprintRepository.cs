using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.Services._Core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint.Models.Entities;
using OmegaExplorer.Server.Services.Databases;
using OmegaExplorer.Server.Services.Databases.Extensions;

namespace OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint;

public class SpaceshipBlueprintRepository : RepositoryCustom
{
    private readonly DatabaseContext _databaseContext;

    public SpaceshipBlueprintRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<BlueprintSpaceship?> GetById(Guid idBlueprintSpaceship, Guid idUser)
    {
        var res = await _databaseContext.BlueprintSpaceships
            .Include(blueprintSpaceship => blueprintSpaceship.Modules)
            .FirstOrDefaultAsync(blueprintSpaceship =>
                blueprintSpaceship.Id == idBlueprintSpaceship && blueprintSpaceship.OwnerId == idUser);
        return res;
    }

    public async Task<List<BlueprintSpaceship>> GetAllByUser(Guid idUser)
    {
        var res = await _databaseContext.BlueprintSpaceships
            .Include(blueprintSpaceship => blueprintSpaceship.Modules)
            .Where(blueprintSpaceship => blueprintSpaceship.OwnerId == idUser)
            .ToListAsync();
        return res;
    }

    public async Task<BlueprintSpaceship> CreateOrUpdate(BlueprintSpaceship blueprintSpaceship, Guid idUser)
    {
        try
        {
            blueprintSpaceship.OwnerId = idUser;

            await _databaseContext.CreateOrUpdateAsync(blueprintSpaceship);

            //Delete old module infos 
            var oldModuleInfos = _databaseContext.BlueprintSpaceshipModules.Where(blueprintModule =>
                blueprintModule.BlueprintSpaceshipId == blueprintSpaceship.Id);
            _databaseContext.RemoveRange(oldModuleInfos);


            //Add new module infos
            blueprintSpaceship.UpdateModuleIds(true);
            _databaseContext.AddRange(blueprintSpaceship.Modules);

            await _databaseContext.SaveChangesAsync();

            return blueprintSpaceship;
        }
        catch (Exception e)
        {
            Log.Error(e, $"Error while creating or updating blueprint spaceship: {e}");
            throw;
        }
    }

    public async Task Delete(Guid spaceshipBlueprintId, Guid userId)
    {
        _databaseContext.BlueprintSpaceships.RemoveRange(_databaseContext.BlueprintSpaceships
            .Where(blueprintSpaceship =>
                blueprintSpaceship.Id == spaceshipBlueprintId && blueprintSpaceship.OwnerId == userId));

        await _databaseContext.SaveChangesAsync();
    }
}