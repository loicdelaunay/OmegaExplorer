using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OmegaExplorer.Server.Extensions;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Limits.Models.Exceptions;
using OmegaExplorer.Server.Services._Game.Orders.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Spaceships.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint;
using OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Classes;
using OmegaExplorer.Server.Services._Game.StarSystems;
using OmegaExplorer.Server.Services.Authentications;
using OmegaExplorer.Server.Services.Servers;
using OmegaExplorer.Shared.Filters;

namespace OmegaExplorer.Server.Services._Game.Spaceships;

[ApiController]
[Route("/api/spaceship")]
public class SpaceshipController : ControllerCustom
{
    private readonly ILogger<SpaceshipController> _logger;
    private readonly IMapper _mapper;
    private readonly SpaceshipBlueprintRepository _spaceshipBlueprintRepository;
    private readonly SpaceshipRepository _spaceshipRepository;
    private readonly SpaceshipService _spaceshipService;

    private readonly StarSystemRepository _starSystemRepository;

    public SpaceshipController(AuthenticationService authenticationService, StarSystemRepository starSystemRepository,
        SpaceshipRepository spaceshipRepository, SpaceshipBlueprintRepository spaceshipBlueprintRepository,
        ILogger<SpaceshipController> logger, IMapper mapper,
        SpaceshipService spaceshipService) : base(authenticationService)
    {
        _starSystemRepository = starSystemRepository;
        _spaceshipRepository = spaceshipRepository;
        _spaceshipBlueprintRepository = spaceshipBlueprintRepository;
        _logger = logger;
        _mapper = mapper;
        _spaceshipService = spaceshipService;
    }

    #region CREATE

    [HttpPost]
    [Route("create", Name = nameof(CreateSpaceship))]
    public async Task<ActionResult<ResponseSpaceship>> CreateSpaceship([FromBody] RequestCreateSpaceship request)
    {
        try
        {
            var user = await GetUser();

            if (user == null)
            {
                _logger.LogError("User not connected.");
                return StatusCodeGenerator.NotConnected();
            }

            var blueprint = await _spaceshipBlueprintRepository.GetById(request.BlueprintId, user.Id);

            if (blueprint == null)
            {
                _logger.LogError("Blueprint not found.");
                return StatusCodeGenerator.NotFound("Blueprint not found.");
            }

            var starSystem = await _starSystemRepository.GetById(request.StarSystemId, user.Id);

            if (starSystem == null)
            {
                _logger.LogError("Star system not found.");
                return StatusCodeGenerator.NotFound("Star system not found.");
            }

            var spaceship = await _spaceshipService.CreateForPlayer(blueprint, starSystem, user.Id);

            var resMapped = _mapper.Map<ResponseSpaceship>(spaceship);

            return resMapped;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while creating spaceship");
            return StatusCodeGenerator.Exception(e);
        }
    }

    #endregion

    #region DELETE

    /// <summary>
    ///     Add a planet for the current user
    /// </summary>
    [HttpDelete]
    [Route("delete", Name = nameof(DeleteSpaceship))]
    public async Task<ActionResult> DeleteSpaceship(Guid spaceshipId)
    {
        try
        {
            var user = await GetUser();

            if (user == null) return StatusCodeGenerator.NotConnected();

            await _spaceshipRepository.DeleteAsOwner(spaceshipId, user.Id);

            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while deleting spaceship {spaceshipId}", spaceshipId);
            return StatusCodeGenerator.Exception(e);
        }
    }

    #endregion

    #region GET

    [HttpPost]
    [Route("get/filtered", Name = nameof(GetFilteredSpaceships))]
    public async Task<ActionResult<List<ResponseSpaceship>>> GetFilteredSpaceships([FromBody] string filterSerialized)
    {
        try
        {
            var user = await GetUser();
            var filter = filterSerialized.JsonDeserialize<DynamicDriverSpaceshipFilter>();

            if (user == null)
            {
                _logger.LogError("User not connected.");
                return StatusCodeGenerator.NotConnected();
            }

            var res = await _spaceshipRepository.GetAllFiltered(user.Id, filter);
            var resMapped = _mapper.Map<List<ResponseSpaceship>>(res);

            return resMapped;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting filtered spaceships");
            return StatusCodeGenerator.Exception(e);
        }
    }

    [HttpGet]
    [Route("get/all-by-user", Name = nameof(GetAllSpaceshipsByUser))]
    public async Task<ActionResult<List<ResponseSpaceship>>> GetAllSpaceshipsByUser()
    {
        try
        {
            var user = await GetUser();

            if (user == null)
            {
                _logger.LogError("User not connected.");
                return StatusCodeGenerator.NotConnected();
            }

            var res = await _spaceshipRepository.GetAll(user.Id);
            var resMapped = _mapper.Map<List<ResponseSpaceship>>(res);

            return resMapped;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting all spaceships");
            return StatusCodeGenerator.Exception(e);
        }
    }

    [HttpGet]
    [Route("get/all-by-star-system", Name = nameof(GetAllSpaceshipsByStarSystem))]
    public async Task<ActionResult<List<ResponseSpaceship>>> GetAllSpaceshipsByStarSystem(Guid starSystemId)
    {
        try
        {
            var user = await GetUser();

            if (user == null)
            {
                _logger.LogError("User not connected.");
                return StatusCodeGenerator.NotConnected();
            }

            var res = await _spaceshipRepository.GetAllInStarCluster(starSystemId);
            var resMapped = _mapper.Map<List<ResponseSpaceship>>(res);

            return resMapped;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting all spaceships");
            return StatusCodeGenerator.Exception(e);
        }
    }

    [HttpGet]
    [Route("get/by-id", Name = nameof(GetSpaceshipById))]
    public async Task<ActionResult<ResponseSpaceship?>> GetSpaceshipById(Guid spaceshipId)
    {
        try
        {
            var user = await GetUser();

            if (user == null)
            {
                _logger.LogError("User not connected.");
                return StatusCodeGenerator.NotConnected();
            }

            var res = await _spaceshipRepository.GetById(spaceshipId);
            if (res == null)
            {
                _logger.LogWarning("Spaceship not found");
                return StatusCodeGenerator.NotFound("Spaceship not found");
            }

            var resMapped = _mapper.Map<ResponseSpaceship>(res);
            return resMapped;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting spaceship by id {SpaceshipId}", spaceshipId);
            return StatusCodeGenerator.Exception(e);
        }
    }

    [HttpGet]
    [Route("get/orders", Name = nameof(GetOrdersSpaceship))]
    public async Task<ActionResult<List<ResponseOrder>>> GetOrdersSpaceship(Guid spaceshipId)
    {
        try
        {
            var user = await GetUser();

            if (user == null)
            {
                _logger.LogError("User not connected.");
                return StatusCodeGenerator.NotConnected();
            }

            var res = await _spaceshipRepository.GetOrders(spaceshipId, user.Id);
            var resMapped = _mapper.Map<List<ResponseOrder>>(res);

            return resMapped;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting orders for spaceship {SpaceshipId}", spaceshipId);
            return StatusCodeGenerator.Exception(e);
        }
    }

    #endregion

    #region ACTION

    [HttpPost]
    [Route("order/move", Name = nameof(OrderMoveSpaceship))]
    public async Task<ActionResult> OrderMoveSpaceship(Guid spaceshipId, int x, int y, Guid? starClusterId,
        Guid? galaxyId, Guid? universeId)
    {
        try
        {
            var user = await GetUser();

            if (user == null)
            {
                _logger.LogError("User not connected.");
                return StatusCodeGenerator.NotConnected();
            }

            SpatialLocation spatialLocation = new(x, y, universeId, galaxyId, starClusterId);

            await _spaceshipService.OrderMoveTo(user.Id, spaceshipId, spatialLocation);

            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while order move spaceship");
            return StatusCodeGenerator.Exception(e);
        }
    }

    [HttpPost]
    [Route("order/attack", Name = nameof(OrderAttackSpaceship))]
    public async Task<ActionResult> OrderAttackSpaceship(Guid spaceshipId, Guid targetId)
    {
        try
        {
            var user = await GetUser();

            if (user == null)
            {
                _logger.LogError("User not connected.");
                return StatusCodeGenerator.NotConnected();
            }

            await _spaceshipService.OrderAttack(user.Id, spaceshipId, targetId);

            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while order attack spaceship");
            return StatusCodeGenerator.Exception(e);
        }
    }

    [HttpPost]
    [Route("order/colonize", Name = nameof(OrderColonizeSpaceship))]
    public async Task<ActionResult> OrderColonizeSpaceship(Guid spaceshipId, Guid targetId)
    {
        try
        {
            var user = await GetUser();

            if (user == null)
            {
                _logger.LogError("User not connected.");
                return StatusCodeGenerator.NotConnected();
            }

            try
            {
                await _spaceshipService.OrderColonize(spaceshipId, user.Id, targetId);
            }
            catch (ExceptionLimitReached limitReached)
            {
                return StatusCodeGenerator.LimitReached("You reached the limit of colonized planets",
                    limitReached.Limit);
            }

            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while order colonize spaceship");
            return StatusCodeGenerator.Exception(e);
        }
    }

    [HttpPost]
    [Route("rename", Name = nameof(RenameSpaceship))]
    public async Task<ActionResult> RenameSpaceship(Guid spaceshipId, string name)
    {
        try
        {
            var user = await GetUser();

            if (user == null)
            {
                _logger.LogError("User not connected.");
                return StatusCodeGenerator.NotConnected();
            }

            await _spaceshipService.RenameSpaceship(spaceshipId, user.Id, name);

            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while renaming spaceship");
            return StatusCodeGenerator.Exception(e);
        }
    }

    #endregion
}