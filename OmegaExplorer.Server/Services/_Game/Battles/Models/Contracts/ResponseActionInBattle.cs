using OmegaExplorer.Server.Services._Core.Models.Contracts._inherits;
using OmegaExplorer.Server.Services._Game.Spaceships.Models.Contracts;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipSkill.Models.Contracts;

namespace OmegaExplorer.Server.Services._Game.Battles.Models.Contracts;

public class ResponseActionInBattle : ResponseIdentifiable
{
    public ResponseSpaceship Actor { get; set; }

    public List<ResponseSpaceship> Targets { get; set; } = new();

    public int Priority { get; set; }

    public int SkillIndex { get; set; }

    public ResponseDynamicItemSpaceshipSkill SpaceshipSkillData { get; set; }

    /// <summary>
    ///     Turn of the action in the battle context
    /// </summary>
    public int Turn { get; set; }

    public int Cycle { get; set; }

    public ResponseActionInBattleResult? Result { get; set; }
}