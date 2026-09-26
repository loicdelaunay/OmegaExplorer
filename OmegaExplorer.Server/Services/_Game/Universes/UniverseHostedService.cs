using OmegaExplorer.Server.Services._Game.Universes.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Universes;

public class UniverseHostedService : IHostedService
{
    private readonly ILogger<UniverseHostedService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public UniverseHostedService(ILogger<UniverseHostedService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await EnsureUniverseExist(EnumUniverseType.Physic);
        await EnsureUniverseExist(EnumUniverseType.Phasic);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task EnsureUniverseExist(EnumUniverseType type)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        var universeService = scope.ServiceProvider.GetRequiredService<UniverseService>();
        var universeRepository = scope.ServiceProvider.GetRequiredService<UniverseRepository>();

        var exist = await universeRepository.GetUniverseByType(type);

        if (exist == null) await universeService.CreateUniverse(type);
    }
}