using OmegaExplorer.Server.Services._Game.Universes.Models.Entities;
using OmegaExplorer.Server.Services._Game.Universes.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Universes;

public class UniverseService
{
    private readonly UniverseRepository _universeRepository;

    public UniverseService(UniverseRepository universeRepository)
    {
        _universeRepository = universeRepository;
    }

    public async Task CreateUniverse(EnumUniverseType type)
    {
        Universe universe = new()
        {
            Name = "Universe " + type,
            Type = type
        };

        await _universeRepository.Create(universe);
    }

    public async Task<Universe> GetUniversePhysic()
    {
        var universe = await _universeRepository.GetUniverseByType(EnumUniverseType.Physic);

        if (universe == null) throw new Exception("Universe Physic not found");

        return universe;
    }

    public async Task<Universe> GetUniversePhasic()
    {
        var universe = await _universeRepository.GetUniverseByType(EnumUniverseType.Phasic);

        if (universe == null) throw new Exception("Universe phasic not found");

        return universe;
    }
}