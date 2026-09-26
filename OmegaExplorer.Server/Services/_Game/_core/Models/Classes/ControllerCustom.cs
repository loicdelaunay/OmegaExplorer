using Microsoft.AspNetCore.Mvc;
using OmegaExplorer.Server.Services.Authentications;
using OmegaExplorer.Server.Services.Users.Models.Entities;
using OmegaExplorer.Server.Services.Validations;

namespace OmegaExplorer.Server.Services._Game._core.Models.Classes;

public class ControllerCustom : ControllerBase
{
    // ReSharper disable once InconsistentNaming
    protected readonly AuthenticationService _authenticationService;

    public ControllerCustom(AuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    /// <summary>
    ///     Get the current user from the authorization header
    /// </summary>
    /// <returns></returns>
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<User?> GetUser(string? token = null)
    {
        User? user;

        if (!string.IsNullOrEmpty(token))
        {
            user = await _authenticationService.GetUserByToken(token);
        }
        else
        {
            var ctx = HttpContext;
            user = (User)ctx.Items["User"]!;
        }

        return user;
    }

    /// <summary>
    ///     Check if request is valid or not
    /// </summary>
    /// <param name="elementToValidate"></param>
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task Validate(IValid elementToValidate)
    {
        var result = await elementToValidate.Validate();

        if (result != null) BadRequest(result.Reason());
    }
}