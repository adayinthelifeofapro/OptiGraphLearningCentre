using OptimizelyLearningCentre.Client.Models.Learning;

namespace OptimizelyLearningCentre.Client.Services;

/// <summary>
/// Interface for course-specific learning content providers
/// </summary>
public interface ILearningContentProvider
{
    /// <summary>
    /// Gets all learning modules for this course
    /// </summary>
    Task<List<LearningModule>> GetModulesAsync();

    /// <summary>
    /// Gets a specific module by ID
    /// </summary>
    Task<LearningModule?> GetModuleAsync(string moduleId);

    /// <summary>
    /// Gets a specific lesson by ID (searches across all modules)
    /// </summary>
    Task<Lesson?> GetLessonAsync(string lessonId);
}
