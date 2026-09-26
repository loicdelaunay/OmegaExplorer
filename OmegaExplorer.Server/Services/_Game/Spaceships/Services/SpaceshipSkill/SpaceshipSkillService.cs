using OmegaExplorer.Server.Services._Game.Spaceships.Models.Entities;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipSkill.Models.Interfaces;

namespace OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipSkill;

public class SpaceshipSkillService
{
    private readonly SpaceshipService _spaceshipService;

    public SpaceshipSkillService(SpaceshipService spaceshipService)
    {
        _spaceshipService = spaceshipService;
    }

    public async Task<List<IDynamicItemSpaceshipSkill>> GetSkills(Spaceship spaceship)
    {
        var skills = await _spaceshipService.GetSkills(spaceship);

        return skills;
    }
}