using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Personalities.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Personalities.Models.Entities;
using OmegaExplorer.Server.Services.Authentications;
using OmegaExplorer.Server.Services.Servers;

namespace OmegaExplorer.Server.Services._Game.Personalities;

[Route("/api/personality")]
[ApiController]
public class PersonalityController : ControllerCustom
{
    private readonly IMapper _mapper;
    private readonly PersonalityRepository _personalityRepository;
    private readonly PersonalityService _personalityService;

    public PersonalityController(AuthenticationService authenticationService,
        PersonalityRepository personalityRepository, IMapper mapper,
        PersonalityService personalityService) : base(authenticationService)
    {
        _personalityRepository = personalityRepository;
        _mapper = mapper;
        _personalityService = personalityService;
    }

    #region POST

    [HttpPost]
    [Route("create", Name = "CreatePersonality")]
    public async Task<ActionResult<ResponsePersonality>> Create()
    {
        var user = await GetUser();

        if (user == null) return StatusCodeGenerator.NotConnected();

        var newPersonality = await _personalityService.Create(user.Id);

        var personalityMapped = _mapper.Map<ResponsePersonality>(newPersonality);
        return personalityMapped;
    }

    #endregion

    #region DELETE

    [HttpDelete]
    [Route("delete", Name = "DeletePersonality")]
    public async Task<ActionResult> Delete(Guid personalityId)
    {
        try
        {
            var user = await GetUser();

            if (user == null) return StatusCodeGenerator.NotConnected();

            await _personalityRepository.Delete(personalityId, user.Id);

            return Ok();
        }
        catch (Exception e)
        {
            return StatusCodeGenerator.Exception(e);
        }
    }

    #endregion

    #region GET

    [HttpGet]
    [Route("get/all", Name = "GetAllPersonalities")]
    public async Task<ActionResult<List<ResponsePersonality>>> GetAll()
    {
        var user = await GetUser();

        if (user == null) return StatusCodeGenerator.NotConnected();

        var personalities = await _personalityRepository.GetAll(user.Id);

        var personalitiesMapped = _mapper.Map<List<ResponsePersonality>>(personalities);
        return personalitiesMapped;
    }

    [HttpGet]
    [Route("get/by-id", Name = "GetPersonalityById")]
    public async Task<ActionResult<ResponsePersonality>> GetById(Guid personalityId)
    {
        var user = await GetUser();

        if (user == null) return StatusCodeGenerator.NotConnected();

        var personality = await _personalityRepository.GetById(personalityId, user.Id);

        if (personality == null) return StatusCodeGenerator.NotFound(nameof(Personality));

        var personalitiesMapped = _mapper.Map<ResponsePersonality>(personality);
        return personalitiesMapped;
    }

    #endregion
}