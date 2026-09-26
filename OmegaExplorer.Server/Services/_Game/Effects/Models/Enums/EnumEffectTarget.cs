namespace OmegaExplorer.Server.Services._Game.Effects.Models.Enums;

public enum EnumEffectTarget
{
    Self = 0, // the element itself
    Ally = 1, // the element's ally
    AllyMultiple = 2, // multiple allies
    Enemy = 3, // the element's enemy
    EnemyMultiple = 4 // multiple enemies
}