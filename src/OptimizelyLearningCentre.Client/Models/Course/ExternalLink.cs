namespace OptimizelyLearningCentre.Client.Models.Course;

/// <summary>
/// An external link to documentation or resources
/// </summary>
public class ExternalLink
{
    /// <summary>
    /// Display title
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// URL to the external resource
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Icon name (Heroicons)
    /// </summary>
    public string Icon { get; set; } = string.Empty;
}
