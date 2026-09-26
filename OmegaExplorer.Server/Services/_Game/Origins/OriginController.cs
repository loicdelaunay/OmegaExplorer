using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Origins.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Origins.Modules.History;
using OmegaExplorer.Server.Services.Authentications;
using OmegaExplorer.Server.Services.Servers;

namespace OmegaExplorer.Server.Services._Game.Origins;

[ApiController]
[Route("/api/origin/")]
public class OriginController : ControllerCustom
{
    private readonly IMapper _mapper;
    private readonly OriginHistoryService _originHistoryService;

    public OriginController(AuthenticationService authenticationService, IMapper mapper,
        OriginHistoryService originHistoryService) : base(authenticationService)
    {
        _mapper = mapper;
        _originHistoryService = originHistoryService;
    }

    [HttpGet]
    [Route("get/origin/by/user", Name = nameof(GetOriginByUser))]
    public async Task<ActionResult<ResponseOrigin>> GetOriginByUser()
    {
        try
        {
            var user = await GetUser();

            var origin = user.Origin;
            var originMapped = _mapper.Map<ResponseOrigin>(origin);

            return originMapped;
        }
        catch (Exception e)
        {
            return StatusCodeGenerator.Exception(e);
        }
    }

    [HttpPost]
    [Route("update/origin/by/user", Name = nameof(UpdateOriginByUser))]
    public async Task<ActionResult<ResponseOrigin>> UpdateOriginByUser([FromBody] RequestUpdateOrigin req)
    {
        try
        {
            var user = await GetUser();

            if (user == null) return StatusCodeGenerator.NotFound("User not found");

            var newOrigin = await _originHistoryService.UpdateUserOrigin(req.IndexSpecies, req.IndexHistory, user.Id);
            var originMapped = _mapper.Map<ResponseOrigin>(newOrigin);

            return originMapped;
        }
        catch (Exception e)
        {
            return StatusCodeGenerator.Exception(e);
        }
    }
}