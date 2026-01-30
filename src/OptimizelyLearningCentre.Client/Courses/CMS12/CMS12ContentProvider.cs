using OptimizelyLearningCentre.Client.Models.Learning;
using OptimizelyLearningCentre.Client.Services;

namespace OptimizelyLearningCentre.Client.Courses.CMS12;

/// <summary>
/// Content provider for the Optimizely CMS 12 (PaaS) course
/// </summary>
public class CMS12ContentProvider : ILearningContentProvider
{
    private List<LearningModule>? _modules;

    public async Task<List<LearningModule>> GetModulesAsync()
    {
        if (_modules == null)
        {
            _modules = BuildModules();
            LinkLessons(_modules);
        }
        return await Task.FromResult(_modules);
    }

    public async Task<LearningModule?> GetModuleAsync(string moduleId)
    {
        var modules = await GetModulesAsync();
        return modules.FirstOrDefault(m => m.Id == moduleId);
    }

    public async Task<Lesson?> GetLessonAsync(string lessonId)
    {
        var modules = await GetModulesAsync();
        return modules.SelectMany(m => m.Lessons).FirstOrDefault(l => l.Id == lessonId);
    }

    private void LinkLessons(List<LearningModule> modules)
    {
        foreach (var module in modules)
        {
            var orderedLessons = module.Lessons.OrderBy(l => l.Order).ToList();
            for (int i = 0; i < orderedLessons.Count; i++)
            {
                if (i > 0)
                    orderedLessons[i].PreviousLessonId = orderedLessons[i - 1].Id;
                if (i < orderedLessons.Count - 1)
                    orderedLessons[i].NextLessonId = orderedLessons[i + 1].Id;
            }
        }
    }

    private List<LearningModule> BuildModules()
    {
        return new List<LearningModule>
        {
            BuildGettingStartedModule(),
            BuildContentTypesModule(),
            BuildTemplatesRenderingModule(),
            BuildContentManagementModule(),
            BuildInitializationEventsModule(),
            BuildLocalizationModule(),
            BuildSearchNavigationModule(),
            BuildFormsModule(),
            BuildAccessRightsModule(),
            BuildCachingPerformanceModule(),
            BuildScheduledJobsAdvancedModule()
        };
    }

    #region Module 1: Getting Started

    private LearningModule BuildGettingStartedModule()
    {
        return new LearningModule
        {
            Id = "getting-started",
            Title = "Getting Started with CMS 12",
            Description = "Learn the fundamentals of Optimizely CMS 12, installation, and project setup.",
            Icon = "academic-cap",
            Order = 1,
            Difficulty = ModuleDifficulty.Beginner,
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "gs-what-is-cms12",
                    ModuleId = "getting-started",
                    Title = "What is Optimizely CMS 12?",
                    Summary = "Discover Optimizely CMS 12 and its capabilities as a .NET-based content management system.",
                    Order = 1,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Understand what Optimizely CMS 12 is and its purpose",
                        "Learn the key features and benefits",
                        "Understand the difference between PaaS and SaaS offerings"
                    },
                    Content = @"
<h2>Introduction to Optimizely CMS 12</h2>
<p>Optimizely CMS 12 (formerly Episerver CMS) is a <strong>powerful enterprise content management system</strong> built on ASP.NET Core. It provides a flexible platform for building websites, intranets, and digital experiences.</p>

<h3>Key Features</h3>
<ul>
    <li><strong>Built on .NET 8+</strong> - Modern, cross-platform, high-performance framework</li>
    <li><strong>Content-first approach</strong> - Strongly-typed content models with rich editing experience</li>
    <li><strong>On-page editing</strong> - WYSIWYG editing directly on the rendered page</li>
    <li><strong>Multi-site support</strong> - Host multiple websites from a single installation</li>
    <li><strong>Multilingual</strong> - Built-in support for content in multiple languages</li>
    <li><strong>Extensible</strong> - Rich API for customization and integration</li>
</ul>

<h3>PaaS vs SaaS</h3>
<p>Optimizely offers two deployment models:</p>
<table class=""min-w-full divide-y divide-gray-200 dark:divide-gray-700 my-4"">
    <thead>
        <tr>
            <th class=""px-4 py-2 text-left"">Feature</th>
            <th class=""px-4 py-2 text-left"">CMS 12 (PaaS)</th>
            <th class=""px-4 py-2 text-left"">CMS (SaaS)</th>
        </tr>
    </thead>
    <tbody>
        <tr><td class=""px-4 py-2"">Hosting</td><td class=""px-4 py-2"">Self-hosted or DXP Cloud</td><td class=""px-4 py-2"">Fully managed by Optimizely</td></tr>
        <tr><td class=""px-4 py-2"">Architecture</td><td class=""px-4 py-2"">Traditional MVC or headless</td><td class=""px-4 py-2"">Headless-first</td></tr>
        <tr><td class=""px-4 py-2"">Customization</td><td class=""px-4 py-2"">Full code access</td><td class=""px-4 py-2"">Via APIs and configuration</td></tr>
        <tr><td class=""px-4 py-2"">Rendering</td><td class=""px-4 py-2"">Server-side Razor views</td><td class=""px-4 py-2"">Client-side via Graph API</td></tr>
        <tr><td class=""px-4 py-2"">Updates</td><td class=""px-4 py-2"">Manual NuGet upgrades</td><td class=""px-4 py-2"">Automatic, versionless</td></tr>
    </tbody>
</table>

<h3>When to Choose CMS 12 (PaaS)</h3>
<ul>
    <li>You need full control over the codebase and hosting</li>
    <li>You have existing .NET development expertise</li>
    <li>You require server-side rendering for SEO or performance</li>
    <li>You need deep customization of the editorial experience</li>
    <li>You're migrating from an earlier Episerver/Optimizely version</li>
</ul>
",
                    Examples = new List<LessonExample>()
                },
                new Lesson
                {
                    Id = "gs-prerequisites",
                    ModuleId = "getting-started",
                    Title = "Prerequisites & System Requirements",
                    Summary = "Understand the tools and requirements needed to develop with CMS 12.",
                    Order = 2,
                    EstimatedMinutes = 6,
                    LearningObjectives = new List<string>
                    {
                        "Know the system requirements for CMS 12 development",
                        "Understand which tools to install",
                        "Learn about the Optimizely NuGet feed"
                    },
                    Content = @"
<h2>System Requirements</h2>
<p>Before you begin developing with Optimizely CMS 12, ensure you have the following:</p>

<h3>Development Tools</h3>
<ul>
    <li><strong>.NET 8.0 SDK</strong> (or later) - <a href=""https://dotnet.microsoft.com/download"" target=""_blank"">Download here</a></li>
    <li><strong>Visual Studio 2022</strong> (recommended) or VS Code with C# extension</li>
    <li><strong>SQL Server</strong> - LocalDB (included with VS), SQL Server Express, or full SQL Server</li>
    <li><strong>Node.js</strong> (optional) - For frontend build tools</li>
</ul>

<h3>Optimizely NuGet Feed</h3>
<p>Optimizely packages are hosted on a dedicated NuGet feed. Add this feed to your NuGet configuration:</p>

<h3>Installing the CLI Tools</h3>
<p>The Optimizely CLI helps with project setup and database management:</p>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "nuget-config",
                            Title = "NuGet.config Setup",
                            Description = "Add the Optimizely NuGet feed to your configuration",
                            Type = ExampleType.Configuration,
                            ExampleContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <packageSources>
    <add key=""nuget.org"" value=""https://api.nuget.org/v3/index.json"" />
    <add key=""Optimizely"" value=""https://nuget.optimizely.com/feed/packages.svc/"" />
  </packageSources>
</configuration>",
                            IsInteractive = false
                        },
                        new LessonExample
                        {
                            Id = "install-cli",
                            Title = "Install Optimizely CLI",
                            Description = "Install the dotnet CLI tool globally",
                            Type = ExampleType.Code,
                            ExampleContent = @"# Install the Optimizely templates
dotnet new install EPiServer.Templates

# Install the Optimizely CLI tool
dotnet tool install EPiServer.Net.Cli --global

# Verify installation
dotnet-episerver --help",
                            IsInteractive = false
                        }
                    }
                },
                new Lesson
                {
                    Id = "gs-create-project",
                    ModuleId = "getting-started",
                    Title = "Creating Your First Project",
                    Summary = "Create a new Optimizely CMS 12 project from scratch.",
                    Order = 3,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Create a new CMS 12 project using templates",
                        "Understand the project creation options",
                        "Run the initial database setup"
                    },
                    Content = @"
<h2>Creating a New Project</h2>
<p>Optimizely provides project templates that set up a working CMS installation. You can create either an empty project or use the Alloy sample site.</p>

<h3>Option 1: Empty CMS Project</h3>
<p>An empty project gives you a clean starting point with minimal content types:</p>

<h3>Option 2: Alloy Sample Site</h3>
<p>The Alloy template includes sample content types, pages, and styling - great for learning:</p>

<h3>Database Setup</h3>
<p>After creating the project, set up the database using the CLI:</p>

<h3>Running the Site</h3>
<p>Once the database is created, run the project:</p>
<ol>
    <li>Navigate to <code>https://localhost:5001</code> (or the configured port)</li>
    <li>You'll be prompted to create an admin user on first run</li>
    <li>Access the CMS edit interface at <code>/episerver/cms</code></li>
</ol>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "create-empty-project",
                            Title = "Create Empty Project",
                            Description = "Create a new empty CMS 12 project",
                            Type = ExampleType.Code,
                            ExampleContent = @"# Create a new empty CMS project
dotnet new epicmsempty --name MyOptimizely

# Navigate to the project
cd MyOptimizely

# Restore packages
dotnet restore",
                            IsInteractive = false
                        },
                        new LessonExample
                        {
                            Id = "create-alloy-project",
                            Title = "Create Alloy Sample Site",
                            Description = "Create a project with sample content and styling",
                            Type = ExampleType.Code,
                            ExampleContent = @"# Create a new Alloy sample site
dotnet new epi-alloy-mvc --name MyAlloySite

# Navigate to the project
cd MyAlloySite

# Restore packages
dotnet restore",
                            IsInteractive = false
                        },
                        new LessonExample
                        {
                            Id = "setup-database",
                            Title = "Database Setup",
                            Description = "Create and configure the CMS database",
                            Type = ExampleType.Code,
                            ExampleContent = @"# Create the CMS database (uses connection string from appsettings.json)
dotnet-episerver create-cms-database

# Or specify a custom connection string
dotnet-episerver create-cms-database -c ""Server=(localdb)\\MSSQLLocalDB;Database=MyOptimizely;Integrated Security=True""

# Run the application
dotnet run",
                            IsInteractive = false
                        }
                    }
                },
                new Lesson
                {
                    Id = "gs-project-structure",
                    ModuleId = "getting-started",
                    Title = "Project Structure Explained",
                    Summary = "Understand the folder structure and key files in a CMS 12 project.",
                    Order = 4,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Understand the recommended project structure",
                        "Know the purpose of key configuration files",
                        "Learn where to place different types of code"
                    },
                    Content = @"
<h2>CMS 12 Project Structure</h2>
<p>A well-organized project structure helps maintain clean, scalable code. Here's the recommended layout:</p>

<h3>Folder Structure</h3>
<pre class=""bg-gray-100 dark:bg-gray-800 p-4 rounded-lg overflow-x-auto"">
MyOptimizely/
├── App_Data/                 # Database files, logs, blob storage
├── Business/                 # Business logic, services, helpers
│   ├── Initialization/      # Initialization modules
│   ├── Rendering/           # Custom renderers
│   └── Services/            # Custom services
├── Controllers/              # MVC controllers
├── Models/                   # Content types and view models
│   ├── Blocks/              # Block content types
│   ├── Media/               # Media content types
│   ├── Pages/               # Page content types
│   └── ViewModels/          # View models
├── Views/                    # Razor views
│   ├── Blocks/              # Block partial views
│   ├── Pages/               # Page views
│   └── Shared/              # Layouts and shared partials
├── wwwroot/                  # Static files (CSS, JS, images)
├── appsettings.json          # Application configuration
├── Program.cs                # Application entry point
└── Startup.cs                # Service configuration (if used)
</pre>

<h3>Key Configuration Files</h3>
<table class=""min-w-full divide-y divide-gray-200 dark:divide-gray-700 my-4"">
    <thead>
        <tr>
            <th class=""px-4 py-2 text-left"">File</th>
            <th class=""px-4 py-2 text-left"">Purpose</th>
        </tr>
    </thead>
    <tbody>
        <tr><td class=""px-4 py-2 font-mono text-sm"">appsettings.json</td><td class=""px-4 py-2"">Connection strings, logging, CMS options</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">Program.cs</td><td class=""px-4 py-2"">Application bootstrap, service registration</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">module.config</td><td class=""px-4 py-2"">Module dependencies (for add-ons)</td></tr>
    </tbody>
</table>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "appsettings-example",
                            Title = "appsettings.json",
                            Description = "Typical CMS 12 configuration file",
                            Type = ExampleType.Configuration,
                            ExampleContent = @"{
  ""ConnectionStrings"": {
    ""EPiServerDB"": ""Server=(localdb)\\MSSQLLocalDB;Database=MyOptimizely;Integrated Security=True;MultipleActiveResultSets=True""
  },
  ""EPiServer"": {
    ""Cms"": {
      ""MappedRoles"": {
        ""CmsAdmins"": [""WebAdmins"", ""Administrators""],
        ""CmsEditors"": [""WebEditors""]
      }
    }
  },
  ""Logging"": {
    ""LogLevel"": {
      ""Default"": ""Warning"",
      ""EPiServer"": ""Warning""
    }
  }
}",
                            IsInteractive = false
                        },
                        new LessonExample
                        {
                            Id = "program-cs-example",
                            Title = "Program.cs",
                            Description = "Minimal CMS 12 Program.cs setup",
                            Type = ExampleType.Code,
                            ExampleContent = @"var builder = WebApplication.CreateBuilder(args);

// Add Optimizely CMS services
builder.Services.AddCms();

// Add MVC services
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapContent();
app.MapControllers();

app.Run();",
                            IsInteractive = false
                        }
                    }
                },
                new Lesson
                {
                    Id = "gs-edit-interface",
                    ModuleId = "getting-started",
                    Title = "The Editorial Interface",
                    Summary = "Navigate the CMS edit interface and understand the editorial experience.",
                    Order = 5,
                    EstimatedMinutes = 7,
                    LearningObjectives = new List<string>
                    {
                        "Navigate the CMS editorial interface",
                        "Understand the page tree and content structure",
                        "Learn about on-page editing"
                    },
                    Content = @"
<h2>CMS Editorial Interface</h2>
<p>The Optimizely CMS editorial interface provides a powerful environment for content editors to create and manage content.</p>

<h3>Accessing the Interface</h3>
<p>Navigate to <code>/episerver/cms</code> to access the editorial interface. You'll need to log in with an account that has editor permissions.</p>

<h3>Key Areas</h3>
<ul>
    <li><strong>Page Tree</strong> - Hierarchical view of all pages on the site</li>
    <li><strong>Assets Panel</strong> - Manage media files and shared blocks</li>
    <li><strong>Properties Panel</strong> - Edit content properties in forms view</li>
    <li><strong>On-Page Editing</strong> - Edit content directly on the rendered page</li>
    <li><strong>Version History</strong> - View and restore previous versions</li>
    <li><strong>Publishing</strong> - Publish content or schedule for future publication</li>
</ul>

<h3>Edit Modes</h3>
<table class=""min-w-full divide-y divide-gray-200 dark:divide-gray-700 my-4"">
    <thead>
        <tr>
            <th class=""px-4 py-2 text-left"">Mode</th>
            <th class=""px-4 py-2 text-left"">Description</th>
        </tr>
    </thead>
    <tbody>
        <tr><td class=""px-4 py-2"">On-Page Edit</td><td class=""px-4 py-2"">Edit content directly on the rendered page with inline editing</td></tr>
        <tr><td class=""px-4 py-2"">All Properties</td><td class=""px-4 py-2"">Form-based view showing all content properties</td></tr>
        <tr><td class=""px-4 py-2"">Preview</td><td class=""px-4 py-2"">View the page as visitors will see it</td></tr>
        <tr><td class=""px-4 py-2"">Compare</td><td class=""px-4 py-2"">Compare different versions side by side</td></tr>
    </tbody>
</table>

<h3>Content Status</h3>
<p>Content in the CMS can have different statuses:</p>
<ul>
    <li><strong>Not Published</strong> - Content has never been published</li>
    <li><strong>Published</strong> - Content is live on the site</li>
    <li><strong>Previously Published</strong> - Changes have been made but not yet published</li>
    <li><strong>Scheduled</strong> - Content will be published at a future date</li>
    <li><strong>Expired</strong> - Content has passed its expiration date</li>
</ul>
",
                    Examples = new List<LessonExample>()
                }
            }
        };
    }

    #endregion

    #region Module 2: Content Types

    private LearningModule BuildContentTypesModule()
    {
        return new LearningModule
        {
            Id = "content-types",
            Title = "Content Types",
            Description = "Learn to create and configure page types, block types, and media types.",
            Icon = "document-duplicate",
            Order = 2,
            Difficulty = ModuleDifficulty.Beginner,
            Prerequisites = new[] { "getting-started" },
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "ct-understanding-content",
                    ModuleId = "content-types",
                    Title = "Understanding Content Types",
                    Summary = "Learn the fundamentals of content types in Optimizely CMS.",
                    Order = 1,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Understand what content types are",
                        "Know the different categories of content types",
                        "Learn how content types map to .NET classes"
                    },
                    Content = @"
<h2>What are Content Types?</h2>
<p>In Optimizely CMS, a <strong>content type</strong> defines the structure and properties of content. Think of it as a template that defines what data a piece of content can hold.</p>

<h3>The Content Type Hierarchy</h3>
<ul>
    <li><strong>IContent</strong> - The base interface for all content</li>
    <li><strong>IContentData</strong> - Adds property access to content</li>
    <li><strong>PageData</strong> - Base class for pages (routable content)</li>
    <li><strong>BlockData</strong> - Base class for blocks (reusable content components)</li>
    <li><strong>MediaData</strong> - Base class for media files</li>
</ul>

<h3>Content Type Categories</h3>
<table class=""min-w-full divide-y divide-gray-200 dark:divide-gray-700 my-4"">
    <thead>
        <tr>
            <th class=""px-4 py-2 text-left"">Type</th>
            <th class=""px-4 py-2 text-left"">Base Class</th>
            <th class=""px-4 py-2 text-left"">Use Case</th>
        </tr>
    </thead>
    <tbody>
        <tr><td class=""px-4 py-2"">Pages</td><td class=""px-4 py-2 font-mono text-sm"">PageData</td><td class=""px-4 py-2"">Routable content with URLs (articles, landing pages)</td></tr>
        <tr><td class=""px-4 py-2"">Blocks</td><td class=""px-4 py-2 font-mono text-sm"">BlockData</td><td class=""px-4 py-2"">Reusable content components (teasers, banners)</td></tr>
        <tr><td class=""px-4 py-2"">Media</td><td class=""px-4 py-2 font-mono text-sm"">MediaData</td><td class=""px-4 py-2"">Files with metadata (images, documents, videos)</td></tr>
        <tr><td class=""px-4 py-2"">Folders</td><td class=""px-4 py-2 font-mono text-sm"">ContentFolder</td><td class=""px-4 py-2"">Organize content in the tree</td></tr>
    </tbody>
</table>

<h3>How Content Types Work</h3>
<ol>
    <li>You define a .NET class inheriting from the appropriate base class</li>
    <li>You decorate it with the <code>[ContentType]</code> attribute</li>
    <li>CMS scans for these classes during startup</li>
    <li>For each class, a content type definition is created in the database</li>
    <li>Editors can then create instances of your content type</li>
</ol>
",
                    Examples = new List<LessonExample>()
                },
                new Lesson
                {
                    Id = "ct-creating-pages",
                    ModuleId = "content-types",
                    Title = "Creating Page Types",
                    Summary = "Learn how to create page types that editors can use to build your site.",
                    Order = 2,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Create a basic page type",
                        "Use the ContentType attribute",
                        "Define page properties"
                    },
                    Content = @"
<h2>Creating Page Types</h2>
<p>Page types are the foundation of your site structure. Each page type represents a different kind of page (e.g., article, product, landing page).</p>

<h3>Basic Page Type</h3>
<p>A page type is a class that:</p>
<ul>
    <li>Inherits from <code>PageData</code></li>
    <li>Is decorated with <code>[ContentType]</code></li>
    <li>Has public virtual properties for content</li>
</ul>

<h3>Important Attributes</h3>
<table class=""min-w-full divide-y divide-gray-200 dark:divide-gray-700 my-4"">
    <thead>
        <tr>
            <th class=""px-4 py-2 text-left"">Attribute</th>
            <th class=""px-4 py-2 text-left"">Purpose</th>
        </tr>
    </thead>
    <tbody>
        <tr><td class=""px-4 py-2 font-mono text-sm"">[ContentType]</td><td class=""px-4 py-2"">Marks the class as a content type with display name and description</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">GUID</td><td class=""px-4 py-2"">Unique identifier - allows renaming the class without losing data</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">GroupName</td><td class=""px-4 py-2"">Groups content types in the ""New Page"" dialog</td></tr>
    </tbody>
</table>

<h3>Why Properties Must Be Virtual</h3>
<p>Properties must be declared as <code>virtual</code> because CMS creates a proxy class that overrides them to read/write from the underlying data store.</p>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "basic-page-type",
                            Title = "Article Page Type",
                            Description = "A complete page type for articles",
                            Type = ExampleType.Code,
                            ExampleContent = @"using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace MyOptimizely.Models.Pages
{
    [ContentType(
        GUID = ""a1b2c3d4-e5f6-7890-abcd-ef1234567890"",
        DisplayName = ""Article Page"",
        Description = ""A page for articles and blog posts"",
        GroupName = ""Content"")]
    public class ArticlePage : PageData
    {
        [CultureSpecific]
        [Display(
            Name = ""Title"",
            Description = ""The main title of the article"",
            GroupName = SystemTabNames.Content,
            Order = 10)]
        public virtual string? Title { get; set; }

        [CultureSpecific]
        [Display(
            Name = ""Introduction"",
            Description = ""A short introduction to the article"",
            GroupName = SystemTabNames.Content,
            Order = 20)]
        public virtual string? Introduction { get; set; }

        [CultureSpecific]
        [Display(
            Name = ""Main Body"",
            Description = ""The main content of the article"",
            GroupName = SystemTabNames.Content,
            Order = 30)]
        public virtual XhtmlString? MainBody { get; set; }

        [Display(
            Name = ""Published Date"",
            GroupName = SystemTabNames.Content,
            Order = 40)]
        public virtual DateTime PublishedDate { get; set; }
    }
}",
                            IsInteractive = false
                        }
                    }
                },
                new Lesson
                {
                    Id = "ct-creating-blocks",
                    ModuleId = "content-types",
                    Title = "Creating Block Types",
                    Summary = "Create reusable content blocks that can be placed on pages.",
                    Order = 3,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Understand the purpose of blocks",
                        "Create block types",
                        "Know when to use blocks vs pages"
                    },
                    Content = @"
<h2>Creating Block Types</h2>
<p>Blocks are reusable content components that can be placed in content areas on pages. They're perfect for:</p>
<ul>
    <li>Teasers and promotional banners</li>
    <li>Call-to-action components</li>
    <li>Navigation elements</li>
    <li>Content that appears on multiple pages</li>
</ul>

<h3>Block vs Page</h3>
<table class=""min-w-full divide-y divide-gray-200 dark:divide-gray-700 my-4"">
    <thead>
        <tr>
            <th class=""px-4 py-2 text-left"">Feature</th>
            <th class=""px-4 py-2 text-left"">Page</th>
            <th class=""px-4 py-2 text-left"">Block</th>
        </tr>
    </thead>
    <tbody>
        <tr><td class=""px-4 py-2"">Has URL</td><td class=""px-4 py-2"">Yes</td><td class=""px-4 py-2"">No (rendered within pages)</td></tr>
        <tr><td class=""px-4 py-2"">Location</td><td class=""px-4 py-2"">Page tree</td><td class=""px-4 py-2"">Assets panel or inline in pages</td></tr>
        <tr><td class=""px-4 py-2"">Reusability</td><td class=""px-4 py-2"">Single location</td><td class=""px-4 py-2"">Can be shared across pages</td></tr>
        <tr><td class=""px-4 py-2"">Base class</td><td class=""px-4 py-2"">PageData</td><td class=""px-4 py-2"">BlockData</td></tr>
    </tbody>
</table>

<h3>Shared vs Local Blocks</h3>
<ul>
    <li><strong>Shared blocks</strong> - Created in Assets panel, can be used on multiple pages</li>
    <li><strong>Local blocks</strong> - Created inline in a ContentArea, belong to that page only</li>
</ul>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "teaser-block",
                            Title = "Teaser Block Type",
                            Description = "A promotional teaser block",
                            Type = ExampleType.Code,
                            ExampleContent = @"using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace MyOptimizely.Models.Blocks
{
    [ContentType(
        GUID = ""b2c3d4e5-f6a7-8901-bcde-f23456789012"",
        DisplayName = ""Teaser Block"",
        Description = ""A promotional teaser with image and link"",
        GroupName = ""Content"")]
    public class TeaserBlock : BlockData
    {
        [CultureSpecific]
        [Display(
            Name = ""Heading"",
            Description = ""The teaser heading"",
            GroupName = SystemTabNames.Content,
            Order = 10)]
        public virtual string? Heading { get; set; }

        [CultureSpecific]
        [Display(
            Name = ""Text"",
            Description = ""The teaser text"",
            GroupName = SystemTabNames.Content,
            Order = 20)]
        [UIHint(UIHint.Textarea)]
        public virtual string? Text { get; set; }

        [Display(
            Name = ""Image"",
            Description = ""The teaser image"",
            GroupName = SystemTabNames.Content,
            Order = 30)]
        [UIHint(UIHint.Image)]
        public virtual ContentReference? Image { get; set; }

        [Display(
            Name = ""Link"",
            Description = ""The link destination"",
            GroupName = SystemTabNames.Content,
            Order = 40)]
        public virtual Url? Link { get; set; }

        [Display(
            Name = ""Link Text"",
            GroupName = SystemTabNames.Content,
            Order = 50)]
        public virtual string? LinkText { get; set; }
    }
}",
                            IsInteractive = false
                        }
                    }
                },
                new Lesson
                {
                    Id = "ct-property-types",
                    ModuleId = "content-types",
                    Title = "Built-in Property Types",
                    Summary = "Explore the built-in property types available for content modeling.",
                    Order = 4,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Know the built-in property types",
                        "Choose the right property type for your data",
                        "Use UIHints to customize editing experience"
                    },
                    Content = @"
<h2>Built-in Property Types</h2>
<p>Optimizely CMS provides many built-in property types for common data scenarios.</p>

<h3>Text Properties</h3>
<table class=""min-w-full divide-y divide-gray-200 dark:divide-gray-700 my-4"">
    <thead>
        <tr>
            <th class=""px-4 py-2 text-left"">Type</th>
            <th class=""px-4 py-2 text-left"">Editor</th>
            <th class=""px-4 py-2 text-left"">Use Case</th>
        </tr>
    </thead>
    <tbody>
        <tr><td class=""px-4 py-2 font-mono text-sm"">string</td><td class=""px-4 py-2"">Single-line text</td><td class=""px-4 py-2"">Titles, short text</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">string + UIHint.Textarea</td><td class=""px-4 py-2"">Multi-line text</td><td class=""px-4 py-2"">Descriptions, summaries</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">XhtmlString</td><td class=""px-4 py-2"">Rich text (TinyMCE)</td><td class=""px-4 py-2"">Main body content</td></tr>
    </tbody>
</table>

<h3>Reference Properties</h3>
<table class=""min-w-full divide-y divide-gray-200 dark:divide-gray-700 my-4"">
    <thead>
        <tr>
            <th class=""px-4 py-2 text-left"">Type</th>
            <th class=""px-4 py-2 text-left"">Description</th>
        </tr>
    </thead>
    <tbody>
        <tr><td class=""px-4 py-2 font-mono text-sm"">ContentReference</td><td class=""px-4 py-2"">Reference to a single content item</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">ContentArea</td><td class=""px-4 py-2"">Collection of content items (drag & drop)</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">Url</td><td class=""px-4 py-2"">Internal or external URL</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">LinkItemCollection</td><td class=""px-4 py-2"">Collection of links with text</td></tr>
    </tbody>
</table>

<h3>Other Types</h3>
<table class=""min-w-full divide-y divide-gray-200 dark:divide-gray-700 my-4"">
    <thead>
        <tr>
            <th class=""px-4 py-2 text-left"">Type</th>
            <th class=""px-4 py-2 text-left"">Description</th>
        </tr>
    </thead>
    <tbody>
        <tr><td class=""px-4 py-2 font-mono text-sm"">int, double</td><td class=""px-4 py-2"">Numeric values</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">bool</td><td class=""px-4 py-2"">Checkbox (true/false)</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">DateTime</td><td class=""px-4 py-2"">Date and time picker</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">CategoryList</td><td class=""px-4 py-2"">Category selection</td></tr>
    </tbody>
</table>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "property-examples",
                            Title = "Property Type Examples",
                            Description = "Various property types in action",
                            Type = ExampleType.Code,
                            ExampleContent = @"public class ProductPage : PageData
{
    // Simple string (single-line text)
    [Display(Name = ""Product Name"")]
    public virtual string? ProductName { get; set; }

    // Multi-line text (textarea)
    [UIHint(UIHint.Textarea)]
    [Display(Name = ""Short Description"")]
    public virtual string? ShortDescription { get; set; }

    // Rich text editor
    [Display(Name = ""Full Description"")]
    public virtual XhtmlString? FullDescription { get; set; }

    // Single content reference (e.g., to an image)
    [UIHint(UIHint.Image)]
    [Display(Name = ""Main Image"")]
    public virtual ContentReference? MainImage { get; set; }

    // Content area for blocks
    [Display(Name = ""Related Content"")]
    public virtual ContentArea? RelatedContent { get; set; }

    // Price (decimal)
    [Display(Name = ""Price"")]
    public virtual decimal Price { get; set; }

    // Featured flag
    [Display(Name = ""Is Featured"")]
    public virtual bool IsFeatured { get; set; }

    // Release date
    [Display(Name = ""Release Date"")]
    public virtual DateTime? ReleaseDate { get; set; }

    // External link
    [Display(Name = ""Manufacturer Website"")]
    public virtual Url? ManufacturerUrl { get; set; }
}",
                            IsInteractive = false
                        }
                    }
                },
                new Lesson
                {
                    Id = "ct-attributes",
                    ModuleId = "content-types",
                    Title = "Property Attributes",
                    Summary = "Control property behavior with attributes.",
                    Order = 5,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Use Display attribute to control appearance",
                        "Apply validation attributes",
                        "Restrict content types with AllowedTypes"
                    },
                    Content = @"
<h2>Property Attributes</h2>
<p>Attributes control how properties behave in the editor and validate input.</p>

<h3>Display Attribute</h3>
<p>Controls how the property appears in the editor:</p>
<ul>
    <li><strong>Name</strong> - Display name in the editor</li>
    <li><strong>Description</strong> - Help text shown to editors</li>
    <li><strong>GroupName</strong> - Tab where property appears</li>
    <li><strong>Order</strong> - Sort order within the tab</li>
</ul>

<h3>Common Attributes</h3>
<table class=""min-w-full divide-y divide-gray-200 dark:divide-gray-700 my-4"">
    <thead>
        <tr>
            <th class=""px-4 py-2 text-left"">Attribute</th>
            <th class=""px-4 py-2 text-left"">Purpose</th>
        </tr>
    </thead>
    <tbody>
        <tr><td class=""px-4 py-2 font-mono text-sm"">[CultureSpecific]</td><td class=""px-4 py-2"">Property has different values per language</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">[Required]</td><td class=""px-4 py-2"">Property must have a value</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">[StringLength(max)]</td><td class=""px-4 py-2"">Maximum character length</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">[Range(min, max)]</td><td class=""px-4 py-2"">Numeric range validation</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">[UIHint(hint)]</td><td class=""px-4 py-2"">Custom editor hint</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">[AllowedTypes]</td><td class=""px-4 py-2"">Restrict allowed content types</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">[Searchable]</td><td class=""px-4 py-2"">Include in search index</td></tr>
    </tbody>
</table>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "attribute-examples",
                            Title = "Common Attribute Usage",
                            Description = "Examples of property attributes",
                            Type = ExampleType.Code,
                            ExampleContent = @"public class ArticlePage : PageData
{
    // Culture-specific with validation
    [CultureSpecific]
    [Required]
    [StringLength(100, MinimumLength = 5)]
    [Display(Name = ""Title"", Order = 10)]
    public virtual string? Title { get; set; }

    // Grouped in custom tab with order
    [Display(
        Name = ""SEO Title"",
        Description = ""Title shown in search results"",
        GroupName = ""SEO"",
        Order = 10)]
    [StringLength(60)]
    public virtual string? SeoTitle { get; set; }

    // Content area restricted to specific block types
    [AllowedTypes(typeof(TeaserBlock), typeof(ImageBlock))]
    [Display(Name = ""Sidebar Content"", Order = 100)]
    public virtual ContentArea? Sidebar { get; set; }

    // Content reference restricted to images only
    [UIHint(UIHint.Image)]
    [AllowedTypes(typeof(ImageFile))]
    [Display(Name = ""Hero Image"")]
    public virtual ContentReference? HeroImage { get; set; }

    // Numeric with range
    [Range(1, 10)]
    [Display(Name = ""Priority"")]
    public virtual int Priority { get; set; }
}",
                            IsInteractive = false
                        }
                    }
                },
                new Lesson
                {
                    Id = "ct-media-types",
                    ModuleId = "content-types",
                    Title = "Media Types",
                    Summary = "Create custom media types for images, videos, and documents.",
                    Order = 6,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Understand the media type system",
                        "Create custom image and video types",
                        "Add metadata to media files"
                    },
                    Content = @"
<h2>Media Types</h2>
<p>Media types allow you to add custom properties to uploaded files like images, videos, and documents.</p>

<h3>Built-in Media Interfaces</h3>
<ul>
    <li><strong>IContentMedia</strong> - Base interface for all media</li>
    <li><strong>IContentImage</strong> - For image files</li>
    <li><strong>IContentVideo</strong> - For video files</li>
</ul>

<h3>Media Type Descriptors</h3>
<p>Use <code>[MediaDescriptor]</code> to specify which file extensions your media type handles.</p>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "image-media-type",
                            Title = "Custom Image Type",
                            Description = "Image type with alt text and copyright",
                            Type = ExampleType.Code,
                            ExampleContent = @"using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Framework.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace MyOptimizely.Models.Media
{
    [ContentType(
        GUID = ""c3d4e5f6-a7b8-9012-cdef-345678901234"",
        DisplayName = ""Image File"")]
    [MediaDescriptor(ExtensionString = ""jpg,jpeg,png,gif,webp,svg"")]
    public class ImageFile : ImageData
    {
        [CultureSpecific]
        [Display(
            Name = ""Alt Text"",
            Description = ""Alternative text for accessibility"",
            Order = 10)]
        public virtual string? AltText { get; set; }

        [Display(
            Name = ""Copyright"",
            Description = ""Image copyright information"",
            Order = 20)]
        public virtual string? Copyright { get; set; }

        [Display(
            Name = ""Photographer"",
            Order = 30)]
        public virtual string? Photographer { get; set; }
    }

    [ContentType(
        GUID = ""d4e5f6a7-b8c9-0123-defa-456789012345"",
        DisplayName = ""Video File"")]
    [MediaDescriptor(ExtensionString = ""mp4,webm,ogg"")]
    public class VideoFile : VideoData
    {
        [Display(Name = ""Thumbnail"")]
        [UIHint(UIHint.Image)]
        public virtual ContentReference? Thumbnail { get; set; }

        [Display(Name = ""Duration (seconds)"")]
        public virtual int? Duration { get; set; }

        [CultureSpecific]
        [Display(Name = ""Transcript"")]
        public virtual XhtmlString? Transcript { get; set; }
    }
}",
                            IsInteractive = false
                        }
                    }
                }
            }
        };
    }

    #endregion

    #region Module 3: Templates & Rendering

    private LearningModule BuildTemplatesRenderingModule()
    {
        return new LearningModule
        {
            Id = "templates-rendering",
            Title = "Templates & Rendering",
            Description = "Learn MVC patterns, controllers, and views for rendering content.",
            Icon = "code-bracket",
            Order = 3,
            Difficulty = ModuleDifficulty.Beginner,
            Prerequisites = new[] { "content-types" },
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "tr-mvc-overview",
                    ModuleId = "templates-rendering",
                    Title = "MVC Architecture in CMS 12",
                    Summary = "Understand how MVC patterns work with Optimizely CMS.",
                    Order = 1,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Understand the MVC pattern in CMS context",
                        "Know how content routing works",
                        "Learn the template resolution process"
                    },
                    Content = @"
<h2>MVC in Optimizely CMS</h2>
<p>Optimizely CMS 12 uses ASP.NET Core MVC for rendering content. The MVC pattern separates:</p>
<ul>
    <li><strong>Model</strong> - Your content types (PageData, BlockData)</li>
    <li><strong>View</strong> - Razor templates that render HTML</li>
    <li><strong>Controller</strong> - Handles requests and prepares data for views</li>
</ul>

<h3>Content Routing</h3>
<p>When a request comes in, CMS uses content routing to:</p>
<ol>
    <li>Match the URL to a page in the content tree</li>
    <li>Find the appropriate controller for that page type</li>
    <li>Execute the controller action</li>
    <li>Render the matching view</li>
</ol>

<h3>Template Resolution</h3>
<p>CMS uses conventions to find templates:</p>
<ul>
    <li>Controller: <code>{PageTypeName}Controller</code> or <code>DefaultPageController</code></li>
    <li>View: <code>Views/{ControllerName}/Index.cshtml</code> or <code>Views/Pages/{PageTypeName}.cshtml</code></li>
</ul>

<h3>Enabling Content Routing</h3>
<p>Content routing is enabled in Program.cs with <code>app.MapContent()</code></p>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "map-content",
                            Title = "Enable Content Routing",
                            Description = "Program.cs configuration for content routing",
                            Type = ExampleType.Code,
                            ExampleContent = @"var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Enable Optimizely content routing
app.MapContent();

// Also map regular MVC controllers
app.MapControllers();

app.Run();",
                            IsInteractive = false
                        }
                    }
                },
                new Lesson
                {
                    Id = "tr-page-controllers",
                    ModuleId = "templates-rendering",
                    Title = "Creating Page Controllers",
                    Summary = "Create controllers that handle page rendering.",
                    Order = 2,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Create a page controller",
                        "Use PageController<T> base class",
                        "Pass data to views"
                    },
                    Content = @"
<h2>Page Controllers</h2>
<p>Page controllers handle requests for specific page types. They inherit from <code>PageController&lt;T&gt;</code> where T is your page type.</p>

<h3>Controller Conventions</h3>
<ul>
    <li>Name: <code>{PageTypeName}Controller</code></li>
    <li>Location: <code>Controllers/</code> folder</li>
    <li>Base class: <code>PageController&lt;T&gt;</code></li>
    <li>Action: Usually <code>Index</code> for main page rendering</li>
</ul>

<h3>The CurrentPage Property</h3>
<p>The base class provides <code>CurrentPage</code> property containing the page being rendered. This is automatically populated by the CMS routing.</p>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "article-controller",
                            Title = "Article Page Controller",
                            Description = "Controller for the ArticlePage type",
                            Type = ExampleType.Code,
                            ExampleContent = @"using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using MyOptimizely.Models.Pages;

namespace MyOptimizely.Controllers
{
    public class ArticlePageController : PageController<ArticlePage>
    {
        public IActionResult Index(ArticlePage currentPage)
        {
            // CurrentPage is also available via the base class
            // currentPage == CurrentPage

            // You can perform additional logic here
            // e.g., load related articles, prepare view models

            return View(currentPage);
        }
    }
}",
                            IsInteractive = false
                        },
                        new LessonExample
                        {
                            Id = "controller-with-viewmodel",
                            Title = "Controller with View Model",
                            Description = "Using a view model to pass additional data",
                            Type = ExampleType.Code,
                            ExampleContent = @"public class ArticlePageController : PageController<ArticlePage>
{
    private readonly IContentLoader _contentLoader;

    public ArticlePageController(IContentLoader contentLoader)
    {
        _contentLoader = contentLoader;
    }

    public IActionResult Index(ArticlePage currentPage)
    {
        var viewModel = new ArticleViewModel
        {
            Page = currentPage,
            RelatedArticles = GetRelatedArticles(currentPage),
            PublishedDateFormatted = currentPage.PublishedDate.ToString(""MMMM dd, yyyy"")
        };

        return View(viewModel);
    }

    private IEnumerable<ArticlePage> GetRelatedArticles(ArticlePage page)
    {
        // Load related articles logic
        return _contentLoader
            .GetChildren<ArticlePage>(page.ParentLink)
            .Where(a => a.ContentLink != page.ContentLink)
            .Take(3);
    }
}",
                            IsInteractive = false
                        }
                    }
                },
                new Lesson
                {
                    Id = "tr-views",
                    ModuleId = "templates-rendering",
                    Title = "Creating Views",
                    Summary = "Create Razor views to render content.",
                    Order = 3,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Create Razor views for page types",
                        "Use Html.PropertyFor for on-page editing",
                        "Understand view conventions"
                    },
                    Content = @"
<h2>Razor Views</h2>
<p>Views are Razor templates (.cshtml) that render your content as HTML.</p>

<h3>View Conventions</h3>
<p>Views should be placed in:</p>
<ul>
    <li><code>Views/{ControllerName}/Index.cshtml</code> - For controller-based routing</li>
    <li><code>Views/Pages/{PageTypeName}.cshtml</code> - Convention-based routing</li>
</ul>

<h3>Html.PropertyFor</h3>
<p>Use <code>Html.PropertyFor()</code> to render properties with on-page editing support:</p>
<ul>
    <li>Automatically wraps content in edit markers</li>
    <li>Enables inline editing in edit mode</li>
    <li>Works with all property types</li>
</ul>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "article-view",
                            Title = "Article Page View",
                            Description = "Razor view with on-page editing",
                            Type = ExampleType.Code,
                            ExampleContent = @"@model MyOptimizely.Models.Pages.ArticlePage
@using EPiServer.Web.Mvc.Html

@{
    Layout = ""~/Views/Shared/_Layout.cshtml"";
}

<article class=""article"">
    <header>
        @* PropertyFor enables on-page editing *@
        <h1>@Html.PropertyFor(m => m.Title)</h1>

        <p class=""article-meta"">
            Published: @Model.PublishedDate.ToString(""MMMM dd, yyyy"")
        </p>
    </header>

    @if (!string.IsNullOrEmpty(Model.Introduction))
    {
        <div class=""article-intro"">
            @Html.PropertyFor(m => m.Introduction)
        </div>
    }

    <div class=""article-body"">
        @* XhtmlString is automatically rendered as HTML *@
        @Html.PropertyFor(m => m.MainBody)
    </div>
</article>",
                            IsInteractive = false
                        }
                    }
                },
                new Lesson
                {
                    Id = "tr-block-rendering",
                    ModuleId = "templates-rendering",
                    Title = "Rendering Blocks",
                    Summary = "Create view components and views to render blocks.",
                    Order = 4,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Create block view components",
                        "Create partial views for blocks",
                        "Render ContentArea properties"
                    },
                    Content = @"
<h2>Rendering Blocks</h2>
<p>Blocks can be rendered using either view components or simple partial views.</p>

<h3>Option 1: Partial View Only</h3>
<p>The simplest approach - just create a partial view:</p>
<ul>
    <li>Location: <code>Views/Shared/Blocks/{BlockTypeName}.cshtml</code></li>
    <li>No controller needed</li>
    <li>Block instance passed directly as model</li>
</ul>

<h3>Option 2: View Component</h3>
<p>For blocks that need additional logic:</p>
<ul>
    <li>Inherit from <code>BlockComponent&lt;T&gt;</code></li>
    <li>Override <code>InvokeComponent</code> method</li>
    <li>Create associated view in <code>Views/Shared/Components/{ComponentName}/Default.cshtml</code></li>
</ul>

<h3>Rendering ContentArea</h3>
<p>Use <code>Html.PropertyFor()</code> on ContentArea properties to render all blocks inside:</p>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "block-partial-view",
                            Title = "Block Partial View",
                            Description = "Simple partial view for TeaserBlock",
                            Type = ExampleType.Code,
                            ExampleContent = @"@* Views/Shared/Blocks/TeaserBlock.cshtml *@
@model MyOptimizely.Models.Blocks.TeaserBlock
@using EPiServer.Web.Mvc.Html

<div class=""teaser"">
    @if (Model.Image != null)
    {
        <img src=""@Url.ContentUrl(Model.Image)"" alt=""@Model.Heading"" />
    }

    <h3>@Html.PropertyFor(m => m.Heading)</h3>
    <p>@Html.PropertyFor(m => m.Text)</p>

    @if (Model.Link != null)
    {
        <a href=""@Model.Link"" class=""teaser-link"">
            @(Model.LinkText ?? ""Read more"")
        </a>
    }
</div>",
                            IsInteractive = false
                        },
                        new LessonExample
                        {
                            Id = "block-component",
                            Title = "Block View Component",
                            Description = "View component with additional logic",
                            Type = ExampleType.Code,
                            ExampleContent = @"// Components/TeaserBlockComponent.cs
using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using MyOptimizely.Models.Blocks;

public class TeaserBlockComponent : BlockComponent<TeaserBlock>
{
    private readonly IContentLoader _contentLoader;

    public TeaserBlockComponent(IContentLoader contentLoader)
    {
        _contentLoader = contentLoader;
    }

    protected override IViewComponentResult InvokeComponent(TeaserBlock currentBlock)
    {
        var viewModel = new TeaserViewModel
        {
            Block = currentBlock,
            ImageUrl = GetImageUrl(currentBlock.Image)
        };

        return View(viewModel);
    }

    private string? GetImageUrl(ContentReference? imageRef)
    {
        if (imageRef == null) return null;
        // Custom image URL logic
        return $""/contentassets/{imageRef.ID}"";
    }
}",
                            IsInteractive = false
                        },
                        new LessonExample
                        {
                            Id = "render-content-area",
                            Title = "Rendering ContentArea",
                            Description = "Render all blocks in a ContentArea",
                            Type = ExampleType.Code,
                            ExampleContent = @"@* In a page view *@
@model MyOptimizely.Models.Pages.StartPage

<main>
    <div class=""hero-section"">
        @Html.PropertyFor(m => m.HeroArea)
    </div>

    <div class=""main-content"">
        @Html.PropertyFor(m => m.MainContentArea)
    </div>

    <aside class=""sidebar"">
        @* You can also render with custom tag and CSS class *@
        @Html.PropertyFor(m => m.SidebarArea, new {
            Tag = ""aside"",
            CssClass = ""sidebar-blocks""
        })
    </aside>
</main>",
                            IsInteractive = false
                        }
                    }
                },
                new Lesson
                {
                    Id = "tr-layouts-partials",
                    ModuleId = "templates-rendering",
                    Title = "Layouts and Partial Views",
                    Summary = "Create shared layouts and reusable partial views.",
                    Order = 5,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Create shared layout templates",
                        "Use partial views for reusable components",
                        "Understand section rendering"
                    },
                    Content = @"
<h2>Layouts</h2>
<p>Layouts define the common HTML structure shared across pages (header, footer, navigation).</p>

<h3>Layout Location</h3>
<p>Standard location: <code>Views/Shared/_Layout.cshtml</code></p>

<h3>Key Layout Features</h3>
<ul>
    <li><code>@RenderBody()</code> - Where page content is inserted</li>
    <li><code>@RenderSection()</code> - Named sections for scripts, styles</li>
    <li><code>@Html.RequiredClientResources()</code> - CMS editor scripts</li>
</ul>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "layout-example",
                            Title = "Base Layout",
                            Description = "Shared layout with CMS support",
                            Type = ExampleType.Code,
                            ExampleContent = @"@* Views/Shared/_Layout.cshtml *@
@using EPiServer.Web.Mvc.Html

<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""utf-8"" />
    <meta name=""viewport"" content=""width=device-width, initial-scale=1"" />
    <title>@ViewBag.Title - My Site</title>
    <link rel=""stylesheet"" href=""~/css/site.css"" />
    @RenderSection(""Styles"", required: false)
    @* Required for on-page editing *@
    @Html.RequiredClientResources(""Header"")
</head>
<body>
    <header>
        @await Html.PartialAsync(""_Navigation"")
    </header>

    <main>
        @RenderBody()
    </main>

    <footer>
        @await Html.PartialAsync(""_Footer"")
    </footer>

    <script src=""~/js/site.js""></script>
    @RenderSection(""Scripts"", required: false)
    @* Required for on-page editing *@
    @Html.RequiredClientResources(""Footer"")
</body>
</html>",
                            IsInteractive = false
                        }
                    }
                }
            }
        };
    }

    #endregion

    #region Module 4: Content Management

    private LearningModule BuildContentManagementModule()
    {
        return new LearningModule
        {
            Id = "content-management",
            Title = "Content Management APIs",
            Description = "Learn to programmatically manage content using IContentRepository and IContentLoader.",
            Icon = "circle-stack",
            Order = 4,
            Difficulty = ModuleDifficulty.Intermediate,
            Prerequisites = new[] { "templates-rendering" },
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "cm-loading-content",
                    ModuleId = "content-management",
                    Title = "Loading Content",
                    Summary = "Learn to load content using IContentLoader.",
                    Order = 1,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Use IContentLoader to retrieve content",
                        "Load content by reference, parent, or ancestor",
                        "Understand language-specific loading"
                    },
                    Content = @"
<h2>Loading Content with IContentLoader</h2>
<p><code>IContentLoader</code> provides read-only access to content. It's the primary way to load content in your code.</p>

<h3>Key Methods</h3>
<table class=""min-w-full divide-y divide-gray-200 dark:divide-gray-700 my-4"">
    <thead>
        <tr>
            <th class=""px-4 py-2 text-left"">Method</th>
            <th class=""px-4 py-2 text-left"">Description</th>
        </tr>
    </thead>
    <tbody>
        <tr><td class=""px-4 py-2 font-mono text-sm"">Get&lt;T&gt;(ContentReference)</td><td class=""px-4 py-2"">Load single content item</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">GetChildren&lt;T&gt;(ContentReference)</td><td class=""px-4 py-2"">Load immediate children</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">GetAncestors(ContentReference)</td><td class=""px-4 py-2"">Load all ancestors (for breadcrumbs)</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">GetDescendents(ContentReference)</td><td class=""px-4 py-2"">Load all descendant references</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">TryGet&lt;T&gt;(ContentReference, out T)</td><td class=""px-4 py-2"">Safe loading (returns false if not found)</td></tr>
    </tbody>
</table>

<h3>IContentLoader vs IContentRepository</h3>
<ul>
    <li><strong>IContentLoader</strong> - Read-only operations, use for rendering</li>
    <li><strong>IContentRepository</strong> - Read and write operations, use for modifications</li>
</ul>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "content-loader-examples",
                            Title = "IContentLoader Examples",
                            Description = "Common content loading patterns",
                            Type = ExampleType.Code,
                            ExampleContent = @"public class ContentService
{
    private readonly IContentLoader _contentLoader;

    public ContentService(IContentLoader contentLoader)
    {
        _contentLoader = contentLoader;
    }

    // Load a specific page by ID
    public ArticlePage? GetArticle(int contentId)
    {
        var reference = new ContentReference(contentId);
        return _contentLoader.Get<ArticlePage>(reference);
    }

    // Safe loading with TryGet
    public ArticlePage? GetArticleSafe(ContentReference reference)
    {
        if (_contentLoader.TryGet<ArticlePage>(reference, out var article))
        {
            return article;
        }
        return null;
    }

    // Get all child pages of a specific type
    public IEnumerable<ArticlePage> GetChildArticles(ContentReference parentRef)
    {
        return _contentLoader.GetChildren<ArticlePage>(parentRef);
    }

    // Build breadcrumb trail
    public IEnumerable<PageData> GetBreadcrumbs(ContentReference pageRef)
    {
        return _contentLoader.GetAncestors(pageRef)
            .OfType<PageData>()
            .Reverse();
    }

    // Load content in a specific language
    public ArticlePage? GetArticleInLanguage(ContentReference reference, string language)
    {
        var culture = new CultureInfo(language);
        return _contentLoader.Get<ArticlePage>(
            reference,
            new LanguageSelector(culture));
    }
}",
                            IsInteractive = false
                        }
                    }
                },
                new Lesson
                {
                    Id = "cm-creating-content",
                    ModuleId = "content-management",
                    Title = "Creating and Saving Content",
                    Summary = "Learn to create, modify, and save content programmatically.",
                    Order = 2,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Create new content instances",
                        "Save content with different save actions",
                        "Understand writable clones"
                    },
                    Content = @"
<h2>Creating Content with IContentRepository</h2>
<p><code>IContentRepository</code> extends IContentLoader with write operations.</p>

<h3>Creating Content</h3>
<ol>
    <li>Use <code>GetDefault&lt;T&gt;(parentRef)</code> to create a new instance</li>
    <li>Set the Name property and other properties</li>
    <li>Call <code>Save()</code> to persist</li>
</ol>

<h3>Save Actions</h3>
<table class=""min-w-full divide-y divide-gray-200 dark:divide-gray-700 my-4"">
    <thead>
        <tr>
            <th class=""px-4 py-2 text-left"">SaveAction</th>
            <th class=""px-4 py-2 text-left"">Description</th>
        </tr>
    </thead>
    <tbody>
        <tr><td class=""px-4 py-2 font-mono text-sm"">Save</td><td class=""px-4 py-2"">Save as draft (not published)</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">Publish</td><td class=""px-4 py-2"">Save and publish immediately</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">CheckIn</td><td class=""px-4 py-2"">Check in for review</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">Schedule</td><td class=""px-4 py-2"">Schedule for future publication</td></tr>
    </tbody>
</table>

<h3>Modifying Existing Content</h3>
<p>Content is read-only by default. To modify, create a writable clone first:</p>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "create-content",
                            Title = "Creating Content",
                            Description = "Create and save new content",
                            Type = ExampleType.Code,
                            ExampleContent = @"public class ContentCreationService
{
    private readonly IContentRepository _contentRepository;

    public ContentCreationService(IContentRepository contentRepository)
    {
        _contentRepository = contentRepository;
    }

    public ContentReference CreateArticle(ContentReference parentRef, string title, string body)
    {
        // Create new article under parent
        var article = _contentRepository.GetDefault<ArticlePage>(parentRef);

        // Set properties
        article.Name = title;  // Name is used for URL segment
        article.Title = title;
        article.MainBody = new XhtmlString(body);
        article.PublishedDate = DateTime.Now;

        // Save and publish
        var savedRef = _contentRepository.Save(
            article,
            SaveAction.Publish,
            AccessLevel.NoAccess);

        return savedRef;
    }

    public void UpdateArticle(ContentReference articleRef, string newTitle)
    {
        // Load the article
        var article = _contentRepository.Get<ArticlePage>(articleRef);

        // Create writable clone
        var writableArticle = article.CreateWritableClone() as ArticlePage;

        // Modify
        writableArticle!.Title = newTitle;

        // Save changes
        _contentRepository.Save(writableArticle, SaveAction.Publish);
    }
}",
                            IsInteractive = false
                        }
                    }
                },
                new Lesson
                {
                    Id = "cm-moving-deleting",
                    ModuleId = "content-management",
                    Title = "Moving and Deleting Content",
                    Summary = "Learn to move, copy, and delete content.",
                    Order = 3,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Move content between locations",
                        "Copy content",
                        "Delete content (soft and hard delete)"
                    },
                    Content = @"
<h2>Moving and Deleting Content</h2>
<p>IContentRepository provides methods for moving, copying, and deleting content.</p>

<h3>Moving Content</h3>
<p>Use <code>Move()</code> to relocate content in the tree. Children are moved with the parent.</p>

<h3>Copying Content</h3>
<p>Use <code>Copy()</code> to duplicate content. The copy gets a new ContentReference.</p>

<h3>Deleting Content</h3>
<ul>
    <li><strong>Delete()</strong> - Moves to Trash (can be restored)</li>
    <li><strong>Delete(forceDelete: true)</strong> - Permanently deletes</li>
</ul>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "move-delete-content",
                            Title = "Move and Delete Operations",
                            Description = "Moving, copying, and deleting content",
                            Type = ExampleType.Code,
                            ExampleContent = @"public class ContentOperationsService
{
    private readonly IContentRepository _contentRepository;

    public ContentOperationsService(IContentRepository contentRepository)
    {
        _contentRepository = contentRepository;
    }

    // Move content to a new parent
    public void MoveContent(ContentReference contentRef, ContentReference newParentRef)
    {
        _contentRepository.Move(
            contentRef,
            newParentRef,
            AccessLevel.NoAccess,
            AccessLevel.NoAccess);
    }

    // Copy content (creates a duplicate)
    public ContentReference CopyContent(ContentReference sourceRef, ContentReference targetParentRef)
    {
        return _contentRepository.Copy(
            sourceRef,
            targetParentRef,
            AccessLevel.NoAccess,
            AccessLevel.NoAccess,
            copyChildContent: true);
    }

    // Soft delete (moves to trash)
    public void DeleteToTrash(ContentReference contentRef)
    {
        _contentRepository.Delete(contentRef, forceDelete: false);
    }

    // Permanent delete
    public void PermanentlyDelete(ContentReference contentRef)
    {
        _contentRepository.Delete(contentRef, forceDelete: true);
    }
}",
                            IsInteractive = false
                        }
                    }
                },
                new Lesson
                {
                    Id = "cm-versioning",
                    ModuleId = "content-management",
                    Title = "Content Versioning",
                    Summary = "Work with content versions and publishing workflow.",
                    Order = 4,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Understand content versioning",
                        "Work with different versions",
                        "Load specific versions"
                    },
                    Content = @"
<h2>Content Versioning</h2>
<p>Every save creates a new version. CMS maintains a version history for each content item.</p>

<h3>Version Types</h3>
<ul>
    <li><strong>Published</strong> - The live version visible to site visitors</li>
    <li><strong>Draft</strong> - Work in progress, not yet published</li>
    <li><strong>Previously Published</strong> - Older published versions</li>
</ul>

<h3>ContentReference and Versions</h3>
<p>ContentReference can optionally include a WorkID to specify a version:</p>
<ul>
    <li><code>ContentReference(5)</code> - Latest published version of content 5</li>
    <li><code>ContentReference(5, 10)</code> - Specific version (WorkID 10) of content 5</li>
</ul>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "versioning-example",
                            Title = "Working with Versions",
                            Description = "Loading and listing content versions",
                            Type = ExampleType.Code,
                            ExampleContent = @"using EPiServer.Core;
using EPiServer.DataAccess;

public class VersioningService
{
    private readonly IContentRepository _contentRepository;
    private readonly IContentVersionRepository _versionRepository;

    public VersioningService(
        IContentRepository contentRepository,
        IContentVersionRepository versionRepository)
    {
        _contentRepository = contentRepository;
        _versionRepository = versionRepository;
    }

    // List all versions of content
    public IEnumerable<ContentVersion> GetAllVersions(ContentReference contentRef)
    {
        return _versionRepository.List(contentRef);
    }

    // Load a specific version
    public T? LoadVersion<T>(ContentReference contentRef, int workId) where T : IContent
    {
        var versionRef = new ContentReference(contentRef.ID, workId);
        return _contentRepository.Get<T>(versionRef);
    }

    // Get the published version
    public ContentVersion? GetPublishedVersion(ContentReference contentRef)
    {
        return _versionRepository.List(contentRef)
            .FirstOrDefault(v => v.Status == VersionStatus.Published);
    }

    // Create a new draft from published version
    public ContentReference CreateDraft(ContentReference publishedRef)
    {
        var content = _contentRepository.Get<IContent>(publishedRef);
        var draft = content.CreateWritableClone();
        return _contentRepository.Save(draft, SaveAction.CheckOut);
    }
}",
                            IsInteractive = false
                        }
                    }
                },
                new Lesson
                {
                    Id = "cm-special-references",
                    ModuleId = "content-management",
                    Title = "Special Content References",
                    Summary = "Learn about root pages, start pages, and global assets.",
                    Order = 5,
                    EstimatedMinutes = 6,
                    LearningObjectives = new List<string>
                    {
                        "Know the special content references",
                        "Access site start page",
                        "Find global assets folders"
                    },
                    Content = @"
<h2>Special Content References</h2>
<p>CMS provides several well-known content references for important locations.</p>

<h3>Key References</h3>
<table class=""min-w-full divide-y divide-gray-200 dark:divide-gray-700 my-4"">
    <thead>
        <tr>
            <th class=""px-4 py-2 text-left"">Reference</th>
            <th class=""px-4 py-2 text-left"">Description</th>
        </tr>
    </thead>
    <tbody>
        <tr><td class=""px-4 py-2 font-mono text-sm"">ContentReference.RootPage</td><td class=""px-4 py-2"">Root of the page tree</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">ContentReference.StartPage</td><td class=""px-4 py-2"">Site start page (homepage)</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">ContentReference.GlobalBlockFolder</td><td class=""px-4 py-2"">Global shared blocks</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">ContentReference.SiteBlockFolder</td><td class=""px-4 py-2"">Site-specific blocks</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">SiteDefinition.Current.GlobalAssetsRoot</td><td class=""px-4 py-2"">Global media folder</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">SiteDefinition.Current.SiteAssetsRoot</td><td class=""px-4 py-2"">Site media folder</td></tr>
    </tbody>
</table>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "special-refs",
                            Title = "Using Special References",
                            Description = "Access well-known content locations",
                            Type = ExampleType.Code,
                            ExampleContent = @"public class SiteService
{
    private readonly IContentLoader _contentLoader;
    private readonly ISiteDefinitionResolver _siteResolver;

    public SiteService(
        IContentLoader contentLoader,
        ISiteDefinitionResolver siteResolver)
    {
        _contentLoader = contentLoader;
        _siteResolver = siteResolver;
    }

    // Get the site start page
    public StartPage GetStartPage()
    {
        return _contentLoader.Get<StartPage>(ContentReference.StartPage);
    }

    // Get all top-level pages
    public IEnumerable<PageData> GetTopLevelPages()
    {
        return _contentLoader.GetChildren<PageData>(ContentReference.StartPage);
    }

    // Get current site's media folder
    public ContentReference GetSiteMediaFolder()
    {
        var site = _siteResolver.GetByContent(
            ContentReference.StartPage,
            fallbackToWildcard: true);
        return site.SiteAssetsRoot;
    }

    // Upload media to site folder
    public ContentReference GetMediaUploadFolder()
    {
        return SiteDefinition.Current.SiteAssetsRoot;
    }
}",
                            IsInteractive = false
                        }
                    }
                }
            }
        };
    }

    #endregion

    #region Module 5-11 Placeholders

    private LearningModule BuildInitializationEventsModule()
    {
        return new LearningModule
        {
            Id = "initialization-events",
            Title = "Initialization & Events",
            Description = "Learn about initialization modules and content events.",
            Icon = "bolt",
            Order = 5,
            Difficulty = ModuleDifficulty.Intermediate,
            Prerequisites = new[] { "content-management" },
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "ie-initialization-overview",
                    ModuleId = "initialization-events",
                    Title = "Initialization System Overview",
                    Summary = "Understand how CMS initializes and how to hook into the startup process.",
                    Order = 1,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Understand the CMS initialization process",
                        "Know when initialization modules run",
                        "Learn the module dependency system"
                    },
                    Content = @"
<h2>CMS Initialization System</h2>
<p>Optimizely CMS uses an initialization system that runs during application startup. This allows you to register services, configure options, and set up event handlers.</p>

<h3>How Initialization Works</h3>
<ol>
    <li>ASP.NET Core starts and calls <code>AddCms()</code></li>
    <li>CMS scans assemblies for <code>IInitializableModule</code> implementations</li>
    <li>Modules are sorted by dependencies</li>
    <li>Each module's <code>Initialize()</code> method is called</li>
    <li>On shutdown, <code>Uninitialize()</code> is called in reverse order</li>
</ol>

<h3>Module Dependencies</h3>
<p>Use <code>[ModuleDependency]</code> to ensure your module runs after CMS core modules:</p>
<ul>
    <li><code>EPiServer.Web.InitializationModule</code> - Core web functionality</li>
    <li><code>EPiServer.Framework.Initialization</code> - Framework services</li>
</ul>
",
                    Examples = new List<LessonExample>()
                },
                new Lesson
                {
                    Id = "ie-creating-modules",
                    ModuleId = "initialization-events",
                    Title = "Creating Initialization Modules",
                    Summary = "Create custom initialization modules to run code at startup.",
                    Order = 2,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Create an initialization module",
                        "Use the InitializationEngine",
                        "Handle module dependencies correctly"
                    },
                    Content = @"
<h2>Creating Initialization Modules</h2>
<p>Create a class that implements <code>IInitializableModule</code> and decorate it with the appropriate attribute.</p>

<h3>Module Attributes</h3>
<table class=""min-w-full divide-y divide-gray-200 dark:divide-gray-700 my-4"">
    <thead>
        <tr>
            <th class=""px-4 py-2 text-left"">Attribute</th>
            <th class=""px-4 py-2 text-left"">Use Case</th>
        </tr>
    </thead>
    <tbody>
        <tr><td class=""px-4 py-2 font-mono text-sm"">[InitializableModule]</td><td class=""px-4 py-2"">No CMS dependencies needed</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">[ModuleDependency(typeof(...))]</td><td class=""px-4 py-2"">Depends on CMS or other modules</td></tr>
    </tbody>
</table>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "init-module-example",
                            Title = "Basic Initialization Module",
                            Description = "A simple initialization module",
                            Type = ExampleType.Code,
                            ExampleContent = @"using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;

namespace MyOptimizely.Business.Initialization
{
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class CustomInitializationModule : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            // This runs once during application startup

            // Access services via the service locator
            var contentEvents = context.Locate.Advanced
                .GetInstance<IContentEvents>();

            // Attach event handlers
            contentEvents.PublishedContent += OnContentPublished;

            // Log initialization
            var logger = context.Locate.Advanced
                .GetInstance<ILogger<CustomInitializationModule>>();
            logger.LogInformation(""Custom module initialized"");
        }

        public void Uninitialize(InitializationEngine context)
        {
            // Cleanup - runs on application shutdown
            var contentEvents = context.Locate.Advanced
                .GetInstance<IContentEvents>();

            contentEvents.PublishedContent -= OnContentPublished;
        }

        private void OnContentPublished(object? sender, ContentEventArgs e)
        {
            // Handle content published event
        }
    }
}",
                            IsInteractive = false
                        }
                    }
                },
                new Lesson
                {
                    Id = "ie-content-events",
                    ModuleId = "initialization-events",
                    Title = "Content Events",
                    Summary = "React to content changes with content events.",
                    Order = 3,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Know the available content events",
                        "Handle publishing, saving, and deleting events",
                        "Understand pre vs post events"
                    },
                    Content = @"
<h2>Content Events</h2>
<p><code>IContentEvents</code> provides events for all content operations. Use these to react to changes.</p>

<h3>Available Events</h3>
<table class=""min-w-full divide-y divide-gray-200 dark:divide-gray-700 my-4"">
    <thead>
        <tr>
            <th class=""px-4 py-2 text-left"">Event</th>
            <th class=""px-4 py-2 text-left"">When Fired</th>
        </tr>
    </thead>
    <tbody>
        <tr><td class=""px-4 py-2 font-mono text-sm"">CreatingContent</td><td class=""px-4 py-2"">Before content is created</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">CreatedContent</td><td class=""px-4 py-2"">After content is created</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">SavingContent</td><td class=""px-4 py-2"">Before content is saved</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">SavedContent</td><td class=""px-4 py-2"">After content is saved</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">PublishingContent</td><td class=""px-4 py-2"">Before content is published</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">PublishedContent</td><td class=""px-4 py-2"">After content is published</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">DeletingContent</td><td class=""px-4 py-2"">Before content is deleted</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">DeletedContent</td><td class=""px-4 py-2"">After content is deleted</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">MovingContent</td><td class=""px-4 py-2"">Before content is moved</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">MovedContent</td><td class=""px-4 py-2"">After content is moved</td></tr>
    </tbody>
</table>

<h3>Pre vs Post Events</h3>
<ul>
    <li><strong>Pre-events</strong> (e.g., SavingContent) - Can cancel the operation</li>
    <li><strong>Post-events</strong> (e.g., SavedContent) - Operation already completed</li>
</ul>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "content-events-example",
                            Title = "Content Event Handlers",
                            Description = "Handling various content events",
                            Type = ExampleType.Code,
                            ExampleContent = @"using EPiServer;
using EPiServer.Core;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;

[ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
public class ContentEventModule : IInitializableModule
{
    public void Initialize(InitializationEngine context)
    {
        var events = context.Locate.Advanced.GetInstance<IContentEvents>();

        events.PublishingContent += OnPublishingContent;
        events.PublishedContent += OnPublishedContent;
        events.SavingContent += OnSavingContent;
    }

    // Pre-event: can cancel or modify
    private void OnPublishingContent(object? sender, ContentEventArgs e)
    {
        if (e.Content is ArticlePage article)
        {
            // Validate before publishing
            if (string.IsNullOrEmpty(article.Title))
            {
                // Cancel the publish operation
                e.CancelAction = true;
                e.CancelReason = ""Title is required"";
            }
        }
    }

    // Post-event: react to completed action
    private void OnPublishedContent(object? sender, ContentEventArgs e)
    {
        // Clear cache, send notifications, etc.
        var logger = LogManager.GetLogger(typeof(ContentEventModule));
        logger.Info($""Content published: {e.Content.Name}"");
    }

    // Modify content before saving
    private void OnSavingContent(object? sender, ContentEventArgs e)
    {
        if (e.Content is PageData page)
        {
            // Auto-set a property
            var writable = page.CreateWritableClone() as PageData;
            // Note: Be careful to avoid infinite loops
        }
    }

    public void Uninitialize(InitializationEngine context)
    {
        var events = context.Locate.Advanced.GetInstance<IContentEvents>();
        events.PublishingContent -= OnPublishingContent;
        events.PublishedContent -= OnPublishedContent;
        events.SavingContent -= OnSavingContent;
    }
}",
                            IsInteractive = false
                        }
                    }
                },
                new Lesson
                {
                    Id = "ie-configuring-services",
                    ModuleId = "initialization-events",
                    Title = "Configuring Services",
                    Summary = "Register custom services and configure CMS options.",
                    Order = 4,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Register services with dependency injection",
                        "Configure CMS options",
                        "Use IConfigurableModule for service registration"
                    },
                    Content = @"
<h2>Configuring Services</h2>
<p>You can register your own services and configure CMS options using <code>IConfigurableModule</code>.</p>

<h3>Service Registration</h3>
<p>Implement <code>IConfigurableModule</code> to access <code>IServiceCollection</code> during startup:</p>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "configurable-module",
                            Title = "Configurable Module",
                            Description = "Register services and configure options",
                            Type = ExampleType.Code,
                            ExampleContent = @"using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using Microsoft.Extensions.DependencyInjection;

[ModuleDependency(typeof(ServiceContainerInitialization))]
public class DependencyInjectionModule : IConfigurableModule
{
    public void ConfigureContainer(ServiceConfigurationContext context)
    {
        // Register custom services
        context.Services.AddScoped<IMyService, MyService>();
        context.Services.AddSingleton<ICacheService, CacheService>();

        // Configure CMS options
        context.Services.Configure<ContentOptions>(options =>
        {
            options.RequireEditAccessToChangedByProperty = true;
        });

        // Configure scheduling options
        context.Services.Configure<SchedulerOptions>(options =>
        {
            options.Enabled = true;
        });

        // Replace a default service
        context.Services.AddSingleton<IContentRenderer, CustomContentRenderer>();
    }

    public void Initialize(InitializationEngine context)
    {
        // Additional initialization after DI is configured
    }

    public void Uninitialize(InitializationEngine context)
    {
    }
}",
                            IsInteractive = false
                        }
                    }
                }
            }
        };
    }

    private LearningModule BuildLocalizationModule()
    {
        return new LearningModule
        {
            Id = "localization",
            Title = "Localization",
            Description = "Master multilingual content and UI localization.",
            Icon = "globe-alt",
            Order = 6,
            Difficulty = ModuleDifficulty.Intermediate,
            Prerequisites = new[] { "content-management" },
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "loc-multilingual-overview",
                    ModuleId = "localization",
                    Title = "Multilingual Content Overview",
                    Summary = "Understand how CMS handles content in multiple languages.",
                    Order = 1,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Understand CMS language architecture",
                        "Know the difference between master and translated versions",
                        "Learn about language fallbacks"
                    },
                    Content = @"
<h2>Multilingual Content in CMS 12</h2>
<p>Optimizely CMS has built-in support for content in multiple languages. Each piece of content can exist in multiple language versions.</p>

<h3>Key Concepts</h3>
<ul>
    <li><strong>Master Language</strong> - The first language version created; holds common properties</li>
    <li><strong>Translated Versions</strong> - Additional language versions of the same content</li>
    <li><strong>Culture-Specific Properties</strong> - Properties marked with <code>[CultureSpecific]</code> have different values per language</li>
    <li><strong>Fallback Languages</strong> - Define which language to use if translation doesn't exist</li>
</ul>

<h3>ILocalizable Interface</h3>
<p>Content types that implement <code>ILocalizable</code> (like PageData and BlockData) support multiple languages.</p>

<h3>Language vs Culture</h3>
<ul>
    <li><strong>Content Language</strong> - The language of the content being edited/viewed</li>
    <li><strong>UI Culture</strong> - The language of the CMS editorial interface</li>
</ul>
",
                    Examples = new List<LessonExample>()
                },
                new Lesson
                {
                    Id = "loc-managing-languages",
                    ModuleId = "localization",
                    Title = "Managing Website Languages",
                    Summary = "Enable and configure languages for your website.",
                    Order = 2,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Enable languages in the CMS",
                        "Configure fallback languages",
                        "Set up language-specific URLs"
                    },
                    Content = @"
<h2>Managing Languages</h2>
<p>Languages are configured in Admin > Config > Manage Website Languages.</p>

<h3>Language Settings</h3>
<table class=""min-w-full divide-y divide-gray-200 dark:divide-gray-700 my-4"">
    <thead>
        <tr>
            <th class=""px-4 py-2 text-left"">Setting</th>
            <th class=""px-4 py-2 text-left"">Description</th>
        </tr>
    </thead>
    <tbody>
        <tr><td class=""px-4 py-2"">Enabled</td><td class=""px-4 py-2"">Language available for content creation</td></tr>
        <tr><td class=""px-4 py-2"">Fallback Language</td><td class=""px-4 py-2"">Use this language if translation missing</td></tr>
        <tr><td class=""px-4 py-2"">URL Segment</td><td class=""px-4 py-2"">Language prefix in URLs (e.g., /en/, /sv/)</td></tr>
    </tbody>
</table>

<h3>URL Patterns</h3>
<p>Languages can be indicated in URLs via:</p>
<ul>
    <li><strong>URL Segment</strong>: <code>example.com/en/about</code></li>
    <li><strong>Hostname</strong>: <code>en.example.com/about</code></li>
    <li><strong>Query String</strong>: <code>example.com/about?lang=en</code></li>
</ul>
",
                    Examples = new List<LessonExample>()
                },
                new Lesson
                {
                    Id = "loc-loading-content",
                    ModuleId = "localization",
                    Title = "Loading Content in Specific Languages",
                    Summary = "Load content in specific languages using LanguageSelector.",
                    Order = 3,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Use LanguageSelector to specify language",
                        "Handle missing translations gracefully",
                        "Work with language branches"
                    },
                    Content = @"
<h2>Loading Localized Content</h2>
<p>Use <code>LanguageSelector</code> to load content in a specific language.</p>

<h3>LanguageSelector Options</h3>
<ul>
    <li><code>AutoDetect()</code> - Uses current culture context</li>
    <li><code>Specific(culture)</code> - Load exact language</li>
    <li><code>Fallback(culture, true)</code> - Load with fallback enabled</li>
    <li><code>MasterLanguage()</code> - Load master language version</li>
</ul>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "language-loading",
                            Title = "Loading Content by Language",
                            Description = "Load content in specific languages",
                            Type = ExampleType.Code,
                            ExampleContent = @"using EPiServer;
using EPiServer.Core;
using EPiServer.Globalization;
using System.Globalization;

public class LocalizedContentService
{
    private readonly IContentLoader _contentLoader;

    public LocalizedContentService(IContentLoader contentLoader)
    {
        _contentLoader = contentLoader;
    }

    // Load content in current culture
    public T? LoadInCurrentCulture<T>(ContentReference reference) where T : IContent
    {
        return _contentLoader.Get<T>(reference, LanguageSelector.AutoDetect());
    }

    // Load content in specific language
    public T? LoadInLanguage<T>(ContentReference reference, string language)
        where T : IContent
    {
        var culture = new CultureInfo(language);
        return _contentLoader.Get<T>(reference, new LanguageSelector(culture));
    }

    // Load with fallback
    public T? LoadWithFallback<T>(ContentReference reference, string language)
        where T : IContent
    {
        var culture = new CultureInfo(language);
        return _contentLoader.Get<T>(
            reference,
            LanguageSelector.Fallback(culture, enableMasterLanguageFallback: true));
    }

    // Get all language versions
    public IEnumerable<T> GetAllLanguageVersions<T>(ContentReference reference)
        where T : IContent, ILocalizable
    {
        var languages = _contentLoader.GetExistingLanguages(reference);
        return languages.Select(lang =>
            _contentLoader.Get<T>(reference, new LanguageSelector(lang)));
    }

    // Check if translation exists
    public bool HasTranslation(ContentReference reference, string language)
    {
        var culture = new CultureInfo(language);
        var languages = _contentLoader.GetExistingLanguages(reference);
        return languages.Contains(culture);
    }
}",
                            IsInteractive = false
                        }
                    }
                },
                new Lesson
                {
                    Id = "loc-localization-service",
                    ModuleId = "localization",
                    Title = "Localizing the UI",
                    Summary = "Use LocalizationService for UI text translations.",
                    Order = 4,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Use LocalizationService for translations",
                        "Create XML translation files",
                        "Override system strings"
                    },
                    Content = @"
<h2>Localizing the User Interface</h2>
<p><code>LocalizationService</code> provides translated strings for the UI (not content). Use it for labels, buttons, and messages.</p>

<h3>Translation Files</h3>
<p>Place XML translation files in the <code>lang/</code> folder:</p>
<pre class=""bg-gray-100 dark:bg-gray-800 p-4 rounded-lg overflow-x-auto"">
lang/
├── Labels_en.xml
├── Labels_sv.xml
└── Labels_de.xml
</pre>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "translation-xml",
                            Title = "Translation XML File",
                            Description = "Create translation files",
                            Type = ExampleType.Configuration,
                            ExampleContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<!-- lang/Labels_en.xml -->
<languages>
  <language name=""English"" id=""en"">
    <mysite>
      <common>
        <readmore>Read more</readmore>
        <contactus>Contact us</contactus>
        <searchplaceholder>Search...</searchplaceholder>
      </common>
      <validation>
        <required>This field is required</required>
        <invalidemail>Please enter a valid email</invalidemail>
      </validation>
      <pages>
        <article>
          <byline>Written by {0}</byline>
          <publishedon>Published on {0:d}</publishedon>
        </article>
      </pages>
    </mysite>
  </language>
</languages>",
                            IsInteractive = false
                        },
                        new LessonExample
                        {
                            Id = "using-localization",
                            Title = "Using LocalizationService",
                            Description = "Get translated strings in code and views",
                            Type = ExampleType.Code,
                            ExampleContent = @"// In a controller or service
public class MyController : Controller
{
    private readonly LocalizationService _localization;

    public MyController(LocalizationService localization)
    {
        _localization = localization;
    }

    public IActionResult Index()
    {
        // Get a simple translation
        var readMore = _localization.GetString(""/mysite/common/readmore"");

        // Get with formatting
        var byline = _localization.GetStringByCulture(
            ""/mysite/pages/article/byline"",
            CultureInfo.CurrentUICulture,
            ""John Doe"");

        ViewBag.ReadMore = readMore;
        ViewBag.Byline = byline;
        return View();
    }
}

// In a Razor view
@inject LocalizationService Localization

<a href=""@Model.Link"">
    @Localization.GetString(""/mysite/common/readmore"")
</a>

// Or use the Html helper
<label>@Html.Translate(""/mysite/common/searchplaceholder"")</label>",
                            IsInteractive = false
                        }
                    }
                }
            }
        };
    }

    private LearningModule BuildSearchNavigationModule()
    {
        return new LearningModule
        {
            Id = "search-navigation",
            Title = "Search & Navigation",
            Description = "Implement search functionality with Optimizely Search & Navigation.",
            Icon = "magnifying-glass",
            Order = 7,
            Difficulty = ModuleDifficulty.Intermediate,
            Prerequisites = new[] { "content-types" },
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "sn-overview",
                    ModuleId = "search-navigation",
                    Title = "Search & Navigation Overview",
                    Summary = "Understand Optimizely Search & Navigation and its capabilities.",
                    Order = 1,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Understand what Search & Navigation provides",
                        "Know the licensing and setup requirements",
                        "Learn about automatic indexing"
                    },
                    Content = @"
<h2>Optimizely Search & Navigation</h2>
<p>Search & Navigation (formerly Episerver Find) is a cloud-based search engine built on Elasticsearch. It provides:</p>

<h3>Key Features</h3>
<ul>
    <li><strong>Full-text search</strong> - Search across all content</li>
    <li><strong>Automatic indexing</strong> - Content indexed on publish</li>
    <li><strong>Faceted search</strong> - Filter by categories, dates, etc.</li>
    <li><strong>Unified search</strong> - Search CMS content and Commerce products</li>
    <li><strong>Synonyms & boosting</strong> - Improve search relevance</li>
</ul>

<h3>Setup</h3>
<p>If using DXP, Search & Navigation is included. Otherwise, you need a license and connection to the cloud service.</p>

<h3>Automatic Indexing</h3>
<p>When you install <code>EPiServer.Find.Cms</code>, published content is automatically indexed. The index updates when content is:</p>
<ul>
    <li>Published</li>
    <li>Moved</li>
    <li>Deleted</li>
</ul>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "find-setup",
                            Title = "Configure Search & Navigation",
                            Description = "Add Search & Navigation to your project",
                            Type = ExampleType.Configuration,
                            ExampleContent = @"// appsettings.json
{
  ""EPiServer"": {
    ""Find"": {
      ""ServiceUrl"": ""https://demo01.find.episerver.net/xxx"",
      ""DefaultIndex"": ""your_index_name""
    }
  }
}

// Program.cs
builder.Services.AddFind();",
                            IsInteractive = false
                        }
                    }
                },
                new Lesson
                {
                    Id = "sn-basic-search",
                    ModuleId = "search-navigation",
                    Title = "Building Search Queries",
                    Summary = "Create search queries using the typed search API.",
                    Order = 2,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Use IClient to create search queries",
                        "Search for specific content types",
                        "Filter and sort results"
                    },
                    Content = @"
<h2>Building Search Queries</h2>
<p>Use <code>IClient</code> to build typed search queries against your content.</p>

<h3>Query Building Pattern</h3>
<ol>
    <li>Start with <code>Search&lt;T&gt;()</code> for typed queries</li>
    <li>Add <code>For(searchText)</code> for full-text search</li>
    <li>Add <code>Filter()</code> for filtering</li>
    <li>Add <code>OrderBy()</code> for sorting</li>
    <li>Call <code>GetContentResult()</code> to execute</li>
</ol>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "search-examples",
                            Title = "Search Query Examples",
                            Description = "Various search query patterns",
                            Type = ExampleType.Code,
                            ExampleContent = @"using EPiServer.Find;
using EPiServer.Find.Cms;
using EPiServer.Find.Framework;

public class SearchService
{
    private readonly IClient _searchClient;

    public SearchService(IClient searchClient)
    {
        _searchClient = searchClient;
    }

    // Basic full-text search
    public IContentResult<ArticlePage> SearchArticles(string query)
    {
        return _searchClient.Search<ArticlePage>()
            .For(query)
            .GetContentResult();
    }

    // Search with filtering
    public IContentResult<ArticlePage> SearchRecentArticles(
        string query, DateTime fromDate)
    {
        return _searchClient.Search<ArticlePage>()
            .For(query)
            .Filter(x => x.PublishedDate.GreaterThan(fromDate))
            .OrderByDescending(x => x.PublishedDate)
            .Take(20)
            .GetContentResult();
    }

    // Search across multiple types
    public IContentResult<PageData> SearchAllPages(string query)
    {
        return _searchClient.Search<PageData>()
            .For(query)
            .FilterForVisitor()  // Respect access rights
            .PublishedInCurrentLanguage()  // Current language only
            .GetContentResult();
    }

    // Pagination
    public IContentResult<ArticlePage> SearchWithPaging(
        string query, int page, int pageSize)
    {
        return _searchClient.Search<ArticlePage>()
            .For(query)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .GetContentResult();
    }
}",
                            IsInteractive = false
                        }
                    }
                },
                new Lesson
                {
                    Id = "sn-facets",
                    ModuleId = "search-navigation",
                    Title = "Faceted Search",
                    Summary = "Add facets to allow filtering search results.",
                    Order = 3,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Understand what facets are",
                        "Add facets to search queries",
                        "Use facets for filtering"
                    },
                    Content = @"
<h2>Faceted Search</h2>
<p>Facets let users filter results by categories, dates, or other properties. They show available options with counts.</p>

<h3>Facet Types</h3>
<ul>
    <li><strong>TermsFacet</strong> - For categories, tags, types</li>
    <li><strong>DateHistogramFacet</strong> - For date ranges</li>
    <li><strong>NumericRangeFacet</strong> - For numeric ranges</li>
</ul>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "facet-example",
                            Title = "Adding Facets to Search",
                            Description = "Implement faceted navigation",
                            Type = ExampleType.Code,
                            ExampleContent = @"public class FacetedSearchResult
{
    public IEnumerable<ArticlePage> Articles { get; set; }
    public IEnumerable<TermCount> Categories { get; set; }
    public IEnumerable<TermCount> Authors { get; set; }
}

public FacetedSearchResult SearchWithFacets(
    string query,
    string? categoryFilter = null)
{
    var search = _searchClient.Search<ArticlePage>()
        .For(query)
        .TermsFacetFor(x => x.Category)
        .TermsFacetFor(x => x.Author);

    // Apply category filter if selected
    if (!string.IsNullOrEmpty(categoryFilter))
    {
        search = search.Filter(x => x.Category.Match(categoryFilter));
    }

    var result = search.GetContentResult();

    return new FacetedSearchResult
    {
        Articles = result.Items,
        Categories = result.TermsFacetFor(x => x.Category).Terms,
        Authors = result.TermsFacetFor(x => x.Author).Terms
    };
}",
                            IsInteractive = false
                        }
                    }
                }
            }
        };
    }

    private LearningModule BuildFormsModule()
    {
        return new LearningModule
        {
            Id = "forms",
            Title = "Optimizely Forms",
            Description = "Build and customize forms with Optimizely Forms.",
            Icon = "clipboard-document-list",
            Order = 8,
            Difficulty = ModuleDifficulty.Intermediate,
            Prerequisites = new[] { "content-types" },
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "forms-overview",
                    ModuleId = "forms",
                    Title = "Optimizely Forms Overview",
                    Summary = "Understand Optimizely Forms and its capabilities.",
                    Order = 1,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Understand what Optimizely Forms provides",
                        "Know the built-in form elements",
                        "Learn about form submissions and actors"
                    },
                    Content = @"
<h2>Optimizely Forms</h2>
<p>Optimizely Forms is an add-on that lets editors create forms without developer involvement. Forms can be embedded in any page using a ContentArea.</p>

<h3>Key Features</h3>
<ul>
    <li><strong>Drag-and-drop builder</strong> - Editors create forms visually</li>
    <li><strong>Built-in elements</strong> - Text, email, selection, file upload, etc.</li>
    <li><strong>Validation</strong> - Required fields, patterns, custom validators</li>
    <li><strong>Actors</strong> - Actions triggered on submission (email, webhook)</li>
    <li><strong>Data export</strong> - Export submissions as CSV/Excel</li>
</ul>

<h3>Installation</h3>
<p>Install the NuGet package:</p>
<pre class=""bg-gray-100 dark:bg-gray-800 p-4 rounded-lg"">
dotnet add package EPiServer.Forms
dotnet add package EPiServer.Forms.UI
</pre>

<h3>Built-in Elements</h3>
<ul>
    <li>Text input, Textarea, Number</li>
    <li>Selection (dropdown, radio, checkbox)</li>
    <li>File upload</li>
    <li>CAPTCHA</li>
    <li>Hidden field</li>
    <li>Submit button</li>
</ul>
",
                    Examples = new List<LessonExample>()
                },
                new Lesson
                {
                    Id = "forms-embedding",
                    ModuleId = "forms",
                    Title = "Embedding Forms in Pages",
                    Summary = "Add forms to pages using ContentArea.",
                    Order = 2,
                    EstimatedMinutes = 6,
                    LearningObjectives = new List<string>
                    {
                        "Create a form container property",
                        "Render forms in views",
                        "Style form elements"
                    },
                    Content = @"
<h2>Embedding Forms</h2>
<p>Forms are content items that can be dropped into any ContentArea. The most common approach is to add a dedicated form area to your pages.</p>

<h3>Steps to Embed</h3>
<ol>
    <li>Add a <code>ContentArea</code> property to your page type</li>
    <li>Optionally restrict to form types using <code>[AllowedTypes]</code></li>
    <li>Render the ContentArea in your view</li>
    <li>Editors can then drag forms into the area</li>
</ol>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "forms-page-property",
                            Title = "Add Form Area to Page",
                            Description = "Page type with form container",
                            Type = ExampleType.Code,
                            ExampleContent = @"using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Forms.Core;

public class ContactPage : PageData
{
    [Display(Name = ""Main Content"")]
    public virtual XhtmlString? MainBody { get; set; }

    // Restrict to only allow FormContainerBlock
    [AllowedTypes(typeof(FormContainerBlock))]
    [Display(Name = ""Contact Form"")]
    public virtual ContentArea? FormArea { get; set; }
}

// In the view
@model ContactPage

<article>
    @Html.PropertyFor(m => m.MainBody)
</article>

<section class=""contact-form"">
    @Html.PropertyFor(m => m.FormArea)
</section>",
                            IsInteractive = false
                        }
                    }
                },
                new Lesson
                {
                    Id = "forms-custom-elements",
                    ModuleId = "forms",
                    Title = "Creating Custom Form Elements",
                    Summary = "Extend Forms with custom element types.",
                    Order = 3,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Create a custom form element",
                        "Add validation to custom elements",
                        "Create the editor and rendering views"
                    },
                    Content = @"
<h2>Custom Form Elements</h2>
<p>You can extend Optimizely Forms by creating custom element types for specialized input scenarios.</p>

<h3>Creating a Custom Element</h3>
<ol>
    <li>Create a class inheriting from an existing element (e.g., <code>TextboxElementBlock</code>)</li>
    <li>Add custom properties for configuration</li>
    <li>Create a view for rendering</li>
    <li>Optionally add custom validation</li>
</ol>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "custom-element",
                            Title = "Custom Phone Number Element",
                            Description = "A phone number input with formatting",
                            Type = ExampleType.Code,
                            ExampleContent = @"using EPiServer.DataAnnotations;
using EPiServer.Forms.Core;
using EPiServer.Forms.Implementation.Elements;

[ContentType(
    DisplayName = ""Phone Number"",
    GUID = ""12345678-1234-1234-1234-123456789012"")]
public class PhoneNumberElementBlock : TextboxElementBlock
{
    [Display(Name = ""Country Code"")]
    public virtual string? DefaultCountryCode { get; set; }

    [Display(Name = ""Placeholder"")]
    public override string PlaceHolder
    {
        get => base.PlaceHolder ?? ""(555) 123-4567"";
        set => base.PlaceHolder = value;
    }

    public override string Validators =>
        $""phone|{base.Validators}"";
}

// View: Views/Shared/ElementBlocks/PhoneNumberElementBlock.cshtml
@model PhoneNumberElementBlock

<div class=""Form__Element"">
    <label for=""@Model.FormElement.Guid"">
        @Model.Label
        @if(Model.IsRequired) { <span class=""required"">*</span> }
    </label>
    <input type=""tel""
           id=""@Model.FormElement.Guid""
           name=""@Model.FormElement.Guid""
           placeholder=""@Model.PlaceHolder""
           data-f-type=""textbox""
           @(Model.IsRequired ? ""required"" : """") />
    <span class=""Form__Element__ValidationError""></span>
</div>",
                            IsInteractive = false
                        }
                    }
                },
                new Lesson
                {
                    Id = "forms-actors",
                    ModuleId = "forms",
                    Title = "Form Actors and Submission Handling",
                    Summary = "Handle form submissions with custom actors.",
                    Order = 4,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Understand form actors",
                        "Create custom actors for integrations",
                        "Access submission data programmatically"
                    },
                    Content = @"
<h2>Form Actors</h2>
<p>Actors are triggered when a form is submitted. Built-in actors include:</p>
<ul>
    <li><strong>Send emails</strong> - Email notification to admins or submitter</li>
    <li><strong>Post to URL</strong> - Webhook for external systems</li>
</ul>

<h3>Custom Actors</h3>
<p>Create custom actors for CRM integration, database storage, or other workflows.</p>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "custom-actor",
                            Title = "Custom CRM Integration Actor",
                            Description = "Send submissions to a CRM system",
                            Type = ExampleType.Code,
                            ExampleContent = @"using EPiServer.Forms.Core.PostSubmissionActor;
using EPiServer.Forms.Core.Models;

public class CrmIntegrationActor : PostSubmissionActorBase
{
    private readonly ICrmService _crmService;

    public CrmIntegrationActor(ICrmService crmService)
    {
        _crmService = crmService;
    }

    public override object Run(object input)
    {
        var submission = input as FormSubmission;
        if (submission == null) return null;

        // Get form field values
        var formData = submission.Data;

        // Map to CRM contact
        var contact = new CrmContact
        {
            Email = GetFieldValue(formData, ""email""),
            Name = GetFieldValue(formData, ""name""),
            Company = GetFieldValue(formData, ""company""),
            Message = GetFieldValue(formData, ""message"")
        };

        // Send to CRM
        _crmService.CreateLead(contact);

        return ""Lead created successfully"";
    }

    private string? GetFieldValue(
        IDictionary<string, object> data, string fieldName)
    {
        var key = data.Keys.FirstOrDefault(k =>
            k.Contains(fieldName, StringComparison.OrdinalIgnoreCase));
        return key != null ? data[key]?.ToString() : null;
    }
}",
                            IsInteractive = false
                        }
                    }
                }
            }
        };
    }

    private LearningModule BuildAccessRightsModule()
    {
        return new LearningModule
        {
            Id = "access-rights",
            Title = "Access Rights & Security",
            Description = "Configure access rights, roles, and security.",
            Icon = "shield-check",
            Order = 9,
            Difficulty = ModuleDifficulty.Intermediate,
            Prerequisites = new[] { "getting-started" },
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "ar-overview",
                    ModuleId = "access-rights",
                    Title = "Access Rights Overview",
                    Summary = "Understand the CMS access rights model.",
                    Order = 1,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Understand the access rights model",
                        "Know the available access levels",
                        "Learn about inheritance"
                    },
                    Content = @"
<h2>Access Rights Model</h2>
<p>Optimizely CMS uses a hierarchical, role-based access control system. Access rights can be set on any content item and are inherited down the tree.</p>

<h3>Access Levels</h3>
<table class=""min-w-full divide-y divide-gray-200 dark:divide-gray-700 my-4"">
    <thead>
        <tr>
            <th class=""px-4 py-2 text-left"">Level</th>
            <th class=""px-4 py-2 text-left"">Description</th>
        </tr>
    </thead>
    <tbody>
        <tr><td class=""px-4 py-2 font-mono text-sm"">Read</td><td class=""px-4 py-2"">View content</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">Create</td><td class=""px-4 py-2"">Create child content</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">Change</td><td class=""px-4 py-2"">Edit existing content</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">Delete</td><td class=""px-4 py-2"">Delete content</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">Publish</td><td class=""px-4 py-2"">Publish content</td></tr>
        <tr><td class=""px-4 py-2 font-mono text-sm"">Administer</td><td class=""px-4 py-2"">Change access rights</td></tr>
    </tbody>
</table>

<h3>Inheritance</h3>
<p>Rights flow down from parent to children. You can break inheritance at any point to set specific permissions.</p>

<h3>Built-in Groups</h3>
<ul>
    <li><strong>CmsAdmins</strong> - Full admin access</li>
    <li><strong>CmsEditors</strong> - Edit view access</li>
    <li><strong>Everyone</strong> - All authenticated users</li>
    <li><strong>Anonymous</strong> - Unauthenticated visitors</li>
</ul>
",
                    Examples = new List<LessonExample>()
                },
                new Lesson
                {
                    Id = "ar-checking-access",
                    ModuleId = "access-rights",
                    Title = "Checking Access in Code",
                    Summary = "Verify user permissions programmatically.",
                    Order = 2,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Check user access to content",
                        "Use IContentSecurityRepository",
                        "Filter content by access rights"
                    },
                    Content = @"
<h2>Checking Access Programmatically</h2>
<p>Use <code>IContentLoader</code> with access checking or <code>IContentSecurityRepository</code> for explicit checks.</p>

<h3>Automatic Filtering</h3>
<p>By default, <code>IContentLoader</code> respects access rights - users only see content they can access.</p>

<h3>Explicit Checks</h3>
<p>Use <code>IContentSecurityRepository</code> for fine-grained permission checks.</p>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "access-checking",
                            Title = "Access Checking Examples",
                            Description = "Check permissions in code",
                            Type = ExampleType.Code,
                            ExampleContent = @"using EPiServer;
using EPiServer.Core;
using EPiServer.Security;

public class ContentAccessService
{
    private readonly IContentLoader _contentLoader;
    private readonly IContentSecurityRepository _securityRepo;

    public ContentAccessService(
        IContentLoader contentLoader,
        IContentSecurityRepository securityRepo)
    {
        _contentLoader = contentLoader;
        _securityRepo = securityRepo;
    }

    // Check if current user can read
    public bool CanRead(ContentReference reference)
    {
        var descriptor = _securityRepo.Get(reference);
        return descriptor.HasAccess(
            PrincipalInfo.CurrentPrincipal,
            AccessLevel.Read);
    }

    // Check if current user can publish
    public bool CanPublish(ContentReference reference)
    {
        var descriptor = _securityRepo.Get(reference);
        return descriptor.HasAccess(
            PrincipalInfo.CurrentPrincipal,
            AccessLevel.Publish);
    }

    // Load content only if user has access
    public T? LoadIfAllowed<T>(ContentReference reference) where T : IContent
    {
        // This automatically checks read access
        if (_contentLoader.TryGet<T>(reference, out var content))
        {
            return content;
        }
        return default;
    }

    // Load bypassing security (admin scenarios)
    public T? LoadBypassingAccess<T>(ContentReference reference) where T : IContent
    {
        return _contentLoader.Get<T>(
            reference,
            new LoaderOptions { BypassAccessCheck = true });
    }
}",
                            IsInteractive = false
                        }
                    }
                },
                new Lesson
                {
                    Id = "ar-virtual-roles",
                    ModuleId = "access-rights",
                    Title = "Virtual Roles",
                    Summary = "Create dynamic roles based on conditions.",
                    Order = 3,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Understand virtual roles",
                        "Create custom virtual roles",
                        "Use virtual roles for content access"
                    },
                    Content = @"
<h2>Virtual Roles</h2>
<p>Virtual roles are dynamic roles evaluated at runtime based on conditions. Unlike regular roles, membership is determined by code.</p>

<h3>Built-in Virtual Roles</h3>
<ul>
    <li><strong>Everyone</strong> - All users including anonymous</li>
    <li><strong>Authenticated</strong> - Any logged-in user</li>
    <li><strong>Anonymous</strong> - Not logged in</li>
    <li><strong>Creator</strong> - The content creator</li>
</ul>

<h3>Custom Virtual Roles</h3>
<p>Create custom virtual roles for scenarios like:</p>
<ul>
    <li>Users from specific IP ranges</li>
    <li>Users with specific claims</li>
    <li>Time-based access (office hours only)</li>
</ul>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "virtual-role",
                            Title = "Custom Virtual Role",
                            Description = "IP-based virtual role",
                            Type = ExampleType.Code,
                            ExampleContent = @"using EPiServer.Security;
using System.Security.Principal;

public class InternalNetworkRole : VirtualRoleProviderBase
{
    private readonly IHttpContextAccessor _httpContext;
    private readonly string[] _internalIpRanges = { ""10."", ""192.168."" };

    public InternalNetworkRole(IHttpContextAccessor httpContext)
    {
        _httpContext = httpContext;
    }

    public override bool IsInVirtualRole(
        IPrincipal principal, object context)
    {
        var ipAddress = _httpContext.HttpContext?.Connection
            ?.RemoteIpAddress?.ToString();

        if (string.IsNullOrEmpty(ipAddress))
            return false;

        return _internalIpRanges.Any(range =>
            ipAddress.StartsWith(range));
    }
}

// Register in Program.cs
services.AddVirtualRole<InternalNetworkRole>(""InternalUsers"");",
                            IsInteractive = false
                        }
                    }
                }
            }
        };
    }

    private LearningModule BuildCachingPerformanceModule()
    {
        return new LearningModule
        {
            Id = "caching-performance",
            Title = "Caching & Performance",
            Description = "Optimize performance with caching strategies.",
            Icon = "rocket-launch",
            Order = 10,
            Difficulty = ModuleDifficulty.Advanced,
            Prerequisites = new[] { "content-management" },
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "cp-caching-overview",
                    ModuleId = "caching-performance",
                    Title = "Caching Overview",
                    Summary = "Understand caching in Optimizely CMS.",
                    Order = 1,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Understand built-in CMS caching",
                        "Know the different cache layers",
                        "Learn cache invalidation patterns"
                    },
                    Content = @"
<h2>Caching in CMS 12</h2>
<p>CMS automatically caches content and other data to improve performance. Understanding these caches helps you optimize your site.</p>

<h3>Built-in Caches</h3>
<ul>
    <li><strong>Content cache</strong> - Loaded content instances are cached</li>
    <li><strong>Property cache</strong> - Property values are cached</li>
    <li><strong>Content type cache</strong> - Type definitions are cached</li>
</ul>

<h3>Cache Invalidation</h3>
<p>CMS automatically invalidates cache when content changes. In load-balanced environments, cache invalidation is synchronized across servers.</p>

<h3>No Built-in Output Caching</h3>
<p>CMS 12 does not have built-in output caching. Use ASP.NET Core response caching or third-party solutions.</p>
",
                    Examples = new List<LessonExample>()
                },
                new Lesson
                {
                    Id = "cp-object-cache",
                    ModuleId = "caching-performance",
                    Title = "Object Caching",
                    Summary = "Cache custom objects with IObjectInstanceCache.",
                    Order = 2,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Use IObjectInstanceCache for custom caching",
                        "Set cache policies and dependencies",
                        "Handle cache in load-balanced environments"
                    },
                    Content = @"
<h2>Object Caching</h2>
<p>Use <code>IObjectInstanceCache</code> to cache your own objects. For load-balanced environments, use <code>ISynchronizedObjectInstanceCache</code>.</p>

<h3>Cache Policies</h3>
<ul>
    <li><strong>Absolute expiration</strong> - Cache expires at a specific time</li>
    <li><strong>Sliding expiration</strong> - Cache expires after inactivity</li>
    <li><strong>Dependencies</strong> - Cache invalidates when dependencies change</li>
</ul>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "object-cache-example",
                            Title = "Using Object Cache",
                            Description = "Cache expensive operations",
                            Type = ExampleType.Code,
                            ExampleContent = @"using EPiServer.Framework.Cache;

public class CachedDataService
{
    private readonly ISynchronizedObjectInstanceCache _cache;
    private readonly IExternalApiClient _apiClient;
    private const string CacheKey = ""ExternalApiData"";

    public CachedDataService(
        ISynchronizedObjectInstanceCache cache,
        IExternalApiClient apiClient)
    {
        _cache = cache;
        _apiClient = apiClient;
    }

    public async Task<ExternalData> GetDataAsync()
    {
        // Try to get from cache
        var cached = _cache.Get(CacheKey) as ExternalData;
        if (cached != null)
        {
            return cached;
        }

        // Load from external source
        var data = await _apiClient.FetchDataAsync();

        // Cache with 5 minute expiration
        var policy = new CacheEvictionPolicy(
            TimeSpan.FromMinutes(5),
            CacheTimeoutType.Absolute);

        _cache.Insert(CacheKey, data, policy);

        return data;
    }

    // Cache with content dependency
    public ArticleData GetArticleData(ContentReference articleRef)
    {
        var cacheKey = $""ArticleData_{articleRef}"";

        var cached = _cache.Get(cacheKey) as ArticleData;
        if (cached != null) return cached;

        var data = BuildArticleData(articleRef);

        // Invalidate when the content changes
        var dependency = new ContentCacheDependency(articleRef);
        var policy = new CacheEvictionPolicy(dependency);

        _cache.Insert(cacheKey, data, policy);

        return data;
    }

    public void InvalidateCache()
    {
        _cache.Remove(CacheKey);
    }
}",
                            IsInteractive = false
                        }
                    }
                },
                new Lesson
                {
                    Id = "cp-output-caching",
                    ModuleId = "caching-performance",
                    Title = "Output Caching",
                    Summary = "Implement output caching for HTML responses.",
                    Order = 3,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Implement response caching",
                        "Use CacheTagHelper for partial caching",
                        "Invalidate cache on content publish"
                    },
                    Content = @"
<h2>Output Caching</h2>
<p>CMS 12 doesn't include output caching, but you can implement it using ASP.NET Core features or third-party packages.</p>

<h3>Options</h3>
<ul>
    <li><strong>Response Caching Middleware</strong> - Basic ASP.NET Core caching</li>
    <li><strong>.NET 7+ Output Caching</strong> - More flexible caching policies</li>
    <li><strong>Cache Tag Helper</strong> - Cache portions of views</li>
    <li><strong>Third-party</strong> - WebEssentials.AspNetCore.OutputCaching</li>
</ul>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "cache-tag-helper",
                            Title = "Partial View Caching",
                            Description = "Cache expensive view sections",
                            Type = ExampleType.Code,
                            ExampleContent = @"@* Cache an expensive section of the view *@
<cache expires-after=""@TimeSpan.FromMinutes(10)"" vary-by=""@Model.ContentLink"">
    @await Component.InvokeAsync(""ExpensiveWidget"", new { page = Model })
</cache>

@* Cache with content-based invalidation *@
@inject IContentCacheKeyCreator CacheKeyCreator

<cache expires-after=""@TimeSpan.FromHours(1)""
       vary-by=""@CacheKeyCreator.CreateCommonCacheKey(Model.ContentLink)"">
    <nav class=""navigation"">
        @await Html.PartialAsync(""_MainNavigation"")
    </nav>
</cache>

// For full page caching, handle publish events
[ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
public class CacheInvalidationModule : IInitializableModule
{
    public void Initialize(InitializationEngine context)
    {
        var events = context.Locate.Advanced.GetInstance<IContentEvents>();
        events.PublishedContent += (s, e) =>
        {
            // Clear output cache when content is published
            var cache = context.Locate.Advanced
                .GetInstance<IOutputCacheManager>();
            cache.ClearAsync().Wait();
        };
    }
}",
                            IsInteractive = false
                        }
                    }
                }
            }
        };
    }

    private LearningModule BuildScheduledJobsAdvancedModule()
    {
        return new LearningModule
        {
            Id = "scheduled-jobs-advanced",
            Title = "Scheduled Jobs & Advanced Topics",
            Description = "Create scheduled jobs and explore advanced development.",
            Icon = "clock",
            Order = 11,
            Difficulty = ModuleDifficulty.Advanced,
            Prerequisites = new[] { "initialization-events" },
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "sja-scheduled-jobs",
                    ModuleId = "scheduled-jobs-advanced",
                    Title = "Creating Scheduled Jobs",
                    Summary = "Build background jobs that run on a schedule.",
                    Order = 1,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Create a scheduled job",
                        "Configure job scheduling",
                        "Handle job execution and progress"
                    },
                    Content = @"
<h2>Scheduled Jobs</h2>
<p>Scheduled jobs run background tasks on a configurable schedule. Use them for:</p>
<ul>
    <li>Content cleanup and maintenance</li>
    <li>Data imports/exports</li>
    <li>Cache warming</li>
    <li>Integration synchronization</li>
</ul>

<h3>Job Lifecycle</h3>
<ol>
    <li>Job is discovered during startup</li>
    <li>Registered in the admin interface</li>
    <li>Executed manually or on schedule</li>
    <li>Progress and status reported to UI</li>
</ol>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "scheduled-job",
                            Title = "Content Cleanup Job",
                            Description = "A job that cleans up old content",
                            Type = ExampleType.Code,
                            ExampleContent = @"using EPiServer.Core;
using EPiServer.PlugIn;
using EPiServer.Scheduler;

[ScheduledPlugIn(
    DisplayName = ""Content Cleanup Job"",
    Description = ""Removes expired content older than 30 days"",
    GUID = ""11111111-2222-3333-4444-555555555555"",
    DefaultEnabled = true,
    InitialTime = ""00:30"",
    IntervalType = ScheduledIntervalType.Days,
    IntervalLength = 1)]
public class ContentCleanupJob : ScheduledJobBase
{
    private readonly IContentRepository _contentRepository;
    private bool _stopSignaled;
    private int _processedCount;

    public ContentCleanupJob(IContentRepository contentRepository)
    {
        _contentRepository = contentRepository;
        IsStoppable = true;
    }

    public override void Stop()
    {
        _stopSignaled = true;
    }

    public override string Execute()
    {
        _stopSignaled = false;
        _processedCount = 0;

        var cutoffDate = DateTime.Now.AddDays(-30);
        var expiredContent = FindExpiredContent(cutoffDate);

        foreach (var content in expiredContent)
        {
            if (_stopSignaled)
            {
                return $""Job stopped. Processed {_processedCount} items."";
            }

            // Delete or archive the content
            _contentRepository.Delete(content.ContentLink, true);
            _processedCount++;

            // Report progress
            OnStatusChanged($""Processed {_processedCount} items..."");
        }

        return $""Cleanup complete. Removed {_processedCount} items."";
    }

    private IEnumerable<IContent> FindExpiredContent(DateTime cutoff)
    {
        // Implementation to find expired content
        yield break;
    }
}",
                            IsInteractive = false
                        }
                    }
                },
                new Lesson
                {
                    Id = "sja-configuration",
                    ModuleId = "scheduled-jobs-advanced",
                    Title = "Configuration Options",
                    Summary = "Configure CMS behavior through options classes.",
                    Order = 2,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Configure CMS options in code",
                        "Use appsettings.json for configuration",
                        "Override settings per environment"
                    },
                    Content = @"
<h2>Configuration Options</h2>
<p>CMS settings are exposed through strongly-typed options classes that can be configured in code or appsettings.json.</p>

<h3>Common Options Classes</h3>
<ul>
    <li><code>ContentOptions</code> - Content behavior settings</li>
    <li><code>SchedulerOptions</code> - Job scheduler settings</li>
    <li><code>BlobOptions</code> - Blob storage configuration</li>
    <li><code>UIOptions</code> - Editor UI settings</li>
</ul>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "config-options",
                            Title = "Configuring CMS Options",
                            Description = "Configure options in code and appsettings.json",
                            Type = ExampleType.Code,
                            ExampleContent = @"// Program.cs - Configure in code
builder.Services.Configure<ContentOptions>(options =>
{
    options.RequireEditAccessToChangedByProperty = true;
    options.MultiSiteEnabled = true;
});

builder.Services.Configure<SchedulerOptions>(options =>
{
    options.Enabled = true;
    options.PingTime = TimeSpan.FromMinutes(1);
});

builder.Services.Configure<BlobOptions>(options =>
{
    options.DefaultProvider = ""azure"";
});

// appsettings.json
{
  ""EPiServer"": {
    ""Cms"": {
      ""Content"": {
        ""RequireEditAccessToChangedByProperty"": true
      },
      ""Scheduler"": {
        ""Enabled"": true,
        ""PingTime"": ""00:01:00""
      }
    }
  }
}

// Environment-specific: appsettings.Production.json
{
  ""EPiServer"": {
    ""Cms"": {
      ""Scheduler"": {
        ""Enabled"": true
      }
    }
  }
}",
                            IsInteractive = false
                        }
                    }
                },
                new Lesson
                {
                    Id = "sja-deployment",
                    ModuleId = "scheduled-jobs-advanced",
                    Title = "Deployment to DXP",
                    Summary = "Deploy your CMS 12 site to Optimizely DXP Cloud.",
                    Order = 3,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Understand DXP Cloud deployment",
                        "Configure for cloud environments",
                        "Use deployment APIs and CI/CD"
                    },
                    Content = @"
<h2>Deploying to DXP Cloud</h2>
<p>Optimizely DXP (Digital Experience Platform) is the managed cloud hosting platform for CMS 12.</p>

<h3>DXP Features</h3>
<ul>
    <li><strong>Managed hosting</strong> - Azure-based infrastructure</li>
    <li><strong>Auto-scaling</strong> - Handle traffic spikes</li>
    <li><strong>Multiple environments</strong> - Integration, Preproduction, Production</li>
    <li><strong>Deployment API</strong> - CI/CD integration</li>
    <li><strong>CDN</strong> - Built-in content delivery</li>
</ul>

<h3>Cloud-Specific Configuration</h3>
<ul>
    <li>Use Azure Blob Storage for media</li>
    <li>Configure distributed cache</li>
    <li>Set up proper connection strings</li>
    <li>Enable Application Insights</li>
</ul>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "dxp-config",
                            Title = "DXP Configuration",
                            Description = "Configure for DXP Cloud deployment",
                            Type = ExampleType.Configuration,
                            ExampleContent = @"// appsettings.Production.json for DXP
{
  ""ConnectionStrings"": {
    ""EPiServerDB"": ""[Set by DXP]""
  },
  ""EPiServer"": {
    ""Cms"": {
      ""MappedRoles"": {
        ""CmsAdmins"": [""WebAdmins""],
        ""CmsEditors"": [""WebEditors""]
      }
    },
    ""Find"": {
      ""ServiceUrl"": ""[Set by DXP]"",
      ""DefaultIndex"": ""[Set by DXP]""
    }
  },
  ""ApplicationInsights"": {
    ""ConnectionString"": ""[Set by DXP]""
  }
}

// Program.cs - DXP-specific services
if (builder.Environment.IsProduction())
{
    // Use Azure Blob Storage
    builder.Services.AddAzureBlobProvider();

    // Use Azure Service Bus for events
    builder.Services.AddAzureEventProvider();
}",
                            IsInteractive = false
                        }
                    }
                },
                new Lesson
                {
                    Id = "sja-best-practices",
                    ModuleId = "scheduled-jobs-advanced",
                    Title = "Best Practices & Tips",
                    Summary = "Learn best practices for CMS 12 development.",
                    Order = 4,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Follow CMS development best practices",
                        "Avoid common pitfalls",
                        "Write maintainable code"
                    },
                    Content = @"
<h2>Best Practices</h2>

<h3>Content Types</h3>
<ul>
    <li>Always assign a GUID to content types for safe refactoring</li>
    <li>Use <code>[CultureSpecific]</code> only when needed</li>
    <li>Keep content types focused - avoid ""god objects""</li>
    <li>Use blocks for reusable components</li>
</ul>

<h3>Performance</h3>
<ul>
    <li>Cache expensive operations</li>
    <li>Avoid loading content in loops (N+1 problem)</li>
    <li>Use <code>IContentLoader</code> for read operations</li>
    <li>Minimize calls to external services</li>
</ul>

<h3>Security</h3>
<ul>
    <li>Never bypass access checks without good reason</li>
    <li>Validate all user input</li>
    <li>Use HTTPS everywhere</li>
    <li>Follow principle of least privilege</li>
</ul>

<h3>Code Organization</h3>
<ul>
    <li>Keep controllers thin</li>
    <li>Use dependency injection</li>
    <li>Write unit tests for business logic</li>
    <li>Document complex content models</li>
</ul>
",
                    Examples = new List<LessonExample>()
                }
            }
        };
    }

    #endregion
}
