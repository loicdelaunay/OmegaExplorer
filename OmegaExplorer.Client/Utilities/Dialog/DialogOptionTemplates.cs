#region

using MudBlazor;

#endregion

namespace OmegaExplorer.Client.Utilities.Dialog;

public static class DialogOptionTemplates
{
    public static DialogOptions DefaultCustomDialogOptions(MaxWidth maxWidth = MaxWidth.Large, bool fullWidth = false)
    {
        DialogOptions res = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = maxWidth,
            CloseOnEscapeKey = true,
            FullWidth = fullWidth,
            NoHeader = true,
            Position = DialogPosition.Center,
            BackgroundClass = "backdrop-blur"
        };

        return res;
    }

    public static DialogOptions DefaultDialogOptions()
    {
        DialogOptions res = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Large,
            CloseOnEscapeKey = true,
            NoHeader = true,
            Position = DialogPosition.Center,
            BackgroundClass = "backdrop-blur"
        };

        return res;
    }

    public static DialogOptions SmallDialogOptions()
    {
        DialogOptions res = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Small,
            CloseOnEscapeKey = true,
            FullWidth = false,
            NoHeader = true,
            Position = DialogPosition.Center,
            BackgroundClass = "backdrop-blur"
        };

        return res;
    }

    public static DialogOptions ExtraLargeDialogOptions(bool fullWidth = true)
    {
        DialogOptions res = new DialogOptions
        {
            CloseButton = true,
            FullWidth = fullWidth,
            MaxWidth = MaxWidth.ExtraExtraLarge,
            CloseOnEscapeKey = true,
            NoHeader = true,
            Position = DialogPosition.Center,
            BackgroundClass = "backdrop-blur"
        };

        return res;
    }

    public static DialogOptions FullscreenDialogOptions()
    {
        DialogOptions res = new DialogOptions
        {
            CloseButton = true,
            FullScreen = true,
            CloseOnEscapeKey = true,
            NoHeader = true,
            Position = DialogPosition.Center,
            BackgroundClass = "backdrop-blur"
        };

        return res;
    }
}