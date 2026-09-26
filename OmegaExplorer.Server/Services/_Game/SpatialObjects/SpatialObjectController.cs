using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Classes;
using OmegaExplorer.Server.Services._Game.SpatialObjects.Models.Contracts;
using OmegaExplorer.Server.Services.Authentications;
using OmegaExplorer.Server.Services.Servers;

namespace OmegaExplorer.Server.Services._Game.SpatialObjects;

[Route("/api/spacial-object")]
public class SpatialObjectController : ControllerCustom
{
    private readonly IMapper _mapper;
    private readonly SpacialObjectRepository _spacialObjectRepository;
    private readonly SpatialTravelService _spatialTravelService;
    private readonly ILogger<SpatialObjectController> _logger;


    public SpatialObjectController(AuthenticationService authenticationService,
                                   SpacialObjectRepository spacialObjectRepository, IMapper mapper,
                                   SpatialTravelService spatialTravelService, ILogger<SpatialObjectController> logger) : base(authenticationService)
    {
        _spacialObjectRepository = spacialObjectRepository;
        _mapper = mapper;
        _spatialTravelService = spatialTravelService;
        _logger = logger;
    }

    /// <summary>
    ///     Get travel info to move an object to another one
    /// </summary>
    /// <param name="requestTravelInfo"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("travel-info", Name = nameof(GetTravelInfo))]
    public async Task<ActionResult<ResponseSpatialTravel>> GetTravelInfo([FromBody] RequestTravelInfo requestTravelInfo)
    {
        try
        {
            var actor = await _spacialObjectRepository.GetByIdWithInclude(requestTravelInfo.ActorId);
            var target = await _spatialTravelService.ResponseSpatialLocationToSpatialLocation(requestTravelInfo.Target);

            if (actor == null) return StatusCodeGenerator.NotFound(nameof(Models.Entities.SpatialObject));

            SpatialTravel spatialTravel = new(actor, target);
            await _spatialTravelService.Compute(spatialTravel);

            var mappedSpatialTravel = _mapper.Map<ResponseSpatialTravel>(spatialTravel);

            return mappedSpatialTravel;
        }
        catch (Exception e)
        {
            _logger.LogError("Not able to get travel info, {e}", e);
            return StatusCodeGenerator.Exception(e);
        }
    }
}