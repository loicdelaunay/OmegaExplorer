using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Galaxies.Models.Contracts;
using OmegaExplorer.Server.Services.Authentications;
using OmegaExplorer.Server.Services.Servers;

namespace OmegaExplorer.Server.Services._Game.Galaxies;

[ApiController]
[Route("/api/galaxy/")]
public class GalaxyController : ControllerCustom
{
    private readonly GalaxyRepository _galaxyRepository;

    private readonly IMapper _mapper;

    public GalaxyController(AuthenticationService authenticationService, GalaxyRepository galaxyRepository,
        IMapper mapper) : base(authenticationService)
    {
        _galaxyRepository = galaxyRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("get/by/id", Name = "GetGalaxyById")]
    public async Task<ActionResult<ResponseGalaxy?>> GetGalaxyById(Guid galaxyId)
    {
        try
        {
            var galaxy = await _galaxyRepository.GetById(galaxyId);
            var galaxyMapped = _mapper.Map<ResponseGalaxy>(galaxy);

            return galaxyMapped;
        }
        catch (Exception e)
        {
            return StatusCodeGenerator.Exception(e);
        }
    }

    [HttpGet]
    [Route("get/all/universe", Name = "GetAllGalaxiesByUniverse")]
    public async Task<ActionResult<List<ResponseGalaxy>>> GetGalaxiesInUniverse(Guid universeId)
    {
        try
        {
            var galaxies = await _galaxyRepository.GetGalaxiesInUniverse(universeId);
            var galaxiesMapped = _mapper.Map<List<ResponseGalaxy>>(galaxies);

            return galaxiesMapped;
        }
        catch (Exception e)
        {
            return StatusCodeGenerator.Exception(e);
        }
    }
}