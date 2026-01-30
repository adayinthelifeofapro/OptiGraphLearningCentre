namespace OptimizelyLearningCentre.Client.Models.Configuration;

/// <summary>
/// User settings for connecting to Optimizely Graph
/// </summary>
public class GraphSettings
{
    /// <summary>
    /// The Optimizely Graph endpoint URL
    /// </summary>
    public string Endpoint { get; set; } = "https://cg.optimizely.com/content/v2";

    /// <summary>
    /// Application key for HMAC authentication
    /// </summary>
    public string? AppKey { get; set; }

    /// <summary>
    /// Secret for HMAC authentication
    /// </summary>
    public string? Secret { get; set; }

    /// <summary>
    /// Single key for public queries
    /// </summary>
    public string? SingleKey { get; set; }

    /// <summary>
    /// The authentication mode to use
    /// </summary>
    public AuthenticationMode AuthMode { get; set; } = AuthenticationMode.SingleKey;

    /// <summary>
    /// Default locale for queries
    /// </summary>
    public string? DefaultLocale { get; set; } = "en";

    /// <summary>
    /// Whether to save query history
    /// </summary>
    public bool SaveQueryHistory { get; set; } = true;

    /// <summary>
    /// Maximum number of history items to keep
    /// </summary>
    public int MaxHistoryItems { get; set; } = 50;

    /// <summary>
    /// Whether dark mode is enabled
    /// </summary>
    public bool IsDarkMode { get; set; } = false;

    /// <summary>
    /// Whether all lessons should be unlocked (skip sequential progression)
    /// </summary>
    public bool UnlockAllLessons { get; set; } = false;
}

/// <summary>
/// Authentication modes supported by Optimizely Graph
/// </summary>
public enum AuthenticationMode
{
    /// <summary>
    /// No authentication (limited functionality)
    /// </summary>
    None,

    /// <summary>
    /// Single key authentication for public queries
    /// </summary>
    SingleKey,

    /// <summary>
    /// HMAC authentication using AppKey and Secret
    /// </summary>
    Hmac
}

/// <summary>
/// Connection status for the Graph endpoint
/// </summary>
public enum ConnectionStatus
{
    Unknown,
    Testing,
    Connected,
    Failed,
    Unauthorized
}
