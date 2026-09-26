namespace OmegaExplorer.Server.Services.Changelogs.Models.Enums;

[Flags]
public enum EnumChangelogType
{
    MajorFeature,
    MinorFeature,
    UI,
    Performance,
    API,
    Security,
    Documentation,
    Refactor,
    Database,
    Deprecated,
    QOA,
    Fix,
    WIP,
}