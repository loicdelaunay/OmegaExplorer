using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Effects;
using OmegaExplorer.Server.Services._Game.Effects.Models.Enums;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipSkill.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipSkill.Data._4001_5001Colonize;

public class DynamicItemSpaceshipSkillColonize_4002_PopulationTsunami : IDynamicItemSpaceshipSkill
{
    public const int INDEX = 4002;
    public static readonly DynamicItemSpaceshipSkillColonize_4002_PopulationTsunami Instance = new();

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "Population Tsunami";
    public string? Description { get; set; } = "A method to unleash billions of people on a planet.";
    public EnumDataKnowledge Knowledge { get; set; } = EnumDataKnowledge.Unlocked;

    public EnumRarity Rarity { get; set; } = EnumRarity.Origin;

    public List<Effect> Effects { get; set; } = new()
    {
        new Effect
        {
            TargetType = EnumEffectTargetType.StarSystem,
            Target = EnumEffectTarget.Enemy,
            Type = EnumEffectType.Colonization,
            Value = 10000,
            ModuleTargetType = EnumEffectModuleTargetType.Random
        }
    };
}