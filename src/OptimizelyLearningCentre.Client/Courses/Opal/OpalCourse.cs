using OptimizelyLearningCentre.Client.Courses.Opal.Components;
using OptimizelyLearningCentre.Client.Models.Course;

namespace OptimizelyLearningCentre.Client.Courses.Opal;

/// <summary>
/// Course definition for Optimizely Opal
/// </summary>
public static class OpalCourse
{
    public static CourseDefinition Definition => new()
    {
        Id = "opal",
        Name = "Optimizely Opal",
        Description = "Learn AI prompt engineering and content generation with Optimizely Opal",
        LongDescription = "Master AI-powered content creation with Optimizely Opal. Learn effective prompting techniques, content generation strategies, and how to leverage AI for your marketing workflows.",
        Icon = "sparkles",
        BrandColor = "#00D4AA",
        ContentProviderType = typeof(OpalContentProvider),
        InteractivePanelType = typeof(PromptPanel),
        NavItems = new List<CourseNavItem>
        {
            new()
            {
                Id = "home",
                Title = "Home",
                Icon = "home",
                Route = "",
                IsShared = true,
                Order = 0,
                Category = "Main"
            },
            new()
            {
                Id = "learn",
                Title = "Learn",
                Icon = "academic-cap",
                Route = "learn",
                IsShared = true,
                Order = 1,
                Category = "Main"
            },
            new()
            {
                Id = "settings",
                Title = "Settings",
                Icon = "cog-6-tooth",
                Route = "settings",
                IsShared = false,
                Order = 10,
                Category = "Settings"
            }
        },
        ExternalLinks = new List<ExternalLink>
        {
            new()
            {
                Title = "Opal Documentation",
                Url = "https://docs.developers.optimizely.com/platform-optimizely/docs/opal",
                Icon = "document-text"
            }
        }
    };
}
