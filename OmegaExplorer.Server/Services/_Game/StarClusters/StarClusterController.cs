using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.StarClusters.Models.Contracts;
using OmegaExplorer.Server.Services.Authentications;
using OmegaExplorer.Server.Services.Servers;

namespace OmegaExplorer.Server.Services._Game.StarClusters;

[ApiController]
[Route("/api/star-cluster/")]
public class StarClusterController : ControllerCustom
{
    private readonly IMapper _mapper;
    private readonly StarClusterRepository _starClusterRepository;

    public StarClusterController(AuthenticationService authenticationService,
        StarClusterRepository starClusterRepository, IMapper mapper) : base(authenticationService)
    {
        _starClusterRepository = starClusterRepository;
        _mapper = mapper;
    }

    #region GET

    [HttpGet]
    [Route("get/by/id", Name = nameof(GetStarClusterById))]
    public async Task<ActionResult<ResponseStarCluster>> GetStarClusterById(Guid starClusterId)
    {
        try
        {
            var starCluster = await _starClusterRepository.GetById(starClusterId);
            var starClusterMapped = _mapper.Map<ResponseStarCluster>(starCluster);

            return starClusterMapped;
        }
        catch (Exception e)
        {
            return StatusCodeGenerator.Exception(e);
        }
    }

    [HttpGet]
    [Route("get/all/by/galaxy", Name = nameof(GetAllStarClustersByGalaxy))]
    public async Task<ActionResult<List<ResponseStarCluster>>> GetAllStarClustersByGalaxy(Guid galaxyId)
    {
        try
        {
            var starClusters = await _starClusterRepository.GetStarClustersInGalaxy(galaxyId);
            var starClustersMapped = _mapper.Map<List<ResponseStarCluster>>(starClusters);

            return starClustersMapped;
        }
        catch (Exception e)
        {
            return StatusCodeGenerator.Exception(e);
        }
    }

    #endregion
}