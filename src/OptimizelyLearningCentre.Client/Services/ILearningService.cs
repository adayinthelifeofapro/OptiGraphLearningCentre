using OptimizelyLearningCentre.Client.Models.Learning;

namespace OptimizelyLearningCentre.Client.Services;

/// <summary>
/// Service for managing learning content and progress
/// </summary>
public interface ILearningService
{
    /// <summary>
    /// Gets all available learning modules
    /// </summary>
    Task<List<LearningModule>> GetModulesAsync();

    /// <summary>
    /// Gets a specific module by ID
    /// </summary>
    Task<LearningModule?> GetModuleAsync(string moduleId);

    /// <summary>
    /// Gets a specific lesson by ID
    /// </summary>
    Task<Lesson?> GetLessonAsync(string lessonId);

    /// <summary>
    /// Gets the user's learning progress
    /// </summary>
    Task<UserProgress> GetUserProgressAsync();

    /// <summary>
    /// Saves user progress
    /// </summary>
    Task SaveUserProgressAsync(UserProgress progress);

    /// <summary>
    /// Marks a lesson as complete
    /// </summary>
    Task MarkLessonCompleteAsync(string lessonId);

    /// <summary>
    /// Gets completion percentage for a module
    /// </summary>
    Task<double> GetModuleCompletionPercentageAsync(string moduleId);

    /// <summary>
    /// Checks if a module is unlocked (previous module is complete)
    /// </summary>
    Task<bool> IsModuleUnlockedAsync(string moduleId);

    /// <summary>
    /// Gets the previous module in sequence (by order)
    /// </summary>
    Task<LearningModule?> GetPreviousModuleAsync(string moduleId);

    /// <summary>
    /// Checks if a module is complete (all lessons completed)
    /// </summary>
    Task<bool> IsModuleCompleteAsync(string moduleId);

    /// <summary>
    /// Checks if a lesson is unlocked (previous lesson is complete or it's the first lesson)
    /// </summary>
    Task<bool> IsLessonUnlockedAsync(string moduleId, string lessonId);
}
