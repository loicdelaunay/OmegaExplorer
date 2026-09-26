using MudBlazor;

namespace OmegaExplorer.Client.Models.Card;

public class CardContent
{

    public string Title;
    public string Value;
    public string ValueDetailed;
    public Typo Typo = Typo.body2;
    public Color Color = Color.Default;
    public string Function = string.Empty;
    public string FunctionDescription = string.Empty;
    public string TargetUrl = string.Empty;

    public CardContent(string title, string value)
    {
        Title = title;
        Value = value;
    }

    public CardContent(string title = null, string value = null, string valueDetailed = null, Typo typo = default, Color color = default, string targetUrl = default)
    {
        Title = title;
        Value = value;
        ValueDetailed = valueDetailed;
        Typo = typo;
        Color = color;
        TargetUrl = targetUrl;
    }

    public override string ToString()
    {
        return $"{Title} : {Value}";
    }
}