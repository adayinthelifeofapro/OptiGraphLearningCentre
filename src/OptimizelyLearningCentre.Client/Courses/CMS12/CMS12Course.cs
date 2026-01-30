using OptimizelyLearningCentre.Client.Courses.SaaS.Components;
using OptimizelyLearningCentre.Client.Models.Course;

namespace OptimizelyLearningCentre.Client.Courses.CMS12;

/// <summary>
/// Course definition for Optimizely CMS 12 (PaaS)
/// </summary>
public static class CMS12Course
{
    public static CourseDefinition Definition => new()
    {
        Id = "cms12",
        Name = "Optimizely CMS 12 (PaaS)",
        Description = "Master development with Optimizely CMS 12, the self-hosted .NET CMS platform",
        LongDescription = "Learn to build powerful websites with Optimizely CMS 12. Master content types, MVC rendering, content management APIs, localization, search, forms, security, caching, and deployment to the Optimizely DXP cloud platform.",
        Icon = "server",
        BrandColor = "#059669",
        ContentProviderType = typeof(CMS12ContentProvider),
        InteractivePanelType = typeof(CodePanel),
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
                Title = "CMS 12 Documentation",
                Url = "https://docs.developers.optimizely.com/content-management-system/docs/getting-started",
                Icon = "document-text"
            },
            new()
            {
                Title = "Developer Community",
                Url = "https://world.optimizely.com/",
                Icon = "users"
            },
            new()
            {
                Title = "Optimizely Academy",
                Url = "https://academy.optimizely.com/",
                Icon = "academic-cap"
            }
        }
    };
}
