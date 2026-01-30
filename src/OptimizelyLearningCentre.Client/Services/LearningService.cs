using Blazored.LocalStorage;
using OptimizelyLearningCentre.Client.Models.Learning;

namespace OptimizelyLearningCentre.Client.Services;

/// <summary>
/// Orchestrator service that delegates to course-specific content providers
/// and handles progress tracking with course-namespaced storage
/// </summary>
public class LearningService : ILearningService
{
    private readonly ILocalStorageService _localStorage;
    private readonly ICourseContext _courseContext;
    private readonly IServiceProvider _serviceProvider;

    public LearningService(
        ILocalStorageService localStorage,
        ICourseContext courseContext,
        IServiceProvider serviceProvider)
    {
        _localStorage = localStorage;
        _courseContext = courseContext;
        _serviceProvider = serviceProvider;
    }

    public async Task<List<LearningModule>> GetModulesAsync()
    {
        var provider = GetContentProvider();
        return await provider.GetModulesAsync();
    }

    public async Task<LearningModule?> GetModuleAsync(string moduleId)
    {
        var provider = GetContentProvider();
        return await provider.GetModuleAsync(moduleId);
    }

    public async Task<Lesson?> GetLessonAsync(string lessonId)
    {
        var provider = GetContentProvider();
        return await provider.GetLessonAsync(lessonId);
    }

    public async Task<UserProgress> GetUserProgressAsync()
    {
        var key = _courseContext.GetStorageKey("progress");
        try
        {
            return await _localStorage.GetItemAsync<UserProgress>(key) ?? new UserProgress();
        }
        catch
        {
            return new UserProgress();
        }
    }

    public async Task SaveUserProgressAsync(UserProgress progress)
    {
        var key = _courseContext.GetStorageKey("progress");
        await _localStorage.SetItemAsync(key, progress);
    }

    public async Task MarkLessonCompleteAsync(string lessonId)
    {
        var progress = await GetUserProgressAsync();

        progress.CompletedLessons[lessonId] = new LessonProgress
        {
            LessonId = lessonId,
            IsCompleted = true,
            CompletedDate = DateTime.UtcNow,
            ViewCount = (progress.CompletedLessons.TryGetValue(lessonId, out var existing)
                ? existing.ViewCount : 0) + 1
        };
        progress.LastActivityDate = DateTime.UtcNow;
        progress.LastAccessedLessonId = lessonId;

        await SaveUserProgressAsync(progress);
    }

    public async Task<double> GetModuleCompletionPercentageAsync(string moduleId)
    {
        var module = await GetModuleAsync(moduleId);
        if (module == null || module.Lessons.Count == 0)
            return 0;

        var progress = await GetUserProgressAsync();
        var completedCount = module.Lessons.Count(l =>
            progress.CompletedLessons.TryGetValue(l.Id, out var p) && p.IsCompleted);

        return (double)completedCount / module.Lessons.Count * 100;
    }

    public async Task<bool> IsModuleUnlockedAsync(string moduleId)
    {
        var modules = await GetModulesAsync();
        var orderedModules = modules.OrderBy(m => m.Order).ToList();

        // Find the target module
        var targetModule = orderedModules.FirstOrDefault(m => m.Id == moduleId);
        if (targetModule == null)
            return false;

        // First module is always unlocked
        var targetIndex = orderedModules.IndexOf(targetModule);
        if (targetIndex == 0)
            return true;

        // Check if the previous module is complete
        var previousModule = orderedModules[targetIndex - 1];
        return await IsModuleCompleteAsync(previousModule.Id);
    }

    public async Task<LearningModule?> GetPreviousModuleAsync(string moduleId)
    {
        var modules = await GetModulesAsync();
        var orderedModules = modules.OrderBy(m => m.Order).ToList();

        var targetModule = orderedModules.FirstOrDefault(m => m.Id == moduleId);
        if (targetModule == null)
            return null;

        var targetIndex = orderedModules.IndexOf(targetModule);
        if (targetIndex == 0)
            return null;

        return orderedModules[targetIndex - 1];
    }

    public async Task<bool> IsModuleCompleteAsync(string moduleId)
    {
        var module = await GetModuleAsync(moduleId);
        if (module == null || module.Lessons.Count == 0)
            return false;

        var progress = await GetUserProgressAsync();
        return module.Lessons.All(l =>
            progress.CompletedLessons.TryGetValue(l.Id, out var p) && p.IsCompleted);
    }

    public async Task<bool> IsLessonUnlockedAsync(string moduleId, string lessonId)
    {
        var module = await GetModuleAsync(moduleId);
        if (module == null)
            return false;

        var orderedLessons = module.Lessons.OrderBy(l => l.Order).ToList();
        var targetLesson = orderedLessons.FirstOrDefault(l => l.Id == lessonId);
        if (targetLesson == null)
            return false;

        // First lesson is always unlocked
        var targetIndex = orderedLessons.IndexOf(targetLesson);
        if (targetIndex == 0)
            return true;

        // Check if the previous lesson is complete
        var previousLesson = orderedLessons[targetIndex - 1];
        var progress = await GetUserProgressAsync();
        return progress.CompletedLessons.TryGetValue(previousLesson.Id, out var p) && p.IsCompleted;
    }

    private ILearningContentProvider GetContentProvider()
    {
        var course = _courseContext.CurrentCourse
            ?? throw new InvalidOperationException("No course context available. Ensure you are within a course.");

        return (ILearningContentProvider)_serviceProvider.GetRequiredService(course.ContentProviderType);
    }
}
