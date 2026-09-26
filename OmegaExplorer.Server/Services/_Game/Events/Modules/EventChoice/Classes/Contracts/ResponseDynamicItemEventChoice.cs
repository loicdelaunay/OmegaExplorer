using OmegaExplorer.Server.Services._Core.Models.Contracts._inherits;
using OmegaExplorer.Server.Services._Game.GameResources.Models.Contracts;

namespace OmegaExplorer.Server.Services._Game.Events.Modules.EventChoice.Classes.Contracts;

public class ResponseDynamicItemEventChoice : ResponseDynamicItem
{
    public string Text { get; set; } = string.Empty;

    public List<ResponseModifierResource> OneTimeModifiers { get; set; } = new();

    public List<ResponseModifierResource> Modifiers { get; set; } = new();
}