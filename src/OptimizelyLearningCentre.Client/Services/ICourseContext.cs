using OptimizelyLearningCentre.Client.Models.Course;

namespace OptimizelyLearningCentre.Client.Services;

/// <summary>
/// Provides context for the currently active course
/// </summary>
public interface ICourseContext
{
    /// <summary>
    /// The currently active course (null if on course catalog page)
    /// </summary>
    CourseDefinition? CurrentCourse { get; }

    /// <summary>
    /// Event fired when the current course changes
    /// </summary>
    event Action<CourseDefinition?>? OnCourseChanged;

    /// <summary>
    /// Sets the current course context
    /// </summary>
    void SetCurrentCourse(CourseDefinition? course);

    /// <summary>
    /// Gets a localStorage key namespaced to the current course
    /// </summary>
    string GetStorageKey(string keyType);
}
