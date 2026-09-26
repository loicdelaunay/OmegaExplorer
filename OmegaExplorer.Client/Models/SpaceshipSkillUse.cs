using OmegaExplorer.Toolkit.API;

namespace OmegaExplorer.Client.Models;

public class SpaceshipSkillUse
{
    public ResponseSpaceship Spaceship { get; set; }
    public ResponseDynamicItemSpaceshipSkill Skill { get; set; }
}