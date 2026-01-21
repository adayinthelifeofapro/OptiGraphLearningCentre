using Blazored.LocalStorage;
using OptiGraphLearningCentre.Client.Models.Learning;

namespace OptiGraphLearningCentre.Client.Services;

/// <summary>
/// Service providing learning content and tracking progress
/// </summary>
public class LearningService : ILearningService
{
    private const string ProgressKey = "optigraph_progress";
    private readonly ILocalStorageService _localStorage;
    private List<LearningModule>? _modulesCache;

    public LearningService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public Task<List<LearningModule>> GetModulesAsync()
    {
        _modulesCache ??= BuildModules();
        return Task.FromResult(_modulesCache);
    }

    public async Task<LearningModule?> GetModuleAsync(string moduleId)
    {
        var modules = await GetModulesAsync();
        return modules.FirstOrDefault(m => m.Id == moduleId);
    }

    public async Task<Lesson?> GetLessonAsync(string lessonId)
    {
        var modules = await GetModulesAsync();
        return modules
            .SelectMany(m => m.Lessons)
            .FirstOrDefault(l => l.Id == lessonId);
    }

    public async Task<UserProgress> GetUserProgressAsync()
    {
        try
        {
            return await _localStorage.GetItemAsync<UserProgress>(ProgressKey) ?? new UserProgress();
        }
        catch
        {
            return new UserProgress();
        }
    }

    public async Task SaveUserProgressAsync(UserProgress progress)
    {
        await _localStorage.SetItemAsync(ProgressKey, progress);
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

    private static List<LearningModule> BuildModules()
    {
        var modules = new List<LearningModule>
        {
            BuildGettingStartedModule(),
            BuildFilteringModule(),
            BuildSortingModule(),
            BuildPaginationModule(),
            BuildSearchModule(),
            BuildFacetsModule(),
            BuildFragmentsModule(),
            BuildContentRelationshipsModule(),
            BuildRecursiveQueriesModule(),
            BuildContentVariationsModule(),
            BuildAdvancedModule(),
            BuildMigrationModule()
        };

        // Link lessons
        foreach (var module in modules)
        {
            for (var i = 0; i < module.Lessons.Count; i++)
            {
                if (i > 0)
                    module.Lessons[i].PreviousLessonId = module.Lessons[i - 1].Id;
                if (i < module.Lessons.Count - 1)
                    module.Lessons[i].NextLessonId = module.Lessons[i + 1].Id;
            }
        }

        return modules;
    }

    private static LearningModule BuildGettingStartedModule()
    {
        return new LearningModule
        {
            Id = "getting-started",
            Title = "Getting Started",
            Description = "Learn the fundamentals of Optimizely Graph and write your first GraphQL queries",
            Icon = "academic-cap",
            Order = 1,
            Difficulty = ModuleDifficulty.Beginner,
            Lessons = new List<Lesson>
            {
                new()
                {
                    Id = "intro-to-graph",
                    ModuleId = "getting-started",
                    Title = "Introduction to Optimizely Graph",
                    Summary = "Understand what Optimizely Graph is and how it powers content delivery",
                    Order = 1,
                    EstimatedMinutes = 5,
                    LearningObjectives = new List<string>
                    {
                        "Understand the purpose of Optimizely Graph",
                        "Learn about GraphQL vs REST APIs",
                        "Understand the Optimizely Graph architecture"
                    },
                    Content = @"<h3>What is Optimizely Graph?</h3>
<p>Optimizely Graph is a powerful GraphQL-based content delivery API that provides fast, flexible access to your CMS content. It enables headless content delivery with:</p>
<ul>
<li><strong>Real-time content synchronization</strong> from Optimizely CMS</li>
<li><strong>Powerful querying</strong> with filtering, sorting, and full-text search</li>
<li><strong>Schema-driven development</strong> with automatic type generation</li>
<li><strong>CDN-backed performance</strong> for global content delivery</li>
</ul>

<h3>Why GraphQL?</h3>
<p>GraphQL offers several advantages over traditional REST APIs:</p>
<ul>
<li><strong>Request exactly what you need</strong> - no over-fetching or under-fetching</li>
<li><strong>Single endpoint</strong> - all data from one request</li>
<li><strong>Strongly typed schema</strong> - self-documenting API</li>
<li><strong>Introspection</strong> - explore available types and fields</li>
</ul>",
                    Examples = new List<LessonExample>
                    {
                        new()
                        {
                            Id = "intro-example-1",
                            Title = "Schema Introspection",
                            Description = "Query the GraphQL schema to see available types",
                            GraphQLQuery = @"{
  __schema {
    queryType {
      name
    }
    types {
      name
      kind
    }
  }
}",
                            IsInteractive = true,
                            Hints = new List<string>
                            {
                                "The __schema field is a special GraphQL introspection field",
                                "This query shows all types defined in the schema"
                            }
                        }
                    }
                },
                new()
                {
                    Id = "first-query",
                    ModuleId = "getting-started",
                    Title = "Your First Query",
                    Summary = "Write and execute your first GraphQL query against Optimizely Graph",
                    Order = 2,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Write a basic GraphQL query",
                        "Understand query structure and syntax",
                        "Select specific fields from content"
                    },
                    Content = @"<h3>Query Structure</h3>
<p>Every GraphQL query follows a simple structure:</p>
<pre class='code-block'>{
  ContentType {
    items {
      field1
      field2
    }
  }
}</pre>

<h3>Key Concepts</h3>
<ul>
<li><strong>Content Type</strong> - The type of content to query (e.g., BlogPost, StandardPage)</li>
<li><strong>items</strong> - Returns the array of matching content items</li>
<li><strong>Fields</strong> - The specific properties you want returned</li>
</ul>

<h3>Discovering Available Fields</h3>
<p>Use introspection to discover what fields are available on your content types. Run the example below to see the fields on the Content type.</p>",
                    Examples = new List<LessonExample>
                    {
                        new()
                        {
                            Id = "first-query-discover",
                            Title = "Discover Content Fields",
                            Description = "Use introspection to see available fields on Content",
                            GraphQLQuery = @"{
  __type(name: ""Content"") {
    name
    fields {
      name
      type {
        name
        kind
      }
    }
  }
}",
                            IsInteractive = true,
                            Hints = new List<string>
                            {
                                "This introspection query shows all fields available on Content",
                                "Look for fields like Name, ContentType, Url, Language"
                            }
                        },
                        new()
                        {
                            Id = "first-query-1",
                            Title = "Basic Content Query",
                            Description = "Query content items (adjust field names based on your schema)",
                            GraphQLQuery = @"{
  Content {
    items {
      Name
      ContentType
      Url
      Language {
        Name
      }
    }
    total
  }
}",
                            IsInteractive = true,
                            Hints = new List<string>
                            {
                                "Content is a base type that matches all content",
                                "If you get errors, use the introspection query above to find correct field names"
                            }
                        }
                    }
                },
                new()
                {
                    Id = "authentication",
                    ModuleId = "getting-started",
                    Title = "Authentication",
                    Summary = "Learn about authentication methods for Optimizely Graph",
                    Order = 3,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Understand SingleKey authentication",
                        "Understand HMAC authentication",
                        "Know when to use each authentication method"
                    },
                    Content = @"<h3>Authentication Methods</h3>
<p>Optimizely Graph supports two main authentication methods:</p>

<h4>1. Single Key Authentication</h4>
<p>Used for public content queries. The single key is sent in the Authorization header:</p>
<pre class='code-block'>Authorization: epi-single YOUR_SINGLE_KEY</pre>
<p>Best for: Published content, public websites, static site generation</p>

<h4>2. HMAC Authentication</h4>
<p>Used for accessing draft content and administrative operations. Requires AppKey and Secret:</p>
<pre class='code-block'>Authorization: epi-hmac APP_KEY:TIMESTAMP:NONCE:SIGNATURE</pre>
<p>Best for: Preview environments, content management integrations</p>

<h3>Configuring Credentials</h3>
<p>Configure your credentials in the Settings page of this learning centre to test queries against your own Optimizely Graph instance.</p>",
                    Examples = new List<LessonExample>()
                }
            }
        };
    }

    private static LearningModule BuildFilteringModule()
    {
        return new LearningModule
        {
            Id = "filtering",
            Title = "Filtering Data",
            Description = "Master the powerful filtering capabilities with where clauses and operators",
            Icon = "funnel",
            Order = 2,
            Difficulty = ModuleDifficulty.Beginner,
            Prerequisites = new[] { "getting-started" },
            Lessons = new List<Lesson>
            {
                new()
                {
                    Id = "basic-filters",
                    ModuleId = "filtering",
                    Title = "Basic Filters",
                    Summary = "Learn to filter content using equality and comparison operators",
                    Order = 1,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Use the where clause to filter results",
                        "Apply equality filters with eq operator",
                        "Use comparison operators (gt, lt, gte, lte)"
                    },
                    Content = @"<h3>The Where Clause</h3>
<p>The <code>where</code> parameter filters results based on field values:</p>
<pre class='code-block'>{
  BlogPost(where: { Status: { eq: ""Published"" } }) {
    items { Title }
  }
}</pre>

<h3>Available Operators</h3>
<table class='w-full text-sm'>
<tr><th class='text-left p-2 border-b'>Operator</th><th class='text-left p-2 border-b'>Description</th><th class='text-left p-2 border-b'>Types</th></tr>
<tr><td class='p-2 border-b'><code>eq</code></td><td class='p-2 border-b'>Equals</td><td class='p-2 border-b'>All</td></tr>
<tr><td class='p-2 border-b'><code>notEq</code></td><td class='p-2 border-b'>Not equals</td><td class='p-2 border-b'>All</td></tr>
<tr><td class='p-2 border-b'><code>gt</code></td><td class='p-2 border-b'>Greater than</td><td class='p-2 border-b'>Number, Date</td></tr>
<tr><td class='p-2 border-b'><code>gte</code></td><td class='p-2 border-b'>Greater than or equal</td><td class='p-2 border-b'>Number, Date</td></tr>
<tr><td class='p-2 border-b'><code>lt</code></td><td class='p-2 border-b'>Less than</td><td class='p-2 border-b'>Number, Date</td></tr>
<tr><td class='p-2 border-b'><code>lte</code></td><td class='p-2 border-b'>Less than or equal</td><td class='p-2 border-b'>Number, Date</td></tr>
</table>",
                    Examples = new List<LessonExample>
                    {
                        new()
                        {
                            Id = "filter-discover",
                            Title = "Discover Filterable Fields",
                            Description = "Find fields you can filter on using introspection",
                            GraphQLQuery = @"{
  __type(name: ""ContentWhereInput"") {
    name
    inputFields {
      name
      type {
        name
      }
    }
  }
}",
                            IsInteractive = true,
                            Hints = new List<string>
                            {
                                "This shows all fields available for filtering",
                                "Use these field names in your where clause"
                            }
                        },
                        new()
                        {
                            Id = "filter-eq",
                            Title = "Equality Filter",
                            Description = "Filter content by content type",
                            GraphQLQuery = @"{
  Content(
    where: { ContentType: { eq: ""Page"" } }
    limit: 5
  ) {
    items {
      Name
      ContentType
    }
    total
  }
}",
                            IsInteractive = true,
                            Hints = new List<string>
                            {
                                "The eq operator matches exact values",
                                "Adjust field names based on your schema (use introspection above)"
                            }
                        }
                    }
                },
                new()
                {
                    Id = "string-filters",
                    ModuleId = "filtering",
                    Title = "String Filters",
                    Summary = "Use string-specific operators like contains, startsWith, and endsWith",
                    Order = 2,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Use contains for partial text matching",
                        "Use startsWith and endsWith operators",
                        "Use the like operator with patterns"
                    },
                    Content = @"<h3>String Operators</h3>
<table class='w-full text-sm'>
<tr><th class='text-left p-2 border-b'>Operator</th><th class='text-left p-2 border-b'>Description</th></tr>
<tr><td class='p-2 border-b'><code>contains</code></td><td class='p-2 border-b'>Text contains the value (full-text search)</td></tr>
<tr><td class='p-2 border-b'><code>startsWith</code></td><td class='p-2 border-b'>Text starts with value</td></tr>
<tr><td class='p-2 border-b'><code>endsWith</code></td><td class='p-2 border-b'>Text ends with value</td></tr>
<tr><td class='p-2 border-b'><code>like</code></td><td class='p-2 border-b'>Pattern matching with wildcards</td></tr>
</table>

<h3>Pattern Matching with Like</h3>
<p>The <code>like</code> operator supports wildcards:</p>
<ul>
<li><code>%</code> - Matches any sequence of characters</li>
<li><code>_</code> - Matches any single character</li>
</ul>",
                    Examples = new List<LessonExample>
                    {
                        new()
                        {
                            Id = "filter-contains",
                            Title = "Full-Text Search",
                            Description = "Search for content containing specific text",
                            GraphQLQuery = @"{
  Content(
    where: { _fulltext: { match: ""product"" } }
    limit: 5
  ) {
    items {
      Name
      ContentType
    }
    total
  }
}",
                            IsInteractive = true,
                            Hints = new List<string>
                            {
                                "_fulltext searches across all indexed text content",
                                "Try 'match' if 'contains' doesn't work in your schema"
                            }
                        }
                    }
                },
                new()
                {
                    Id = "combining-filters",
                    ModuleId = "filtering",
                    Title = "Combining Filters",
                    Summary = "Combine multiple filters with AND, OR, and NOT logic",
                    Order = 3,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Combine filters with implicit AND",
                        "Use _or for OR conditions",
                        "Use _not for negation"
                    },
                    Content = @"<h3>Implicit AND</h3>
<p>Multiple conditions in where are combined with AND by default:</p>
<pre class='code-block'>where: {
  Status: { eq: ""Published"" }
  Category: { eq: ""News"" }
}</pre>

<h3>Using _or</h3>
<p>Use <code>_or</code> to match any of multiple conditions:</p>
<pre class='code-block'>where: {
  _or: [
    { Category: { eq: ""News"" } }
    { Category: { eq: ""Blog"" } }
  ]
}</pre>

<h3>Using _not</h3>
<p>Use <code>_not</code> to negate conditions:</p>
<pre class='code-block'>where: {
  _not: { Status: { eq: ""Draft"" } }
}</pre>",
                    Examples = new List<LessonExample>
                    {
                        new()
                        {
                            Id = "filter-combined",
                            Title = "Combined Filters",
                            Description = "Combine multiple filter conditions",
                            GraphQLQuery = @"{
  Content(
    where: {
      ContentType: { eq: ""Page"" }
      _fulltext: { contains: ""welcome"" }
    }
    limit: 5
  ) {
    items {
      Name
      ContentType
    }
    total
  }
}",
                            IsInteractive = true
                        }
                    }
                }
            }
        };
    }

    private static LearningModule BuildSortingModule()
    {
        return new LearningModule
        {
            Id = "sorting",
            Title = "Sorting Results",
            Description = "Learn to order your query results by fields and relevance",
            Icon = "arrows-up-down",
            Order = 3,
            Difficulty = ModuleDifficulty.Beginner,
            Prerequisites = new[] { "getting-started" },
            Lessons = new List<Lesson>
            {
                new()
                {
                    Id = "basic-sorting",
                    ModuleId = "sorting",
                    Title = "Basic Sorting",
                    Summary = "Sort results by field values in ascending or descending order",
                    Order = 1,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Use orderBy to sort results",
                        "Sort in ascending (ASC) or descending (DESC) order",
                        "Sort by multiple fields"
                    },
                    Content = @"<h3>The orderBy Parameter</h3>
<p>Use <code>orderBy</code> to sort query results:</p>
<pre class='code-block'>{
  BlogPost(orderBy: { PublishedDate: DESC }) {
    items { Title PublishedDate }
  }
}</pre>

<h3>Sort Directions</h3>
<ul>
<li><code>ASC</code> - Ascending order (A-Z, 0-9, oldest first)</li>
<li><code>DESC</code> - Descending order (Z-A, 9-0, newest first)</li>
</ul>

<h3>Multiple Sort Fields</h3>
<p>Sort by multiple fields with tie-breakers:</p>
<pre class='code-block'>orderBy: { Category: ASC, PublishedDate: DESC }</pre>",
                    Examples = new List<LessonExample>
                    {
                        new()
                        {
                            Id = "sort-basic",
                            Title = "Sort by Date",
                            Description = "Sort content by modification date",
                            GraphQLQuery = @"{
  Content(
    orderBy: { Name: ASC }
    limit: 10
  ) {
    items {
      Name
      _id
    }
  }
}",
                            IsInteractive = true
                        }
                    }
                },
                new()
                {
                    Id = "relevance-sorting",
                    ModuleId = "sorting",
                    Title = "Relevance Sorting",
                    Summary = "Sort search results by relevance score",
                    Order = 2,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Understand relevance scoring",
                        "Use _ranking: RELEVANCE",
                        "Combine relevance with other sort fields"
                    },
                    Content = @"<h3>Relevance Ranking</h3>
<p>When performing searches, sort by relevance to show best matches first:</p>
<pre class='code-block'>orderBy: { _ranking: RELEVANCE }</pre>

<h3>How Relevance Works</h3>
<p>Optimizely Graph uses BM25 algorithm for relevance scoring, considering:</p>
<ul>
<li>Term frequency in the document</li>
<li>Document length normalization</li>
<li>Field importance weighting</li>
</ul>

<h3>Boosting Results</h3>
<p>Use the <code>boost</code> operator to influence relevance scores.</p>",
                    Examples = new List<LessonExample>
                    {
                        new()
                        {
                            Id = "sort-relevance",
                            Title = "Search with Relevance",
                            Description = "Search and sort by relevance score",
                            GraphQLQuery = @"{
  Content(
    where: { _fulltext: { contains: ""content"" } }
    orderBy: { _ranking: RELEVANCE }
    limit: 5
  ) {
    items {
      Name
      _score
    }
  }
}",
                            IsInteractive = true,
                            Hints = new List<string>
                            {
                                "_ranking: RELEVANCE sorts by search score",
                                "_score shows the actual relevance value"
                            }
                        }
                    }
                }
            }
        };
    }

    private static LearningModule BuildPaginationModule()
    {
        return new LearningModule
        {
            Id = "pagination",
            Title = "Pagination",
            Description = "Handle large result sets with skip/limit and cursor-based pagination",
            Icon = "document-duplicate",
            Order = 4,
            Difficulty = ModuleDifficulty.Intermediate,
            Prerequisites = new[] { "getting-started" },
            Lessons = new List<Lesson>
            {
                new()
                {
                    Id = "skip-limit",
                    ModuleId = "pagination",
                    Title = "Skip and Limit",
                    Summary = "Use skip and limit for offset-based pagination",
                    Order = 1,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Use limit to control result count",
                        "Use skip for offset pagination",
                        "Understand total count"
                    },
                    Content = @"<h3>Offset Pagination</h3>
<p>Use <code>skip</code> and <code>limit</code> for traditional pagination:</p>
<pre class='code-block'>{
  BlogPost(skip: 10, limit: 10) {
    items { Title }
    total
  }
}</pre>

<h3>Parameters</h3>
<ul>
<li><code>limit</code> - Maximum number of items to return (default: 10, max: 100)</li>
<li><code>skip</code> - Number of items to skip from the beginning</li>
<li><code>total</code> - Total count of matching items</li>
</ul>

<h3>Calculating Pages</h3>
<pre class='code-block'>Page 1: skip: 0, limit: 10
Page 2: skip: 10, limit: 10
Page 3: skip: 20, limit: 10
...</pre>",
                    Examples = new List<LessonExample>
                    {
                        new()
                        {
                            Id = "pagination-offset",
                            Title = "Offset Pagination",
                            Description = "Page through results with skip and limit",
                            GraphQLQuery = @"{
  Content(
    skip: 0
    limit: 5
  ) {
    items {
      Name
      ContentType
    }
    total
  }
}",
                            IsInteractive = true,
                            Hints = new List<string>
                            {
                                "Change skip to 5 to get the next page",
                                "total shows the full count of matching items"
                            }
                        }
                    }
                },
                new()
                {
                    Id = "cursor-pagination",
                    ModuleId = "pagination",
                    Title = "Cursor Pagination",
                    Summary = "Use cursor-based pagination for efficient navigation",
                    Order = 2,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Understand cursor-based pagination",
                        "Use cursor parameter for navigation",
                        "Know when to use cursor vs offset"
                    },
                    Content = @"<h3>Cursor-Based Pagination</h3>
<p>Cursors provide more efficient pagination for large datasets:</p>
<pre class='code-block'>{
  BlogPost(limit: 10) {
    items { Title }
    cursor
  }
}</pre>

<h3>Using the Cursor</h3>
<p>Pass the returned cursor to get the next page:</p>
<pre class='code-block'>{
  BlogPost(limit: 10, cursor: ""abc123"") {
    items { Title }
    cursor
  }
}</pre>

<h3>Cursor vs Offset</h3>
<table class='w-full text-sm'>
<tr><th class='text-left p-2 border-b'>Feature</th><th class='text-left p-2 border-b'>Cursor</th><th class='text-left p-2 border-b'>Offset</th></tr>
<tr><td class='p-2 border-b'>Performance</td><td class='p-2 border-b'>Better for large datasets</td><td class='p-2 border-b'>Degrades with high skip values</td></tr>
<tr><td class='p-2 border-b'>Jump to page</td><td class='p-2 border-b'>Not supported</td><td class='p-2 border-b'>Supported</td></tr>
<tr><td class='p-2 border-b'>Consistency</td><td class='p-2 border-b'>More consistent if data changes</td><td class='p-2 border-b'>May skip/duplicate on changes</td></tr>
</table>",
                    Examples = new List<LessonExample>
                    {
                        new()
                        {
                            Id = "pagination-cursor",
                            Title = "Cursor Pagination",
                            Description = "Use cursor for efficient pagination",
                            GraphQLQuery = @"{
  Content(limit: 3) {
    items {
      Name
      ContentType
    }
    cursor
    total
  }
}",
                            IsInteractive = true,
                            Hints = new List<string>
                            {
                                "Copy the cursor value and use it in the next query",
                                "An empty cursor means no more results"
                            }
                        }
                    }
                }
            }
        };
    }

    private static LearningModule BuildSearchModule()
    {
        return new LearningModule
        {
            Id = "search",
            Title = "Site Search",
            Description = "Implement powerful site search with full-text, fuzzy, semantic search and highlighting",
            Icon = "magnifying-glass",
            Order = 5,
            Difficulty = ModuleDifficulty.Intermediate,
            Prerequisites = new[] { "filtering" },
            Lessons = new List<Lesson>
            {
                new()
                {
                    Id = "fulltext-search",
                    ModuleId = "search",
                    Title = "Full-Text Search",
                    Summary = "Search across all text content with the _fulltext field",
                    Order = 1,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Use _fulltext for searching all text content",
                        "Understand search scoring",
                        "Use searchTerm parameter"
                    },
                    Content = @"<h3>Searching Content</h3>
<p>Optimizely Graph provides powerful full-text search capabilities:</p>

<h4>Using _fulltext</h4>
<pre class='code-block'>where: { _fulltext: { contains: ""search term"" } }</pre>

<h4>Using searchTerm</h4>
<pre class='code-block'>BlogPost(searchTerm: ""search term"") {
  items { ... }
}</pre>

<h3>Search Features</h3>
<ul>
<li>Multi-language support</li>
<li>Relevance scoring with BM25</li>
<li>Field-specific boosting</li>
<li>Synonym expansion</li>
</ul>",
                    Examples = new List<LessonExample>
                    {
                        new()
                        {
                            Id = "search-fulltext",
                            Title = "Full-Text Search",
                            Description = "Search across all text fields",
                            GraphQLQuery = @"{
  Content(
    where: { _fulltext: { contains: ""optimizely"" } }
    orderBy: { _ranking: RELEVANCE }
    limit: 5
  ) {
    items {
      Name
      ContentType
      _score
    }
    total
  }
}",
                            IsInteractive = true
                        }
                    }
                },
                new()
                {
                    Id = "fuzzy-search",
                    ModuleId = "search",
                    Title = "Fuzzy Search",
                    Summary = "Handle typos and misspellings with fuzzy matching",
                    Order = 2,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Understand fuzzy matching concepts",
                        "Use fuzzy search operators",
                        "Configure fuzziness levels"
                    },
                    Content = @"<h3>What is Fuzzy Search?</h3>
<p>Fuzzy search finds matches even when the search term contains typos or slight variations. This improves user experience by being forgiving of spelling mistakes.</p>

<h3>How Fuzzy Matching Works</h3>
<p>Fuzzy search uses edit distance (Levenshtein distance) to find similar terms:</p>
<ul>
<li><strong>Edit distance 1</strong>: ""optimzely"" matches ""optimizely"" (missing 'i')</li>
<li><strong>Edit distance 2</strong>: ""optmzely"" matches ""optimizely"" (missing 'i' and 'i')</li>
</ul>

<h3>Fuzzy Search Syntax</h3>
<p>Use the <code>fuzzy</code> operator for fuzzy matching:</p>
<pre class='code-block'>where: { _fulltext: { fuzzy: ""optmizely"" } }</pre>

<h3>Controlling Fuzziness</h3>
<p>You can control the fuzziness level:</p>
<ul>
<li><strong>AUTO</strong> - Automatically determines fuzziness based on term length</li>
<li><strong>0</strong> - Exact match only (no fuzziness)</li>
<li><strong>1</strong> - Allow 1 character difference</li>
<li><strong>2</strong> - Allow 2 character differences</li>
</ul>

<h3>Best Practices</h3>
<ul>
<li>Use fuzzy search for user-facing search boxes</li>
<li>Combine with relevance sorting for best results</li>
<li>Consider performance - fuzzy searches are more expensive</li>
<li>Use AUTO fuzziness for balanced accuracy and tolerance</li>
</ul>",
                    Examples = new List<LessonExample>
                    {
                        new()
                        {
                            Id = "fuzzy-search-example",
                            Title = "Fuzzy Search Example",
                            Description = "Search with typo tolerance using fuzzy matching",
                            GraphQLQuery = @"{
  Content(
    where: { _fulltext: { fuzzy: ""optmizely"" } }
    orderBy: { _ranking: RELEVANCE }
    limit: 5
  ) {
    items {
      Name
      ContentType
      _score
    }
    total
  }
}",
                            IsInteractive = true,
                            Hints = new List<string>
                            {
                                "Try intentionally misspelling search terms",
                                "Fuzzy search will still find relevant content"
                            }
                        }
                    }
                },
                new()
                {
                    Id = "semantic-search",
                    ModuleId = "search",
                    Title = "Semantic Search",
                    Summary = "Search by meaning using AI-powered semantic understanding",
                    Order = 3,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Understand semantic search concepts",
                        "Use semantic search operators",
                        "Combine semantic with keyword search"
                    },
                    Content = @"<h3>What is Semantic Search?</h3>
<p>Semantic search goes beyond keyword matching to understand the <em>meaning</em> of your search query. It uses AI embeddings to find conceptually related content.</p>

<h3>Keyword vs Semantic Search</h3>
<table class='w-full text-sm mb-4'>
<tr><th class='text-left p-2 border-b'>Feature</th><th class='text-left p-2 border-b'>Keyword</th><th class='text-left p-2 border-b'>Semantic</th></tr>
<tr><td class='p-2 border-b'>Matches</td><td class='p-2 border-b'>Exact terms</td><td class='p-2 border-b'>Concepts & meaning</td></tr>
<tr><td class='p-2 border-b'>""automobile""</td><td class='p-2 border-b'>Only ""automobile""</td><td class='p-2 border-b'>Also ""car"", ""vehicle""</td></tr>
<tr><td class='p-2 border-b'>""happy""</td><td class='p-2 border-b'>Only ""happy""</td><td class='p-2 border-b'>Also ""joyful"", ""pleased""</td></tr>
</table>

<h3>Semantic Search Syntax</h3>
<p>Use the <code>semantic</code> operator for meaning-based search:</p>
<pre class='code-block'>where: { _fulltext: { semantic: ""content management solutions"" } }</pre>

<h3>Hybrid Search</h3>
<p>Combine keyword and semantic search for best results:</p>
<pre class='code-block'>where: {
  _or: [
    { _fulltext: { contains: ""CMS"" } }
    { _fulltext: { semantic: ""content management"" } }
  ]
}</pre>

<h3>When to Use Semantic Search</h3>
<ul>
<li><strong>Natural language queries</strong> - When users search conversationally</li>
<li><strong>Concept exploration</strong> - Finding related content</li>
<li><strong>Multilingual content</strong> - Meaning transcends exact words</li>
<li><strong>Synonym handling</strong> - Automatically matches related terms</li>
</ul>

<h3>Considerations</h3>
<ul>
<li>Semantic search requires additional indexing configuration</li>
<li>May have slightly higher latency than keyword search</li>
<li>Works best with descriptive, natural language queries</li>
</ul>",
                    Examples = new List<LessonExample>
                    {
                        new()
                        {
                            Id = "semantic-search-example",
                            Title = "Semantic Search Example",
                            Description = "Search by meaning rather than exact keywords",
                            GraphQLQuery = @"{
  Content(
    where: { _fulltext: { semantic: ""website content management"" } }
    orderBy: { _ranking: RELEVANCE }
    limit: 5
  ) {
    items {
      Name
      ContentType
      _score
    }
    total
  }
}",
                            IsInteractive = true,
                            Hints = new List<string>
                            {
                                "Semantic search finds conceptually related content",
                                "Try different phrasings of the same concept"
                            }
                        }
                    }
                },
                new()
                {
                    Id = "search-highlight",
                    ModuleId = "search",
                    Title = "Search Highlighting",
                    Summary = "Highlight matching terms in search results",
                    Order = 4,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Enable search result highlighting",
                        "Customize highlight markup",
                        "Display highlighted snippets"
                    },
                    Content = @"<h3>What is Search Highlighting?</h3>
<p>Highlighting marks the matching terms in search results, helping users quickly see why a result matched their query.</p>

<h3>Enabling Highlighting</h3>
<p>Use the <code>highlight</code> field to get highlighted snippets:</p>
<pre class='code-block'>{
  Content(
    where: { _fulltext: { contains: ""optimizely"" } }
  ) {
    items {
      Name
      _highlight {
        _fulltext
      }
    }
  }
}</pre>

<h3>Highlight Output</h3>
<p>Highlighted results contain HTML markup around matching terms:</p>
<pre class='code-block'>{
  ""_highlight"": {
    ""_fulltext"": ""Learn about &lt;em&gt;Optimizely&lt;/em&gt; Graph and how to...""
  }
}</pre>

<h3>Customizing Highlights</h3>
<p>You can customize the highlight markup:</p>
<ul>
<li><strong>Pre/Post tags</strong> - Custom HTML tags around matches</li>
<li><strong>Fragment size</strong> - Length of highlighted snippets</li>
<li><strong>Number of fragments</strong> - How many snippets to return</li>
</ul>

<h3>Displaying Highlights</h3>
<p>When displaying highlighted content:</p>
<ul>
<li>Render the HTML safely (the markup is from your search engine)</li>
<li>Style the <code>&lt;em&gt;</code> tags or custom tags appropriately</li>
<li>Fall back to regular content if no highlight available</li>
</ul>

<h3>Best Practices</h3>
<ul>
<li>Always show context around the matched term</li>
<li>Use CSS to make highlights visually distinct</li>
<li>Consider accessibility - don't rely on color alone</li>
<li>Test with various query lengths and content types</li>
</ul>",
                    Examples = new List<LessonExample>
                    {
                        new()
                        {
                            Id = "highlight-example",
                            Title = "Search with Highlighting",
                            Description = "Get highlighted snippets showing where terms match",
                            GraphQLQuery = @"{
  Content(
    where: { _fulltext: { contains: ""content"" } }
    orderBy: { _ranking: RELEVANCE }
    limit: 5
  ) {
    items {
      Name
      ContentType
      _highlight {
        _fulltext
      }
      _score
    }
    total
  }
}",
                            IsInteractive = true,
                            Hints = new List<string>
                            {
                                "_highlight returns snippets with <em> tags around matches",
                                "Use CSS to style the highlighted terms"
                            }
                        }
                    }
                },
                new()
                {
                    Id = "autocomplete",
                    ModuleId = "search",
                    Title = "Autocomplete",
                    Summary = "Implement type-ahead search suggestions",
                    Order = 5,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Use Autocomplete queries",
                        "Configure autocomplete fields",
                        "Implement search suggestions"
                    },
                    Content = @"<h3>Autocomplete Queries</h3>
<p>Optimizely Graph supports autocomplete for search-as-you-type functionality:</p>
<pre class='code-block'>{
  BlogPostAutocomplete(
    searchTerm: ""opt""
    limit: 5
  ) {
    items {
      Title
    }
  }
}</pre>

<h3>How It Works</h3>
<ul>
<li>Matches partial words from the beginning</li>
<li>Optimized for fast response times</li>
<li>Returns suggested completions</li>
</ul>

<h3>Implementing Autocomplete</h3>
<p>For a good autocomplete experience:</p>
<ol>
<li>Debounce user input (wait 150-300ms after typing stops)</li>
<li>Show suggestions in a dropdown</li>
<li>Allow keyboard navigation</li>
<li>Execute full search on selection or Enter</li>
</ol>

<h3>Autocomplete vs Full Search</h3>
<table class='w-full text-sm'>
<tr><th class='text-left p-2 border-b'>Feature</th><th class='text-left p-2 border-b'>Autocomplete</th><th class='text-left p-2 border-b'>Full Search</th></tr>
<tr><td class='p-2 border-b'>Speed</td><td class='p-2 border-b'>Optimized for speed</td><td class='p-2 border-b'>More comprehensive</td></tr>
<tr><td class='p-2 border-b'>Matching</td><td class='p-2 border-b'>Prefix matching</td><td class='p-2 border-b'>Full-text matching</td></tr>
<tr><td class='p-2 border-b'>Use case</td><td class='p-2 border-b'>Suggestions while typing</td><td class='p-2 border-b'>Final search results</td></tr>
</table>",
                    Examples = new List<LessonExample>()
                },
                new()
                {
                    Id = "synonyms",
                    ModuleId = "search",
                    Title = "Synonyms",
                    Summary = "Expand search queries with synonym matching",
                    Order = 6,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Understand synonym configuration",
                        "Use synonyms in search queries",
                        "Manage synonym lists"
                    },
                    Content = @"<h3>What Are Synonyms?</h3>
<p>Synonyms allow search to match related terms that users might use interchangeably. For example, searching for ""car"" can also return results containing ""automobile"" or ""vehicle"".</p>

<h3>How Synonyms Work in Optimizely Graph</h3>
<p>Synonyms are configured at the index level and automatically expand search queries:</p>
<ul>
<li><strong>One-way synonyms</strong>: ""TV"" → ""television"" (TV matches television, but not vice versa)</li>
<li><strong>Two-way synonyms</strong>: ""car"" ↔ ""automobile"" (both match each other)</li>
</ul>

<h3>Synonym Configuration</h3>
<p>Synonyms are typically configured in the Optimizely Graph administration:</p>
<pre class='code-block'># Example synonym rules
car, automobile, vehicle
TV => television
laptop => notebook, portable computer</pre>

<h3>Benefits of Synonyms</h3>
<ul>
<li><strong>Improved recall</strong> - Find more relevant results</li>
<li><strong>Better user experience</strong> - Users don't need to guess exact terminology</li>
<li><strong>Domain-specific vocabulary</strong> - Map industry jargon to common terms</li>
<li><strong>Abbreviation expansion</strong> - Match ""NYC"" with ""New York City""</li>
</ul>

<h3>Best Practices</h3>
<ul>
<li>Start with common abbreviations and industry terms</li>
<li>Review search analytics to identify missed opportunities</li>
<li>Be careful with broad synonyms that might reduce precision</li>
<li>Test synonym rules before deploying to production</li>
</ul>",
                    Examples = new List<LessonExample>
                    {
                        new()
                        {
                            Id = "synonym-search-example",
                            Title = "Search with Synonyms",
                            Description = "Search terms are automatically expanded with configured synonyms",
                            GraphQLQuery = @"{
  Content(
    where: { _fulltext: { contains: ""car"" } }
    orderBy: { _ranking: RELEVANCE }
    limit: 5
  ) {
    items {
      Name
      ContentType
      _score
    }
    total
  }
}",
                            IsInteractive = true,
                            Hints = new List<string>
                            {
                                "Synonyms are applied automatically based on configuration",
                                "Results may include content with synonym terms"
                            }
                        }
                    }
                },
                new()
                {
                    Id = "boosting",
                    ModuleId = "search",
                    Title = "Search Boosting",
                    Summary = "Influence search relevance with field and query boosting",
                    Order = 7,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Understand relevance boosting concepts",
                        "Apply boost to search filters",
                        "Configure field-level boosting"
                    },
                    Content = @"<h3>What is Boosting?</h3>
<p>Boosting allows you to influence the relevance score of search results, making certain matches more or less important.</p>

<h3>Types of Boosting</h3>
<ul>
<li><strong>Query boosting</strong> - Boost specific filter conditions</li>
<li><strong>Field boosting</strong> - Make matches in certain fields more important</li>
<li><strong>Document boosting</strong> - Promote specific content items</li>
</ul>

<h3>Using the Boost Operator</h3>
<p>Apply boost to filter conditions:</p>
<pre class='code-block'>{
  Content(
    where: {
      _or: [
        { _fulltext: { contains: ""optimizely"", boost: 2.0 } }
        { _fulltext: { contains: ""cms"", boost: 1.0 } }
      ]
    }
    orderBy: { _ranking: RELEVANCE }
  ) {
    items {
      Name
      _score
    }
  }
}</pre>

<h3>Boost Values</h3>
<ul>
<li><code>boost: 1.0</code> - Default, no change</li>
<li><code>boost: 2.0</code> - Double the importance</li>
<li><code>boost: 0.5</code> - Half the importance</li>
<li><code>boost: 0</code> - Exclude from scoring (still matches)</li>
</ul>

<h3>Common Boosting Patterns</h3>
<ul>
<li><strong>Title over body</strong> - Boost title field matches higher</li>
<li><strong>Recent content</strong> - Boost newer content</li>
<li><strong>Featured items</strong> - Boost content marked as featured</li>
<li><strong>Content type priority</strong> - Boost pages over media assets</li>
</ul>

<h3>Best Practices</h3>
<ul>
<li>Start with small boost values and adjust based on results</li>
<li>Test boosting with real user queries</li>
<li>Don't over-boost - it can make results feel unnatural</li>
<li>Combine boosting with relevance sorting</li>
</ul>",
                    Examples = new List<LessonExample>
                    {
                        new()
                        {
                            Id = "boost-example",
                            Title = "Boosted Search Query",
                            Description = "Apply boost to prioritize certain matches",
                            GraphQLQuery = @"{
  Content(
    where: {
      _fulltext: { contains: ""content"", boost: 1.5 }
    }
    orderBy: { _ranking: RELEVANCE }
    limit: 5
  ) {
    items {
      Name
      ContentType
      _score
    }
    total
  }
}",
                            IsInteractive = true,
                            Hints = new List<string>
                            {
                                "Boost values multiply the relevance score",
                                "Use _score to see the effect of boosting"
                            }
                        }
                    }
                },
                new()
                {
                    Id = "hit-count",
                    ModuleId = "search",
                    Title = "Hit Count and Totals",
                    Summary = "Get accurate counts of search results and matches",
                    Order = 8,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Query total result counts",
                        "Understand count accuracy",
                        "Use counts for pagination UI"
                    },
                    Content = @"<h3>Getting Result Counts</h3>
<p>Optimizely Graph provides total counts with search results for building pagination and displaying result summaries.</p>

<h3>The total Field</h3>
<p>Include <code>total</code> in your query to get the count of matching items:</p>
<pre class='code-block'>{
  Content(
    where: { _fulltext: { contains: ""search term"" } }
    limit: 10
  ) {
    items {
      Name
    }
    total
  }
}</pre>

<h3>Using Total for Pagination</h3>
<p>Calculate pagination information from the total:</p>
<pre class='code-block'>// Given: total = 47, limit = 10, skip = 20
currentPage = (skip / limit) + 1  // Page 3
totalPages = ceil(total / limit)  // 5 pages
hasNextPage = skip + limit < total  // true
hasPrevPage = skip > 0  // true</pre>

<h3>Count Accuracy</h3>
<p>For performance reasons, very large result sets may return approximate counts. The count is exact for:</p>
<ul>
<li>Result sets under 10,000 items</li>
<li>When using specific filters</li>
</ul>

<h3>Displaying Results</h3>
<p>Common patterns for showing counts:</p>
<ul>
<li>""Showing 1-10 of 47 results""</li>
<li>""47 items found""</li>
<li>""Page 3 of 5""</li>
</ul>",
                    Examples = new List<LessonExample>
                    {
                        new()
                        {
                            Id = "hit-count-example",
                            Title = "Query with Hit Count",
                            Description = "Get the total count of matching results",
                            GraphQLQuery = @"{
  Content(
    where: { _fulltext: { contains: ""page"" } }
    limit: 5
  ) {
    items {
      Name
      ContentType
    }
    total
  }
}",
                            IsInteractive = true,
                            Hints = new List<string>
                            {
                                "total returns the full count of matching items",
                                "Use total with skip/limit to build pagination"
                            }
                        }
                    }
                },
                new()
                {
                    Id = "pinned-results",
                    ModuleId = "search",
                    Title = "Pinned Results",
                    Summary = "Curate search results by pinning specific content",
                    Order = 9,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Understand pinned results concept",
                        "Configure pinned content for queries",
                        "Combine pinned with organic results"
                    },
                    Content = @"<h3>What Are Pinned Results?</h3>
<p>Pinned results (also called ""best bets"" or ""featured results"") allow you to manually curate which content appears at the top of search results for specific queries.</p>

<h3>Use Cases</h3>
<ul>
<li><strong>Promotional content</strong> - Feature campaigns for relevant searches</li>
<li><strong>High-value pages</strong> - Ensure important pages are found</li>
<li><strong>Corrective action</strong> - Override poor organic rankings</li>
<li><strong>Seasonal content</strong> - Promote timely content</li>
</ul>

<h3>How Pinning Works</h3>
<p>Pinned results are typically configured in the Optimizely interface by:</p>
<ol>
<li>Defining trigger keywords or phrases</li>
<li>Selecting content to pin</li>
<li>Setting the pin position (1st, 2nd, etc.)</li>
<li>Optionally setting date ranges for pins</li>
</ol>

<h3>Pinning Strategy</h3>
<pre class='code-block'># Example pin configuration
Query: ""return policy""
Pinned: /customer-service/returns (Position 1)

Query: ""careers"", ""jobs"", ""work here""
Pinned: /about/careers (Position 1)</pre>

<h3>Best Practices</h3>
<ul>
<li><strong>Use sparingly</strong> - Too many pins defeat the purpose</li>
<li><strong>Review regularly</strong> - Remove outdated pins</li>
<li><strong>Track performance</strong> - Monitor click-through on pinned items</li>
<li><strong>Consider multiple keywords</strong> - Pin to related search terms</li>
</ul>

<h3>Combining with Organic Results</h3>
<p>Pinned results typically appear first, followed by organic search results. Duplicates are usually removed automatically.</p>",
                    Examples = new List<LessonExample>
                    {
                        new()
                        {
                            Id = "pinned-results-example",
                            Title = "Search with Pinned Results",
                            Description = "Pinned results appear at the top when configured",
                            GraphQLQuery = @"{
  Content(
    where: { _fulltext: { contains: ""help"" } }
    orderBy: { _ranking: RELEVANCE }
    limit: 5
  ) {
    items {
      Name
      ContentType
      _score
    }
    total
  }
}",
                            IsInteractive = true,
                            Hints = new List<string>
                            {
                                "Pinned results are configured in Optimizely admin",
                                "They appear at the top regardless of relevance score"
                            }
                        }
                    }
                }
            }
        };
    }

    private static LearningModule BuildFacetsModule()
    {
        return new LearningModule
        {
            Id = "facets",
            Title = "Faceted Search",
            Description = "Create powerful filtering interfaces with facets and aggregations",
            Icon = "adjustments-horizontal",
            Order = 6,
            Difficulty = ModuleDifficulty.Intermediate,
            Prerequisites = new[] { "filtering", "search" },
            Lessons = new List<Lesson>
            {
                new()
                {
                    Id = "facets-intro",
                    ModuleId = "facets",
                    Title = "Introduction to Facets",
                    Summary = "Learn how facets enable dynamic filtering interfaces",
                    Order = 1,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Understand what facets are",
                        "Query facet values and counts",
                        "Use facets for filtering"
                    },
                    Content = @"<h3>What Are Facets?</h3>
<p>Facets are aggregations that help users filter content by showing available values and their counts. They're commonly used for:</p>
<ul>
<li>Category filters</li>
<li>Date range selectors</li>
<li>Tag clouds</li>
<li>Price range filters</li>
</ul>

<h3>Querying Facets</h3>
<p>Add facet fields to your query to get aggregated counts:</p>
<pre class='code-block'>{
  BlogPost {
    items { Title Category }
    CategoryFacet {
      name
      count
    }
  }
}</pre>",
                    Examples = new List<LessonExample>
                    {
                        new()
                        {
                            Id = "facets-basic",
                            Title = "Basic Facet Query",
                            Description = "Query content with facet aggregations",
                            GraphQLQuery = @"{
  Content(limit: 5) {
    items {
      Name
      ContentType
    }
    total
  }
}",
                            IsInteractive = true,
                            Hints = new List<string>
                            {
                                "Facets show unique values with their occurrence counts",
                                "Use facet results to build filter UI components"
                            }
                        }
                    }
                }
            }
        };
    }

    private static LearningModule BuildFragmentsModule()
    {
        return new LearningModule
        {
            Id = "fragments",
            Title = "GraphQL Fragments",
            Description = "Reuse query parts with fragments for cleaner, more maintainable queries",
            Icon = "puzzle-piece",
            Order = 7,
            Difficulty = ModuleDifficulty.Intermediate,
            Prerequisites = new[] { "getting-started", "filtering" },
            Lessons = new List<Lesson>
            {
                new()
                {
                    Id = "fragment-basics",
                    ModuleId = "fragments",
                    Title = "Introduction to Fragments",
                    Summary = "Learn how to define and use fragments to reuse query selections",
                    Order = 1,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Understand what GraphQL fragments are",
                        "Define reusable fragments",
                        "Use fragments in queries"
                    },
                    Content = @"<h3>What Are Fragments?</h3>
<p>Fragments are reusable pieces of a GraphQL query. They allow you to define a set of fields once and include them in multiple queries or selections.</p>

<h3>Fragment Syntax</h3>
<pre class='code-block'>fragment ContentFields on IContent {
  _id
  Name
  ContentType
  Language { Name }
}</pre>

<h3>Using Fragments</h3>
<p>Use the spread operator (<code>...</code>) to include a fragment:</p>
<pre class='code-block'>{
  Content {
    items {
      ...ContentFields
    }
  }
}

fragment ContentFields on IContent {
  _id
  Name
}</pre>

<h3>Benefits of Fragments</h3>
<ul>
<li><strong>DRY Principle</strong> - Don't repeat the same field selections</li>
<li><strong>Maintainability</strong> - Update fields in one place</li>
<li><strong>Readability</strong> - Cleaner, more organized queries</li>
<li><strong>Type Safety</strong> - Fragments are tied to specific types</li>
</ul>",
                    Examples = new List<LessonExample>
                    {
                        new()
                        {
                            Id = "fragment-basic-example",
                            Title = "Basic Fragment Usage",
                            Description = "Define and use a simple fragment for metadata fields",
                            GraphQLQuery = @"{
  Content(limit: 5) {
    items {
      ...ContentMeta
    }
    total
  }
}

fragment ContentMeta on IContent {
  _id
  Name
  ContentType
  Language { Name }
}",
                            IsInteractive = true,
                            Hints = new List<string>
                            {
                                "Fragments must be defined on a specific type (on IContent)",
                                "Use ...FragmentName to spread the fragment fields"
                            }
                        }
                    }
                },
                new()
                {
                    Id = "inline-fragments",
                    ModuleId = "fragments",
                    Title = "Inline Fragments",
                    Summary = "Use inline fragments to query type-specific fields",
                    Order = 2,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Understand inline fragment syntax",
                        "Query fields from specific content types",
                        "Handle polymorphic content"
                    },
                    Content = @"<h3>Inline Fragments</h3>
<p>Inline fragments allow you to query type-specific fields when dealing with interfaces or unions:</p>
<pre class='code-block'>{
  Content {
    items {
      Name
      ContentType
      ... on BlogPost {
        Author
        PublishedDate
      }
      ... on ProductPage {
        Price
        SKU
      }
    }
  }
}</pre>

<h3>When to Use Inline Fragments</h3>
<ul>
<li>Querying different content types from a shared interface</li>
<li>Conditionally including fields based on type</li>
<li>Working with polymorphic content structures</li>
</ul>

<h3>Type Conditions</h3>
<p>The <code>... on TypeName</code> syntax creates a type condition. Fields inside only apply when the item is of that specific type.</p>",
                    Examples = new List<LessonExample>
                    {
                        new()
                        {
                            Id = "inline-fragment-example",
                            Title = "Inline Fragment Example",
                            Description = "Query type-specific fields using inline fragments",
                            GraphQLQuery = @"{
  Content(limit: 10) {
    items {
      Name
      ContentType
      Url
    }
  }
}",
                            IsInteractive = true,
                            Hints = new List<string>
                            {
                                "Inline fragments use ... on TypeName syntax",
                                "Only matching types will include those fields"
                            }
                        }
                    }
                },
                new()
                {
                    Id = "fragment-composition",
                    ModuleId = "fragments",
                    Title = "Fragment Composition",
                    Summary = "Compose multiple fragments for complex queries",
                    Order = 3,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Nest fragments within fragments",
                        "Organize complex queries with multiple fragments",
                        "Best practices for fragment organization"
                    },
                    Content = @"<h3>Composing Fragments</h3>
<p>Fragments can include other fragments for complex queries:</p>
<pre class='code-block'>fragment FullContent on IContent {
  ...BasicFields
  ...LinkFields
}

fragment BasicFields on IContent {
  _id
  Name
}

fragment LinkFields on IContent {
  Url
}</pre>

<h3>Best Practices</h3>
<ul>
<li><strong>Name descriptively</strong> - Use clear names like UserFields, ProductDetails</li>
<li><strong>Keep fragments focused</strong> - Each fragment should have one purpose</li>
<li><strong>Avoid circular references</strong> - Fragments cannot reference themselves</li>
<li><strong>Co-locate with components</strong> - Define fragments near where they're used</li>
</ul>

<h3>Fragment Variables</h3>
<p>Note: GraphQL fragments themselves don't accept variables, but they can use variables defined at the query level.</p>",
                    Examples = new List<LessonExample>
                    {
                        new()
                        {
                            Id = "fragment-composition-example",
                            Title = "Composed Fragments",
                            Description = "Use multiple composed fragments in a query",
                            GraphQLQuery = @"{
  Content(limit: 5) {
    items {
      ...FullContentInfo
    }
  }
}

fragment FullContentInfo on IContent {
  ...BasicMeta
  ...ContentLink
}

fragment BasicMeta on IContent {
  _id
  Name
  ContentType
}

fragment ContentLink on IContent {
  Url
}",
                            IsInteractive = true,
                            Hints = new List<string>
                            {
                                "FullContentInfo includes both BasicMeta and ContentLink",
                                "This keeps each fragment focused and reusable"
                            }
                        }
                    }
                }
            }
        };
    }

    private static LearningModule BuildContentRelationshipsModule()
    {
        return new LearningModule
        {
            Id = "content-relationships",
            Title = "Content Relationships",
            Description = "Query parent-child relationships, linked content, and navigate content hierarchies",
            Icon = "link",
            Order = 8,
            Difficulty = ModuleDifficulty.Intermediate,
            Prerequisites = new[] { "getting-started", "filtering" },
            Lessons = new List<Lesson>
            {
                new()
                {
                    Id = "parent-child",
                    ModuleId = "content-relationships",
                    Title = "Parent and Child Content",
                    Summary = "Navigate content hierarchies with parent and children queries",
                    Order = 1,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Query parent content items",
                        "Query child content items",
                        "Navigate content trees"
                    },
                    Content = @"<h3>Content Hierarchy in Optimizely</h3>
<p>Optimizely CMS organizes content in a tree structure. Each content item can have a parent and multiple children.</p>

<h3>Querying Parent Content</h3>
<p>Access the parent of a content item using the <code>_parent</code> field:</p>
<pre class='code-block'>{
  BlogPost {
    items {
      Title
      _parent {
        Name
      }
    }
  }
}</pre>

<h3>Querying Children</h3>
<p>Get child items using the <code>_children</code> field:</p>
<pre class='code-block'>{
  StartPage {
    items {
      _children {
        items {
          Name
          ContentType
        }
      }
    }
  }
}</pre>

<h3>Hierarchy Fields</h3>
<ul>
<li><code>_parent</code> - The immediate parent content item</li>
<li><code>_children</code> - Direct child content items</li>
<li><code>_ancestors</code> - All parent items up to the root</li>
<li><code>_path</code> - The content path in the tree</li>
</ul>",
                    Examples = new List<LessonExample>
                    {
                        new()
                        {
                            Id = "parent-query-example",
                            Title = "Query with Parent",
                            Description = "Get content items along with their parent information",
                            GraphQLQuery = @"{
  Content(limit: 5) {
    items {
      Name
      ContentType
      Url
    }
    total
  }
}",
                            IsInteractive = true,
                            Hints = new List<string>
                            {
                                "Use _parent to navigate up the content tree",
                                "Parent may be null for root content items"
                            }
                        }
                    }
                },
                new()
                {
                    Id = "content-references",
                    ModuleId = "content-relationships",
                    Title = "Content References",
                    Summary = "Work with content references and linked items",
                    Order = 2,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Understand content reference types",
                        "Query referenced content",
                        "Expand content links"
                    },
                    Content = @"<h3>Content References</h3>
<p>Content often references other content items. Optimizely Graph allows you to expand these references in a single query.</p>

<h3>Reference Properties</h3>
<p>When a content type has a reference property, you can expand it:</p>
<pre class='code-block'>{
  BlogPost {
    items {
      Title
      Author {
        ... on AuthorPage {
          Name
          Bio
        }
      }
      RelatedPosts {
        items {
          Title
          _link { default }
        }
      }
    }
  }
}</pre>

<h3>Content Area Items</h3>
<p>Content areas contain blocks that can also be queried:</p>
<pre class='code-block'>{
  LandingPage {
    items {
      MainContent {
        items {
          ContentType
          ... on TextBlock {
            Heading
            Body
          }
          ... on ImageBlock {
            Image { url }
            Caption
          }
        }
      }
    }
  }
}</pre>

<h3>Reference Types</h3>
<ul>
<li><strong>Single reference</strong> - Points to one content item</li>
<li><strong>Multiple references</strong> - Array of content items</li>
<li><strong>Content areas</strong> - Collection of blocks</li>
</ul>",
                    Examples = new List<LessonExample>
                    {
                        new()
                        {
                            Id = "content-ref-example",
                            Title = "Content Reference Query",
                            Description = "Query content with expanded references",
                            GraphQLQuery = @"{
  Content(
    where: { ContentType: { eq: ""Page"" } }
    limit: 5
  ) {
    items {
      Name
      ContentType
    }
  }
}",
                            IsInteractive = true,
                            Hints = new List<string>
                            {
                                "Reference properties can be expanded inline",
                                "Use inline fragments for type-specific fields"
                            }
                        }
                    }
                },
                new()
                {
                    Id = "cyclic-queries",
                    ModuleId = "content-relationships",
                    Title = "Cyclic Queries",
                    Summary = "Handle circular references and bi-directional relationships",
                    Order = 3,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Understand cyclic reference challenges",
                        "Use query depth limits",
                        "Design queries to avoid infinite loops"
                    },
                    Content = @"<h3>What Are Cyclic Queries?</h3>
<p>Cyclic queries occur when content references create circular relationships:</p>
<ul>
<li>Page A references Page B, which references Page A</li>
<li>A category contains subcategories that reference back to parent</li>
<li>Related content pointing to each other</li>
</ul>

<h3>Handling Cycles</h3>
<p>GraphQL prevents infinite recursion through query depth limits. Optimizely Graph typically limits nesting to prevent performance issues.</p>

<h3>Strategies for Cyclic Data</h3>
<pre class='code-block'># Strategy 1: Limit depth manually
{
  Category {
    items {
      Name
      Subcategories {  # Level 1
        items {
          Name
          # Don't query deeper subcategories here
        }
      }
    }
  }
}

# Strategy 2: Query references as IDs only
{
  Category {
    items {
      Name
      ParentId: _parent { _id }
      ChildIds: _children { items { _id } }
    }
  }
}</pre>

<h3>Best Practices</h3>
<ul>
<li><strong>Plan query depth</strong> - Know how deep you need to go</li>
<li><strong>Use separate queries</strong> - Fetch related content in follow-up queries</li>
<li><strong>Cache wisely</strong> - Store results to avoid re-fetching cycles</li>
<li><strong>Return IDs for deep relations</strong> - Fetch details only when needed</li>
</ul>",
                    Examples = new List<LessonExample>
                    {
                        new()
                        {
                            Id = "cyclic-example",
                            Title = "Controlled Depth Query",
                            Description = "Query hierarchical content with controlled depth",
                            GraphQLQuery = @"{
  Content(limit: 5) {
    items {
      _id
      Name
      Url
    }
  }
}",
                            IsInteractive = true,
                            Hints = new List<string>
                            {
                                "Limit query depth to prevent performance issues",
                                "Use IDs for deep references and fetch details separately"
                            }
                        }
                    }
                }
            }
        };
    }

    private static LearningModule BuildRecursiveQueriesModule()
    {
        return new LearningModule
        {
            Id = "recursive-queries",
            Title = "Recursive Queries",
            Description = "Build navigation trees, breadcrumbs, and hierarchical menus",
            Icon = "arrow-path",
            Order = 9,
            Difficulty = ModuleDifficulty.Advanced,
            Prerequisites = new[] { "content-relationships", "fragments" },
            Lessons = new List<Lesson>
            {
                new()
                {
                    Id = "navigation-trees",
                    ModuleId = "recursive-queries",
                    Title = "Building Navigation Trees",
                    Summary = "Create multi-level navigation menus from content hierarchies",
                    Order = 1,
                    EstimatedMinutes = 15,
                    LearningObjectives = new List<string>
                    {
                        "Query multiple levels of navigation",
                        "Build nested menu structures",
                        "Filter navigation items"
                    },
                    Content = @"<h3>Navigation in Optimizely Graph</h3>
<p>Building navigation requires querying content hierarchies to create menu structures.</p>

<h3>Multi-Level Navigation Query</h3>
<pre class='code-block'>{
  StartPage {
    items {
      _children(
        where: { Status: { eq: ""Published"" } }
      ) {
        items {
          Name
          _id
          Url
          _children {
            items {
              Name
              _id
              Url
            }
          }
        }
      }
    }
  }
}</pre>

<h3>Navigation Patterns</h3>
<ul>
<li><strong>Top navigation</strong> - First level children of start page</li>
<li><strong>Mega menu</strong> - Two or three levels deep</li>
<li><strong>Sidebar</strong> - Children of current section</li>
<li><strong>Footer</strong> - Specific pages by reference</li>
</ul>

<h3>Filtering Navigation Items</h3>
<p>Not all content should appear in navigation. Filter by:</p>
<ul>
<li>Published status</li>
<li>Custom ""show in navigation"" property</li>
<li>Content type</li>
</ul>",
                    Examples = new List<LessonExample>
                    {
                        new()
                        {
                            Id = "nav-tree-example",
                            Title = "Navigation Tree Query",
                            Description = "Build a two-level navigation structure",
                            GraphQLQuery = @"{
  Content(
    where: { ContentType: { eq: ""Page"" } }
    limit: 10
  ) {
    items {
      Name
      _id
      ContentType
      Url
    }
  }
}",
                            IsInteractive = true,
                            Hints = new List<string>
                            {
                                "Filter by ShowInNavigation if your content has this property",
                                "Limit depth based on your navigation design"
                            }
                        }
                    }
                },
                new()
                {
                    Id = "breadcrumbs",
                    ModuleId = "recursive-queries",
                    Title = "Breadcrumb Navigation",
                    Summary = "Generate breadcrumb trails using ancestor queries",
                    Order = 2,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Query content ancestors",
                        "Build breadcrumb paths",
                        "Handle root and special pages"
                    },
                    Content = @"<h3>Breadcrumb Navigation</h3>
<p>Breadcrumbs show the path from the root to the current page. Use ancestor queries to build them.</p>

<h3>Querying Ancestors</h3>
<pre class='code-block'>{
  Content(where: { _id: { eq: ""current-page-id"" } }) {
    items {
      Name
      _ancestors {
        items {
          Name
          _id
          Url
        }
      }
    }
  }
}</pre>

<h3>Building the Trail</h3>
<ol>
<li>Query the current page with its ancestors</li>
<li>Reverse the ancestor list (root first)</li>
<li>Add the current page at the end</li>
<li>Render as linked breadcrumb items</li>
</ol>

<h3>Considerations</h3>
<ul>
<li><strong>Performance</strong> - Ancestors are pre-computed, so queries are fast</li>
<li><strong>Hidden ancestors</strong> - Some ancestors might not be in navigation</li>
<li><strong>Localization</strong> - Ensure correct locale for display names</li>
</ul>",
                    Examples = new List<LessonExample>
                    {
                        new()
                        {
                            Id = "breadcrumb-example",
                            Title = "Breadcrumb Query",
                            Description = "Query a page with its ancestor trail",
                            GraphQLQuery = @"{
  Content(limit: 1) {
    items {
      Name
      _id
      Url
    }
  }
}",
                            IsInteractive = true,
                            Hints = new List<string>
                            {
                                "_ancestors returns all parent items up to root",
                                "Results are typically ordered from immediate parent to root"
                            }
                        }
                    }
                },
                new()
                {
                    Id = "sitemap-queries",
                    ModuleId = "recursive-queries",
                    Title = "Sitemap Generation",
                    Summary = "Create sitemaps by traversing the full content tree",
                    Order = 3,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Query all published pages",
                        "Generate XML sitemaps",
                        "Handle large content trees efficiently"
                    },
                    Content = @"<h3>Sitemap Generation</h3>
<p>Generate sitemaps by querying all publishable content with their URLs.</p>

<h3>Sitemap Query</h3>
<pre class='code-block'>{
  Content(
    where: {
      Status: { eq: ""Published"" }
    }
    limit: 1000
  ) {
    items {
      _id
      _modified
      ContentType
      Url
    }
    cursor
    total
  }
}</pre>

<h3>Sitemap Considerations</h3>
<ul>
<li><strong>Pagination</strong> - Use cursor for large sites</li>
<li><strong>Last modified</strong> - Include for SEO</li>
<li><strong>Priority/Frequency</strong> - Calculate based on content type or depth</li>
<li><strong>Exclude content</strong> - Filter out non-indexable pages</li>
</ul>

<h3>Multi-Language Sitemaps</h3>
<p>For multi-language sites, generate separate sitemaps or include hreflang entries by querying all locales.</p>",
                    Examples = new List<LessonExample>
                    {
                        new()
                        {
                            Id = "sitemap-example",
                            Title = "Sitemap Query",
                            Description = "Query content for sitemap generation",
                            GraphQLQuery = @"{
  Content(
    limit: 20
  ) {
    items {
      _id
      Name
      ContentType
      Language { Name }
      Url
    }
    cursor
    total
  }
}",
                            IsInteractive = true,
                            Hints = new List<string>
                            {
                                "Use cursor pagination for large sites",
                                "Filter by published status in production"
                            }
                        }
                    }
                }
            }
        };
    }

    private static LearningModule BuildContentVariationsModule()
    {
        return new LearningModule
        {
            Id = "content-variations",
            Title = "Content Variations",
            Description = "Work with localized content, draft versions, and content channels",
            Icon = "language",
            Order = 10,
            Difficulty = ModuleDifficulty.Intermediate,
            Prerequisites = new[] { "getting-started", "filtering" },
            Lessons = new List<Lesson>
            {
                new()
                {
                    Id = "localization",
                    ModuleId = "content-variations",
                    Title = "Localized Content",
                    Summary = "Query content in different languages and locales",
                    Order = 1,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Query content by locale",
                        "Understand locale fallback behavior",
                        "Work with multi-language content"
                    },
                    Content = @"<h3>Localization in Optimizely Graph</h3>
<p>Optimizely Graph fully supports multi-language content. Each content item can exist in multiple locales.</p>

<h3>Querying by Locale</h3>
<p>Use the <code>locale</code> parameter to filter by language:</p>
<pre class='code-block'>{
  Content(
    locale: en
    limit: 10
  ) {
    items {
      Name
      Language { Name }
    }
  }
}</pre>

<h3>Available Locale Values</h3>
<p>Common locale codes:</p>
<ul>
<li><code>en</code> - English</li>
<li><code>sv</code> - Swedish</li>
<li><code>de</code> - German</li>
<li><code>fr</code> - French</li>
<li><code>es</code> - Spanish</li>
</ul>

<h3>Locale Fallback</h3>
<p>When content doesn't exist in the requested locale, Optimizely can fall back to:</p>
<ul>
<li>Master language content</li>
<li>Parent locale (e.g., en-GB falls back to en)</li>
<li>No fallback (return null)</li>
</ul>

<h3>Querying All Locales</h3>
<p>Use <code>locale: ALL</code> to get content in all available languages:</p>
<pre class='code-block'>{
  Content(locale: ALL) {
    items {
      Name
      Language { Name }
    }
  }
}</pre>",
                    Examples = new List<LessonExample>
                    {
                        new()
                        {
                            Id = "locale-example",
                            Title = "Query by Locale",
                            Description = "Query content filtered by language",
                            GraphQLQuery = @"{
  Content(
    locale: en
    limit: 5
  ) {
    items {
      Name
      Language { Name }
      ContentType
    }
    total
  }
}",
                            IsInteractive = true,
                            Hints = new List<string>
                            {
                                "Try changing locale to see content in other languages",
                                "Use locale: ALL to see all language versions"
                            }
                        }
                    }
                },
                new()
                {
                    Id = "draft-content",
                    ModuleId = "content-variations",
                    Title = "Draft and Published Content",
                    Summary = "Access draft content for preview using HMAC authentication",
                    Order = 2,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Understand content status",
                        "Query draft content with HMAC auth",
                        "Build preview functionality"
                    },
                    Content = @"<h3>Content Status</h3>
<p>Content in Optimizely has different statuses:</p>
<ul>
<li><code>Published</code> - Live content visible to the public</li>
<li><code>Draft</code> - Work in progress, not yet published</li>
<li><code>PreviouslyPublished</code> - Content that was published but has newer drafts</li>
</ul>

<h3>Querying Draft Content</h3>
<p>To access draft content, you must use HMAC authentication (AppKey + Secret):</p>
<pre class='code-block'>{
  Content(
    where: {
      Status: { eq: ""Draft"" }
    }
  ) {
    items {
      Name
      Status
    }
  }
}</pre>

<h3>Preview Implementation</h3>
<p>For content preview:</p>
<ol>
<li>Use HMAC authentication</li>
<li>Query by content key/guid</li>
<li>Don't filter by status to get latest version</li>
<li>Handle both draft and published states</li>
</ol>

<h3>Single Key vs HMAC</h3>
<ul>
<li><strong>Single Key</strong> - Only returns published content</li>
<li><strong>HMAC</strong> - Can access all content including drafts</li>
</ul>",
                    Examples = new List<LessonExample>
                    {
                        new()
                        {
                            Id = "status-example",
                            Title = "Content Status Query",
                            Description = "Query content and see its publication status",
                            GraphQLQuery = @"{
  Content(limit: 5) {
    items {
      Name
      Status
      _modified
    }
  }
}",
                            IsInteractive = true,
                            Hints = new List<string>
                            {
                                "With Single Key auth, you only see Published content",
                                "Use HMAC auth in Settings to access draft content"
                            }
                        }
                    }
                },
                new()
                {
                    Id = "content-versions",
                    ModuleId = "content-variations",
                    Title = "Content Versions",
                    Summary = "Understand how versioning works in Optimizely Graph",
                    Order = 3,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Understand version metadata",
                        "Query specific versions",
                        "Track content changes"
                    },
                    Content = @"<h3>Content Versioning</h3>
<p>Optimizely tracks content versions as changes are made. Optimizely Graph provides access to version metadata.</p>

<h3>Version Metadata</h3>
<pre class='code-block'>{
  Content {
    items {
      _id
      Status
      _modified
      Changed
    }
  }
}</pre>

<h3>Version Fields</h3>
<ul>
<li><code>version</code> - The version identifier</li>
<li><code>published</code> - When the content was published</li>
<li><code>lastModified</code> - When the content was last changed</li>
<li><code>status</code> - Current publication status</li>
</ul>

<h3>Historical Versions</h3>
<p>Note: Optimizely Graph typically provides access to the current/latest version. Historical versions are available through the CMS but not directly queryable via Graph.</p>

<h3>Change Tracking</h3>
<p>Use <code>lastModified</code> for:</p>
<ul>
<li>Cache invalidation</li>
<li>Content freshness indicators</li>
<li>Sitemap lastmod dates</li>
</ul>",
                    Examples = new List<LessonExample>
                    {
                        new()
                        {
                            Id = "version-example",
                            Title = "Version Metadata Query",
                            Description = "Query content with version information",
                            GraphQLQuery = @"{
  Content(limit: 5) {
    items {
      Name
      _id
      Status
      _modified
      Changed
    }
  }
}",
                            IsInteractive = true,
                            Hints = new List<string>
                            {
                                "_modified shows when content was last updated",
                                "Changed shows the most recent change timestamp"
                            }
                        }
                    }
                },
                new()
                {
                    Id = "channels",
                    ModuleId = "content-variations",
                    Title = "Content Channels",
                    Summary = "Work with content delivered to different channels",
                    Order = 4,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Understand content channels concept",
                        "Query channel-specific content",
                        "Implement omnichannel delivery"
                    },
                    Content = @"<h3>Content Channels</h3>
<p>Optimizely supports delivering content to different channels - web, mobile apps, IoT devices, etc.</p>

<h3>Channel-Aware Queries</h3>
<p>While the core content is the same, you might query different fields or apply different filters based on the consuming channel:</p>
<pre class='code-block'># Web query - full content
{
  Article {
    items {
      Title
      HeroImage { url }
      FullBody
      RelatedArticles { items { Title } }
    }
  }
}

# Mobile query - lighter content
{
  Article {
    items {
      Title
      Thumbnail { url }
      Summary
    }
  }
}</pre>

<h3>Headless Benefits</h3>
<p>Optimizely Graph's headless nature enables:</p>
<ul>
<li><strong>Multi-platform delivery</strong> - Same content, different presentations</li>
<li><strong>Optimized payloads</strong> - Query only what each channel needs</li>
<li><strong>Consistent content</strong> - Single source of truth</li>
<li><strong>Independent deployments</strong> - Frontend changes without CMS changes</li>
</ul>",
                    Examples = new List<LessonExample>()
                },
                new()
                {
                    Id = "modified-deleted-content",
                    ModuleId = "content-variations",
                    Title = "Modified and Deleted Content",
                    Summary = "Track content changes and handle deleted items",
                    Order = 5,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Query recently modified content",
                        "Handle deleted content items",
                        "Build change tracking systems"
                    },
                    Content = @"<h3>Tracking Content Changes</h3>
<p>Optimizely Graph provides metadata to track when content was modified, enabling cache invalidation and change detection.</p>

<h3>Querying Modified Content</h3>
<p>Find content modified after a specific date:</p>
<pre class='code-block'>{
  Content(
    where: {
      _modified: { gte: ""2024-01-01T00:00:00Z"" }
    }
    orderBy: { _modified: DESC }
  ) {
    items {
      Name
      _modified
      Status
    }
  }
}</pre>

<h3>Modified Content Fields</h3>
<ul>
<li><code>lastModified</code> - Timestamp of last change</li>
<li><code>published</code> - When content was published</li>
<li><code>created</code> - When content was first created</li>
</ul>

<h3>Handling Deleted Content</h3>
<p>When content is deleted from the CMS, it's removed from the Graph index. Your application should handle:</p>
<ul>
<li><strong>Broken references</strong> - Referenced content may no longer exist</li>
<li><strong>Cache invalidation</strong> - Remove deleted items from caches</li>
<li><strong>Graceful degradation</strong> - Handle missing content in UI</li>
</ul>

<h3>Webhooks for Changes</h3>
<p>Optimizely Graph supports webhooks to notify your application of changes:</p>
<ul>
<li><strong>Content published</strong> - New or updated content</li>
<li><strong>Content unpublished</strong> - Content removed from public view</li>
<li><strong>Content deleted</strong> - Content permanently removed</li>
</ul>

<h3>Change Detection Strategies</h3>
<ol>
<li><strong>Polling</strong> - Periodically query for recent changes</li>
<li><strong>Webhooks</strong> - Receive push notifications on changes</li>
<li><strong>Cursor-based sync</strong> - Use cursors to track sync position</li>
</ol>",
                    Examples = new List<LessonExample>
                    {
                        new()
                        {
                            Id = "modified-content-example",
                            Title = "Query Modified Content",
                            Description = "Find recently modified content items",
                            GraphQLQuery = @"{
  Content(
    orderBy: { _modified: DESC }
    limit: 5
  ) {
    items {
      Name
      _modified
      Status
      ContentType
    }
  }
}",
                            IsInteractive = true,
                            Hints = new List<string>
                            {
                                "_modified shows when content was last changed",
                                "Use date filters to find changes in a time range"
                            }
                        }
                    }
                }
            }
        };
    }

    private static LearningModule BuildAdvancedModule()
    {
        return new LearningModule
        {
            Id = "advanced",
            Title = "Advanced Topics",
            Description = "Explore advanced features like geo-search, cached templates, and joins",
            Icon = "beaker",
            Order = 11,
            Difficulty = ModuleDifficulty.Advanced,
            Prerequisites = new[] { "filtering", "pagination", "search" },
            Lessons = new List<Lesson>
            {
                new()
                {
                    Id = "cached-templates",
                    ModuleId = "advanced",
                    Title = "Cached Templates",
                    Summary = "Optimize performance with cached query templates",
                    Order = 1,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Understand cached templates",
                        "Use the stored query feature",
                        "Optimize query performance"
                    },
                    Content = @"<h3>Cached Templates</h3>
<p>Cached templates speed up queries by reusing stored query structures:</p>
<pre class='code-block'>POST /content/v2?stored=true
Header: cg-stored-query: template</pre>

<h3>Benefits</h3>
<ul>
<li>Reduced query parsing overhead</li>
<li>Faster response times</li>
<li>Lower server load</li>
</ul>

<h3>When to Use</h3>
<p>Use cached templates for:</p>
<ul>
<li>Frequently executed queries</li>
<li>Complex queries with many fields</li>
<li>High-traffic pages</li>
</ul>",
                    Examples = new List<LessonExample>()
                },
                new()
                {
                    Id = "json-payload",
                    ModuleId = "advanced",
                    Title = "JSON Payload",
                    Summary = "Retrieve full content items as JSON payloads",
                    Order = 2,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Use _json for full content retrieval",
                        "Simplify complex queries",
                        "Reduce query complexity"
                    },
                    Content = @"<h3>The _json Field</h3>
<p>Return full content items as a single JSON payload:</p>
<pre class='code-block'>{
  BlogPost {
    items {
      _json
    }
  }
}</pre>

<h3>Benefits</h3>
<ul>
<li>Eliminates need for large fragment-heavy queries</li>
<li>Reduces query complexity</li>
<li>Improves performance for complex content models</li>
</ul>

<h3>Use Cases</h3>
<ul>
<li>Content preview/editing interfaces</li>
<li>Full content export</li>
<li>Dynamic content rendering</li>
</ul>",
                    Examples = new List<LessonExample>()
                },
                new()
                {
                    Id = "geo-search",
                    ModuleId = "advanced",
                    Title = "Geo-Search",
                    Summary = "Query content by geographic location",
                    Order = 3,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Understand Geopoint types",
                        "Query by distance",
                        "Sort by proximity"
                    },
                    Content = @"<h3>Geographical Queries</h3>
<p>Optimizely Graph supports geographical queries with Geopoint types:</p>
<pre class='code-block'>{
  Location(
    where: {
      Coordinates: {
        distance: {
          from: { lat: 51.5, lon: -0.12 }
          unit: km
          lte: 10
        }
      }
    }
  ) {
    items { Name Coordinates { lat lon } }
  }
}</pre>

<h3>Geo Features</h3>
<ul>
<li>Distance-based filtering</li>
<li>Sorting by proximity</li>
<li>Bounding box queries</li>
</ul>",
                    Examples = new List<LessonExample>()
                },
                new()
                {
                    Id = "joins-linking",
                    ModuleId = "advanced",
                    Title = "Joins with Linking",
                    Summary = "Connect related content using join queries and linking",
                    Order = 4,
                    EstimatedMinutes = 15,
                    LearningObjectives = new List<string>
                    {
                        "Understand join concepts in GraphQL",
                        "Link content across types",
                        "Optimize queries with joins"
                    },
                    Content = @"<h3>Joining Content in Optimizely Graph</h3>
<p>Joins allow you to connect related content items in a single query, reducing the need for multiple round trips.</p>

<h3>Link Fields</h3>
<p>Optimizely Graph provides the <code>_link</code> field for accessing URLs and related content:</p>
<pre class='code-block'>{
  BlogPost {
    items {
      Title
      _link {
        default
        base
      }
    }
  }
}</pre>

<h3>Cross-Type Linking</h3>
<p>Link content across different types using reference fields:</p>
<pre class='code-block'>{
  BlogPost {
    items {
      Title
      Author {
        ... on AuthorPage {
          Name
          Bio
          Photo { url }
        }
      }
      Category {
        ... on CategoryPage {
          Name
          _link { default }
        }
      }
    }
  }
}</pre>

<h3>Linking Strategies</h3>
<ul>
<li><strong>Direct references</strong> - Content reference properties in the CMS</li>
<li><strong>Content areas</strong> - Blocks and nested content</li>
<li><strong>Hierarchical links</strong> - Parent/child relationships</li>
<li><strong>Manual linking</strong> - Query by shared properties</li>
</ul>

<h3>Joining by Shared Properties</h3>
<p>Link content that shares common values:</p>
<pre class='code-block'>{
  # Get all posts in a category
  BlogPost(
    where: { Category: { Name: { eq: ""Technology"" } } }
  ) {
    items {
      Title
      PublishedDate
    }
  }
}</pre>

<h3>Performance Considerations</h3>
<ul>
<li>Deep joins increase query complexity</li>
<li>Limit join depth to 2-3 levels</li>
<li>Use fragments for repeated join patterns</li>
<li>Consider separate queries for very complex joins</li>
</ul>",
                    Examples = new List<LessonExample>
                    {
                        new()
                        {
                            Id = "joins-example",
                            Title = "Query with Links",
                            Description = "Join content using link fields",
                            GraphQLQuery = @"{
  Content(limit: 5) {
    items {
      Name
      ContentType
      Url
    }
  }
}",
                            IsInteractive = true,
                            Hints = new List<string>
                            {
                                "Url provides the URL for content",
                                "Use reference properties to join related content"
                            }
                        }
                    }
                },
                new()
                {
                    Id = "text-extraction",
                    ModuleId = "advanced",
                    Title = "Text Extraction Field",
                    Summary = "Extract searchable text from rich content and documents",
                    Order = 5,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Understand text extraction",
                        "Search within documents",
                        "Configure extraction settings"
                    },
                    Content = @"<h3>What is Text Extraction?</h3>
<p>Text extraction automatically extracts searchable text from rich content types like PDFs, Word documents, and rich text fields.</p>

<h3>How It Works</h3>
<p>When content is indexed, Optimizely Graph:</p>
<ol>
<li>Identifies content with extractable text (PDFs, DOCX, HTML, etc.)</li>
<li>Extracts plain text from the content</li>
<li>Indexes the extracted text for full-text search</li>
<li>Associates the text with the content item</li>
</ol>

<h3>Supported Formats</h3>
<ul>
<li><strong>PDF</strong> - Adobe PDF documents</li>
<li><strong>Word</strong> - DOCX, DOC files</li>
<li><strong>Rich Text</strong> - HTML, XHTML content</li>
<li><strong>Plain Text</strong> - TXT files</li>
</ul>

<h3>Searching Extracted Text</h3>
<p>Use full-text search to find content within documents:</p>
<pre class='code-block'>{
  Media(
    where: { _fulltext: { contains: ""quarterly report"" } }
  ) {
    items {
      Name
      ContentType
      Url
      _score
    }
  }
}</pre>

<h3>The _extracted Field</h3>
<p>Some schemas expose the extracted text directly:</p>
<pre class='code-block'>{
  Document {
    items {
      FileName
      _extracted {
        text
      }
    }
  }
}</pre>

<h3>Configuration</h3>
<p>Text extraction is typically configured at the schema level:</p>
<ul>
<li>Enable/disable extraction per content type</li>
<li>Configure maximum extraction size</li>
<li>Set language detection options</li>
</ul>

<h3>Best Practices</h3>
<ul>
<li>Enable extraction only for searchable document types</li>
<li>Consider storage implications for large documents</li>
<li>Test search quality after enabling extraction</li>
<li>Use boosting to prioritize content fields over extracted text</li>
</ul>",
                    Examples = new List<LessonExample>
                    {
                        new()
                        {
                            Id = "text-extraction-example",
                            Title = "Search Document Content",
                            Description = "Search within document text",
                            GraphQLQuery = @"{
  Content(
    where: {
      _fulltext: { contains: ""document"" }
      ContentType: { eq: ""Media"" }
    }
    limit: 5
  ) {
    items {
      Name
      ContentType
      Url
      _score
    }
    total
  }
}",
                            IsInteractive = true,
                            Hints = new List<string>
                            {
                                "Extracted text is included in full-text search",
                                "Filter by Media type to focus on documents"
                            }
                        }
                    }
                }
            }
        };
    }

    private static LearningModule BuildMigrationModule()
    {
        return new LearningModule
        {
            Id = "migration",
            Title = "Migrating from Search & Navigation",
            Description = "Learn how to migrate from Optimizely Search & Navigation (Find) to Optimizely Graph",
            Icon = "arrow-path",
            Order = 12,
            Difficulty = ModuleDifficulty.Advanced,
            Prerequisites = new[] { "getting-started", "filtering", "search" },
            Lessons = new List<Lesson>
            {
                new()
                {
                    Id = "migration-overview",
                    ModuleId = "migration",
                    Title = "Migration Overview",
                    Summary = "Understand why and when to migrate from Search & Navigation to Optimizely Graph",
                    Order = 1,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Understand the key differences between Search & Navigation and Optimizely Graph",
                        "Learn the benefits of migrating to Graph",
                        "Identify prerequisites and requirements for migration",
                        "Understand current feature gaps and workarounds"
                    },
                    Content = @"<h3>Why Migrate to Optimizely Graph?</h3>
<p>Optimizely Graph represents a significant evolution from Search & Navigation, offering modern capabilities for content delivery:</p>

<table class='w-full border-collapse border border-slate-400 dark:border-slate-600 my-4'>
<thead>
<tr class='bg-slate-100 dark:bg-slate-700'>
<th class='border border-slate-300 dark:border-slate-600 p-2 text-left'>Feature</th>
<th class='border border-slate-300 dark:border-slate-600 p-2 text-left'>Search & Navigation</th>
<th class='border border-slate-300 dark:border-slate-600 p-2 text-left'>Optimizely Graph</th>
</tr>
</thead>
<tbody>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'>Architecture</td><td class='border border-slate-300 dark:border-slate-600 p-2'>Elasticsearch-based, page-centric</td><td class='border border-slate-300 dark:border-slate-600 p-2'>GraphQL-based, content-centric</td></tr>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'>Query Language</td><td class='border border-slate-300 dark:border-slate-600 p-2'>C# Fluent API</td><td class='border border-slate-300 dark:border-slate-600 p-2'>GraphQL + .NET Client</td></tr>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'>AI Search</td><td class='border border-slate-300 dark:border-slate-600 p-2'>Keyword matching only</td><td class='border border-slate-300 dark:border-slate-600 p-2'>Semantic search with AI</td></tr>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'>Distribution</td><td class='border border-slate-300 dark:border-slate-600 p-2'>Single region</td><td class='border border-slate-300 dark:border-slate-600 p-2'>CDN edge network, global</td></tr>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'>Headless Support</td><td class='border border-slate-300 dark:border-slate-600 p-2'>Limited</td><td class='border border-slate-300 dark:border-slate-600 p-2'>Built for headless, replaces Content Delivery API</td></tr>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'>CMS Compatibility</td><td class='border border-slate-300 dark:border-slate-600 p-2'>All CMS versions</td><td class='border border-slate-300 dark:border-slate-600 p-2'>CMS 12+ only</td></tr>
</tbody>
</table>

<h3>CMS 13 Requirement</h3>
<p class='bg-amber-50 dark:bg-amber-900/30 border-l-4 border-amber-500 p-4 my-4'>
<strong>Important:</strong> CMS 13 (releasing Q1 2026) will use Optimizely Graph as its foundation for content delivery. If you plan to upgrade to CMS 13, migrating to Graph is a prerequisite.
</p>

<h3>Current Feature Gaps</h3>
<p>Some Search & Navigation features are not yet available in Optimizely Graph:</p>
<ul>
<li><strong>Best Bets</strong> - Manual result positioning (use boosting via API as alternative)</li>
<li><strong>Tracking</strong> - User behavior tracking for personalized search</li>
<li><strong>Did You Mean</strong> - Spelling suggestions</li>
<li><strong>Spellcheck</strong> - Automatic spelling correction</li>
<li><strong>GDPR Tools</strong> - Built-in data management features</li>
</ul>

<h3>Prerequisites for Migration</h3>
<ul>
<li><strong>CMS 12+</strong> - Optimizely Graph requires CMS 12 or later</li>
<li><strong>SaaS or PaaS Core</strong> - SaaS customers have immediate access; PaaS requires separate licensing</li>
<li><strong>.NET 6+</strong> - The Graph .NET SDK requires .NET 6 or higher</li>
</ul>

<h3>Migration Strategy</h3>
<p>Most organizations run Search & Navigation and Graph in parallel during migration:</p>
<ol>
<li>Install and configure Optimizely Graph alongside S&N</li>
<li>Sync content to Graph index</li>
<li>Migrate queries incrementally</li>
<li>Test thoroughly before switching production traffic</li>
<li>Remove S&N when migration is complete</li>
</ol>",
                    Examples = new List<LessonExample>()
                },
                new()
                {
                    Id = "migration-installation",
                    ModuleId = "migration",
                    Title = "Installation & Configuration",
                    Summary = "Set up Optimizely Graph packages and configure your CMS for migration",
                    Order = 2,
                    EstimatedMinutes = 15,
                    LearningObjectives = new List<string>
                    {
                        "Install the required NuGet packages",
                        "Configure appsettings.json for Graph",
                        "Register Graph services in the DI container",
                        "Run the content synchronization job"
                    },
                    Content = @"<h3>Step 1: Install NuGet Packages</h3>
<p>Remove the old Search & Navigation package and install the Graph packages:</p>
<pre class='code-block'># Remove Search & Navigation
Uninstall-Package EPiServer.Find

# Install Optimizely Graph packages
Install-Package Optimizely.ContentGraph.Cms
Install-Package Optimizely.Graph.Client</pre>

<h3>Step 2: Configure appsettings.json</h3>
<p>Add your Optimizely Graph credentials to your configuration:</p>
<pre class='code-block'>{
  ""Optimizely"": {
    ""ContentGraph"": {
      ""GatewayAddress"": ""https://cg.optimizely.com"",
      ""AppKey"": ""your-app-key"",
      ""Secret"": ""your-secret"",
      ""SingleKey"": ""your-single-key"",
      ""AllowSendingLog"": true
    }
  }
}</pre>

<p class='bg-blue-50 dark:bg-blue-900/30 border-l-4 border-blue-500 p-4 my-4'>
<strong>Tip:</strong> You can find your credentials in the Optimizely PaaS Portal or DXP management interface.
</p>

<h3>Step 3: Register Services</h3>
<p>Update your <code>Startup.cs</code> or <code>Program.cs</code> to register the Graph services:</p>
<pre class='code-block'>public void ConfigureServices(IServiceCollection services)
{
    // Required: Sync CMS data to Graph
    services.AddContentDeliveryApi();

    // Core service to sync CMS data to Optimizely Graph
    services.AddContentGraph();

    // Client API for querying Graph
    services.AddContentGraphClient();
}</pre>

<h3>Step 4: Configure Content Type Exposure</h3>
<p>Optionally customize which content types are exposed through the API. Remove abstract modifiers from base types if needed:</p>
<pre class='code-block'>[ContentType(
    AvailableInEditMode = false,
    DisplayName = ""Site Page Base"")]
public class SitePageData : PageData
{
    // Base page properties
}</pre>

<h3>Step 5: Run Content Synchronization</h3>
<p>After installation, run the <strong>""Optimizely Graph content synchronization job""</strong> from the Admin interface to index existing content.</p>

<h3>Step 6: Generate Model Classes (Optional)</h3>
<p>Use the CLI tool to generate strongly-typed C# classes from your GraphQL schema:</p>
<pre class='code-block'># Install the tool
dotnet tool install Optimizely.Graph.Client.Tools --local

# Generate models
dotnet ogschema appsettings.json Models\GraphModels.cs default MyApp.Models</pre>",
                    Examples = new List<LessonExample>
                    {
                        new()
                        {
                            Id = "migration-schema-check",
                            Title = "Verify Schema After Sync",
                            Description = "Check that your content types are available in Graph after synchronization",
                            GraphQLQuery = @"{
  __schema {
    types {
      name
      kind
    }
  }
}",
                            IsInteractive = true,
                            Hints = new List<string>
                            {
                                "After sync, your CMS content types should appear in the schema",
                                "Look for types matching your page and block names"
                            }
                        }
                    }
                },
                new()
                {
                    Id = "migration-query-syntax",
                    ModuleId = "migration",
                    Title = "Query Syntax Comparison",
                    Summary = "Compare Search & Navigation query syntax with Optimizely Graph GraphQL",
                    Order = 3,
                    EstimatedMinutes = 18,
                    LearningObjectives = new List<string>
                    {
                        "Understand the fundamental syntax differences",
                        "Translate common S&N patterns to GraphQL",
                        "Learn the Graph .NET Client fluent API",
                        "Understand when to use raw GraphQL vs .NET Client"
                    },
                    Content = @"<h3>Basic Query Comparison</h3>
<p>Let's compare how common operations are written in both systems:</p>

<h4>Search & Navigation (Old)</h4>
<pre class='code-block'>var results = _searchClient.Search&lt;ArticlePage&gt;()
    .For(""optimizely"")
    .Filter(x => x.PublishedDate.After(DateTime.Now.AddMonths(-6)))
    .OrderByDescending(x => x.PublishedDate)
    .Skip(0)
    .Take(10)
    .GetResult();</pre>

<h4>Optimizely Graph - GraphQL</h4>
<pre class='code-block'>{
  ArticlePage(
    where: {
      _fulltext: { match: ""optimizely"" }
      PublishedDate: { gte: ""2024-07-01"" }
    }
    orderBy: { PublishedDate: DESC }
    skip: 0
    limit: 10
  ) {
    items {
      Name
      PublishedDate
      MainBody
    }
    total
  }
}</pre>

<h4>Optimizely Graph - .NET Client</h4>
<pre class='code-block'>var query = _queryBuilder
    .ForType&lt;ArticlePage&gt;()
    .Search(""optimizely"")
    .Where(x => x.PublishedDate.Gte(DateTime.Now.AddMonths(-6)))
    .OrderBy(x => x.PublishedDate, OrderMode.DESC)
    .Skip(0)
    .Limit(10)
    .Fields(x => x.Name, x => x.PublishedDate, x => x.MainBody)
    .Total()
    .ToQuery()
    .BuildQueries();

var result = await _graphClient.RunQueryAsync(query);</pre>

<h3>Key Syntax Differences</h3>
<table class='w-full border-collapse border border-slate-400 dark:border-slate-600 my-4'>
<thead>
<tr class='bg-slate-100 dark:bg-slate-700'>
<th class='border border-slate-300 dark:border-slate-600 p-2 text-left'>Concept</th>
<th class='border border-slate-300 dark:border-slate-600 p-2 text-left'>S&N</th>
<th class='border border-slate-300 dark:border-slate-600 p-2 text-left'>Graph</th>
</tr>
</thead>
<tbody>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'>Search text</td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.For(""text"")</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>_fulltext: { match: ""text"" }</code></td></tr>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'>Filter equals</td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.Filter(x => x.Status.Match(""Published""))</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>Status: { eq: ""Published"" }</code></td></tr>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'>Filter contains</td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.Filter(x => x.Title.Contains(""news""))</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>Title: { contains: ""news"" }</code></td></tr>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'>Date range</td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.Filter(x => x.Date.InRange(start, end))</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>Date: { gte: ""start"", lte: ""end"" }</code></td></tr>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'>Pagination</td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.Skip(10).Take(20)</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>skip: 10, limit: 20</code></td></tr>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'>Sort desc</td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.OrderByDescending(x => x.Date)</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>orderBy: { Date: DESC }</code></td></tr>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'>Total count</td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>result.TotalMatching</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>total</code> field</td></tr>
</tbody>
</table>

<h3>Selecting Fields</h3>
<p class='bg-green-50 dark:bg-green-900/30 border-l-4 border-green-500 p-4 my-4'>
<strong>Key Difference:</strong> In GraphQL, you must explicitly select the fields you want. This prevents over-fetching and improves performance compared to S&N which returns all indexed fields by default.
</p>

<h3>When to Use Each Approach</h3>
<ul>
<li><strong>Raw GraphQL</strong> - Frontend/headless applications, JavaScript/React, testing in playground</li>
<li><strong>.NET Client</strong> - Backend C# code, strongly-typed queries, familiar fluent API</li>
</ul>",
                    Examples = new List<LessonExample>
                    {
                        new()
                        {
                            Id = "migration-basic-query",
                            Title = "Basic Content Query",
                            Description = "A simple query to retrieve content items - the Graph equivalent of Search<T>().GetResult()",
                            GraphQLQuery = @"{
  Content(
    limit: 10
  ) {
    items {
      Name
      ContentType
      Url
      Changed
    }
    total
  }
}",
                            IsInteractive = true,
                            Hints = new List<string>
                            {
                                "Unlike S&N, you must specify which fields to return",
                                "The 'total' field gives you the count like TotalMatching"
                            }
                        }
                    }
                },
                new()
                {
                    Id = "migration-filters",
                    ModuleId = "migration",
                    Title = "Migrating Filters",
                    Summary = "Learn how to translate Search & Navigation filter expressions to Optimizely Graph",
                    Order = 4,
                    EstimatedMinutes = 15,
                    LearningObjectives = new List<string>
                    {
                        "Convert S&N filter expressions to Graph where clauses",
                        "Understand the different filter operators available",
                        "Handle complex boolean filter combinations",
                        "Use the .NET Client filter builders"
                    },
                    Content = @"<h3>Filter Operator Mapping</h3>
<p>Search & Navigation filter operators map to Graph operators as follows:</p>

<table class='w-full border-collapse border border-slate-400 dark:border-slate-600 my-4'>
<thead>
<tr class='bg-slate-100 dark:bg-slate-700'>
<th class='border border-slate-300 dark:border-slate-600 p-2 text-left'>Operation</th>
<th class='border border-slate-300 dark:border-slate-600 p-2 text-left'>S&N</th>
<th class='border border-slate-300 dark:border-slate-600 p-2 text-left'>Graph GraphQL</th>
<th class='border border-slate-300 dark:border-slate-600 p-2 text-left'>Graph .NET Client</th>
</tr>
</thead>
<tbody>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'>Equals</td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.Match(""value"")</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>eq: ""value""</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.Eq(""value"")</code></td></tr>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'>Not equals</td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>!.Match(""value"")</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>notEq: ""value""</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.NotEq(""value"")</code></td></tr>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'>Contains</td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.Contains(""text"")</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>contains: ""text""</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.Contains(""text"")</code></td></tr>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'>In list</td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.In(new[]{...})</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>in: [...]</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.In(1, 2, 3)</code></td></tr>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'>Greater than</td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.GreaterThan(n)</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>gt: n</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.Gt(n)</code></td></tr>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'>Less than</td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.LessThan(n)</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>lt: n</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.Lt(n)</code></td></tr>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'>Range</td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.InRange(min, max)</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>gte: min, lte: max</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.InRange(min, max)</code></td></tr>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'>Exists</td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.Exists()</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>exist: true</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.Exists()</code></td></tr>
</tbody>
</table>

<h3>S&N Filter Example</h3>
<pre class='code-block'>// Search & Navigation
var results = _searchClient.Search&lt;ArticlePage&gt;()
    .Filter(x => x.Category.Match(""Technology""))
    .Filter(x => x.PublishedDate.After(startDate))
    .Filter(x => x.PublishedDate.Before(endDate))
    .Filter(x => x.Author.In(new[] { ""John"", ""Jane"" }))
    .GetResult();</pre>

<h3>Graph GraphQL Equivalent</h3>
<pre class='code-block'>{
  ArticlePage(
    where: {
      Category: { eq: ""Technology"" }
      PublishedDate: {
        gte: ""2024-01-01""
        lte: ""2024-12-31""
      }
      Author: { in: [""John"", ""Jane""] }
    }
  ) {
    items { Name PublishedDate Author }
    total
  }
}</pre>

<h3>Graph .NET Client Equivalent</h3>
<pre class='code-block'>var query = _queryBuilder
    .ForType&lt;ArticlePage&gt;()
    .Where(x => x.Category.Eq(""Technology""))
    .Where(x => x.PublishedDate.Gte(startDate))
    .Where(x => x.PublishedDate.Lte(endDate))
    .Where(x => x.Author.In(""John"", ""Jane""))
    .Fields(x => x.Name, x => x.PublishedDate, x => x.Author)
    .Total()
    .ToQuery()
    .BuildQueries();</pre>

<h3>Boolean Logic (AND/OR/NOT)</h3>
<p>In S&N, multiple <code>.Filter()</code> calls are implicitly AND-ed. Graph works the same way:</p>

<h4>AND Logic (Implicit)</h4>
<pre class='code-block'>// Both systems: multiple filters = AND
where: {
  Status: { eq: ""Published"" }
  Category: { eq: ""News"" }
}</pre>

<h4>OR Logic</h4>
<pre class='code-block'>// S&N
.Filter(x => x.Category.Match(""News"") | x.Category.Match(""Events""))

// Graph GraphQL
where: {
  _or: [
    { Category: { eq: ""News"" } }
    { Category: { eq: ""Events"" } }
  ]
}

// Graph .NET Client
var orFilter = BooleanFilter
    .OrFilter&lt;ArticlePage&gt;()
    .Or(x => x.Category.Eq(""News""))
    .Or(x => x.Category.Eq(""Events""));</pre>

<h4>NOT Logic</h4>
<pre class='code-block'>// S&N
.Filter(x => !x.Status.Match(""Draft""))

// Graph GraphQL
where: {
  _not: { Status: { eq: ""Draft"" } }
}

// Graph .NET Client
var notFilter = BooleanFilter
    .NotFilter&lt;ArticlePage&gt;()
    .Not(x => x.Status.Eq(""Draft""));</pre>",
                    Examples = new List<LessonExample>
                    {
                        new()
                        {
                            Id = "migration-filter-example",
                            Title = "Combined Filters",
                            Description = "Multiple filters combined - equivalent to chained .Filter() calls in S&N",
                            GraphQLQuery = @"{
  Content(
    where: {
      ContentType: { in: [""ArticlePage"", ""NewsPage""] }
      Status: { eq: ""Published"" }
    }
    limit: 10
  ) {
    items {
      Name
      ContentType
      Status
    }
    total
  }
}",
                            IsInteractive = true,
                            Hints = new List<string>
                            {
                                "Multiple where conditions are AND-ed together by default",
                                "Use _or for OR logic, _not for negation"
                            }
                        }
                    }
                },
                new()
                {
                    Id = "migration-fulltext-search",
                    ModuleId = "migration",
                    Title = "Full-Text Search Migration",
                    Summary = "Migrate full-text search functionality from Search & Navigation to Optimizely Graph",
                    Order = 5,
                    EstimatedMinutes = 15,
                    LearningObjectives = new List<string>
                    {
                        "Migrate .For() searches to Graph _fulltext queries",
                        "Understand relevance scoring differences",
                        "Implement search highlighting",
                        "Configure searchable fields"
                    },
                    Content = @"<h3>Full-Text Search Comparison</h3>

<h4>Search & Navigation</h4>
<pre class='code-block'>var results = _searchClient.Search&lt;ArticlePage&gt;()
    .For(""climate change policy"")
    .InField(x => x.Title, 2.0)      // Boost title matches
    .InField(x => x.MainBody)
    .InField(x => x.Summary)
    .ApplyBestBets()                  // Not available in Graph
    .Track()                          // Not available in Graph
    .GetResult();</pre>

<h4>Optimizely Graph</h4>
<pre class='code-block'>{
  ArticlePage(
    where: {
      _fulltext: {
        match: ""climate change policy""
        boost: 2
      }
    }
    orderBy: { _ranking: RELEVANCE }
  ) {
    items {
      Name
      Title
      MainBody
      _score
      _highlight {
        Title
        MainBody
      }
    }
    total
  }
}</pre>

<h3>Key Differences</h3>
<table class='w-full border-collapse border border-slate-400 dark:border-slate-600 my-4'>
<thead>
<tr class='bg-slate-100 dark:bg-slate-700'>
<th class='border border-slate-300 dark:border-slate-600 p-2 text-left'>Feature</th>
<th class='border border-slate-300 dark:border-slate-600 p-2 text-left'>S&N</th>
<th class='border border-slate-300 dark:border-slate-600 p-2 text-left'>Graph</th>
</tr>
</thead>
<tbody>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'>Search method</td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.For(""text"")</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>_fulltext: { match: ""text"" }</code></td></tr>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'>Field boosting</td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.InField(x => x.Title, 2.0)</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>boost: 2</code> on field</td></tr>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'>Relevance score</td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>hit.Score</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>_score</code></td></tr>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'>Highlighting</td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.Highlight()</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>_highlight { }</code></td></tr>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'>Sort by relevance</td><td class='border border-slate-300 dark:border-slate-600 p-2'>Default</td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>orderBy: { _ranking: RELEVANCE }</code></td></tr>
</tbody>
</table>

<h3>Match vs Contains</h3>
<p>Graph offers two full-text operators:</p>
<ul>
<li><strong>match</strong> - Full-text search with relevance scoring (like S&N's <code>.For()</code>)</li>
<li><strong>contains</strong> - Substring match, case-insensitive (like S&N's <code>.Contains()</code>)</li>
</ul>

<pre class='code-block'>// Full-text search with relevance
_fulltext: { match: ""climate policy"" }

// Substring/contains search
Title: { contains: ""climate"" }</pre>

<h3>Search Highlighting</h3>
<p>Request highlighted snippets showing where matches occurred:</p>
<pre class='code-block'>{
  ArticlePage(
    where: { _fulltext: { match: ""renewable energy"" } }
  ) {
    items {
      Title
      _highlight {
        Title
        MainBody
      }
    }
  }
}</pre>
<p>The <code>_highlight</code> field returns text with matched terms wrapped in <code>&lt;em&gt;</code> tags.</p>

<h3>Searchable vs Queryable Fields</h3>
<p class='bg-blue-50 dark:bg-blue-900/30 border-l-4 border-blue-500 p-4 my-4'>
<strong>Important:</strong> In Optimizely Graph, fields must be marked as <strong>Searchable</strong> in the schema to be included in full-text search. This is configured at the content type level in your CMS.
</p>
<ul>
<li><strong>Searchable</strong> - Included in full-text search (<code>_fulltext</code>), supports <code>match</code> operator</li>
<li><strong>Queryable</strong> - Available for filtering, sorting, and faceting with <code>contains</code> operator</li>
</ul>",
                    Examples = new List<LessonExample>
                    {
                        new()
                        {
                            Id = "migration-search-example",
                            Title = "Full-Text Search with Scoring",
                            Description = "Search content and retrieve relevance scores - equivalent to .For().GetResult()",
                            GraphQLQuery = @"{
  Content(
    where: {
      _fulltext: { match: ""optimizely"" }
    }
    orderBy: { _ranking: RELEVANCE }
    limit: 10
  ) {
    items {
      Name
      ContentType
      _score
    }
    total
  }
}",
                            IsInteractive = true,
                            Hints = new List<string>
                            {
                                "_score shows the relevance ranking like hit.Score in S&N",
                                "Use orderBy: { _ranking: RELEVANCE } for relevance-sorted results"
                            }
                        }
                    }
                },
                new()
                {
                    Id = "migration-facets",
                    ModuleId = "migration",
                    Title = "Facets & Aggregations",
                    Summary = "Migrate faceted navigation from Search & Navigation to Optimizely Graph",
                    Order = 6,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Convert S&N TermsFacet to Graph facets",
                        "Implement facet filtering",
                        "Handle hierarchical facets",
                        "Understand facet count behavior differences"
                    },
                    Content = @"<h3>Facet Comparison</h3>

<h4>Search & Navigation</h4>
<pre class='code-block'>var results = _searchClient.Search&lt;ArticlePage&gt;()
    .TermsFacetFor(x => x.Category)
    .TermsFacetFor(x => x.Author)
    .RangeFacetFor(x => x.PublishedDate,
        r => r.From(DateTime.Now.AddYears(-1)).To(DateTime.Now))
    .GetResult();

// Access facets
var categoryFacet = results.TermsFacetFor(x => x.Category);
foreach (var term in categoryFacet.Terms)
{
    Console.WriteLine($""{term.Term}: {term.Count}"");
}</pre>

<h4>Optimizely Graph</h4>
<pre class='code-block'>{
  ArticlePage(
    where: { Status: { eq: ""Published"" } }
  ) {
    items { Name Category Author }
    total
    facets {
      Category {
        name
        count
      }
      Author {
        name
        count
      }
      PublishedDate(ranges: [
        { from: ""2024-01-01"", to: ""2024-12-31"" }
      ]) {
        name
        count
      }
    }
  }
}</pre>

<h3>Facet Types</h3>
<table class='w-full border-collapse border border-slate-400 dark:border-slate-600 my-4'>
<thead>
<tr class='bg-slate-100 dark:bg-slate-700'>
<th class='border border-slate-300 dark:border-slate-600 p-2 text-left'>Type</th>
<th class='border border-slate-300 dark:border-slate-600 p-2 text-left'>S&N</th>
<th class='border border-slate-300 dark:border-slate-600 p-2 text-left'>Graph</th>
</tr>
</thead>
<tbody>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'>Terms</td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.TermsFacetFor()</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>facets { Field { name count } }</code></td></tr>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'>Date Range</td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.RangeFacetFor()</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>Field(ranges: [...]) { }</code></td></tr>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'>Numeric Range</td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.RangeFacetFor()</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>Field(ranges: [...]) { }</code></td></tr>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'>Histogram</td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.HistogramFacetFor()</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'>Range facets with intervals</td></tr>
</tbody>
</table>

<h3>Facet Limiting and Ordering</h3>
<pre class='code-block'>{
  ArticlePage {
    facets {
      Category(
        limit: 10
        orderType: COUNT
        orderBy: DESC
      ) {
        name
        count
      }
    }
  }
}</pre>

<h3>.NET Client Facets</h3>
<pre class='code-block'>var query = _queryBuilder
    .ForType&lt;ArticlePage&gt;()
    .Fields(x => x.Name, x => x.Category)
    .Facet(x => x.Category.FacetLimit(10))
    .Facet(x => x.Author.FacetLimit(5))
    .Total()
    .ToQuery()
    .BuildQueries();</pre>

<h3>Applying Facet Filters</h3>
<p>When a user selects a facet value, add it to your where clause:</p>
<pre class='code-block'>{
  ArticlePage(
    where: {
      Category: { eq: ""Technology"" }  # Selected facet value
    }
  ) {
    items { Name }
    facets {
      Category { name count }
      Author { name count }
    }
  }
}</pre>",
                    Examples = new List<LessonExample>
                    {
                        new()
                        {
                            Id = "migration-facet-example",
                            Title = "Faceted Search",
                            Description = "Get content with facet counts - equivalent to TermsFacetFor()",
                            GraphQLQuery = @"{
  Content(limit: 5) {
    items {
      Name
      ContentType
    }
    total
    facets {
      ContentType {
        name
        count
      }
    }
  }
}",
                            IsInteractive = true,
                            Hints = new List<string>
                            {
                                "Facets return aggregated counts like S&N TermsFacet",
                                "You can limit facets and control their ordering"
                            }
                        }
                    }
                },
                new()
                {
                    Id = "migration-semantic-search",
                    ModuleId = "migration",
                    Title = "Semantic Search (New in Graph)",
                    Summary = "Leverage AI-powered semantic search capabilities unique to Optimizely Graph",
                    Order = 7,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Understand what semantic search offers beyond keyword matching",
                        "Enable semantic search in your queries",
                        "Combine semantic and keyword search",
                        "Understand current limitations"
                    },
                    Content = @"<h3>What is Semantic Search?</h3>
<p>Semantic search is an AI-powered search capability <strong>not available in Search & Navigation</strong>. It understands the <em>meaning</em> and <em>intent</em> behind queries, not just keyword matches.</p>

<h4>The Vocabulary Mismatch Problem</h4>
<p>Traditional keyword search fails when users use different words than content creators:</p>
<ul>
<li>User searches: ""<em>non-alcoholic cold beverage</em>"" → Should find ""<em>cola</em>"" and ""<em>soft drinks</em>""</li>
<li>User searches: ""<em>how to fix a leaky faucet</em>"" → Should find ""<em>plumbing repair guide</em>""</li>
<li>User searches: ""<em>affordable housing options</em>"" → Should find ""<em>budget apartments</em>""</li>
</ul>

<h3>Enabling Semantic Search</h3>
<p>Add <code>_ranking: SEMANTIC</code> to your queries:</p>
<pre class='code-block'>{
  ArticlePage(
    where: {
      _fulltext: { match: ""renewable energy alternatives"" }
    }
    orderBy: { _ranking: SEMANTIC }
  ) {
    items {
      Name
      Title
      _score
    }
    total
  }
}</pre>

<h3>Ranking Options</h3>
<table class='w-full border-collapse border border-slate-400 dark:border-slate-600 my-4'>
<thead>
<tr class='bg-slate-100 dark:bg-slate-700'>
<th class='border border-slate-300 dark:border-slate-600 p-2 text-left'>Ranking</th>
<th class='border border-slate-300 dark:border-slate-600 p-2 text-left'>Description</th>
<th class='border border-slate-300 dark:border-slate-600 p-2 text-left'>Best For</th>
</tr>
</thead>
<tbody>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'><code>RELEVANCE</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'>Traditional keyword relevance (like S&N)</td><td class='border border-slate-300 dark:border-slate-600 p-2'>Exact term matching</td></tr>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'><code>SEMANTIC</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'>AI-powered contextual understanding</td><td class='border border-slate-300 dark:border-slate-600 p-2'>Natural language queries</td></tr>
</tbody>
</table>

<h3>Combining with Date Sorting</h3>
<p>Use semantic relevance as a tiebreaker after primary sort:</p>
<pre class='code-block'>{
  ArticlePage(
    where: { _fulltext: { match: ""climate policy"" } }
    orderBy: [
      { PublishedDate: DESC }
      { _ranking: SEMANTIC }
    ]
  ) {
    items { Name PublishedDate _score }
  }
}</pre>

<h3>Use Cases for Semantic Search</h3>
<ul>
<li><strong>Natural language queries</strong> - ""How do I return a product?""</li>
<li><strong>Conceptual searches</strong> - Finding related content by meaning</li>
<li><strong>Chatbots/RAG</strong> - Retrieval Augmented Generation for AI assistants</li>
<li><strong>Content recommendations</strong> - Finding semantically similar content</li>
</ul>

<h3>Current Limitations</h3>
<p class='bg-amber-50 dark:bg-amber-900/30 border-l-4 border-amber-500 p-4 my-4'>
<strong>Note:</strong> Semantic search currently supports <strong>English only</strong>. Multi-language semantic search may be added in future releases.
</p>

<h3>.NET Client Semantic Search</h3>
<pre class='code-block'>var query = _queryBuilder
    .ForType&lt;ArticlePage&gt;()
    .Search(""renewable energy alternatives"")
    .OrderBy(Ranking.SEMANTIC)
    .Fields(x => x.Name, x => x.Title)
    .ToQuery()
    .BuildQueries();</pre>",
                    Examples = new List<LessonExample>
                    {
                        new()
                        {
                            Id = "migration-semantic-example",
                            Title = "Semantic Search Query",
                            Description = "Use AI-powered semantic search for contextual understanding",
                            GraphQLQuery = @"{
  Content(
    where: {
      _fulltext: { match: ""how to get started"" }
    }
    orderBy: { _ranking: SEMANTIC }
    limit: 10
  ) {
    items {
      Name
      ContentType
      _score
    }
    total
  }
}",
                            IsInteractive = true,
                            Hints = new List<string>
                            {
                                "SEMANTIC ranking uses AI to understand query intent",
                                "Results may include content that doesn't contain exact keywords"
                            }
                        }
                    }
                },
                new()
                {
                    Id = "migration-dotnet-client",
                    ModuleId = "migration",
                    Title = "Using the .NET Client SDK",
                    Summary = "Learn the Optimizely Graph .NET Client for a familiar fluent API experience",
                    Order = 8,
                    EstimatedMinutes = 15,
                    LearningObjectives = new List<string>
                    {
                        "Set up the .NET Client in your project",
                        "Build queries using the fluent API",
                        "Understand the mapping from S&N patterns",
                        "Execute queries and handle responses"
                    },
                    Content = @"<h3>Why Use the .NET Client?</h3>
<p>The Optimizely Graph .NET Client provides a familiar fluent API similar to Search & Navigation, making migration easier for C# developers.</p>

<h3>Setup</h3>
<pre class='code-block'>// Register in DI container
services.AddContentGraphClient();

// Inject in your service
public class SearchService
{
    private readonly IContentGraphClient _graphClient;
    private readonly IQueryBuilder _queryBuilder;

    public SearchService(
        IContentGraphClient graphClient,
        IQueryBuilder queryBuilder)
    {
        _graphClient = graphClient;
        _queryBuilder = queryBuilder;
    }
}</pre>

<h3>Building Queries</h3>
<p>The .NET Client uses a builder pattern similar to S&N:</p>
<pre class='code-block'>// Search & Navigation (old)
var sn_results = _searchClient.Search&lt;ArticlePage&gt;()
    .For(""optimizely"")
    .Filter(x => x.Status.Match(""Published""))
    .OrderByDescending(x => x.PublishedDate)
    .Skip(0)
    .Take(10)
    .GetResult();

// Optimizely Graph .NET Client (new)
var query = _queryBuilder
    .ForType&lt;ArticlePage&gt;()
    .Search(""optimizely"")
    .Where(x => x.Status.Eq(""Published""))
    .OrderBy(x => x.PublishedDate, OrderMode.DESC)
    .Skip(0)
    .Limit(10)
    .Fields(x => x.Name, x => x.Title, x => x.PublishedDate)
    .Total()
    .ToQuery()
    .BuildQueries();

var result = await _graphClient.RunQueryAsync(query);</pre>

<h3>Query Builder Methods</h3>
<table class='w-full border-collapse border border-slate-400 dark:border-slate-600 my-4'>
<thead>
<tr class='bg-slate-100 dark:bg-slate-700'>
<th class='border border-slate-300 dark:border-slate-600 p-2 text-left'>S&N Method</th>
<th class='border border-slate-300 dark:border-slate-600 p-2 text-left'>Graph .NET Client Method</th>
</tr>
</thead>
<tbody>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'><code>Search&lt;T&gt;()</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>ForType&lt;T&gt;()</code></td></tr>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.For(""text"")</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.Search(""text"")</code></td></tr>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.Filter(x => ...)</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.Where(x => ...)</code></td></tr>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.OrderBy(x => ...)</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.OrderBy(x => ..., OrderMode.ASC)</code></td></tr>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.OrderByDescending(x => ...)</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.OrderBy(x => ..., OrderMode.DESC)</code></td></tr>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.Skip(n)</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.Skip(n)</code></td></tr>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.Take(n)</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.Limit(n)</code></td></tr>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.TermsFacetFor(x => ...)</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.Facet(x => x.Field.FacetLimit(n))</code></td></tr>
<tr><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.GetResult()</code></td><td class='border border-slate-300 dark:border-slate-600 p-2'><code>.ToQuery().BuildQueries()</code> then <code>RunQueryAsync()</code></td></tr>
</tbody>
</table>

<h3>Filter Operators</h3>
<pre class='code-block'>// String operators
.Where(x => x.Title.Eq(""exact match""))
.Where(x => x.Title.Contains(""partial""))
.Where(x => x.Title.Match(""full text search""))
.Where(x => x.Status.In(""Draft"", ""Published""))

// Numeric operators
.Where(x => x.ViewCount.Gt(100))
.Where(x => x.ViewCount.Lt(1000))
.Where(x => x.ViewCount.InRange(100, 1000))

// Date operators
.Where(x => x.PublishedDate.Gte(DateTime.Now.AddMonths(-6)))
.Where(x => x.PublishedDate.Lte(DateTime.Now))

// Boolean combinations
.Where(x => x.Title.Match(""news"") & x.Status.Eq(""Published""))
.Where(x => x.Category.Eq(""Tech"") | x.Category.Eq(""Science""))</pre>

<h3>Handling Responses</h3>
<pre class='code-block'>var result = await _graphClient.RunQueryAsync(query);

// Access items
foreach (var item in result.Data.ArticlePage.Items)
{
    Console.WriteLine($""{item.Name} - {item.PublishedDate}"");
}

// Get total count
var total = result.Data.ArticlePage.Total;

// Access facets
foreach (var facet in result.Data.ArticlePage.Facets.Category)
{
    Console.WriteLine($""{facet.Name}: {facet.Count}"");
}</pre>",
                    Examples = new List<LessonExample>()
                },
                new()
                {
                    Id = "migration-best-practices",
                    ModuleId = "migration",
                    Title = "Migration Best Practices",
                    Summary = "Strategies and tips for a successful migration from Search & Navigation",
                    Order = 9,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Plan an incremental migration strategy",
                        "Test and validate migrated queries",
                        "Handle missing S&N features",
                        "Optimize performance post-migration"
                    },
                    Content = @"<h3>Migration Strategy</h3>
<p>Follow these steps for a successful migration:</p>

<h4>1. Run in Parallel</h4>
<p>Keep Search & Navigation running while you migrate:</p>
<pre class='code-block'>// Feature flag to switch between systems
if (_featureFlags.UseOptimizelyGraph)
{
    return await SearchWithGraph(query);
}
else
{
    return SearchWithSN(query);
}</pre>

<h4>2. Migrate Incrementally</h4>
<p>Migrate one search feature at a time:</p>
<ol>
<li>Start with simple listing queries</li>
<li>Move to filtered searches</li>
<li>Migrate faceted navigation</li>
<li>Convert full-text search last</li>
</ol>

<h4>3. Test Thoroughly</h4>
<p>Compare results between systems:</p>
<pre class='code-block'>// Log both results for comparison
var snResults = SearchWithSN(query);
var graphResults = await SearchWithGraph(query);

_logger.LogInformation(
    ""S&N: {SNCount} results, Graph: {GraphCount} results"",
    snResults.TotalMatching,
    graphResults.Total);</pre>

<h3>Handling Missing Features</h3>

<h4>Best Bets Alternative</h4>
<p>Use query boosting to promote specific content:</p>
<pre class='code-block'>{
  ArticlePage(
    where: {
      _or: [
        { ContentGuid: { in: [""promoted-guid-1"", ""promoted-guid-2""] } }
        { _fulltext: { match: ""search term"" } }
      ]
    }
    orderBy: [
      { _boost: DESC }  # Boosted items first
      { _ranking: RELEVANCE }
    ]
  ) { ... }
}</pre>

<h4>Tracking Alternative</h4>
<p>Implement custom analytics:</p>
<ul>
<li>Log search queries and clicks to your analytics platform</li>
<li>Use Google Analytics events for search tracking</li>
<li>Build a custom search analytics service</li>
</ul>

<h3>Performance Optimization</h3>

<h4>Select Only Needed Fields</h4>
<pre class='code-block'>// Bad: Returns all fields (like S&N default)
.Fields(x => x)

// Good: Select only what you need
.Fields(x => x.Name, x => x.Url, x => x.PublishedDate)</pre>

<h4>Use Cached Templates</h4>
<p>For frequently-run queries, use cached templates to reduce parsing overhead.</p>

<h4>Leverage Edge Caching</h4>
<p>Optimizely Graph runs on a CDN edge network. Design queries to be cache-friendly:</p>
<ul>
<li>Avoid highly personalized queries where possible</li>
<li>Use consistent query structures</li>
<li>Consider query result caching in your application</li>
</ul>

<h3>Common Migration Pitfalls</h3>
<ul>
<li><strong>Forgetting to select fields</strong> - GraphQL requires explicit field selection</li>
<li><strong>Over-fetching</strong> - Select only needed fields, not entire objects</li>
<li><strong>Ignoring pagination</strong> - Always use limit to avoid large result sets</li>
<li><strong>Missing error handling</strong> - Graph queries can fail; handle errors gracefully</li>
</ul>

<h3>Helpful Resources</h3>
<ul>
<li><a href='https://docs.developers.optimizely.com/platform-optimizely/docs/how-to-migrate-sn-to-content-graph' target='_blank'>Official Migration Guide</a></li>
<li><a href='https://github.com/episerver/graph-net-sdk' target='_blank'>Graph .NET SDK Repository</a></li>
<li><a href='https://world.optimizely.com/forum/' target='_blank'>Optimizely World Forum</a></li>
</ul>",
                    Examples = new List<LessonExample>()
                }
            }
        };
    }
}
