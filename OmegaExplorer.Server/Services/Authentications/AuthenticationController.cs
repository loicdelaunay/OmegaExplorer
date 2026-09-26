#region

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services.Authentications.Contracts;
using OmegaExplorer.Server.Services.Authentications.Exceptions;
using OmegaExplorer.Server.Services.Jwt;
using OmegaExplorer.Server.Services.Loggers.Models.Enums;
using OmegaExplorer.Server.Services.Servers;
using OmegaExplorer.Server.Services.Users.Exceptions;
using OmegaExplorer.Server.Services.Users.Models.Contracts;
using OmegaExplorer.Server.Services.Users.Models.Entities;

#endregion

namespace OmegaExplorer.Server.Services.Authentications;

[Route("/api/auth")]
[ApiController]
public class AuthenticationController : ControllerCustom
{
    /// <summary>
    /// </summary>
    private readonly ILogger<AuthenticationController> _logger;
    private readonly IMapper _mapper;

    private readonly AuthenticationService _authenticationService;

    /// <summary>
    ///     Controller to authenticate user
    /// </summary>
    public AuthenticationController(
        ILogger<AuthenticationController> logger,
        AuthenticationService authenticationService,
        IMapper mapper) : base(authenticationService)
    {
        _logger = logger;
        _authenticationService = authenticationService;
        _mapper = mapper;
    }

    /// <summary>
    ///     Log user into OmegaExplorer and save JWT token
    /// </summary>
    /// <param name="requestAuthLogin"> </param>
    /// <returns> </returns>
    [HttpPost]
    [Route("login", Name = nameof(Login))]
    [AllowAnonymous]
    public async Task<ActionResult<ResponseUser>> Login([FromBody] RequestAuthLogin requestAuthLogin)
    {
        _logger.LogInformation($"login of {requestAuthLogin.Email}", EnumLogSeverity.Information, this);

        try
        {
            await Validate(requestAuthLogin);

            User user;

            user = await _authenticationService.GetUser(requestAuthLogin);

            var userMapped = _mapper.Map<ResponseUser>(user);

            var jwtToken = AuthorizationJwt.GenerateJwtToken(user);
            userMapped.Token = jwtToken;

            _logger.LogInformation($"user {userMapped.Email} logged");
            return userMapped;
        }
        catch (UserNotFoundException)
        {
            _logger.LogError($"user not found for email {requestAuthLogin.Email}");
            return StatusCode(StatusCodes.Status404NotFound);
        }
        catch (InvalidPasswordException)
        {
            _logger.LogError($"invalid password for email {requestAuthLogin.Email}");
            return StatusCode(StatusCodes.Status403Forbidden, "Invalid password");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "error during login");
            return StatusCodeGenerator.Exception(e);
        }
    }

    /// <summary>
    ///     Log user into OmegaExplorer and save JWT token
    /// </summary>
    /// <param name="requestAuth"> </param>
    /// <returns> </returns>
    [HttpPost]
    [Route("login/token", Name = nameof(LoginWithToken))]
    [AllowAnonymous]
    public async Task<ActionResult<ResponseUser>> LoginWithToken([FromBody] RequestAuthLoginWithToken requestAuth)
    {
        _logger.LogInformation("login using token");

        try
        {
            var jwtToken = AuthorizationJwt.ReadJwtToken(requestAuth.Token);
            User user;

            user = await _authenticationService.GetUserByToken(jwtToken);

            var userMapped = _mapper.Map<ResponseUser>(user);
            var newJwtToken = AuthorizationJwt.GenerateJwtToken(user);
            userMapped.Token = newJwtToken;

            _logger.LogInformation($"{user.Email} logged with token");
            return userMapped;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "error during login with token");
            return StatusCodeGenerator.Exception(e);
        }
    }

    /// <summary>
    ///     Register a new user into the database
    /// </summary>
    /// <param name="requestAuthRegister"> </param>
    /// <returns> </returns>
    [HttpPost]
    [Route("register", Name = nameof(Register))]
    [AllowAnonymous]
    public async Task<ActionResult<ResponseUser>> Register([FromBody] RequestAuthRegister requestAuthRegister)
    {
        _logger.LogInformation("Register user...");

        await Validate(requestAuthRegister);

        try
        {
            var newUser = await _authenticationService.CreateUser(requestAuthRegister);

            var res = _mapper.Map<ResponseUser>(newUser);

            _logger.LogInformation("User registered with id {NewUserId} and email {NewUserEmail}", newUser.Id, newUser.Email);

            return res;
        }
        catch (UserNotFoundException userNotFoundException)
        {
            _logger.LogError("User not found for email {Email}", requestAuthRegister.Email);
            return StatusCode(StatusCodes.Status404NotFound, userNotFoundException.Message);
        }
        catch (UserDisabledException)
        {
            _logger.LogError("User is disabled for email {Email}", requestAuthRegister.Email);
            return StatusCode(StatusCodes.Status403Forbidden, "User is disabled");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while creating user");
            return StatusCodeGenerator.Exception(e);
        }
    }
}
