using OptimizelyLearningCentre.Client.Models.Course;

namespace OptimizelyLearningCentre.Client.Services;

/// <summary>
/// Provides context for the currently active course
/// </summary>
public class CourseContext : ICourseContext
{
    /// <inheritdoc />
    public CourseDefinition? CurrentCourse { get; private set; }

    /// <inheritdoc />
    public event Action<CourseDefinition?>? OnCourseChanged;

    /// <inheritdoc />
    public void SetCurrentCourse(CourseDefinition? course)
    {
        if (CurrentCourse?.Id != course?.Id)
        {
            CurrentCourse = course;
            OnCourseChanged?.Invoke(course);
        }
    }

    /// <inheritdoc />
    public string GetStorageKey(string keyType)
    {
        var courseId = CurrentCourse?.Id ?? "global";
        return $"lp_{courseId}_{keyType}";
    }
}
