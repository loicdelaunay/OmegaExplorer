using Microsoft.AspNetCore.Components.Web;

namespace OmegaExplorer.Client.Utilities.Extensions;

public static class KeyboardEventArgsExtension
{
    /// <summary>
    ///     Check if the enter key is pressed
    /// </summary>
    /// <param name="args"> </param>
    /// <returns> </returns>
    public static bool EnterKeyPressed(this KeyboardEventArgs args)
    {
        if (args.Code is "Enter" or "NumpadEnter")
        {
            return true;
        }

        return false;
    }
}