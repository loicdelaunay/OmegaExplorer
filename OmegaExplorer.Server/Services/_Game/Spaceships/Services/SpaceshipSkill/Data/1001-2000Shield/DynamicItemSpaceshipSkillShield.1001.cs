using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Effects;
using OmegaExplorer.Server.Services._Game.Effects.Models.Enums;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipSkill.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipSkill.Data._1001_2000Shield;

public class DynamicItemSpaceshipSkillShield : IDynamicItemSpaceshipSkill
{
    public const int INDEX = 1001;
    public static readonly DynamicItemSpaceshipSkillShield Instance = new();

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "Shield";
    public string? Description { get; set; } = "A shield that protects you from enemy attacks.";
    public EnumDataKnowledge Knowledge { get; set; } = EnumDataKnowledge.Unlocked;
    public EnumRarity Rarity { get; set; } = EnumRarity.Common;

    public List<Effect> Effects { get; set; } = new()
    {
        new Effect
        {
            TargetType = EnumEffectTargetType.Spaceship,
            Target = EnumEffectTarget.Self,
            Type = EnumEffectType.Protect,
            Value = 20,
            ModuleTargetType = EnumEffectModuleTargetType.All
        }
    };
}