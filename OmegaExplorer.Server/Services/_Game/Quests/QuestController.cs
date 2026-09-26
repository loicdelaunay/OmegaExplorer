using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Quests.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Quests.Models.Interfaces;
using OmegaExplorer.Server.Services.Authentications;
using OmegaExplorer.Server.Services.Servers;

namespace OmegaExplorer.Server.Services._Game.Quests;

[ApiController]
[Route("api/quest/")]
public class QuestController : ControllerCustom
{
    private readonly IMapper _mapper;
    private readonly QuestRepository _questRepository;
    private readonly QuestService _questService;

    public QuestController(AuthenticationService authenticationService, QuestRepository questRepository, IMapper mapper,
        QuestService questService) : base(authenticationService)
    {
        _questRepository = questRepository;
        _mapper = mapper;
        _questService = questService;
    }

    #region DELETE

    [HttpDelete]
    [Route("cancel/", Name = nameof(CancelQuest))]
    public async Task<ActionResult> CancelQuest(Guid questId)
    {
        var user = await GetUser();

        if (user is null) return StatusCodeGenerator.NotConnected();

        await _questService.CancelQuest(user.Id, questId);

        return Ok();
    }

    #endregion

    #region ACTION

    [HttpPost]
    [Route("action/accept/", Name = nameof(AcceptQuest))]
    public async Task<ActionResult<ResponseQuestProgress>> AcceptQuest(int questIndex)
    {
        var user = await GetUser();

        if (user is null) return StatusCodeGenerator.NotConnected();

        var res = await _questService.AcceptQuest(user.Id, questIndex);
        var mapped = _mapper.Map<ResponseQuestProgress>(res);

        return mapped;
    }

    [HttpPost]
    [Route("action/complete/", Name = nameof(CompleteQuest))]
    public async Task<ActionResult> CompleteQuest(Guid questId)
    {
        var user = await GetUser();

        if (user is null) return StatusCodeGenerator.NotConnected();

        await _questService.CompleteQuest(user.Id, questId);

        return Ok();
    }

    #endregion

    #region GET

    /// <summary>
    ///     Return all quests available & in progress
    /// </summary>
    /// <returns></returns>
    [HttpGet("get/all/progress/user-connected", Name = nameof(GetAllQuestsInProgressByUserConnected))]
    public async Task<ActionResult<List<ResponseQuestProgress>>> GetAllQuestsInProgressByUserConnected()
    {
        var user = await GetUser();

        if (user is null) return StatusCodeGenerator.NotConnected();

        var res = await _questService.GetAllQuestsInProgress(user.Id);
        var mapped = _mapper.Map<List<ResponseQuestProgress>>(res);

        return mapped;
    }

    [HttpGet("get/all/", Name = nameof(GetAllQuestsAvailableByUserConnected))]
    public async Task<ActionResult<List<IDynamicItemQuest>>> GetAllQuestsAvailableByUserConnected()
    {
        var user = await GetUser();

        if (user is null) return StatusCodeGenerator.NotConnected();

        var res = await _questService.GetAllQuestsAvailable(user.Id);

        return res;
    }

    #endregion
}