#region

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services.Authentications;
using OmegaExplorer.Server.Services.Authentications.Contracts;
using OmegaExplorer.Server.Services.Loggers.Models.Enums;
using OmegaExplorer.Server.Services.Servers;
using OmegaExplorer.Server.Services.Users.Extensions;
using OmegaExplorer.Server.Services.Users.Models.Contracts;
using OmegaExplorer.Server.Services.Users.Models.Enum;
using System.Net;

#endregion

namespace OmegaExplorer.Server.Services.Users;

[Route("/api/user")]
[ApiController]
public class UserController : ControllerCustom
{
    private readonly ILogger<UserController> _logger;

    private readonly UserService _userService;
    private readonly IMapper _mapper;
    private readonly UserRepository _userRepository;

    public UserController(
        ILogger<UserController> logger,
        AuthenticationService authenticationService, UserService userService, IMapper mapper, UserRepository userRepository)
        : base(authenticationService)
    {
        _logger = logger;
        _userService = userService;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    /// <summary>
    ///     Log user into OmegaExplorer and save JWT token
    /// </summary>
    /// <param name="requestAuthLogin"> </param>
    /// <returns> </returns>
    [HttpGet]
    [Route("users", Name = nameof(GetAllUsers))]
    public async Task<ActionResult<List<ResponseUser>>> GetAllUsers()
    {
        var user = await GetUser();
        _logger.LogInformation($"getting all users...");

        if (user == null)
        {
            _logger.LogError("user not connected");
            return StatusCodeGenerator.NotConnected();
        }

        if (!user.HaveAccess(EnumUserAccessLevel.Admin))
        {
            _logger.LogError($"user not admin");
            return StatusCodeGenerator.Forbidden();
        }

        var users = await _userService.GetUsers();

        var usersMapped = _mapper.Map<List<ResponseUser>>(users);

        _logger.LogInformation("{UsersCount} users retrieved", users.Count);
        return usersMapped;
    }

    [HttpGet]
    [Route("check/name", Name = nameof(CheckNameExist))]
    [AllowAnonymous]
    public async Task<ActionResult<bool>> CheckNameExist(string username)
    {
        var exist = await _userRepository.UserNameExist(username);

        return exist;
    }

    /// <summary>
    ///     Log user into OmegaExplorer and save JWT token
    /// </summary>
    /// <param name="requestAuthLogin"> </param>
    /// <returns> </returns>
    [HttpGet]
    [Route("contacts", Name = nameof(GetUserContacts))]
    public async Task<ActionResult<List<ResponseUser>>> GetUserContacts()
    {
        var user = await GetUser();
        _logger.LogInformation($"getting contact users...", EnumLogSeverity.Information, this, user);

        var users = await _userRepository.GetContacts(user);

        var usersMapped = _mapper.Map<List<ResponseUser>>(users);

        _logger.LogInformation($"users got", EnumLogSeverity.Success, this, user);
        return usersMapped;
    }

    /// <summary>
    ///     Log user into OmegaExplorer and save JWT token
    /// </summary>
    /// <param name="requestAuthLogin"> </param>
    /// <param name="userId"></param>
    /// <returns> </returns>
    [HttpGet]
    [Route("get/user", Name = nameof(GetUserById))]
    public async Task<ActionResult<ResponseUser>> GetUserById(Guid userId)
    {
        var user = await GetUser();
        _logger.LogInformation($"getting users {userId}...", EnumLogSeverity.Information, this, user);

        var users = await _userRepository.GetUserById(userId, user);

        var userMapped = _mapper.Map<ResponseUser>(users);

        _logger.LogInformation($"user got", EnumLogSeverity.Success, this, user);
        return userMapped;
    }

    [HttpPatch]
    [Route("update", Name = nameof(UpdateUser))]
    public async Task<ActionResult> UpdateUser(RequestUpdateUser requestUpdateUser)
    {
        var user = await GetUser();
        _logger.LogInformation($"updating user {requestUpdateUser.UserId}...", EnumLogSeverity.Information, this, user);

        if (user == null)
        {
            return StatusCodeGenerator.NotConnected();
        }

        if (!user.HaveAccess(EnumUserAccessLevel.Admin))
        {
            _logger.LogInformation($"user try to update without admin right", EnumLogSeverity.Success, this, user);
            return Ok();
        }

        try
        {
            await _userRepository.UpdateUser(requestUpdateUser);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"error when updating user {e}");
            return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
        }

        _logger.LogInformation($"user updated");
        return Ok();
    }

    [HttpPatch]
    [Route("update/name", Name = nameof(UpdateUserName))]
    public async Task<ActionResult> UpdateUserName(string newName)
    {
        var currentUser = await GetUser();

        if (currentUser == null)
        {
            return StatusCodeGenerator.NotFound(nameof(User));
        }

        _logger.LogInformation($"updating user name of {currentUser.Email} with {newName}...", EnumLogSeverity.Information, this, currentUser);

        try
        {
            await _userRepository.UpdateUserName(currentUser.Id, newName);
        }
        catch (Exception e)
        {
            _logger.LogError("error when updating user name {e}", e);
            return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
        }

        _logger.LogInformation("user name updated");
        return Ok();
    }

    /// <summary>
    ///     Update password of a user
    /// </summary>
    /// <param name="requestUpdateUserPassword"> </param>
    /// <returns> </returns>
    [HttpPost]
    [Route("update/password", Name = nameof(UpdatePassword))]
    public async Task<ActionResult> UpdatePassword([FromBody] RequestUpdateUserPassword requestUpdateUserPassword)
    {
        try
        {
            var user = await GetUser();

            if (user == null)
            {
                _logger.LogError($"user not connected");
                return StatusCodeGenerator.NotConnected();
            }

            await _userService.UpdatePassword(user, requestUpdateUserPassword);

            await Validate(requestUpdateUserPassword);

            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "error while updating password");
            return StatusCodeGenerator.Exception(e);
        }
    }
}
