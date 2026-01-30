using OptimizelyLearningCentre.Client.Models.Course;

namespace OptimizelyLearningCentre.Client.Services;

/// <summary>
/// Registry for managing available courses
/// </summary>
public interface ICourseRegistry
{
    /// <summary>
    /// Gets all registered courses
    /// </summary>
    IReadOnlyList<CourseDefinition> GetAllCourses();

    /// <summary>
    /// Gets a course by its ID
    /// </summary>
    CourseDefinition? GetCourse(string courseId);

    /// <summary>
    /// Registers a course
    /// </summary>
    void RegisterCourse(CourseDefinition course);
}
