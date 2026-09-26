using OmegaExplorer.Server.Services._Game._core.Models.Enums;
using OmegaExplorer.Server.Services._Game._core.Models.Interfaces;
using OmegaExplorer.Server.Services._Game.Technologies.Models.Enums;

namespace OmegaExplorer.Server.Services._Game._core.Models.Classes;

/// <summary>
///     Base class for all dynamic elements
/// </summary>
/// <typeparam name="T">Type of concrete class</typeparam>
public abstract class DynamicItemBase<T> : IDynamicItem where T : DynamicItemBase<T>, new()
{
    // Instance singleton accessible
    public static readonly T Instance = new();

    // Idynamicitem implementation
    public int Index
    {
        get => GetIndex();
        set => throw new NotImplementedException();
    }

    public virtual string Name { get; set; } = string.Empty;
    public virtual string? Description { get; set; } = string.Empty;
    public virtual EnumDataKnowledge Knowledge { get; set; } = EnumDataKnowledge.Locked;
    public virtual EnumRarity Rarity { get; set; } = EnumRarity.Common;

    public virtual void Feed()
    {
    }

    // Index to be defined in derivative classes
    protected abstract int GetIndex();
}