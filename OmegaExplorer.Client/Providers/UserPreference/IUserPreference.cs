namespace OmegaExplorer.Client.Services.UserPreference;

public interface IUserPreference
{
    Task<T> GetPreferenceAsync<T>(string key);
    Task SetPreferenceAsync<T>(string key, T value);
}