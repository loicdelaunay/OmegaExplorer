using OmegaExplorer.Server.Services._Game._core.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Events.Models.Entities;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Classes;

namespace OmegaExplorer.Server.Services._Game.Events.Modules.EventChoice.Classes.Interfaces;

public interface IDynamicItemEventChoice : IDynamicItem
{
    public string Text { get; set; }

    public List<ModifierResource> OneTimeModifiers { get; set; }

    public List<ModifierResource> Modifiers { get; set; }

    public Task Execute(IServiceProvider serviceProvider, Guid userId, Event @event);
}