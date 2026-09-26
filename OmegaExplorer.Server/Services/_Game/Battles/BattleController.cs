using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Battles.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Battles.Models.Entities;
using OmegaExplorer.Server.Services._Game.Spaceships;
using OmegaExplorer.Server.Services._Game.Spaceships.Models.Entities;
using OmegaExplorer.Server.Services.Authentications;
using OmegaExplorer.Server.Services.Servers;
using OmegaExplorer.Server.Services.Users.Extensions;
using OmegaExplorer.Server.Services.Users.Models.Enum;
using OmegaExplorer.Shared.Enums;

namespace OmegaExplorer.Server.Services._Game.Battles;

[ApiController]
[Route("api/battle")]
public class BattleController : ControllerCustom
{
    private readonly BattleRepository _battleRepository;

    private readonly BattleService _battleService;

    private readonly ILogger<BattleController> _logger;
    private readonly IMapper _mapper;
    private readonly SpaceshipRepository _spaceshipRepository;
    private readonly SpaceshipService _spaceshipService;

    public BattleController(AuthenticationService authenticationService, ILogger<BattleController> logger,
        IMapper mapper, BattleService battleService, SpaceshipRepository spaceshipRepository,
        BattleRepository battleRepository, SpaceshipService spaceshipService) : base(authenticationService)
    {
        _logger = logger;
        _mapper = mapper;
        _battleService = battleService;
        _spaceshipRepository = spaceshipRepository;
        _battleRepository = battleRepository;
        _spaceshipService = spaceshipService;
    }

    #region GET

    [HttpGet]
    [Route("get/by/id", Name = nameof(GetBattleById))]
    public async Task<ActionResult<ResponseBattle>> GetBattleById(Guid id)
    {
        try
        {
            var battle = await _battleRepository.GetBattleById(id);
            if (battle == null) return StatusCodeGenerator.NotFound(nameof(Battle));

            var responseBattle = _mapper.Map<ResponseBattle>(battle);
            return responseBattle;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting battle by id {Id} : {e}", id, e);
            return StatusCodeGenerator.Exception(e);
        }
    }

    [HttpGet]
    [Route("get/by/spaceship/id", Name = nameof(GetBattleBySpaceshipId))]
    public async Task<ActionResult<ResponseBattle>> GetBattleBySpaceshipId(Guid spaceshipId)
    {
        try
        {
            var user = await GetUser();

            //Check if user is connected
            if (user == null)
            {
                _logger.LogError("User not connected");
                return StatusCodeGenerator.NotConnected();
            }

            var battle = await _battleRepository.GetBattleBySpaceshipId(spaceshipId, user.Id);
            if (battle == null) return StatusCodeGenerator.NotFound(nameof(Battle));

            var responseBattle = _mapper.Map<ResponseBattle>(battle);
            return responseBattle;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting battle by spaceship id {SpaceshipId} : {e}", spaceshipId, e);
            return StatusCodeGenerator.Exception(e);
        }
    }

    /// <summary>
    ///     Get all battles from the connected user
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("get/all/by/connected", Name = nameof(GetAllBattlesByConnected))]
    public async Task<ActionResult<List<ResponseBattle>>> GetAllBattlesByConnected()
    {
        try
        {
            var user = await GetUser();

            //Check if user is connected
            if (user == null)
            {
                _logger.LogError("User not connected");
                return StatusCodeGenerator.NotConnected();
            }

            var battles = await _battleService.GetAllBattlesByUser(user.Id);
            var responseBattles = _mapper.Map<List<ResponseBattle>>(battles);
            return responseBattles;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting all battles by connected user");
            return StatusCodeGenerator.Exception(e);
        }
    }

    [HttpGet]
    [Route("action/get/all/by/battle", Name = nameof(GetAllActionsInBattle))]
    public async Task<ActionResult<List<ResponseActionInBattle>>> GetAllActionsInBattle(Guid battleId)
    {
        var currentUser = await GetUser();

        if (currentUser == null) return StatusCodeGenerator.NotConnected();

        var actionsInBattle = await _battleService.GetAllActionsInBattle(battleId, currentUser.Id);
        var responseActionsInBattle = _mapper.Map<List<ResponseActionInBattle>>(actionsInBattle);

        return responseActionsInBattle;
    }

    [HttpGet]
    [Route("action/get/all/by/battle/cycle", Name = nameof(GetAllActionsInBattleByCycle))]
    public async Task<ActionResult<List<ResponseActionInBattle>>> GetAllActionsInBattleByCycle(Guid battleId,
        long cycle)
    {
        try
        {
            var currentUser = await GetUser();

            if (currentUser == null) return StatusCodeGenerator.NotConnected();

            var battle = await _battleRepository.GetBattleById(battleId);

            if (battle == null) return StatusCodeGenerator.NotFound(nameof(Battle));

            var actionsInBattle =
                await _battleService.GetAllActionsInBattleByCycleAndByUser(battle, cycle, currentUser.Id);
            var responseActionsInBattle = _mapper.Map<List<ResponseActionInBattle>>(actionsInBattle);

            return responseActionsInBattle;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting all actions in battle by cycle");
            return StatusCodeGenerator.Exception(e);
        }
    }

    [HttpGet]
    [Route("action/get/all/by/battle/turn/current", Name = nameof(GetAllActionsInBattleByCurrentTurn))]
    public async Task<ActionResult<List<ResponseActionInBattle>>> GetAllActionsInBattleByCurrentTurn(Guid battleId)
    {
        var currentUser = await GetUser();

        if (currentUser == null) return StatusCodeGenerator.NotConnected();

        var actionsInBattle = await _battleService.GetAllActionsInBattleByCurrentCycle(battleId, currentUser.Id);
        var responseActionsInBattle = _mapper.Map<List<ResponseActionInBattle>>(actionsInBattle);

        return responseActionsInBattle;
    }

    #endregion

    #region CREATE

    [HttpPost]
    [Route("create", Name = nameof(CreateBattle))]
    public async Task<ActionResult<ResponseBattle>> CreateBattle([FromBody] RequestCreateBattle request)
    {
        try
        {
            var user = await GetUser();

            //Check if user is connected
            if (user == null) return StatusCodeGenerator.NotConnected();

            //Check if user is owner of the initiator spaceship
            if (!await _spaceshipService.IsOwner(request.SpaceshipId, user.Id))
                return StatusCodeGenerator.Forbidden("You are not the owner of the spaceship");

            #region Create battle

            //Get spaceship location
            var spaceshipTarget = await _spaceshipRepository.GetById(request.SpaceshipTargetId);
            if (spaceshipTarget == null)
                return StatusCodeGenerator.NotFound(
                    $"No target found for {request.SpaceshipTargetId} : {nameof(Spaceship)}");

            var location = spaceshipTarget.SpatialLocation;

            List<Guid> listSpaceships = [request.SpaceshipId, request.SpaceshipTargetId];

            var newBattle = await _battleService.CreateBattle(location, listSpaceships);
            var newBattleMapped = _mapper.Map<ResponseBattle>(newBattle);

            return newBattleMapped;

            #endregion
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while creating battle");
            return StatusCodeGenerator.Exception(e);
        }
    }

    [HttpPost]
    [Route("action/create", Name = nameof(CreateActionInBattle))]
    public async Task<ActionResult<ResponseActionInBattle>> CreateActionInBattle(
        [FromBody] RequestCreateActionInBattle requestCreateActionInBattle)
    {
        var currentUser = await GetUser();

        if (currentUser == null) return StatusCodeGenerator.NotConnected();

        var spaceship = await _spaceshipService.GetById(requestCreateActionInBattle.SpaceshipId);
        if (spaceship == null) return StatusCodeGenerator.NotFound(nameof(Spaceship));

        //Check if spaceship can use one more action point
        //Get current list of actions
        var actionsInBattle =
            await _battleService.GetAllActionsInBattleByCurrentCycleAndSpaceship(spaceship.Id,
                requestCreateActionInBattle.BattleId);
        if (actionsInBattle.Count >= spaceship.GetActionPoints())
            return StatusCodeGenerator.OmegaExplorerError(EnumCustomStatusCode.TooManyActionInBattle);

        var targets = await _spaceshipService.GetByIds(requestCreateActionInBattle.TargetIds);

        ActionInBattle actionInBattle = new(requestCreateActionInBattle.SpaceshipId, targets,
            requestCreateActionInBattle.Turn, requestCreateActionInBattle.SkillIndex);

        var newActionInBattle =
            await _battleService.CreateActionInBattle(requestCreateActionInBattle.BattleId, actionInBattle);
        var responseActionInBattle = _mapper.Map<ResponseActionInBattle>(newActionInBattle);

        return responseActionInBattle;
    }

    #endregion

    #region DELETE

    [HttpDelete]
    [Route("delete", Name = nameof(DeleteBattle))]
    public async Task<ActionResult> DeleteBattle(Guid battleId)
    {
        try
        {
            var user = await GetUser();

            //Check if user is connected
            if (user == null) return StatusCodeGenerator.NotConnected();

            //Check if user is admin
            if (!user.HaveAccess(EnumUserAccessLevel.Admin))
                return StatusCodeGenerator.Forbidden("You are not an admin");

            await _battleService.DeleteBattle(battleId);

            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while deleting battle");
            return StatusCodeGenerator.Exception(e);
        }
    }

    [HttpDelete]
    [Route("action/delete", Name = nameof(DeleteActionInBattle))]
    public async Task<ActionResult> DeleteActionInBattle(Guid battleId, Guid idAction)
    {
        try
        {
            var currentUser = await GetUser();

            if (currentUser == null) return StatusCodeGenerator.NotConnected();

            await _battleService.RemoveActionInBattle(battleId, idAction);

            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while deleting action in battle");
            return StatusCodeGenerator.Exception(e);
        }
    }

    #endregion
}