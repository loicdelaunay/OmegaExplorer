using OmegaExplorer.Server.Services._Game._core;
using OmegaExplorer.Server.Services._Game.Events.Modules.EventChoice.Classes.Interfaces;

namespace OmegaExplorer.Server.Services._Game.Events.Modules.EventChoice;

public class EventChoiceDataProvider : GameDataProvider<IDynamicItemEventChoice>
{
    public EventChoiceDataProvider(ILogger<EventChoiceDataProvider> logger) : base(logger)
    {
    }
}