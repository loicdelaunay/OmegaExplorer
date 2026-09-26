using OmegaExplorer.Client.Components.Blocks.Utilities;

namespace OmegaExplorer.Client.Managers;

public static class UserSettingsManager
{
    private static UserSettingsProvider? _userSettingsProvider;
    private static byte[] _encryptionKey;

    public static Action<UserSettings> OnSettingsChanged;

    public static async Task Initialize(UserSettingsProvider userSettingsProvider)
    {
        _userSettingsProvider = userSettingsProvider;
        OnSettingsChanged.Invoke(await GetUserSettings());
    }

    public static void SetUserSettings(UserSettings userSettings)
    {
        Console.WriteLine("Set settings : " + userSettings);
        OnSettingsChanged?.Invoke(userSettings);

        if (_userSettingsProvider == null)
        {
            Console.WriteLine("[WARNING] Try to get settings but user settings provider is null");
            return;
        }

        _userSettingsProvider.Save(userSettings);
    }


    /// <summary>
    ///     Return the current user settings
    /// </summary>
    /// <returns> </returns>
    public static async Task<UserSettings?> GetUserSettings()
    {
        if (_userSettingsProvider == null)
        {
            Console.WriteLine("[WARNING] Try to get settings but user settings provider is null");
            return null;
        }

        UserSettings userSettings = await _userSettingsProvider.Load();

        return userSettings;
    }
}