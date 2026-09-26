using OmegaExplorer.Server.Services._Game.Effects.Models.Enums;

namespace OmegaExplorer.Server.Services._Game.Effects;

public class Effect
{
    /// <summary>
    ///     Target of the skill
    /// </summary>
    public required EnumEffectTargetType TargetType { get; set; } = EnumEffectTargetType.Spaceship;

    public required EnumEffectTarget Target { get; set; }

    public required EnumEffectModuleTargetType ModuleTargetType { get; set; }

    /// <summary>
    ///     If <see cref="EnumEffectModuleTargetType" /> is set to multiple, this value will be used to determine how many
    ///     modules will be impacted
    /// </summary>
    public int ModuleTargetCount { get; set; } = 1;

    public required EnumEffectType Type { get; set; }
    public required int Value { get; set; }
}