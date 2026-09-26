namespace OmegaExplorer.Server.Services._Game._core.Models.Classes;

/// <summary>
///     Inherit from `Identifiable` and add logical creation and deletion date
/// </summary>
public class Metadata : Identifiable
{
    public DateTime DateCreation { get; set; } = DateTime.Now;
    public DateTime? DateEdition { get; set; }

    public DateTime? DateLastAccess { get; set; }

    public DateTime? DateDeletion { get; set; }

    public int NumberAccess { get; set; }
    public bool Disabled { get; set; }

    public string? DisableReason { get; set; }

    /// <summary>
    ///     Cycle when the entity will be removed definitively
    /// </summary>
    public long? CycleWhenRemove { get; set; }
}