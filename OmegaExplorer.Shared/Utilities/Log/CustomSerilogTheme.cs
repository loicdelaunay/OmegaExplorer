using Serilog.Sinks.SystemConsole.Themes;

namespace OmegaExplorer.Server.OmegaExplorer.Shared.Utilities.Log
{
    public class CustomSerilogTheme : Serilog.Sinks.SystemConsole.Themes.ConsoleTheme
    {
        public override bool CanBuffer => true;

        protected override int ResetCharCount => 5; // Longueur de "\x1b[0m"

        public override int Set(TextWriter output, ConsoleThemeStyle style)
        {
            string ansiColor = style switch
            {
                ConsoleThemeStyle.LevelInformation => "\x1b[36m", // Cyan (par défaut)
                ConsoleThemeStyle.LevelWarning => "\x1b[33m", // Jaune (par défaut)
                ConsoleThemeStyle.LevelError => "\x1b[31m", // Rouge (par défaut)
                ConsoleThemeStyle.LevelFatal => "\x1b[31m", // Rouge (par défaut)
                _ => "\x1b[37m" // Blanc/gris clair (par défaut)
            };
            output.Write(ansiColor);
            return ansiColor.Length;
        }

        public override void Reset(TextWriter output)
        {
            output.Write("\x1b[0m"); // Réinitialiser la couleur
        }
    }
}