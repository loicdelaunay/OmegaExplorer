namespace OmegaExplorer.Client.Managers;

public class UserSettings
{
    public bool DarkMode { get; set; } = true;
    public bool IsDev { get; set; } = false;
    public string Language { get; set; }
    public string LastEmail { get; set; }
    public string LastPassword { get; set; }

    //Last user token
    public string Token { get; set; }

    public bool AccountRemember { get; set; } = true;

    public override string ToString()
    {
        return $"Dark mode : {DarkMode} | IsDev {IsDev} | Language {Language}";
    }
}