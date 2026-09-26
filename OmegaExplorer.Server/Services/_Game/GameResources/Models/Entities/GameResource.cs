using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services.Databases;
using OmegaExplorer.Server.Services.Users.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmegaExplorer.Server.Services._Game.GameResources.Models.Entities;

[Table(nameof(DatabaseContext.Resources))]
/// <summary>
/// 
/// </summary>
public class GameResource : Identifiable
{
    /// <summary>
    ///     Different from the ID in database
    ///     identify the resource unique to link it to Game Data
    /// </summary>
    public required int Index { get; set; }

    /// <summary>
    ///     How much the user earn this resource
    /// </summary>
    public int Amount { get; set; }

    /// <summary>
    ///     Balance gain and lose of the resource
    /// </summary>
    public int Balance { get; set; } = 0;

    [ForeignKey(nameof(User))]
    public required Guid? UserId { get; set; }

    /// <summary>
    ///     Owner of the resource
    /// </summary>
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public User? User { get; set; }
}