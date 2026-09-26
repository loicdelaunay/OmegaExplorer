#region

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Contracts;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Entities;
using OmegaExplorer.Server.Services.Authentications;
using OmegaExplorer.Server.Services.Servers;
using OmegaExplorer.Server.Services.Users.Extensions;
using OmegaExplorer.Server.Services.Users.Models.Enum;

#endregion

namespace OmegaExplorer.Server.Services._Game.GameResources;

[Route("/api/game-resource")]
[ApiController]
public class GameResourceController : ControllerCustom
{
    private readonly GameResourceRepository _gameResourceRepository;

    private readonly ILogger<GameResourceController> _logger;
    private readonly IMapper _mapper;


    public GameResourceController(AuthenticationService authenticationService, ILogger<GameResourceController> logger,
        IMapper mapper, GameResourceRepository gameResourceRepository) : base(authenticationService)
    {
        _logger = logger;
        _mapper = mapper;

        _gameResourceRepository = gameResourceRepository;
    }

    /// <summary>
    ///     Get all resources from the user connected
    /// </summary>
    /// <returns> </returns>
    [HttpGet]
    [Route("get/user-connected", Name = "GetResourcesByUserConnected")]
    public async Task<ActionResult<List<ResponseGameResource>>> GetAllByConnected()
    {
        var user = await GetUser();

        if (user == null) return StatusCodeGenerator.NotConnected();

        var res = await _gameResourceRepository.GetAllByUser(user.Id);
        var resMapped = _mapper.Map<List<ResponseGameResource>>(res);

        return resMapped;
    }

    /// <summary>
    ///     Get a resource from the user connected and the index of the resource
    /// </summary>
    /// <param name="index">
    ///     <see cref="GameResource.Index" />
    /// </param>
    /// <returns> </returns>
    [HttpGet]
    [Route("get/by-index", Name = "GetResourceByIndex")]
    public async Task<ActionResult<ResponseGameResource?>> GetByIndex(int index)
    {
        var user = await GetUser();

        if (user == null) return StatusCodeGenerator.NotConnected();

        var res = await _gameResourceRepository.GetByIndex(user.Id, index);
        var resMapped = _mapper.Map<ResponseGameResource>(res);
        return resMapped;
    }

    /// <summary>
    ///     Get all resources of a player
    /// </summary>
    /// <returns> </returns>
    [HttpGet]
    [Route("get/as-admin/user", Name = "GetGameResourcesByUserAsAdmin")]
    public async Task<ActionResult<List<ResponseGameResource>>> GetAsAdminByPlayer(Guid playerId)
    {
        var user = await GetUser();

        //Check connected
        if (user == null) return StatusCodeGenerator.NotConnected();

        //Check admin
        if (!user.HaveAccess(EnumUserAccessLevel.Admin)) return StatusCodeGenerator.Forbidden();

        var res = await _gameResourceRepository.GetAllByUser(playerId);
        var resMapped = _mapper.Map<List<ResponseGameResource>>(res);

        return resMapped;
    }

    [HttpPost]
    [Route("update/as-admin/edit/player-game-resource", Name = "UpdatePlayerGameResourceAsAdmin")]
    public async Task<ActionResult<int>> UpdateAsAdmin(Guid playerId, int index, int value)
    {
        var user = await GetUser();
        _logger.LogInformation($"Update game resource {index} as admin for player {playerId} with value {value}");

        try
        {
            //Check connected
            if (user == null)
            {
                _logger.LogError("User not connected");
                return StatusCodeGenerator.NotConnected();
            }

            //Check admin
            if (!user.HaveAccess(EnumUserAccessLevel.Admin))
            {
                _logger.LogError("User not admin");
                return StatusCodeGenerator.Forbidden();
            }

            var newAmount = await _gameResourceRepository.UpdateAmount(playerId, index, value);

            _logger.LogInformation("Game resource updated");
            return newAmount;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while updating game resource");
            return StatusCodeGenerator.Exception(e);
        }
    }
}