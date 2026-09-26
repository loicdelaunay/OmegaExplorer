using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.Services.Databases;

namespace OmegaExplorer.Server.Services.Authentications;

public class AuthenticationRepository
{
    private readonly DatabaseContext _databaseContext;

    private readonly ILogger<AuthenticationRepository> _logger;

    public AuthenticationRepository(ILogger<AuthenticationRepository> logger, DatabaseContext databaseContext)
    {
        _logger = logger;
        _databaseContext = databaseContext;
    }

    public async Task<bool> IsUserMaxCountReached()
    {


        var usersCount = await _databaseContext.Users.CountAsync();
        if (usersCount >= ConfigurationManager.Configuration.Server.MaxRegisteredUsers)
        {
            return true;
        }

        return false;
    }
}