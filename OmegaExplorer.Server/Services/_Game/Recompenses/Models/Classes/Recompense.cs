namespace OmegaExplorer.Server.Services._Game.Recompenses.Models.Classes;

public class Recompense
{
    public enum RecompenseType
    {
        Spaceship
    }

    public RecompenseType Type { get; set; }

    public int Index { get; set; }

    public int Amount { get; set; }
}