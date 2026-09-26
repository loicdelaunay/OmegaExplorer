using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services.Authentications.Contracts;
using OmegaExplorer.Server.Services.Servers;
using OmegaExplorer.Server.Services.Users.Models.Contracts;

namespace OmegaExplorer.Server.Services.Authentications;

[ApiController]
[Route("api/auth/")]
public class AuthenticationGoogleController : ControllerCustom
{
    private readonly ILogger<AuthenticationGoogleController> _logger;
    private readonly IMapper _mapper;

    public AuthenticationGoogleController(
        ILogger<AuthenticationGoogleController> logger,
        AuthenticationService authenticationService,
        IMapper mapper)
        : base(authenticationService)
    {
        _logger = logger;
        _mapper = mapper;
    }

    [HttpPost("google-login")]
    public async Task<ActionResult<ResponseUser>> GoogleLogin([FromBody] RequestAuthGoogleLogin request)
    {
        try
        {
            _logger.LogInformation("Google login attempt");

            var user = await _authenticationService.GetUserByGoogleToken(request.TokenId);

            var userMapped = _mapper.Map<ResponseUser>(user);

            if (user.Token == null)
            {
                throw new Exception("User token is null");
            }

            userMapped.Token = user.Token;

            return userMapped;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error during Google login");
            return StatusCodeGenerator.Exception(e);
        }
    }
}