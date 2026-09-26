using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using Serilog.Sinks.SystemConsole.Themes;

namespace OmegaExplorer.Server.OmegaExplorer.Shared.Utilities.Log
{
    public class CustomColoredFormatter : ITextFormatter
    {
        private readonly string _outputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

        public void Format(LogEvent logEvent, TextWriter output)
        {
            bool isSuccessful = logEvent.Properties.TryGetValue("IsSuccessful", out LogEventPropertyValue? value) &&
                                value is ScalarValue { Value: true };

            if (isSuccessful)
            {
                output.Write("\x1b[32m"); // Vert pour succès
            }
            else
            {
                string ansiColor = logEvent.Level switch
                {
                    LogEventLevel.Information => "\x1b[37m", // Cyan (par défaut)
                    LogEventLevel.Warning => "\x1b[33m", // Jaune (par défaut)
                    LogEventLevel.Error => "\x1b[31m", // Rouge (par défaut)
                    LogEventLevel.Fatal => "\x1b[31m", // Rouge (par défaut)
                    _ => "\x1b[90m" // Blanc/gris clair (par défaut)
                };
                output.Write(ansiColor);
            }

            StringWriter messageOutput = new StringWriter();
            new MessageTemplateTextFormatter(_outputTemplate).Format(logEvent, messageOutput);
            output.Write(messageOutput.ToString());

            output.Write("\x1b[0m"); // Réinitialiser la couleur après le message
        }
    }
}