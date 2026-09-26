using AutoMapper;
using OmegaExplorer.Server.Services._Game.Battles.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Battles.Models.Entities;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipSkill;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipSkill.Models.Contracts;

namespace OmegaExplorer.Server.Services.Mappers.AfterMapper;

public class AfterMapActionInBattle : IMappingAction<ActionInBattle, ResponseActionInBattle>
{
    private readonly SpaceshipSkillDataProvider _spaceshipSkillDataProvider;

    public AfterMapActionInBattle(SpaceshipSkillDataProvider spaceshipSkillDataProvider)
    {
        _spaceshipSkillDataProvider = spaceshipSkillDataProvider;
    }

    public void Process(ActionInBattle src, ResponseActionInBattle dest, ResolutionContext ctx)
    {
        var skill = _spaceshipSkillDataProvider.GetByIndex(src.SkillIndex);

        dest.SpaceshipSkillData = ctx.Mapper.Map<ResponseDynamicItemSpaceshipSkill>(skill);
    }
}
