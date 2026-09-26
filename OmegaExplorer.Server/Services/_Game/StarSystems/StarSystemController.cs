using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.StarSystems.Models.Contracts;
using OmegaExplorer.Server.Services.Authentications;
using OmegaExplorer.Server.Services.Servers;
using System.Net;

namespace OmegaExplorer.Server.Services._Game.StarSystems;

[Route("/api/star-system")]
[ApiController]
public class StarSystemController : ControllerCustom
{
    private readonly IMapper _mapper;
    private readonly StarSystemRepository _starSystemRepository;

    public StarSystemController(AuthenticationService authenticationService, StarSystemRepository starSystemRepository,
        IMapper mapper) : base(authenticationService)
    {
        _starSystemRepository = starSystemRepository;
        _mapper = mapper;
    }

    #region ACTION

    [HttpPost]
    [Route("decolonize", Name = nameof(Decolonize))]
    public async Task<ActionResult> Decolonize(Guid systemId)
    {
        try
        {
            var user = await GetUser();

            if (user == null) return StatusCodeGenerator.NotConnected();

            // Decolonize planet
            await _starSystemRepository.Decolonize(systemId, user.Id);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCodeGenerator.Exception(e);
        }
    }

    #endregion

    #region DELETE

    /// <summary>
    ///     Add a planet for the current user
    /// </summary>
    [HttpDelete]
    [Route("delete/by/id", Name = "DeleteSystem")]
    public async Task<ActionResult> Delete(Guid systemId)
    {
        var user = await GetUser();

        if (user == null) return StatusCodeGenerator.NotConnected();

        await _starSystemRepository.Delete(systemId, user.Id);

        return Ok();
    }

    #endregion

    #region GET

    [HttpGet]
    [Route("get/all", Name = nameof(GetAllStarSystems))]
    public async Task<ActionResult<List<ResponseStarSystem>>> GetAllStarSystems()
    {
        var user = await GetUser();

        if (user == null) return StatusCodeGenerator.NotConnected();

        var systems = await _starSystemRepository.GetAllStarSystems();
        var systemsMapped = _mapper.Map<List<ResponseStarSystem>>(systems);

        return systemsMapped;
    }

    /// <summary>
    /// </summary>
    /// <param name="userId">if null get connected one</param>
    [HttpGet]
    [Route("get/all/by/user", Name = nameof(GetAllStarSystemsOwnedByUser))]
    public async Task<ActionResult<List<ResponseStarSystem?>>> GetAllStarSystemsOwnedByUser(Guid? userId)
    {
        var user = await GetUser();

        if (user == null) return StatusCodeGenerator.NotConnected();

        //If the player is not inspecting user, return his own planets
        userId ??= user.Id;

        if (userId == Guid.Empty) return StatusCode((int)HttpStatusCode.BadRequest, "Not able to get user id");

        var systems = await _starSystemRepository.GetAllOwnerByUser((Guid)userId);
        var systemsMapped = _mapper.Map<List<ResponseStarSystem>>(systems);

        return systemsMapped;
    }

    [HttpGet]
    [Route("/get/all/systems/by/star-cluster", Name = nameof(GetAllStarSystemsByStarCluster))]
    public async Task<ActionResult<List<ResponseStarSystem>>> GetAllStarSystemsByStarCluster(Guid galaxyId)
    {
        try
        {
            var systems = await _starSystemRepository.GetSystemsInStarCluster(galaxyId);
            var systemsMapped = _mapper.Map<List<ResponseStarSystem>>(systems);

            return systemsMapped;
        }
        catch (Exception e)
        {
            return StatusCodeGenerator.Exception(e);
        }
    }

    [HttpGet]
    [Route("get/by/id", Name = nameof(GetStarSystemById))]
    public async Task<ActionResult<ResponseStarSystem>> GetStarSystemById(Guid systemId)
    {
        var user = await GetUser();

        if (user == null) return StatusCodeGenerator.NotConnected();

        var planet = await _starSystemRepository.GetById(systemId, user.Id);

        if (planet == null) return StatusCodeGenerator.NotFound(nameof(Models.Entities.StarSystem));

        var planetMapped = _mapper.Map<ResponseStarSystem>(planet);

        return planetMapped;
    }

    #endregion
}