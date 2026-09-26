using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game.Effects;
using OmegaExplorer.Server.Services._Game.Effects.Models.Enums;
using OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipSkill.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Spaceships.Services.SpaceshipSkill.Data._0_1000Ballistic;

public class DynamicItemSpaceshipSkillBallisticShootADM : IDynamicItemSpaceshipSkill
{
    public const int INDEX = 1000;
    public static readonly DynamicItemSpaceshipSkillBallisticShootADM Instance = new();

    public int Index
    {
        get => INDEX;
        set => throw new NotImplementedException();
    }

    public string Name { get; set; } = "Ballistic Shoot ADM";

    public string? Description { get; set; } =
        "Shooting big rock into the face of your enemies with additional lines of codes and cheating to debug the game.";

    public EnumDataKnowledge Knowledge { get; set; } = EnumDataKnowledge.Unlocked;
    public EnumRarity Rarity { get; set; } = EnumRarity.Origin;

    public List<Effect> Effects { get; set; } = new()
    {
        new Effect
        {
            TargetType = EnumEffectTargetType.Spaceship,
            Target = EnumEffectTarget.Enemy,
            Type = EnumEffectType.Damage,
            Value = 1000,
            ModuleTargetType = EnumEffectModuleTargetType.All
        }
    };
}