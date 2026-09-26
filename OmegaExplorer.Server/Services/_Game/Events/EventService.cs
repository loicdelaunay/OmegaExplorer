using OmegaExplorer.Server.Services._Game.Events.Models.Entities;
using OmegaExplorer.Server.Services._Game.Events.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Events.Modules.EventChoice;

namespace OmegaExplorer.Server.Services._Game.Events;

public class EventService
{
    private readonly EventChoiceDataProvider _eventChoiceDataProvider;
    private readonly EventDataProvider _eventDataProvider;

    private readonly EventRepository _eventRepository;

    private readonly IServiceProvider _serviceProvider;

    public EventService(EventRepository eventRepository, EventDataProvider eventDataProvider,
        EventChoiceDataProvider eventChoiceDataProvider, IServiceProvider serviceProvider)
    {
        _eventRepository = eventRepository;
        _eventDataProvider = eventDataProvider;
        _eventChoiceDataProvider = eventChoiceDataProvider;
        _serviceProvider = serviceProvider;
    }

    public async Task CreateEvent(Guid userId, int eventIndex, Guid targetId, string targetType)
    {
        Event newEvent = new()
        {
            UserId = userId,
            EventItemIndex = eventIndex,
            TargetId = targetId,
            TargetType = targetType
        };

        await _eventRepository.CreateEvent(newEvent);
    }

    public async Task<List<Event>> GetEvents(Guid userId)
    {
        var events = await _eventRepository.GetAllByUser(userId);

        return events;
    }

    public async Task MakeChoice(Guid userId, Guid eventId, int choiceIndex)
    {
        //Get event & event data

        var @event = await _eventRepository.GetById(userId, eventId);

        if (@event == null) throw new Exception($"Event not found at {eventId}");

        var eventData = _eventDataProvider.GetByIndex(@event.EventItemIndex);

        if (eventData == null) throw new Exception($"Event data not found at {@event.EventItemIndex}");

        //Execute
        await Execute(eventData, userId, choiceIndex, @event);

        //End event
        await _eventRepository.EndEvent(eventId);
    }

    public Task Execute(IDynamicItemEvent eventData, Guid userId, int choiceIndex, Event @event)
    {
        //Get the choice and execute the choice
        var choice = _eventChoiceDataProvider.GetByIndex(choiceIndex);

        if (choice == null)
            throw new Exception($"Choice {choiceIndex} not found");

        choice.Execute(_serviceProvider, userId, @event);

        return Task.CompletedTask;
    }
}