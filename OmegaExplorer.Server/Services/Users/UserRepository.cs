using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.Services.Authentications.Contracts;
using OmegaExplorer.Server.Services.Databases;
using OmegaExplorer.Server.Services.Users.Models.Contracts;
using OmegaExplorer.Server.Services.Users.Models.Enum;

namespace OmegaExplorer.Server.Services.Users;

public class UserRepository
{
    private readonly DatabaseContext _databaseContext;

    public UserRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    /// <summary> The GetById function retrieves a user from the database by their id.</summary>
    /// <param name="id">
    ///     /// the id of the user to be deleted.
    /// </param>
    /// <returns> A user object.</returns>
    public async Task<Models.Entities.User?> GetUserById(Guid id, Models.Entities.User? userAsking = null)
    {
        var user = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        return user;
    }

    public async Task<bool> UserNameExist(string username)
    {
        return await _databaseContext.Users.AnyAsync(user => user.Name == username);
    }

    public async Task<List<Models.Entities.User>> GetUsers()
    {
        var users = await _databaseContext.Users.ToListAsync();

        return users;
    }

    public async Task<List<Models.Entities.User>?> GetContacts(Models.Entities.User user)
    {
        var users = await _databaseContext.Users.ToListAsync();
        return users;
    }

    public async Task UpdatePassword(Guid userId, string secureNewPassword)
    {
        var user = await _databaseContext.Users.SingleOrDefaultAsync(predicate: user => user.Id == userId);

        if (user == null)
        {
            throw new KeyNotFoundException();
        }

        user.DateEdition = DateTime.Now;
        user.Password = secureNewPassword;

        await _databaseContext.SaveChangesAsync();
    }

    public async Task UpdateLastConnection(Guid userId)
    {
        var user = await _databaseContext.Users.FindAsync(userId);

        if (user == null)
        {
            throw new KeyNotFoundException();
        }

        user.DateLastConnection = DateTime.Now;

        await _databaseContext.SaveChangesAsync();
    }

    /// <summary> The GetByEmail function returns a user from the database by their email address.</summary>
    /// <param name="email"> /// &lt;param name=&quot;string password&quot;&gt;</param>
    /// <returns> A user or null.</returns>
    public async Task<Models.Entities.User?> GetUserByEmail(string email)
    {
        var user = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Email == email);
        return user;
    }

    public async Task UpdateUser(RequestUpdateUser requestUpdateUser)
    {
        var user = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Id == requestUpdateUser.UserId);

        if (user == null)
        {
            throw new KeyNotFoundException();
        }

        if (requestUpdateUser.Name != null)
        {
            user.Name = requestUpdateUser.Name;
        }

        if (requestUpdateUser.Disabled != null)
        {
            user.Disabled = (bool)requestUpdateUser.Disabled;
        }

        await _databaseContext.SaveChangesAsync();
    }

    /// <summary> The Add function adds a new user to the database.</summary>
    /// <param name="requestAuthRegister"></param>
    /// <param name="securePassword"> this is the password that will be stored in the database. </param>
    /// <param name="accessLevel"></param>
    /// <param name="enabled"></param>
    /// <returns> A user object</returns>
    public async Task<Models.Entities.User> CreateUser(RequestAuthRegister requestAuthRegister, string securePassword,
        EnumUserAccessLevel accessLevel = EnumUserAccessLevel.Player, bool? enabled = null)
    {
        // By default, follow the configuration but if the parameter is set, use it.
        var accountEnabled = ConfigurationManager.Configuration.Server.DefaultEnabledAccount;
        if (enabled != null)
        {
            accountEnabled = (bool)enabled;
        }

        Models.Entities.User newUser = new()
        {
            Email = requestAuthRegister.Email,
            Password = securePassword,
            Disabled = !accountEnabled,
            AccessLevel = accessLevel
        };

        await _databaseContext.Users.AddAsync(newUser);
        await _databaseContext.SaveChangesAsync();
        return newUser;
    }

    /// <summary> The SetUserDisable function sets the Disabled property of a user to true or false.</summary>
    /// <param name="value"> the value to set the user.disabled property to. true or false.</param>
    /// <param name="userId"> the guid iduser parameter is used to identify the user.</param>
    /// <returns> The value of the user.disabled property.</returns>
    public async Task SetUserDisable(bool value, Guid userId)
    {
        var user = await _databaseContext.Users.FirstOrDefaultAsync(user => user.Id == userId);

        if (user == null) throw new KeyNotFoundException();

        user.Disabled = value;
        await _databaseContext.SaveChangesAsync();
    }


    /// <summary> The SetUserRole function sets the user's role to a new value.</summary>
    /// <param name="value"></param>this is the access level that you want to set for the user.
    /// <param name="userId"> the guid  parameter is used to identify the user.</param>
    /// <returns> A task.</returns>
    public async Task SetUserRole(EnumUserAccessLevel value, Guid userId)
    {
        var user = await _databaseContext.Users.FirstOrDefaultAsync(user => user.Id == userId);

        if (user == null) throw new KeyNotFoundException();

        user.AccessLevel = value;
        await _databaseContext.SaveChangesAsync();
    }

    public async Task UpdateUserName(Guid userId, string newName)
    {
        var user = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
        {
            throw new KeyNotFoundException();
        }

        user.Name = newName;
        await _databaseContext.SaveChangesAsync();
    }

    public async Task<int> GetUsersCount()
    {
        var usersCount = await _databaseContext.Users.CountAsync();

        return usersCount;
    }
}