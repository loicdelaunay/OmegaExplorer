using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.Services._Game.Events.Models.Entities;
using OmegaExplorer.Server.Services.Databases;

namespace OmegaExplorer.Server.Services._Game.Events;

public class EventRepository
{
    private readonly DatabaseContext _databaseContext;

    public EventRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task CreateEvent(Event newEvent)
    {
        _databaseContext.Events.Add(newEvent);

        await _databaseContext.SaveChangesAsync();
    }

    public async Task<List<Event>> GetAllByUser(Guid userId)
    {
        var res = await _databaseContext.Events.Where(e => e.UserId == userId).ToListAsync();

        return res;
    }

    public async Task EndEvent(Guid eventId)
    {
        var @event = await _databaseContext.Events.FirstOrDefaultAsync(@event => @event.Id == eventId);

        if (@event == null) throw new Exception("Event not found");

        _databaseContext.Events.Remove(@event);

        await _databaseContext.SaveChangesAsync();
    }

    public async Task<Event?> GetById(Guid userId, Guid eventId)
    {
        var @event =
            await _databaseContext.Events.FirstOrDefaultAsync(@event =>
                @event.Id == eventId && @event.UserId == userId);

        return @event;
    }
}