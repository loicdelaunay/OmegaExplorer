using Microsoft.AspNetCore.Mvc;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Celebrities.Models.Interfaces;
using OmegaExplorer.Server.Services.Authentications;
using OmegaExplorer.Server.Services.Servers;

namespace OmegaExplorer.Server.Services._Game.Celebrities;

[ApiController]
[Route("api/celebrity")]
public class CelebrityController : ControllerCustom
{
    private readonly CelebrityDataProvider _celebrityDataProvider;
    private readonly CelebrityService _celebrityService;
    private readonly ILogger<CelebrityController> _logger;


    #region Get

    public CelebrityController(AuthenticationService authenticationService, ILogger<CelebrityController> logger,
        CelebrityDataProvider celebrityDataProvider) : base(authenticationService)
    {
        _logger = logger;

        _celebrityDataProvider = celebrityDataProvider;
    }

    [HttpGet]
    [Route("get/all", Name = nameof(GetAllCelebrities))]
    public ActionResult<List<IDynamicItemCelebrity>> GetAllCelebrities()
    {
        try
        {
            var celebrities = _celebrityDataProvider.GetAll();

            return celebrities;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting all celebrities");
            return StatusCodeGenerator.Exception(e);
        }
    }

    [HttpGet]
    [Route("get/by/index", Name = nameof(GetCelebrityById))]
    public ActionResult<IDynamicItemCelebrity?> GetCelebrityById(int index)
    {
        try
        {
            var celebrity = _celebrityDataProvider.GetByIndex(index);

            return Ok(celebrity);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting celebrity by index {Index}", index);
            return StatusCodeGenerator.Exception(e);
        }
    }

    #endregion
}