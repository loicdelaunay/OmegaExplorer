using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services.Databases;
using OmegaExplorer.Server.Services.Users.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmegaExplorer.Server.Services._Game.Events.Models.Entities;

[Table(nameof(DatabaseContext.Events))]
public class Event : Metadata
{
    public int EventItemIndex { get; set; }

    /// <summary>
    ///     Main id item of the event (planet, spaceship, celebrities ...)
    /// </summary>
    public Guid? TargetId { get; set; }

    /// <summary>
    ///     Define the type of the target (planet, spaceship, celebrities ...)
    /// </summary>
    [MaxLength(50)]
    public string? TargetType { get; set; }

    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public User? User { get; set; }
}