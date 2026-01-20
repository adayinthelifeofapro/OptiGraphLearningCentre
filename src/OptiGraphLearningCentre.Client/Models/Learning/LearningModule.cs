namespace OptiGraphLearningCentre.Client.Models.Learning;

/// <summary>
/// A learning module containing related lessons
/// </summary>
public class LearningModule
{
    /// <summary>
    /// Unique identifier for the module
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Display title
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Brief description of what the module covers
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Icon name (Heroicons or similar)
    /// </summary>
    public string Icon { get; set; } = string.Empty;

    /// <summary>
    /// Display order
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Difficulty level
    /// </summary>
    public ModuleDifficulty Difficulty { get; set; }

    /// <summary>
    /// Lessons in this module
    /// </summary>
    public List<Lesson> Lessons { get; set; } = new();

    /// <summary>
    /// IDs of modules that should be completed first
    /// </summary>
    public string[] Prerequisites { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Estimated total time to complete the module
    /// </summary>
    public int EstimatedMinutes => Lessons.Sum(l => l.EstimatedMinutes);
}

/// <summary>
/// Difficulty levels for learning modules
/// </summary>
public enum ModuleDifficulty
{
    Beginner,
    Intermediate,
    Advanced
}

/// <summary>
/// A single lesson within a module
/// </summary>
public class Lesson
{
    /// <summary>
    /// Unique identifier
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Parent module ID
    /// </summary>
    public string ModuleId { get; set; } = string.Empty;

    /// <summary>
    /// Lesson title
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Brief summary
    /// </summary>
    public string Summary { get; set; } = string.Empty;

    /// <summary>
    /// Full lesson content in markdown/HTML
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Display order within the module
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Estimated time to complete
    /// </summary>
    public int EstimatedMinutes { get; set; }

    /// <summary>
    /// Interactive examples for this lesson
    /// </summary>
    public List<LessonExample> Examples { get; set; } = new();

    /// <summary>
    /// Learning objectives/goals
    /// </summary>
    public List<string> LearningObjectives { get; set; } = new();

    /// <summary>
    /// ID of the next lesson, if any
    /// </summary>
    public string? NextLessonId { get; set; }

    /// <summary>
    /// ID of the previous lesson, if any
    /// </summary>
    public string? PreviousLessonId { get; set; }
}

/// <summary>
/// An interactive example within a lesson
/// </summary>
public class LessonExample
{
    /// <summary>
    /// Unique identifier
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Example title
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Description of what this example demonstrates
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The GraphQL query for this example
    /// </summary>
    public string GraphQLQuery { get; set; } = string.Empty;

    /// <summary>
    /// Variables for the query
    /// </summary>
    public Dictionary<string, object>? Variables { get; set; }

    /// <summary>
    /// Description of expected results
    /// </summary>
    public string? ExpectedResultDescription { get; set; }

    /// <summary>
    /// Sample response JSON for offline demos
    /// </summary>
    public string? SampleResponse { get; set; }

    /// <summary>
    /// Whether users can modify and run this example
    /// </summary>
    public bool IsInteractive { get; set; } = true;

    /// <summary>
    /// Hints to help users understand the example
    /// </summary>
    public List<string> Hints { get; set; } = new();
}

/// <summary>
/// Tracks user progress through lessons
/// </summary>
public class UserProgress
{
    /// <summary>
    /// Progress for each lesson by ID
    /// </summary>
    public Dictionary<string, LessonProgress> CompletedLessons { get; set; } = new();

    /// <summary>
    /// Last lesson the user accessed
    /// </summary>
    public string? LastAccessedLessonId { get; set; }

    /// <summary>
    /// When the user last used the application
    /// </summary>
    public DateTime LastActivityDate { get; set; }
}

/// <summary>
/// Progress for a single lesson
/// </summary>
public class LessonProgress
{
    /// <summary>
    /// The lesson ID
    /// </summary>
    public string LessonId { get; set; } = string.Empty;

    /// <summary>
    /// Whether the lesson is marked complete
    /// </summary>
    public bool IsCompleted { get; set; }

    /// <summary>
    /// When the lesson was completed
    /// </summary>
    public DateTime? CompletedDate { get; set; }

    /// <summary>
    /// Number of times the user has viewed this lesson
    /// </summary>
    public int ViewCount { get; set; }
}
