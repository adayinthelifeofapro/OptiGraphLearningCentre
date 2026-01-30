using OptimizelyLearningCentre.Client.Models.Course;

namespace OptimizelyLearningCentre.Client.Services;

/// <summary>
/// Registry for managing available courses
/// </summary>
public class CourseRegistry : ICourseRegistry
{
    private readonly Dictionary<string, CourseDefinition> _courses = new(StringComparer.OrdinalIgnoreCase);

    /// <inheritdoc />
    public IReadOnlyList<CourseDefinition> GetAllCourses()
        => _courses.Values.OrderBy(c => c.Name).ToList();

    /// <inheritdoc />
    public CourseDefinition? GetCourse(string courseId)
        => _courses.TryGetValue(courseId, out var course) ? course : null;

    /// <inheritdoc />
    public void RegisterCourse(CourseDefinition course)
    {
        _courses[course.Id] = course;
    }
}
