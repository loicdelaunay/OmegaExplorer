using Microsoft.AspNetCore.Mvc;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Origins.Modules.History.Models._interfaces;
using OmegaExplorer.Server.Services.Authentications;
using OmegaExplorer.Server.Services.Servers;

namespace OmegaExplorer.Server.Services._Game.Origins.Modules.History;

[ApiController]
[Route("/api/origin/history/")]
public class OriginHistoryController : ControllerCustom
{
    private readonly OriginHistoryDataProvider _originHistoryDataProvider;
    private readonly OriginHistoryService _originHistoryService;

    public OriginHistoryController(AuthenticationService authenticationService,
        OriginHistoryService originHistoryService,
        OriginHistoryDataProvider originHistoryDataProvider) : base(authenticationService)
    {
        _originHistoryService = originHistoryService;
        _originHistoryDataProvider = originHistoryDataProvider;
    }

    [HttpGet]
    [Route("get/all/", Name = nameof(GetAllOriginHistories))]
    public async Task<ActionResult<List<IDynamicItemOriginHistory>>> GetAllOriginHistories()
    {
        try
        {
            var originHistories = _originHistoryDataProvider.GetAll();

            return originHistories;
        }
        catch (Exception e)
        {
            return StatusCodeGenerator.Exception(e);
        }
    }
}