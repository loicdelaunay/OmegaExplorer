using OmegaExplorer.Server.Services._Game._core.Models.Classes;

namespace OmegaExplorer.Server.Services._Core.Models.Contracts._inherits;

public class ResponseMetadata : ResponseIdentifiable
{
    public DateTime? DateLastAccess { get; set; }
    public DateTime DateCreation { get; set; }
    public DateTime? DateEdition { get; set; }
    public DateTime? DateDeletion { get; set; }
    public bool Disabled { get; set; }

    /// <summary>
    /// <see cref="Metadata.CycleWhenRemove"/>
    /// </summary>
    public long? CycleWhenRemove { get; set; }
}