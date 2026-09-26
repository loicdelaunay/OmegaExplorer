using Microsoft.AspNetCore.Mvc;
using OmegaExplorer.Server.Services._Core.Extensions;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Maps;
using OmegaExplorer.Server.Services._Game.Spaceships;
using OmegaExplorer.Server.Services.Authentications;
using OmegaExplorer.Server.Services.Servers;
using OmegaExplorer.Server.Services.Users.Models.Enum;
using OmegaExplorer.Server.Services.Volumes;

namespace OmegaExplorer.Server.Services.Admins;

[ApiController]
[Route("/api/admin")]
public class AdminController : ControllerCustom
{
    private readonly ILogger<AdminController> _logger;

    private readonly MapService _mapService;
    private readonly SpaceshipService _spaceshipService;

    public AdminController(
        ILogger<AdminController> logger,
        AuthenticationService authenticationService, MapService mapService, SpaceshipService spaceshipService)
        : base(authenticationService)
    {
        _logger = logger;
        _mapService = mapService;
        _spaceshipService = spaceshipService;
    }

    [HttpPost]
    [Route("generate/map", Name = nameof(GenerateMap))]
    public async Task<ActionResult> GenerateMap(bool clean = false)
    {
        try
        {
            var user = await GetUser();

            _logger.LogInformation($"User {user?.Id} requested to generate a new map");

            if (!Program.StartMode.IsDevelopment())
            {
                _logger.LogError($"User is not allowed to generate a map in this environment {Program.StartMode}");
                return StatusCodeGenerator.Forbidden();
            }

            if (user == null || user.AccessLevel < EnumUserAccessLevel.Admin)
            {
                _logger.LogError($"User is not allowed to generate a map with access level {user?.AccessLevel}");
                return StatusCodeGenerator.Forbidden();
            }

            await _mapService.GenerateMap(clean);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCodeGenerator.Exception(e);
        }
    }

    [HttpPost]
    [Route("delete/pirates", Name = nameof(DeleteAllPirates))]
    public async Task<ActionResult> DeleteAllPirates()
    {
        try
        {
            var user = await GetUser();

            _logger.LogInformation($"User {user?.Id} requested to delete all pirates");

            if (user == null || user.AccessLevel < EnumUserAccessLevel.Admin)
            {
                _logger.LogError($"User is not allowed to delete all pirates with access level {user?.AccessLevel}");
                return StatusCodeGenerator.Forbidden();
            }

            await _spaceshipService.DeleteAllPirateSpaceships();

            _logger.LogInformation($"All pirates deleted");

            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError($"Error while deleting all pirates : {e}");
            return StatusCodeGenerator.Exception(e);
        }
    }

    [HttpPost]
    [Route("cache/clear", Name = nameof(ClearCache))]
    public async Task<ActionResult> ClearCache()
    {
        var user = await GetUser();

        _logger.LogInformation($"User {user?.Id} requested to delete caches");

        if (user == null || user.AccessLevel < EnumUserAccessLevel.Admin)
        {
            _logger.LogError($"User is not allowed to delete caches with access level {user?.AccessLevel}");
            return StatusCodeGenerator.Forbidden();
        }

        var cacheDirectory = VolumeManager.GetSpaceshipThumbnailDirectory();

        cacheDirectory.Delete(true);

        return Ok();
    }

    [HttpPost]
    [Route("cache/generate", Name = nameof(GenerateCache))]
    public async Task<ActionResult> GenerateCache()
    {
        throw new NotImplementedException();
    }

}
