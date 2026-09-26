using OmegaExplorer.Server.Services._Core.Models.Contracts._inherits;
using OmegaExplorer.Server.Services._Game.Celebrities.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Events.Models.Enums;
using OmegaExplorer.Server.Services._Game.Events.Modules.EventChoice.Classes.Contracts;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Classes;

namespace OmegaExplorer.Server.Services._Game.Events.Models.Contracts;

public class ResponseDynamicItemEvent : ResponseDynamicItem
{
    public EnumEventType Type { get; set; }

    public EnumEventInterlocutor Interlocutor { get; set; }

    public List<IDynamicItemCelebrity> Celebrities { get; set; } = new();

    public string Text { get; set; } = string.Empty;

    public List<ModifierResource> OneTimeModifiers { get; set; } = new();

    public List<ModifierResource> Modifiers { get; set; } = new();

    public List<ResponseDynamicItemEventChoice> Choices { get; set; } = new();
}