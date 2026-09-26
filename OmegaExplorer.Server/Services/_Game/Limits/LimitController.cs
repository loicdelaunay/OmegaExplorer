using Microsoft.AspNetCore.Mvc;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Limits.Models.Entities;
using OmegaExplorer.Server.Services.Authentications;
using OmegaExplorer.Server.Services.Servers;

namespace OmegaExplorer.Server.Services._Game.Limits;

[ApiController]
[Route("/api/user/limit")]
public class LimitController : ControllerCustom
{
    private readonly LimitService _limitService;

    public LimitController(AuthenticationService authenticationService, LimitService limitService) : base(
        authenticationService)
    {
        _limitService = limitService;
    }

    [HttpGet]
    [Route("max/planets/colonized", Name = nameof(GetLimitSystemsColonized))]
    public async Task<ActionResult<Limit>> GetLimitSystemsColonized()
    {
        var user = await GetUser();

        if (user == null) return StatusCodeGenerator.NotConnected();

        var limit = await _limitService.GetLimitStarSystemByUser(user.Id);

        return limit;
    }
}