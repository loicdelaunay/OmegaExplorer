using Microsoft.AspNetCore.Mvc;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Cycles.Models.Contracts;
using OmegaExplorer.Server.Services.Authentications;
using OmegaExplorer.Server.Services.Servers;
using OmegaExplorer.Server.Services.Users.Models.Enum;

namespace OmegaExplorer.Server.Services._Game.Cycles;

[ApiController]
[Route("/api/cycle")]
public class CycleController : ControllerCustom
{
    private readonly CycleBackgroundService _cycleBackgroundService;

    private readonly CycleService _cycleService;
    private readonly ILogger<CycleController> _logger;


    public CycleController(AuthenticationService authenticationService, ILogger<CycleController> logger,
        CycleService cycleService, CycleBackgroundService cycleBackgroundService) : base(authenticationService)
    {
        _logger = logger;

        _cycleService = cycleService;
        _cycleBackgroundService = cycleBackgroundService;
    }

    [HttpGet]
    [Route("get", Name = nameof(GetCycleState))]
    public ActionResult<ResponseCycleState>? GetCycleState()
    {
        try
        {
            var res = _cycleBackgroundService.GetCycleState();
            return res;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting cycle state");
        }

        return null;
    }

    [HttpGet]
    [Route("get/is-paused", Name = nameof(GetCycleIsPaused))]
    public async Task<ActionResult<bool>> GetCycleIsPaused()
    {
        try
        {
            var user = await GetUser();

            if (user == null) return StatusCodeGenerator.NotConnected();

            var isPaused = _cycleBackgroundService.IsCyclePaused();

            return isPaused;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting cycle paused state");
            return StatusCodeGenerator.Exception(e);
        }
    }

    [HttpPost]
    [Route("admin/force-next-cycle", Name = "AdminForceNextCycle")]
    public async Task<ActionResult<ResponseCycleState>?> DebugForceNextCycle()
    {
        var user = await GetUser();

        if (user == null) return StatusCodeGenerator.NotConnected();

        if (user.AccessLevel < EnumUserAccessLevel.Admin) return StatusCodeGenerator.Forbidden();

        await _cycleBackgroundService.ForceSkipCycle();

        return GetCycleState();
    }

    [HttpPost]
    [Route("admin/pause-cycle", Name = "AdminPauseCycle")]
    public async Task<ActionResult<ResponseCycleState>?> DebugPauseCycle()
    {
        var user = await GetUser();

        if (user == null) return StatusCodeGenerator.NotConnected();

        if (user.AccessLevel < EnumUserAccessLevel.Admin) return StatusCodeGenerator.Forbidden();

        _cycleBackgroundService.PauseCycle();

        return GetCycleState();
    }

    [HttpPost]
    [Route("admin/resume-cycle", Name = "AdminResumeCycle")]
    public async Task<ActionResult<ResponseCycleState>?> DebugResumeCycle()
    {
        var user = await GetUser();

        if (user == null) return StatusCodeGenerator.NotConnected();

        if (user.AccessLevel < EnumUserAccessLevel.Admin) return StatusCodeGenerator.Forbidden();

        _cycleBackgroundService.ResumeCycle();

        return GetCycleState();
    }
}