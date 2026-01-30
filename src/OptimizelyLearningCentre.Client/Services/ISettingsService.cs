using OptimizelyLearningCentre.Client.Models.Configuration;

namespace OptimizelyLearningCentre.Client.Services;

/// <summary>
/// Service for managing application settings
/// </summary>
public interface ISettingsService
{
    /// <summary>
    /// Gets the current settings
    /// </summary>
    Task<GraphSettings> GetSettingsAsync();

    /// <summary>
    /// Saves settings to storage
    /// </summary>
    Task SaveSettingsAsync(GraphSettings settings);

    /// <summary>
    /// Clears all settings
    /// </summary>
    Task ClearSettingsAsync();

    /// <summary>
    /// Event fired when settings change
    /// </summary>
    event Action<GraphSettings>? OnSettingsChanged;
}
