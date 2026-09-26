using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Scenarios.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Scenarios.Models.Interfaces;
using OmegaExplorer.Server.Services.Authentications;
using OmegaExplorer.Server.Services.Servers;
using OmegaExplorer.Server.Services.Users.Extensions;
using OmegaExplorer.Server.Services.Users.Models.Enum;

namespace OmegaExplorer.Server.Services._Game.Scenarios;

[ApiController]
[Route("api/scenario/")]
public class ScenarioController : ControllerCustom
{
    private readonly IMapper _mapper;
    private readonly ScenarioDataProvider _scenarioDataProvider;
    private readonly ScenarioService _scenarioService;

    public ScenarioController(AuthenticationService authenticationService, IMapper mapper,
        ScenarioService scenarioService, ScenarioDataProvider scenarioDataProvider) : base(authenticationService)
    {
        _mapper = mapper;
        _scenarioService = scenarioService;
        _scenarioDataProvider = scenarioDataProvider;
    }

    #region GET

    [HttpGet]
    [Route("get/all/by/user/connected", Name = nameof(GetAllScenariosByConnectedUser))]
    public async Task<ActionResult<List<ResponseUserScenarioProgress>>> GetAllScenariosByConnectedUser()
    {
        try
        {
            var user = await GetUser();

            if (user == null) return StatusCodeGenerator.NotConnected();

            var scenarioProgresses = await _scenarioService.GetScenariosByUser(user.Id, true);

            var mapped = _mapper.Map<List<ResponseUserScenarioProgress>>(scenarioProgresses);

            return mapped;
        }
        catch (Exception e)
        {
            Log.Error(e, "Error in GetAllScenariosByConnectedUser");
            return StatusCodeGenerator.Exception(e);
        }
    }

    #endregion

    #region ACTION

    [HttpGet]
    [Route("set/choice", Name = nameof(SetChoice))]
    public async Task<ActionResult<ResponseUserScenarioProgress>> SetChoice(Guid scenarioId, int dialogIndex,
        int choiceIndex)
    {
        try
        {
            var user = await GetUser();

            if (user == null) return StatusCodeGenerator.NotConnected();

            var newScenarioProgress =
                await _scenarioService.SetScenarioChoice(user, scenarioId, dialogIndex, choiceIndex);

            var newScenarioProgressMapped = _mapper.Map<ResponseUserScenarioProgress>(newScenarioProgress);

            return newScenarioProgressMapped;
        }
        catch (Exception e)
        {
            Log.Error(e, "Error in SetChoice");
            return StatusCodeGenerator.Exception(e);
        }
    }

    #endregion

    #region ADMIN

    [HttpGet]
    [Route("admin/get/all/datas", Name = nameof(GetAllDataScenarioAsAdmin))]
    public async Task<ActionResult<List<IDynamicItemScenario>>> GetAllDataScenarioAsAdmin()
    {
        var user = await GetUser();

        if (user == null) return StatusCodeGenerator.NotConnected();

        if (!user.HaveAccess(EnumUserAccessLevel.Admin)) return StatusCodeGenerator.Forbidden();

        var scenarios = _scenarioDataProvider.GetAll();

        return scenarios;
    }

    [HttpPost]
    [Route("admin/add/to/user", Name = nameof(AddScenarioToUserAsAdmin))]
    public async Task<ActionResult> AddScenarioToUserAsAdmin(int scenarioIndex, Guid userId)
    {
        var user = await GetUser();

        if (!user.HaveAccess(EnumUserAccessLevel.Admin)) return StatusCodeGenerator.Forbidden();

        await _scenarioService.AssignScenarioToAPlayer(scenarioIndex, userId);

        return Ok();
    }

    #endregion
}