using OmegaExplorer.Server.Services._Game.Limits.Models.Entities;

namespace OmegaExplorer.Server.Services._Game.Limits.Models.Exceptions;

public class ExceptionLimitReached : Exception
{
    public ExceptionLimitReached(Limit limit) : base($"Limit reached: {limit.Current} ({limit.Max})")
    {
        Limit = limit;
    }

    public Limit Limit { get; set; }
}