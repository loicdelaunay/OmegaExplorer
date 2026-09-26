using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Universes.Models.Contracts.Responses;
using OmegaExplorer.Server.Services.Authentications;

namespace OmegaExplorer.Server.Services._Game.Universes;

[Route("/api/universe")]
[ApiController]
public class ControllerUniverse : ControllerCustom
{
    private readonly IMapper _mapper;
    private readonly UniverseRepository _universeRepository;

    public ControllerUniverse(AuthenticationService authenticationService, UniverseRepository universeRepository,
        IMapper mapper) : base(authenticationService)
    {
        _universeRepository = universeRepository;
        _mapper = mapper;
    }

    #region GET

    [HttpGet]
    [Route("get/by/id", Name = "GetUniverseById")]
    public async Task<ActionResult<ResponseUniverse?>> GetById(Guid universeId)
    {
        var res = await _universeRepository.GetById(universeId);
        var resMapped = _mapper.Map<ResponseUniverse>(res);

        return resMapped;
    }

    /// <summary>
    /// </summary>
    [HttpGet]
    [Route("get/all", Name = "GetAllUniverse")]
    public async Task<ActionResult<List<ResponseUniverse>>?> GetAll()
    {
        var res = await _universeRepository.GetAll();
        var resMapped = _mapper.Map<List<ResponseUniverse>>(res);

        return resMapped;
    }

    #endregion
}