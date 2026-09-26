using OmegaExplorer.Server.Services._Game._core.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Celebrities.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Events.Models.Enums;
using OmegaExplorer.Server.Services._Game.Events.Modules.EventChoice.Classes.Interfaces;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Classes;

namespace OmegaExplorer.Server.Services._Game.Events.Models.Interfaces;

public interface IDynamicItemEvent : IDynamicItem
{
    public EnumEventType Type { get; set; }

    public EnumEventInterlocutor Interlocutor { get; set; }

    public List<IDynamicItemCelebrity> Celebrities { get; set; }

    public string Text { get; set; }

    public List<ModifierResource> OneTimeModifiers { get; set; }

    public List<ModifierResource> Modifiers { get; set; }

    public List<IDynamicItemEventChoice> Choices { get; set; }
}