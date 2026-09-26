using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Spaceships.Models.Entities;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipBlueprint;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule.Models.Contracts;
using OmegaExplorer.Server.Services.Authentications;
using OmegaExplorer.Server.Services.Servers;

namespace OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipModule;

[Microsoft.AspNetCore.Components.Route("/api/spaceship/module")]
public class SpaceshipModuleController : ControllerCustom
{
    private readonly IMapper _mapper;
    private readonly SpaceshipBlueprintService _spaceshipBlueprintService;
    private readonly SpaceshipModuleDataProvider _spaceshipModuleDataProvider;

    public SpaceshipModuleController(AuthenticationService authenticationService, IMapper mapper,
        SpaceshipModuleDataProvider spaceshipModuleDataProvider,
        SpaceshipBlueprintService spaceshipBlueprintService) : base(authenticationService)
    {
        _mapper = mapper;
        _spaceshipModuleDataProvider = spaceshipModuleDataProvider;
        _spaceshipBlueprintService = spaceshipBlueprintService;
    }

    /// <summary>
    /// </summary>
    [HttpGet]
    [Route("get/all-data-by-user", Name = "GetAllSpaceshipModuleDataByUser")]
    public async Task<ActionResult<IEnumerable<ResponseDynamicItemSpaceshipModule>>> GetAllData()
    {
        var user = await GetUser();

        if (user == null) return StatusCodeGenerator.NotConnected();

        var res = _spaceshipModuleDataProvider.GetAll();
        var resMapped = _mapper.Map<List<ResponseDynamicItemSpaceshipModule>>(res);

        return resMapped;
    }

    [HttpGet]
    [Route("get/max-module-count/by/size", Name = "GetMaxModuleCountBySize")]
    public async Task<ActionResult<int>> GetMaxModuleCountBySize([FromQuery] Spaceship.SpaceshipSize size)
    {
        var user = await GetUser();

        if (user == null) return StatusCodeGenerator.NotConnected();

        var maxCount = _spaceshipBlueprintService.GetMaxModuleCount(size, user.Id);

        return maxCount;
    }
}