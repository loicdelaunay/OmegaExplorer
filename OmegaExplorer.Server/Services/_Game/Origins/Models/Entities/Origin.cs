using Microsoft.EntityFrameworkCore;

namespace OmegaExplorer.Server.Services._Game.Origins.Models.Entities;

/// <summary>
///     Determine the origin of the user in the space
/// </summary>
[Owned]
public class Origin
{
    /// <summary>
    ///     Selected species index
    /// </summary>
    public int IndexSpecies { get; set; }

    public int IndexHistory { get; set; }
}