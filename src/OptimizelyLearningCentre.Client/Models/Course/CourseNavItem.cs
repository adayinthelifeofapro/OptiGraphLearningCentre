namespace OptimizelyLearningCentre.Client.Models.Course;

/// <summary>
/// A navigation item for a course, defining a page/route
/// </summary>
public class CourseNavItem
{
    /// <summary>
    /// Unique identifier for the nav item
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Display title in navigation
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Icon name (Heroicons)
    /// </summary>
    public string Icon { get; set; } = string.Empty;

    /// <summary>
    /// Route relative to the course (e.g., "playground" becomes /courses/{courseId}/playground)
    /// </summary>
    public string Route { get; set; } = string.Empty;

    /// <summary>
    /// Component type for this page. Null for shared pages handled by the platform.
    /// </summary>
    public Type? ComponentType { get; set; }

    /// <summary>
    /// Display order in navigation
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Whether this uses a shared page component (like Learn) vs course-specific
    /// </summary>
    public bool IsShared { get; set; }

    /// <summary>
    /// Category for grouping in navigation (e.g., "Main", "Tools", "Settings")
    /// </summary>
    public string Category { get; set; } = "Main";
}
