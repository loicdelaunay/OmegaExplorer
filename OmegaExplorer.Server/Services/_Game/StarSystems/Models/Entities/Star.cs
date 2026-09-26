using OmegaExplorer.Server.Services.Databases;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmegaExplorer.Server.Services._Game.StarSystems.Models.Entities;

[Table(nameof(DatabaseContext.Stars))]
public class Star : StarSystem
{
    [ActivatorUtilitiesConstructor]
    public Star()
    {
    }

    public Star(string name, int size)
        : base(name, size)
    {
        Rarity = GetRandomRarity();
    }
}