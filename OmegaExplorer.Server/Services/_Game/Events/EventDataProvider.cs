using OmegaExplorer.Server.Services._Game._core;
using OmegaExplorer.Server.Services._Game.Events.Models.Interfaces;

namespace OmegaExplorer.Server.Services._Game.Events;

public class EventDataProvider : GameDataProvider<IDynamicItemEvent>
{
    public EventDataProvider(ILogger<EventDataProvider> logger) : base(logger)
    {
    }
}