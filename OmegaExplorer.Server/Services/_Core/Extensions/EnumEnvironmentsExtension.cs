using OmegaExplorer.Server.Services.Servers.Models.Enums;

namespace OmegaExplorer.Server.Services._Core.Extensions;

public static class EnumEnvironmentsExtension
{
    public static bool IsDevelopment(this EnumStartMode startMode)
    {
        return startMode >= EnumStartMode.Development;
    }
}