using MudBlazor;
using OmegaExplorer.Client.Utilities.Enums;

namespace OmegaExplorer.Client.Models.Card;

public class CardAction
{
    public CardAction(string title, string icon, string function, Color color = Color.Default, EnumPriority priority = EnumPriority.Normal, bool cardMethod = false)
    {
        Icon = icon;
        Title = title;
        Function = function;
        Color = color;
        Priority = priority;
        CardMethod = cardMethod;
    }

    public string Icon;
    public string Title;
    public string Function;
    public Color Color = Color.Default;
    public EnumPriority Priority = EnumPriority.Normal;
    public bool Multiple = false;
    public bool CardMethod = false;
}