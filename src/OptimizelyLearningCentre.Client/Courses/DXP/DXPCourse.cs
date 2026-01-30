using OptimizelyLearningCentre.Client.Courses.DXP.Components;
using OptimizelyLearningCentre.Client.Models.Course;

namespace OptimizelyLearningCentre.Client.Courses.DXP;

/// <summary>
/// Course definition for Optimizely DXP (Digital Experience Platform)
/// </summary>
public static class DXPCourse
{
    public static CourseDefinition Definition => new()
    {
        Id = "dxp",
        Name = "Optimizely DXP",
        Description = "Learn to deploy and manage solutions on Optimizely's Digital Experience Platform",
        LongDescription = "Master Optimizely DXP (Digital Experience Platform), the enterprise cloud hosting solution built on Microsoft Azure. Learn about environments, deployments, monitoring, security, and best practices for running Optimizely solutions in the cloud.",
        Icon = "server-stack",
        BrandColor = "#0EA5E9",
        ContentProviderType = typeof(DXPContentProvider),
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
                Title = "DXP Documentation",
                Url = "https://docs.developers.optimizely.com/digital-experience-platform/docs/dxp-overview",
                Icon = "document-text"
            },
            new()
            {
                Title = "DXP Management Portal",
                Url = "https://paasportal.episerver.net/",
                Icon = "server"
            },
            new()
            {
                Title = "Developer Community",
                Url = "https://world.optimizely.com/",
                Icon = "users"
            }
        }
    };
}
