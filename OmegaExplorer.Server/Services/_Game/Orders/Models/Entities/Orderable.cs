using Microsoft.EntityFrameworkCore;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services.Databases;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmegaExplorer.Server.Services._Game.Orders.Models.Entities;

[Table(nameof(DatabaseContext.Orderables))]
public class Orderable : Metadata
{
    [InverseProperty(nameof(Order.Orderable))]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public List<Order> Orders { get; set; } = new();
}