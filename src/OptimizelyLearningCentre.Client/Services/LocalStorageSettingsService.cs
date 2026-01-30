using Blazored.LocalStorage;
using OptimizelyLearningCentre.Client.Models.Configuration;

namespace OptimizelyLearningCentre.Client.Services;

/// <summary>
/// Settings service that persists to browser LocalStorage
/// </summary>
public class LocalStorageSettingsService : ISettingsService
{
    private const string SettingsKey = "optigraph_settings";
    private readonly ILocalStorageService _localStorage;
    private GraphSettings? _cachedSettings;

    public event Action<GraphSettings>? OnSettingsChanged;

    public LocalStorageSettingsService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task<GraphSettings> GetSettingsAsync()
    {
        if (_cachedSettings is not null)
            return _cachedSettings;

        try
        {
            _cachedSettings = await _localStorage.GetItemAsync<GraphSettings>(SettingsKey);
        }
        catch
        {
            // Handle case where stored data is corrupted
            _cachedSettings = null;
        }

        _cachedSettings ??= new GraphSettings();
        return _cachedSettings;
    }

    public async Task SaveSettingsAsync(GraphSettings settings)
    {
        _cachedSettings = settings;
        await _localStorage.SetItemAsync(SettingsKey, settings);
        OnSettingsChanged?.Invoke(settings);
    }

    public async Task ClearSettingsAsync()
    {
        _cachedSettings = null;
        await _localStorage.RemoveItemAsync(SettingsKey);
        OnSettingsChanged?.Invoke(new GraphSettings());
    }
}
