namespace OptimizelyLearningCentre.Client.Models.Course;

/// <summary>
/// Defines a course with its metadata, content provider, and navigation
/// </summary>
public class CourseDefinition
{
    /// <summary>
    /// Unique identifier for the course (e.g., "graph", "opal")
    /// Used in URLs and localStorage keys
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Display name (e.g., "Optimizely Graph")
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Short description for course catalog
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Detailed description for course landing page
    /// </summary>
    public string LongDescription { get; set; } = string.Empty;

    /// <summary>
    /// Icon identifier (Heroicons name)
    /// </summary>
    public string Icon { get; set; } = string.Empty;

    /// <summary>
    /// Brand color for course theming (CSS color value)
    /// </summary>
    public string BrandColor { get; set; } = "#0037FF";

    /// <summary>
    /// Type that provides learning content (implements ILearningContentProvider)
    /// </summary>
    public Type ContentProviderType { get; set; } = null!;

    /// <summary>
    /// Type for the interactive panel component used in lessons (e.g., TryItPanel, PromptPanel)
    /// </summary>
    public Type InteractivePanelType { get; set; } = null!;

    /// <summary>
    /// Navigation items for this course - defines what pages the course has
    /// </summary>
    public List<CourseNavItem> NavItems { get; set; } = new();

    /// <summary>
    /// External documentation and resource links
    /// </summary>
    public List<ExternalLink> ExternalLinks { get; set; } = new();
}
