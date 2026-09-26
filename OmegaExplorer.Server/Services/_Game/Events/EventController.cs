using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Events.Models.Contracts;
using OmegaExplorer.Server.Services.Authentications;
using OmegaExplorer.Server.Services.Servers;

namespace OmegaExplorer.Server.Services._Game.Events;

[ApiController]
[Route("api/event")]
public class EventController : ControllerCustom
{
    private readonly EventService _eventService;
    private readonly IMapper _mapper;

    public EventController(AuthenticationService authenticationService, IMapper mapper, EventService eventService) :
        base(authenticationService)
    {
        _mapper = mapper;
        _eventService = eventService;
    }

    #region GET

    [HttpGet]
    [Route("/events", Name = nameof(GetAllEventsByConnectedUser))]
    public async Task<ActionResult<List<ResponseEvent>>> GetAllEventsByConnectedUser()
    {
        var user = await GetUser();

        if (user == null) return StatusCodeGenerator.NotConnected();


        var events = await _eventService.GetEvents(user.Id);
        var eventsMapped = _mapper.Map<List<ResponseEvent>>(events);

        return eventsMapped;
    }

    #endregion

    #region ACTION

    [HttpPost]
    [Route("/choice", Name = nameof(EventMakeChoice))]
    public async Task<ActionResult> EventMakeChoice(Guid eventId, int choiceIndex)
    {
        try
        {
            var user = await GetUser();

            if (user == null) return StatusCodeGenerator.NotConnected();

            await _eventService.MakeChoice(user.Id, eventId, choiceIndex);

            return Ok();
        }
        catch (Exception e)
        {
            return StatusCodeGenerator.Exception(e);
        }
    }

    #endregion
}