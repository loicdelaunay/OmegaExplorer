using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Results;
using OmegaExplorer.Server.Services.Databases;
using OmegaExplorer.Server.Services.Users.Models.Entities;

namespace OmegaExplorer.Server.Services._Game.GameResources;

public class GameResourceRepository
{
    private readonly DatabaseContext _databaseContext;

    public GameResourceRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    #region CREATE

    public async Task<Models.Entities.GameResource> Create(Guid userId, int index)
    {
        var user = await _databaseContext.Users.FindAsync(userId);

        if (user == null) throw new Exception($"User not found when creating a new game resource : {userId}");


        Models.Entities.GameResource newResource = new()
        {
            Index = index,
            UserId = userId
        };

        _databaseContext.Resources.Add(newResource);
        await _databaseContext.SaveChangesAsync();

        return newResource;
    }

    #endregion

    /// <summary>
    ///     Change the balance of a resource for a user
    /// </summary>
    /// <param name="index"></param>
    /// <param name="amount"> Modifier (can be positive or negative) </param>
    /// <param name="set"> If the amount is a new value </param>
    /// <param name="userId"></param>
    public async Task ChangeBalance(Guid userId, int index, int amount, bool set = false)
    {
        var res = await GetByIndex(userId, index);

        if (res == null) res = await Create(userId, index);

        if (set)
            res.Balance = amount;
        else
            res.Balance += amount;

        _databaseContext.Update(res);
        await _databaseContext.SaveChangesAsync();
    }

    /// <summary>
    ///     Use the stock of a resource for a user
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="index"> </param>
    /// <param name="amount"> </param>
    public async Task<ResultGameResourceConsumed> Consume(Guid userId, int index, int amount)
    {
        ResultGameResourceConsumed res = new();

        var resource = await GetByIndex(userId, index);

        //No resource from this type
        if (resource == null)
        {
            res.Success = false;
            return res;
        }

        if (resource.Amount < amount)
        {
            res.AmountRemaining = resource.Amount;
            res.Success = false;
            return res;
        }

        resource.Amount -= amount;

        res.GameResource = resource;
        res.AmountRemaining = resource.Amount;
        res.Success = true;

        _databaseContext.Update(resource);
        await _databaseContext.SaveChangesAsync();

        return res;
    }

    /// <summary>
    ///     Add amount to a resource
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="index"> </param>
    /// <param name="amount"> </param>
    /// <returns> </returns>
    public async Task<Models.Entities.GameResource> AddAmount(Guid userId, int index, int amount)
    {
        var resource = await GetByIndex(userId, index);

        if (resource == null) resource = await Create(userId, index);

        resource.Amount += amount;

        _databaseContext.Update(resource);
        await _databaseContext.SaveChangesAsync();

        return resource;
    }

    /// <summary>
    ///     Set amount of a resource
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="index"> </param>
    /// <param name="amount"> </param>
    /// <returns> </returns>
    public async Task<int> UpdateAmount(Guid userId, int index, int amount)
    {
        var resource = await GetByIndex(userId, index);

        if (resource == null) resource = await Create(userId, index);

        resource.Amount = amount;

        _databaseContext.Update(resource);
        await _databaseContext.SaveChangesAsync();

        return resource.Amount;
    }

    #region GET

    /// <summary>
    ///     Get all resources of a user
    /// </summary>
    /// <param name="idUser"> </param>
    /// <returns> </returns>
    public async Task<List<Models.Entities.GameResource>> GetAllByUser(Guid idUser)
    {
        var res = await _databaseContext.Resources
            .Where(resource => resource.User != null && resource.User.Id == idUser)
            .Include(resource => resource.User)
            .ToListAsync();

        return res;
    }

    public async Task<Models.Entities.GameResource?> GetById(User user, Guid resourceId)
    {
        var res = await _databaseContext.Resources
            .Where(resource => resource.User != null && resource.User.Id == user.Id && resource.Id == resourceId)
            .Include(resource => resource.User)
            .FirstOrDefaultAsync();
        return res;
    }

    /// <summary>
    ///     Get a resource by its unique ID index
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="index"></param>
    /// <returns> </returns>
    public async Task<Models.Entities.GameResource?> GetByIndex(Guid userId, int index)
    {
        var res = await _databaseContext.Resources
            .Where(resource => resource.User != null && resource.User.Id == userId && resource.Index == index)
            .Include(resource => resource.User)
            .FirstOrDefaultAsync();
        return res;
    }

    public async Task<Models.Entities.GameResource?> GetByName(User user, string name)
    {
        var res = await _databaseContext.Resources
            .Where(resource => resource.User != null && resource.User.Id == user.Id && resource.Name == name)
            .Include(resource => resource.User)
            .FirstOrDefaultAsync();
        return res;
    }

    #endregion
}