using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Spaceships.Models.Entities;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint.Models.Interfaces;
using OmegaExplorer.Server.Services.Authentications;
using OmegaExplorer.Server.Services.Servers;
using OmegaExplorer.Server.Services.Users.Models.Enum;

namespace OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint;

[Route("/api/spaceship/blueprint")]
public class SpaceshipBlueprintController : ControllerCustom
{
    private readonly ILogger<SpaceshipBlueprintController> _logger;
    private readonly IMapper _mapper;
    private readonly SpaceshipBlueprintDataProvider _spaceshipBlueprintDataProvider;

    private readonly SpaceshipBlueprintRepository _spaceshipBlueprintRepository;
    private readonly SpaceshipRepository _spaceshipRepository;

    public SpaceshipBlueprintController(AuthenticationService authenticationService,
        SpaceshipBlueprintRepository spaceshipBlueprintRepository, SpaceshipRepository spaceshipRepository,
        ILogger<SpaceshipBlueprintController> logger, IMapper mapper,
        SpaceshipBlueprintDataProvider spaceshipBlueprintDataProvider) : base(authenticationService)
    {
        _spaceshipBlueprintRepository = spaceshipBlueprintRepository;
        _spaceshipRepository = spaceshipRepository;
        _logger = logger;
        _mapper = mapper;
        _spaceshipBlueprintDataProvider = spaceshipBlueprintDataProvider;
    }

    #region CREATE

    /// <summary>
    ///     Create a new spaceship blueprint
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("create-or-update", Name = nameof(CreateOrUpdateSpaceshipBlueprint))]
    public async Task<ActionResult<ResponseBlueprintSpaceship>> CreateOrUpdateSpaceshipBlueprint(
        [FromBody] RequestCreateBlueprintSpaceship? req)
    {
        try
        {
            if (req is null)
            {
                _logger.LogWarning("Request is null");
                return StatusCodeGenerator.BadRequest("Request is null");
            }

            var user = await GetUser();

            if (user == null)
            {
                _logger.LogWarning("User not connected");
                return StatusCodeGenerator.NotConnected();
            }

            var newBlueprint = req.ToBlueprintSpaceship(user.Id);

            var res = await _spaceshipBlueprintRepository.CreateOrUpdate(newBlueprint, user.Id);
            var resMapped = _mapper.Map<ResponseBlueprintSpaceship>(res);

            return resMapped;
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            return StatusCodeGenerator.Exception(e);
        }
    }

    #endregion

    #region DELETE

    [HttpDelete]
    [Route("delete", Name = "DeleteSpaceshipBlueprint")]
    public async Task<ActionResult> Delete(Guid spaceshipBlueprintId)
    {
        try
        {
            var user = await GetUser();

            if (user == null)
            {
                _logger.LogWarning("User not connected");
                return StatusCodeGenerator.NotConnected();
            }

            await _spaceshipBlueprintRepository.Delete(spaceshipBlueprintId, user.Id);

            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while deleting spaceship blueprint");
            return StatusCodeGenerator.Exception(e);
        }
    }

    #endregion

    #region GET

    /// <summary>
    ///     Get all spaceship blueprint of the connected user
    /// </summary>
    /// <param name="idUser">if null get connected one</param>
    [HttpGet]
    [Route("get/all-by-user", Name = nameof(GetSpaceshipBlueprintsByUserConnected))]
    public async Task<ActionResult<List<ResponseBlueprintSpaceship>>> GetSpaceshipBlueprintsByUserConnected()
    {
        try
        {
            var user = await GetUser();

            if (user == null)
            {
                _logger.LogWarning("User not connected");
                return StatusCodeGenerator.NotConnected();
            }

            var res = await _spaceshipBlueprintRepository.GetAllByUser(user.Id);
            var resMapped = _mapper.Map<List<ResponseBlueprintSpaceship>>(res);

            return resMapped;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting all spaceship blueprints");
            return StatusCodeGenerator.Exception(e);
        }
    }

    [HttpGet]
    [Route("get/by/spaceship/id", Name = nameof(GetSpaceshipBlueprintBySpaceshipId))]
    public async Task<ActionResult<ResponseBlueprintSpaceship>> GetSpaceshipBlueprintBySpaceshipId(Guid spaceshipId)
    {
        try
        {
            var user = await GetUser();

            if (user == null)
            {
                _logger.LogWarning("User not connected");
                return StatusCodeGenerator.NotConnected();
            }

            var spaceship = await _spaceshipRepository.GetById(spaceshipId);
            if (spaceship == null) return StatusCodeGenerator.NotFound($"{nameof(Spaceship)} not found");


            var bp = spaceship.Blueprint;

            var resMapped = _mapper.Map<ResponseBlueprintSpaceship>(bp);
            return resMapped;
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            return StatusCodeGenerator.Exception(e);
        }
    }

    [HttpGet]
    [Route("get/all/models", Name = nameof(GetAllModels))]
    public async Task<ActionResult<List<IDynamicItemSpaceshipBlueprint>>> GetAllModels()
    {
        try
        {
            var user = await GetUser();

            if (user == null)
            {
                _logger.LogWarning("User not connected");
                return StatusCodeGenerator.NotConnected();
            }

            if (user.AccessLevel != EnumUserAccessLevel.Admin)
            {
                _logger.LogWarning("User not allowed to access this route");
                return StatusCodeGenerator.Forbidden();
            }

            var res = _spaceshipBlueprintDataProvider.GetAll();

            return res;
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            return StatusCodeGenerator.Exception(e);
        }
    }

    #endregion
}