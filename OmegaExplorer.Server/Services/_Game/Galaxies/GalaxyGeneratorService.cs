using OmegaExplorer.Server.Extensions;
using OmegaExplorer.Server.Services._Game._core.Models;
using OmegaExplorer.Server.Services._Game._core.Models.Classes;
using OmegaExplorer.Server.Services._Game.Galaxies.Models.Entities;
using OmegaExplorer.Server.Services._Game.Galaxies.Models.Enums;
using OmegaExplorer.Server.Services._Game.Universes.Models.Entities;
using OmegaExplorer.Server.Services.ProjectFiles;

namespace OmegaExplorer.Server.Services._Game.Galaxies;

public class GalaxyGeneratorService : IHostedService
{
    public NamingContent NamingContent { get; private set; } = new();

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var fileNamingGalaxy = FileProjectManager.GetProjectFile("Services/_Game/Galaxies/Data/galaxy.naming.json");
        NamingContent = fileNamingGalaxy.ReadJson<NamingContent>();

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public async Task<Galaxy> Generate(Vector2 position, Universe universe)
    {
        var name = NamingContent.Names.GetRandom();
        var adjective = NamingContent.Adjectives.GetRandom();

        Galaxy galaxy = new()
        {
            Name = $"{name} {adjective}",
            Type = EnumGalaxyType.Safe,
            PositionX = position.X,
            PositionY = position.Y,
            UniverseId = universe.Id
        };

        return galaxy;
    }
}