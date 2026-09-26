using Microsoft.AspNetCore.Mvc;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Interfaces;
using OmegaExplorer.Server.Services.Authentications;
using OmegaExplorer.Server.Services.Servers;

namespace OmegaExplorer.Server.Services._Game.Technologies;

[Route("/api/technologies")]
public class TechnologyController : ControllerCustom
{
    private readonly ILogger<TechnologyController> _logger;
    private readonly TechnologyService _technologyService;

    public TechnologyController(AuthenticationService authenticationService, ILogger<TechnologyController> logger,
        TechnologyService technologyService) : base(authenticationService)
    {
        _logger = logger;
        _technologyService = technologyService;
    }

    #region GET

    /// <summary>
    /// </summary>
    [HttpGet]
    [Route("get/all", Name = "GetAllTechnologies")]
    public async Task<ActionResult<List<IDynamicItemTechnology>>> GetAll()
    {
        var user = await GetUser();
        _logger.LogInformation("getting all technologies ...");

        if (user == null)
        {
            _logger.LogInformation("user not connected ...");
            return StatusCodeGenerator.NotConnected();
        }

        var res = await _technologyService.GetAllTechnologies(user.Id);

        _logger.LogInformation("get all technologies done");
        return res;
    }

    #endregion
}