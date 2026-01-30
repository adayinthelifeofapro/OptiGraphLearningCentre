using OptimizelyLearningCentre.Client.Courses.Graph.Components;
using OptimizelyLearningCentre.Client.Models.Course;

namespace OptimizelyLearningCentre.Client.Courses.Graph;

/// <summary>
/// Course definition for Optimizely Graph
/// </summary>
public static class GraphCourse
{
    public static CourseDefinition Definition => new()
    {
        Id = "graph",
        Name = "Optimizely Graph",
        Description = "Master GraphQL queries with Optimizely Graph - the powerful content delivery API",
        LongDescription = "Learn to build efficient, flexible content queries using Optimizely Graph. From basic queries to advanced filtering, sorting, and full-text search.",
        Icon = "bolt",
        BrandColor = "#0037FF",
        ContentProviderType = typeof(GraphContentProvider),
        InteractivePanelType = typeof(TryItPanel),
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
                Id = "query-builder",
                Title = "Query Builder",
                Icon = "cube",
                Route = "query-builder",
                IsShared = false,
                Order = 2,
                Category = "Tools"
            },
            new()
            {
                Id = "playground",
                Title = "Playground",
                Icon = "code-bracket",
                Route = "playground",
                IsShared = false,
                Order = 3,
                Category = "Tools"
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
                Title = "Graph Documentation",
                Url = "https://docs.developers.optimizely.com/platform-optimizely/docs/optimizely-graph",
                Icon = "document-text"
            },
            new()
            {
                Title = "GraphQL Playground",
                Url = "https://cg.optimizely.com/app/graphiql",
                Icon = "play"
            }
        }
    };
}
