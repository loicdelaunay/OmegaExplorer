namespace OmegaExplorer.Client.Services.UserPreference;

using Blazored.LocalStorage;

public class UserPreference : IUserPreference
{
    private readonly ILocalStorageService _localStorageService;

    public UserPreference(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    public async Task<T> GetPreferenceAsync<T>(string key)
    {
        return await _localStorageService.GetItemAsync<T>(key);
    }

    public async Task SetPreferenceAsync<T>(string key, T value)
    {
        await _localStorageService.RemoveItemAsync(key);

        await _localStorageService.SetItemAsync(key, value);
    }
}