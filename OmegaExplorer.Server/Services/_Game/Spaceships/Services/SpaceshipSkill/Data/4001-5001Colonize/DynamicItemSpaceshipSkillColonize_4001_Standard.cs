using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Effects;
using OmegaExplorer.Server.Services._Game.Effects.Models.Enums;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipSkill.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipSkill.Data._4001_5001Colonize;

public class DynamicItemSpaceshipSkillColonize_4001_Standard : IDynamicItemSpaceshipSkill
{
    public const int INDEX = 4001;
    public static readonly DynamicItemSpaceshipSkillColonize_4001_Standard Instance = new();

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "Colonize";
    public string? Description { get; set; } = "A colonize method to colonize planet.";
    public EnumDataKnowledge Knowledge { get; set; } = EnumDataKnowledge.Unlocked;
    public EnumRarity Rarity { get; set; } = EnumRarity.Common;

    public List<Effect> Effects { get; set; } = new()
    {
        new Effect
        {
            TargetType = EnumEffectTargetType.StarSystem,
            Target = EnumEffectTarget.Enemy,
            Type = EnumEffectType.Colonization,
            Value = 20,
            ModuleTargetType = EnumEffectModuleTargetType.Random
        }
    };
}