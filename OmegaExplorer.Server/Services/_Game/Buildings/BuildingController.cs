#region

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Buildings.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Buildings.Models.Interfaces;
using OmegaExplorer.Server.Services.Authentications;
using OmegaExplorer.Server.Services.Servers;

#endregion

namespace OmegaExplorer.Server.Services._Game.Buildings;

[Route("/api/building")]
[ApiController]
public class BuildingController : ControllerCustom
{
    private readonly BuildingDataProvider _buildingDataProvider;
    private readonly BuildingRepository _buildingRepository;

    private readonly BuildingService _buildingService;

    private readonly ILogger<BuildingController> _logger;
    private readonly IMapper _mapper;

    public BuildingController(AuthenticationService authenticationService, ILogger<BuildingController> logger,
        IMapper mapper, BuildingService buildingService, BuildingDataProvider buildingDataProvider,
        BuildingRepository buildingRepository) : base(authenticationService)
    {
        _logger = logger;
        _mapper = mapper;

        _buildingService = buildingService;
        _buildingDataProvider = buildingDataProvider;

        _buildingRepository = buildingRepository;
    }

    #region ACTION

    [HttpPost]
    [Route("create/on/system", Name = nameof(CreateBuildingOnSystem))]
    public async Task<ActionResult<ResponseBuilding?>> CreateBuildingOnSystem(Guid systemId, int indexBuilding)
    {
        var user = await GetUser();

        _logger.LogInformation("Building {indexBuilding} on {systemId}...", indexBuilding, systemId);

        if (user == null)
        {
            _logger.LogError("User not connected ...");
            return StatusCodeGenerator.NotConnected();
        }

        try
        {
            var newBuilding = await _buildingService.CreateBuildingOnSystem(user.Id, systemId, indexBuilding);
            var mapped = _mapper.Map<ResponseBuilding>(newBuilding);

            _logger.LogInformation("Building done");
            return mapped;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while building");
            return StatusCodeGenerator.Exception(e);
        }
    }

    #endregion

    #region DELETE

    [HttpDelete]
    [Route("delete", Name = nameof(DeleteBuilding))]
    public async Task<ActionResult> DeleteBuilding(Guid buildingId)
    {
        var user = await GetUser();
        _logger.LogInformation("deleting building {buildingId} ...", buildingId);

        if (user == null)
        {
            _logger.LogInformation("User not connected ...");
            return StatusCodeGenerator.NotConnected();
        }

        try
        {
            await _buildingRepository.Delete(buildingId, user.Id);
            _logger.LogInformation("Delete building done");
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while deleting building");
            return StatusCodeGenerator.Exception(e);
        }
    }

    #endregion

    #region GET

    /// <summary>
    /// </summary>
    [HttpGet]
    [Route("get/all/data", Name = "GetAllBuildingsData")]
    public async Task<ActionResult<List<IDynamicItemBuilding>>> GetAllBuildingsData()
    {
        var user = await GetUser();
        _logger.LogInformation("getting all buildable ...");

        if (user == null)
        {
            _logger.LogError("user not connected ...");
            return StatusCodeGenerator.NotConnected();
        }

        var buildingDefinitions = _buildingDataProvider.GetAll();

        _logger.LogInformation("get all buildable done");
        return buildingDefinitions;
    }

    [HttpGet]
    [Route("get/all/by/system", Name = nameof(GetAllBuildingsBySystem))]
    public async Task<ActionResult<List<ResponseBuilding>>> GetAllBuildingsBySystem(Guid systemId)
    {
        var user = await GetUser();
        _logger.LogInformation($"getting all buildings on system {systemId} ...");

        if (user == null)
        {
            _logger.LogError("user not connected ...");
            return StatusCodeGenerator.NotConnected();
        }

        var buildings = _buildingRepository.GetAllBuildingsBySystem(systemId);
        var buildingsMapped = _mapper.Map<List<ResponseBuilding>>(buildings);

        _logger.LogInformation($"get {buildings.Count} buildings  done");
        return buildingsMapped;
    }

    #endregion
}