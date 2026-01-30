using OptimizelyLearningCentre.Client.Courses.SaaS.Components;
using OptimizelyLearningCentre.Client.Models.Course;

namespace OptimizelyLearningCentre.Client.Courses.SaaS;

/// <summary>
/// Course definition for Optimizely CMS (SaaS)
/// </summary>
public static class SaaSCourse
{
    public static CourseDefinition Definition => new()
    {
        Id = "saas",
        Name = "Optimizely CMS (SaaS)",
        Description = "Learn to build and manage content with Optimizely's headless CMS platform",
        LongDescription = "Master Optimizely CMS (SaaS), the powerful headless content management system. Learn content modeling, Visual Builder, REST API integration, and how to deliver content through Optimizely Graph.",
        Icon = "cloud",
        BrandColor = "#6366F1",
        ContentProviderType = typeof(SaaSContentProvider),
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
                Title = "SaaS Documentation",
                Url = "https://docs.developers.optimizely.com/content-management-system/v1.0.0-CMS-SaaS/docs/overview-saas",
                Icon = "document-text"
            },
            new()
            {
                Title = "Developer Community",
                Url = "https://world.optimizely.com/products/cms/saas/",
                Icon = "users"
            }
        }
    };
}
