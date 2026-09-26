using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.Extensions;
using OmegaExplorer.Server.Services._Game.GameResources;
using OmegaExplorer.Server.Services._Game.GameResources.Data;
using OmegaExplorer.Server.Services.Authentications.Contracts;
using OmegaExplorer.Server.Services.Authentications.Exceptions;
using OmegaExplorer.Server.Services.Databases;
using OmegaExplorer.Server.Services.Securities;
using OmegaExplorer.Server.Services.Users.Exceptions;
using OmegaExplorer.Server.Services.Users.Extensions;
using OmegaExplorer.Server.Services.Users.Models.Enum;

namespace OmegaExplorer.Server.Services.Users;

public class UserService
{
    private readonly DatabaseContext _databaseContext;

    private readonly ILogger<UserService> _logger;

    private readonly SecurityService _securityService;
    private readonly GameResourceService _gameResourceService;

    private readonly UserRepository _userRepository;

    public UserService(
        ILogger<UserService> logger,
        UserRepository userRepository,
        SecurityService securityService, GameResourceService gameResourceService, DatabaseContext databaseContext)
    {
        _logger = logger;
        _userRepository = userRepository;
        _securityService = securityService;
        _gameResourceService = gameResourceService;
        _databaseContext = databaseContext;
    }

    public async Task<List<Models.Entities.User>> GetUsers()
    {
        var users = await _userRepository.GetUsers();

        return users;
    }

    /// <summary>
    ///     Register defaults values of the player
    /// </summary>
    /// <param name="user"> </param>
    public async Task RegisterNewUser(Models.Entities.User user)
    {
        await AddDefaultResources(user: user);
    }

    private async Task AddDefaultResources(Models.Entities.User user)
    {
        await _gameResourceService.AddResourceToPlayer(DynamicItemGameResource_0_Credit.INDEX, 100000, user.Id);
    }

    public async Task<Models.Entities.User?> GetUserById(Guid userId)
    {
        var user = await _databaseContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

        return user;
    }


    public async Task UpdatePassword(Models.Entities.User user,
        RequestUpdateUserPassword requestUpdateUserPassword)
    {
        //Check if a user try to edit other user
        if (!requestUpdateUserPassword.UserId.IsNullOrEmpty())
        {
            if (!user.HaveAccess(EnumUserAccessLevel.Admin))
            {
                _logger.LogError($"user {user.Email} try to edit other user without authorization !");
                throw new UnauthorizedAccessException();
            }
        }

        var userToEditId = user.Id;
        if (requestUpdateUserPassword.UserId != null)
        {
            userToEditId = (Guid)requestUpdateUserPassword.UserId;
        }

        //If the current user is modifying his own password, check if the old password is valid
        if (requestUpdateUserPassword.UserId.IsNullOrEmpty())
        {
            var userToEdit = await _userRepository.GetUserById(userToEditId);

            if (userToEdit == null)
            {
                throw new UserNotFoundException();
            }

            if (string.IsNullOrEmpty(requestUpdateUserPassword.OldPassword))
            {
                throw new ArgumentNullException(requestUpdateUserPassword.OldPassword);
            }

            var oldSecuredPassword = _securityService.ProtectString(requestUpdateUserPassword.OldPassword);

            if (userToEdit.Password != oldSecuredPassword)
            {
                throw new InvalidPasswordException();
            }
        }

        //Hash password
        var newSecuredPassword = _securityService.ProtectString(requestUpdateUserPassword.NewPassword);

        await _userRepository.UpdatePassword(userToEditId, newSecuredPassword);
    }

    public async Task<Models.Entities.User?> GetUserByEmail(string payloadEmail)
    {
        var user = await _userRepository.GetUserByEmail(payloadEmail);

        return user;
    }
}