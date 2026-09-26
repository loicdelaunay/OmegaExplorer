namespace OmegaExplorer.Server.Services._Game.Limits.Models.Entities;

public class Limit
{
    public Limit(int current, int max)
    {
        Current = current;
        Max = max;
    }

    public int Current { get; set; }
    public int Max { get; set; }

    public bool IsLimitReached()
    {
        return Current >= Max;
    }
}