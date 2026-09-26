using ILogger = Serilog.ILogger;

namespace OmegaExplorer.Server.OmegaExplorer.Shared.Utilities.Log
{
    public static class SerilogExtensions
    {
        public static void Success(this ILogger logger, string messageTemplate, params object[] propertyValues)
        {
            logger.ForContext("IsSuccessful", true).Information(messageTemplate, propertyValues);
        }
    }
}