using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Technologies.Services.TechnologyKnowledge.Models.Contracts;
using OmegaExplorer.Server.Services.Authentications;
using OmegaExplorer.Server.Services.Servers;

namespace OmegaExplorer.Server.Services._Game.Technologies.Services.TechnologyKnowledge;

[ApiController]
[Route("api/technology/knowledge")]
public class TechnologyKnowledgeController : ControllerCustom
{
    private readonly IMapper _mapper;
    private readonly TechnologyKnowledgeRepository _technologyKnowledgeRepository;
    private readonly TechnologyService _technologyService;

    public TechnologyKnowledgeController(AuthenticationService authenticationService,
        TechnologyKnowledgeRepository technologyKnowledgeRepository, IMapper mapper,
        TechnologyService technologyService) : base(authenticationService)
    {
        _technologyKnowledgeRepository = technologyKnowledgeRepository;
        _mapper = mapper;
        _technologyService = technologyService;
    }

    #region GET

    [HttpGet]
    [Route("get/all", Name = nameof(GetAllTechnologyKnowledges))]
    public async Task<ActionResult<List<ResponseTechnologyKnowledge>>> GetAllTechnologyKnowledges()
    {
        try
        {
            var user = await GetUser();

            if (user == null) return StatusCodeGenerator.NotConnected();

            var res = await _technologyKnowledgeRepository.GetTechnologyKnowledges(user.Id);
            var mapped = _mapper.Map<List<ResponseTechnologyKnowledge>>(res);

            return mapped;
        }
        catch (Exception e)
        {
            return StatusCodeGenerator.Exception(e);
        }
    }

    [HttpGet]
    [Route("get/current", Name = nameof(GetCurrentTechnologyProgress))]
    public async Task<ActionResult<ResponseTechnologyKnowledge?>> GetCurrentTechnologyProgress()
    {
        try
        {
            var user = await GetUser();

            if (user == null) return StatusCodeGenerator.NotConnected();

            var res = await _technologyService.GetCurrentTechnologyProgress(user);
            var mapped = _mapper.Map<ResponseTechnologyKnowledge>(res);

            return Ok(mapped);
        }
        catch (Exception e)
        {
            return StatusCodeGenerator.Exception(e);
        }
    }

    #endregion

    #region ACTIONS

    [HttpPost]
    [Route("search", Name = nameof(StartSearchTechnology))]
    public async Task<ActionResult<ResponseTechnologyKnowledge>> StartSearchTechnology(int technologyIndex)
    {
        try
        {
            var user = await GetUser();

            var res = await _technologyKnowledgeRepository.StartSearchTechnology(user.Id, technologyIndex);
            var mapped = _mapper.Map<ResponseTechnologyKnowledge>(res);

            return mapped;
        }
        catch (Exception e)
        {
            return StatusCodeGenerator.Exception(e);
        }
    }

    [HttpPost]
    [Route("stop", Name = nameof(StopSearchTechnology))]
    public async Task<ActionResult> StopSearchTechnology()
    {
        try
        {
            var user = await GetUser();

            if (user == null) return StatusCodeGenerator.NotConnected();

            await _technologyKnowledgeRepository.StopSearchTechnology(user.Id);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCodeGenerator.Exception(e);
        }
    }

    #endregion
}