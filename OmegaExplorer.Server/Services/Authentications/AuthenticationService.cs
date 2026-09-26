using Google.Apis.Auth;
using OmegaExplorer.Server.Services.Authentications.Contracts;
using OmegaExplorer.Server.Services.Authentications.Exceptions;
using OmegaExplorer.Server.Services.Jwt;
using OmegaExplorer.Server.Services.Securities;
using OmegaExplorer.Server.Services.Users;
using OmegaExplorer.Server.Services.Users.Exceptions;
using OmegaExplorer.Server.Services.Users.Models.Entities;
using OmegaExplorer.Server.Services.Users.Models.Enum;
using System.IdentityModel.Tokens.Jwt;

namespace OmegaExplorer.Server.Services.Authentications;

public class AuthenticationService
{
    private readonly ILogger<AuthenticationService> _logger;

    private readonly AuthenticationRepository _authenticationRepository;

    private readonly UserRepository _userRepository;
    private readonly SecurityService _securityService;
    private readonly UserService _userService;

    public AuthenticationService(AuthenticationRepository authenticationRepository, UserRepository userRepository,
        SecurityService securityService, UserService userService, ILogger<AuthenticationService> logger)
    {
        _authenticationRepository = authenticationRepository;
        _userRepository = userRepository;
        _securityService = securityService;
        _userService = userService;
        _logger = logger;
    }

    public async Task<User> CreateUser(RequestAuthRegister requestAuthRegister)
    {
        // Check if user max count is reached
        var userMaxCountReached = await _authenticationRepository.IsUserMaxCountReached();

        if (userMaxCountReached)
        {
            throw new MaxUserReachedException();
        }

        //Check if user exist
        var userAlreadyExist = await _userRepository.GetUserByEmail(requestAuthRegister.Email);

        if (userAlreadyExist is not null)
        {
            throw new UserAlreadyExistException();
        }

        //Hash password
        var securePassword = _securityService.ProtectString(requestAuthRegister.Password);

        //if first user set as admin and enabled account
        var usersCount = await _userRepository.GetUsersCount();
        var isFirstUser = usersCount == 0;

        var accessLevel = isFirstUser ? EnumUserAccessLevel.Admin : EnumUserAccessLevel.Player;

        // Set default enabled account if not set, but if it is the first user, set it to true
        var enabled = ConfigurationManager.Configuration.Server.DefaultEnabledAccount;
        if (!enabled && isFirstUser)
        {
            enabled = true;
        }

        var newUser = await _userRepository.CreateUser(requestAuthRegister,
            securePassword, accessLevel, enabled);

        await _userService.RegisterNewUser(newUser);

        return newUser;
    }

    public async Task<User> GetUser(RequestAuthLogin requestAuthLogin)
    {
        //First check user by email
        var user = await _userRepository.GetUserByEmail(requestAuthLogin.Email);

        if (user == null)
        {
            throw new UserNotFoundException();
        }

        if (user.Disabled)
        {
            throw new UserDisabledException();
        }

        //Check pwd
        var securePassword = _securityService.VerifyString(requestAuthLogin.Password, user.Password);

        if (!securePassword)
        {
            throw new InvalidPasswordException();
        }

        return user;
    }

    public async Task<User> GetUserByToken(JwtSecurityToken jwtSecurityToken)
    {
        var userId = Guid.Parse(jwtSecurityToken.Claims.First(x => x.Type == "id").Value);

        var user = await _userRepository.GetUserById(userId);

        if (user == null)
        {
            throw new UserNotFoundException();
        }

        if (user.Disabled)
        {
            throw new UserDisabledException();
        }

        return user;
    }

    public async Task<User> GetUserByGoogleToken(string googleTokenId)
    {
        // Validate the Google token
        var payload = await ValidateGoogleTokenAsync(googleTokenId);
        if (payload == null)
        {
            throw new InvalidGoogleTokenException();
        }

        // Try to get user with email if exist return jwt else create
        var user = await _userService.GetUserByEmail(payload.Email);

        if (user == null)
        {
            RequestAuthRegister requestAuth = new()
            {
                Email = payload.Email,
                Password = _securityService.GenerateRandomPassword(),
            };

            user = await CreateUser(requestAuth);
        }


        var jwt = AuthorizationJwt.GenerateJwtToken(user);
        user.Token = jwt;

        return user;
    }

    /// <summary>
    ///     Get user from a raw jwt token
    /// </summary>
    /// <param name="token"> </param>
    /// <returns> </returns>
    public async Task<User?> GetUserByToken(string token)
    {
        var jwtToken = AuthorizationJwt.ReadJwtToken(token);
        var userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
        var user = await _userRepository.GetUserById(userId);

        return user;
    }

    public async Task<GoogleJsonWebSignature.Payload?> ValidateGoogleTokenAsync(string idToken)
    {
        try
        {
            var googleId = ConfigurationManager.Configuration.Server.GoogleClientId;

            GoogleJsonWebSignature.ValidationSettings settings = new()
            {
                // Replace with your Google Client ID
                Audience = new List<string>() { googleId }
            };
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
            return payload;
        }
        catch (Exception)
        {
            // Log exception if needed
            return null;
        }
    }
}