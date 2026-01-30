using OptimizelyLearningCentre.Client.Models.Learning;
using OptimizelyLearningCentre.Client.Services;

namespace OptimizelyLearningCentre.Client.Courses.SaaS;

/// <summary>
/// Content provider for the Optimizely CMS (SaaS) course
/// </summary>
public class SaaSContentProvider : ILearningContentProvider
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
            BuildContentModelingModule(),
            BuildVisualBuilderEssentialsModule(),
            BuildVisualBuilderAdvancedModule(),
            BuildRestApiModule(),
            BuildGraphIntegrationModule(),
            BuildAccessRightsModule(),
            BuildAdvancedTopicsModule()
        };
    }

    #region Module 1: Getting Started with CMS (SaaS)

    private LearningModule BuildGettingStartedModule()
    {
        return new LearningModule
        {
            Id = "getting-started",
            Title = "Getting Started with CMS (SaaS)",
            Description = "Learn the fundamentals of Optimizely CMS (SaaS), the powerful headless content management system.",
            Icon = "academic-cap",
            Order = 1,
            Difficulty = ModuleDifficulty.Beginner,
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "gs-what-is-saas",
                    ModuleId = "getting-started",
                    Title = "What is Optimizely CMS (SaaS)?",
                    Summary = "Discover Optimizely CMS (SaaS) and how it enables headless content management.",
                    Order = 1,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Understand what Optimizely CMS (SaaS) is and its purpose",
                        "Learn the benefits of a headless CMS architecture",
                        "Understand the difference between SaaS and PaaS offerings"
                    },
                    Content = @"
<h2>Introduction to Optimizely CMS (SaaS)</h2>
<p>Optimizely CMS (SaaS) is a <strong>versatile headless CMS</strong> that lets you manage and distribute content across multiple platforms. By separating content management from the presentation layer, you can deliver rich digital experiences on any device or platform.</p>

<h3>What Makes It Headless?</h3>
<p>In a headless architecture, the CMS (the ""body"") is separated from the frontend presentation layer (the ""head""). This separation provides significant benefits:</p>
<ul>
    <li><strong>Multi-channel delivery</strong> - Content can be delivered to websites, mobile apps, IoT devices, and more</li>
    <li><strong>Technology flexibility</strong> - Frontend developers can use any framework (React, Vue, Next.js, etc.)</li>
    <li><strong>API-first approach</strong> - Content is delivered via APIs (REST and GraphQL)</li>
</ul>

<h3>SaaS vs PaaS</h3>
<p>Optimizely offers two deployment models:</p>
<table class=""min-w-full divide-y divide-gray-200 dark:divide-gray-700 my-4"">
    <thead>
        <tr>
            <th class=""px-4 py-2 text-left"">Feature</th>
            <th class=""px-4 py-2 text-left"">SaaS</th>
            <th class=""px-4 py-2 text-left"">PaaS (CMS 12)</th>
        </tr>
    </thead>
    <tbody>
        <tr><td class=""px-4 py-2"">Hosting</td><td class=""px-4 py-2"">Fully managed by Optimizely</td><td class=""px-4 py-2"">Customer managed</td></tr>
        <tr><td class=""px-4 py-2"">Updates</td><td class=""px-4 py-2"">Automatic, versionless</td><td class=""px-4 py-2"">Manual upgrades</td></tr>
        <tr><td class=""px-4 py-2"">Architecture</td><td class=""px-4 py-2"">Headless-first</td><td class=""px-4 py-2"">Traditional or headless</td></tr>
        <tr><td class=""px-4 py-2"">Customization</td><td class=""px-4 py-2"">Via APIs and configuration</td><td class=""px-4 py-2"">Full code access</td></tr>
    </tbody>
</table>

<h3>Key Benefits</h3>
<ul>
    <li><strong>Scalability</strong> - Add websites and applications without affecting existing sites</li>
    <li><strong>Security</strong> - Content separated from presentation layer reduces attack surface</li>
    <li><strong>Future-proofing</strong> - Flexible architecture adapts to technological changes</li>
    <li><strong>Performance</strong> - Optimized content delivery through Optimizely Graph</li>
    <li><strong>Faster time-to-market</strong> - Focus on content and frontend, not infrastructure</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "gs-architecture",
                    ModuleId = "getting-started",
                    Title = "Architecture Overview",
                    Summary = "Understand the key components that make up the CMS (SaaS) architecture.",
                    Order = 2,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Identify the major components of CMS (SaaS)",
                        "Understand how components interact",
                        "Learn the role of Optimizely Graph in content delivery"
                    },
                    Content = @"
<h2>CMS (SaaS) Architecture</h2>
<p>The CMS (SaaS) architecture consists of several interconnected components that work together to provide a complete content management and delivery solution.</p>

<h3>Core Components</h3>

<h4>1. CMS Platform</h4>
<p>The foundation that drives the entire solution. It handles content storage, versioning, workflow management, and provides the APIs for content operations.</p>

<h4>2. CMS UI</h4>
<p>The user interface that provides the editing experience for content editors and administrators. This is where day-to-day content management happens.</p>

<h4>3. Visual Builder</h4>
<p>A powerful WYSIWYG editor that enables content managers to create and design page layouts using drag-and-drop functionality. Visual Builder introduces new concepts:</p>
<ul>
    <li><strong>Experiences</strong> - The main routable entry point for pages</li>
    <li><strong>Sections</strong> - Vertical content areas within experiences</li>
    <li><strong>Elements</strong> - The smallest building blocks containing actual content</li>
</ul>

<h4>4. Opti ID</h4>
<p>Optimizely's identity management system that handles authentication and user management across all Optimizely products.</p>

<h4>5. REST API</h4>
<p>Enables programmatic management of CMS resources including content types, content items, and configuration. Used primarily for:</p>
<ul>
    <li>Content type definitions</li>
    <li>Content CRUD operations</li>
    <li>System configuration</li>
</ul>

<h4>6. Optimizely Graph</h4>
<p>The content delivery layer that provides GraphQL-based content retrieval. Graph is optimized for high-performance, read-heavy operations and is the recommended way to fetch content for your frontend applications.</p>

<h3>Data Flow</h3>
<ol>
    <li>Content is created/edited in the CMS UI or via REST API</li>
    <li>Changes are indexed in Optimizely Graph</li>
    <li>Frontend applications query Graph for content delivery</li>
    <li>Content is rendered on websites, apps, or other channels</li>
</ol>
"
                },
                new Lesson
                {
                    Id = "gs-key-tools",
                    ModuleId = "getting-started",
                    Title = "Key Tools Overview",
                    Summary = "Learn about the three primary tools: Visual Builder, REST API, and Optimizely Graph.",
                    Order = 3,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Understand when to use Visual Builder",
                        "Know the capabilities of the REST API",
                        "Learn how Optimizely Graph delivers content"
                    },
                    Content = @"
<h2>The Three Primary Tools</h2>
<p>CMS (SaaS) provides three primary tools for different aspects of content management and delivery.</p>

<h3>Visual Builder</h3>
<p>Visual Builder provides an <strong>intuitive, drag-and-drop interface</strong> where you can create and organize content with real-time previews.</p>
<p><strong>Use Visual Builder when:</strong></p>
<ul>
    <li>Creating page layouts and designs</li>
    <li>Editing content with immediate visual feedback</li>
    <li>Working with reusable sections and blueprints</li>
    <li>Non-technical users need to manage content</li>
</ul>

<h3>REST API</h3>
<p>The REST API lets developers <strong>programmatically configure</strong> CMS instances, set up content types, and manage resources.</p>
<p><strong>Use the REST API when:</strong></p>
<ul>
    <li>Setting up content type definitions</li>
    <li>Automating content operations</li>
    <li>Integrating with external systems</li>
    <li>Building custom admin tools</li>
</ul>

<h3>Optimizely Graph</h3>
<p>Optimizely Graph facilitates <strong>efficient content retrieval</strong> across platforms using GraphQL, ensuring consistent, structured digital experiences.</p>
<p><strong>Use Optimizely Graph when:</strong></p>
<ul>
    <li>Fetching content for frontend rendering</li>
    <li>Building high-performance applications</li>
    <li>Querying content with complex filters</li>
    <li>Delivering content to multiple channels</li>
</ul>

<h3>Tool Comparison</h3>
<table class=""min-w-full divide-y divide-gray-200 dark:divide-gray-700 my-4"">
    <thead>
        <tr>
            <th class=""px-4 py-2 text-left"">Purpose</th>
            <th class=""px-4 py-2 text-left"">Tool</th>
        </tr>
    </thead>
    <tbody>
        <tr><td class=""px-4 py-2"">Content creation & editing</td><td class=""px-4 py-2"">Visual Builder</td></tr>
        <tr><td class=""px-4 py-2"">Content type management</td><td class=""px-4 py-2"">REST API</td></tr>
        <tr><td class=""px-4 py-2"">Content delivery</td><td class=""px-4 py-2"">Optimizely Graph</td></tr>
        <tr><td class=""px-4 py-2"">Automation & integration</td><td class=""px-4 py-2"">REST API</td></tr>
    </tbody>
</table>
"
                },
                new Lesson
                {
                    Id = "gs-user-roles",
                    ModuleId = "getting-started",
                    Title = "User Roles & Workflows",
                    Summary = "Understand the different user roles and their responsibilities in CMS (SaaS).",
                    Order = 4,
                    EstimatedMinutes = 7,
                    LearningObjectives = new List<string>
                    {
                        "Identify the three main user roles",
                        "Understand each role's responsibilities",
                        "Learn the typical development workflow"
                    },
                    Content = @"
<h2>User Roles in CMS (SaaS)</h2>
<p>CMS (SaaS) supports three primary user roles, each with distinct responsibilities and workflows.</p>

<h3>Content Managers</h3>
<p>Content managers are the primary users who create and maintain content. Their responsibilities include:</p>
<ul>
    <li>Creating and editing content using Visual Builder</li>
    <li>Managing digital assets (images, documents, videos)</li>
    <li>Publishing content to live sites</li>
    <li>Creating and using blueprints for consistent layouts</li>
    <li>Collaborating with team members on content</li>
</ul>

<h3>Content Administrators</h3>
<p>Administrators handle system configuration and setup. Their responsibilities include:</p>
<ul>
    <li>Configuring system settings</li>
    <li>Managing languages and localization</li>
    <li>Setting up access rights and permissions</li>
    <li>Creating and managing content types (in collaboration with developers)</li>
    <li>Managing approval sequences and workflows</li>
    <li>Importing and exporting content</li>
</ul>

<h3>Developers</h3>
<p>Developers implement the technical foundation and content delivery. Their workflow follows three stages:</p>
<ol>
    <li><strong>Plan</strong> - Define the site architecture and content model</li>
    <li><strong>Build</strong> - Create content types and configure Visual Builder</li>
    <li><strong>Render</strong> - Build frontend applications that consume content via Graph</li>
</ol>

<h3>Collaboration Model</h3>
<p>These roles work together in a continuous cycle:</p>
<ol>
    <li>Developers define content types and structure</li>
    <li>Administrators configure permissions and workflows</li>
    <li>Content managers create content using the defined structure</li>
    <li>Feedback loops inform improvements to the content model</li>
</ol>
"
                }
            }
        };
    }

    #endregion

    #region Module 2: Content Modeling Fundamentals

    private LearningModule BuildContentModelingModule()
    {
        return new LearningModule
        {
            Id = "content-modeling",
            Title = "Content Modeling Fundamentals",
            Description = "Learn how to design and implement content types that form the foundation of your CMS.",
            Icon = "cube-transparent",
            Order = 2,
            Difficulty = ModuleDifficulty.Beginner,
            Prerequisites = new[] { "getting-started" },
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "cm-content-types",
                    ModuleId = "content-modeling",
                    Title = "Understanding Content Types",
                    Summary = "Learn what content types are and how they define your content structure.",
                    Order = 1,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Understand what a content type is",
                        "Learn how content types define content structure",
                        "Know the relationship between types and instances"
                    },
                    Content = @"
<h2>What Are Content Types?</h2>
<p>A content type in CMS (SaaS) defines the <strong>characteristics and data model schema</strong> of a content item. It serves as the foundation for creating pages, blocks, or experiences by specifying properties that hold information.</p>

<h3>Content Types as Blueprints</h3>
<p>Think of a content type as a blueprint or template. Just as an architectural blueprint defines the structure of a building, a content type defines:</p>
<ul>
    <li><strong>Properties</strong> - The fields that hold data (text, images, links, etc.)</li>
    <li><strong>Validation rules</strong> - Requirements for each property</li>
    <li><strong>Display settings</strong> - How content appears in the editor</li>
    <li><strong>Behaviors</strong> - Whether it's a page, block, or media item</li>
</ul>

<h3>Content Instances</h3>
<p>When you create content based on a content type, you create an <strong>instance</strong> of that type. For example:</p>
<ul>
    <li>""Article Page"" content type defines the structure</li>
    <li>""10 Tips for Better SEO"" is an instance with actual content</li>
</ul>

<h3>Standard Metadata</h3>
<p>Every content type includes standard metadata fields:</p>
<ul>
    <li><code>key</code> - Unique identifier</li>
    <li><code>displayName</code> - Human-readable name</li>
    <li><code>description</code> - Purpose description</li>
    <li><code>baseType</code> - The fundamental type it extends</li>
    <li><code>created</code> / <code>lastModified</code> - Timestamps</li>
</ul>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "cm-simple-content-type",
                            Title = "Simple Content Type Definition",
                            Description = "A basic page content type with common properties.",
                            Type = ExampleType.Configuration,
                            ExampleContent = @"{
  ""key"": ""articlePage"",
  ""baseType"": ""_page"",
  ""displayName"": ""Article Page"",
  ""description"": ""A page for blog articles and news"",
  ""properties"": {
    ""title"": {
      ""type"": ""string"",
      ""displayName"": ""Title"",
      ""required"": true
    },
    ""summary"": {
      ""type"": ""string"",
      ""displayName"": ""Summary"",
      ""format"": ""textarea""
    },
    ""publishDate"": {
      ""type"": ""dateTime"",
      ""displayName"": ""Publish Date""
    }
  }
}",
                            SampleResponse = @"Content type 'articlePage' created successfully.

You can now create instances of this content type in the CMS UI
or via the REST API.",
                            Hints = new List<string>
                            {
                                "The 'key' must be unique across all content types",
                                "Base type determines fundamental behavior (page, block, media, etc.)"
                            }
                        }
                    }
                },
                new Lesson
                {
                    Id = "cm-base-types",
                    ModuleId = "content-modeling",
                    Title = "Base Types Explained",
                    Summary = "Understand the different base types and when to use each one.",
                    Order = 2,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Know all available base types",
                        "Understand the purpose of each base type",
                        "Choose the right base type for your needs"
                    },
                    Content = @"
<h2>Base Types in CMS (SaaS)</h2>
<p>Every content type inherits from a <strong>base type</strong> that determines its fundamental behavior. Each base type has a corresponding Optimizely Graph schema for querying.</p>

<div class=""bg-yellow-50 dark:bg-yellow-900/20 border-l-4 border-yellow-400 p-4 my-4"">
    <p class=""text-yellow-800 dark:text-yellow-200""><strong>Important:</strong> You cannot change the base type after you create the content type.</p>
</div>

<h3>Available Base Types</h3>

<h4>_page</h4>
<p>Displayable content with a unique URL representing a webpage. Use for:</p>
<ul>
    <li>Standard website pages</li>
    <li>Landing pages</li>
    <li>Article/blog pages</li>
</ul>

<h4>_component (block)</h4>
<p>Reusable, locale-aware components without URLs. Use for:</p>
<ul>
    <li>Reusable content blocks</li>
    <li>Shared components across pages</li>
    <li>Template sections</li>
</ul>

<h4>_experience</h4>
<p>Extension of page type enhanced for Visual Builder. Use for:</p>
<ul>
    <li>Visual Builder-enabled pages</li>
    <li>Dynamic layouts with sections</li>
</ul>

<h4>_section</h4>
<p>Organizational containers within experiences. System-provided for Visual Builder.</p>

<h4>_media, _image, _video</h4>
<p>Binary data storage types. Use for:</p>
<ul>
    <li>Images and photos (_image)</li>
    <li>Video content (_video)</li>
    <li>Documents and other files (_media)</li>
</ul>

<h4>_folder</h4>
<p>Content organization without versioning. Use for:</p>
<ul>
    <li>Organizing content in hierarchies</li>
    <li>Creating content containers</li>
</ul>

<h3>Graph Schema Mapping</h3>
<p>Each base type maps to specific Graph types for querying. For example, <code>_page</code> types can be queried using the page-related Graph queries.</p>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "cm-base-type-comparison",
                            Title = "Base Type Selection Guide",
                            Description = "Choosing the right base type for different scenarios.",
                            Type = ExampleType.Code,
                            ExampleContent = @"// Page with URL - use _page
{
  ""key"": ""productPage"",
  ""baseType"": ""_page"",
  ...
}

// Reusable component - use _component
{
  ""key"": ""heroBlock"",
  ""baseType"": ""_component"",
  ...
}

// Visual Builder page - use _experience
{
  ""key"": ""landingExperience"",
  ""baseType"": ""_experience"",
  ...
}

// Image with metadata - use _image
{
  ""key"": ""productImage"",
  ""baseType"": ""_image"",
  ...
}",
                            SampleResponse = @"Base Type Selection Criteria:

1. Does it need a URL? -> _page or _experience
2. Is it reusable content? -> _component
3. Does it use Visual Builder? -> _experience
4. Is it binary data? -> _media, _image, or _video
5. Just organizing content? -> _folder",
                            Hints = new List<string>
                            {
                                "Choose _experience over _page when using Visual Builder",
                                "Components registered in Graph both standalone and with 'Property' suffix"
                            }
                        }
                    }
                },
                new Lesson
                {
                    Id = "cm-property-types",
                    ModuleId = "content-modeling",
                    Title = "Property Types & Validation",
                    Summary = "Learn about available property types and how to validate content.",
                    Order = 3,
                    EstimatedMinutes = 15,
                    LearningObjectives = new List<string>
                    {
                        "Know all available property types",
                        "Understand validation options",
                        "Configure property settings correctly"
                    },
                    Content = @"
<h2>Property Types</h2>
<p>Properties define the fields within your content types. CMS (SaaS) supports a variety of data types to handle different content needs.</p>

<h3>Basic Data Types</h3>
<ul>
    <li><code>string</code> - Text content (single line or multiline)</li>
    <li><code>boolean</code> - True/false values</li>
    <li><code>integer</code> - Whole numbers</li>
    <li><code>float</code> - Decimal numbers</li>
    <li><code>dateTime</code> - Date and time values</li>
</ul>

<h3>Content-Specific Types</h3>
<ul>
    <li><code>richText</code> - Formatted HTML content</li>
    <li><code>contentReference</code> - Reference to other content items</li>
    <li><code>link</code> - URLs with optional text</li>
    <li><code>component</code> - Nested content type</li>
    <li><code>binary</code> - File data</li>
</ul>

<h3>Collection Types</h3>
<ul>
    <li><code>array</code> - Lists of any type (strings, references, components)</li>
</ul>

<h3>Validation Options</h3>
<p>Properties support various validations:</p>
<ul>
    <li><code>required</code> - Property must have a value</li>
    <li><code>maxLength</code> - Maximum length for strings/arrays</li>
    <li><code>pattern</code> - Regex pattern matching</li>
    <li><code>minimum</code> / <code>maximum</code> - Numeric bounds</li>
    <li><code>enum</code> - Predefined allowed values</li>
    <li><code>allowedTypes</code> - Restrict content references</li>
</ul>

<h3>Property Attributes</h3>
<p>Each property can have additional attributes:</p>
<ul>
    <li><code>displayName</code> - Label shown in the UI</li>
    <li><code>description</code> - Help text for editors</li>
    <li><code>format</code> - Display format (textarea, etc.)</li>
    <li><code>localized</code> - Enable per-language values</li>
    <li><code>group</code> - Organize properties in the UI</li>
</ul>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "cm-property-examples",
                            Title = "Property Configuration Examples",
                            Description = "Various property type configurations with validation.",
                            Type = ExampleType.Configuration,
                            ExampleContent = @"{
  ""properties"": {
    ""title"": {
      ""type"": ""string"",
      ""displayName"": ""Page Title"",
      ""required"": true,
      ""maxLength"": 100,
      ""localized"": true
    },
    ""body"": {
      ""type"": ""richText"",
      ""displayName"": ""Body Content"",
      ""indexingType"": ""searchable""
    },
    ""category"": {
      ""type"": ""string"",
      ""displayName"": ""Category"",
      ""enum"": [""News"", ""Blog"", ""Product"", ""Support""]
    },
    ""rating"": {
      ""type"": ""integer"",
      ""displayName"": ""Rating"",
      ""minimum"": 1,
      ""maximum"": 5
    },
    ""relatedPages"": {
      ""type"": ""array"",
      ""items"": {
        ""type"": ""contentReference"",
        ""allowedTypes"": [""articlePage""]
      },
      ""maxLength"": 5
    },
    ""featuredImage"": {
      ""type"": ""contentReference"",
      ""allowedTypes"": [""_image""],
      ""displayName"": ""Featured Image""
    }
  }
}",
                            SampleResponse = @"Property Configuration Summary:
- title: Required localized string, max 100 chars
- body: Searchable rich text content
- category: Dropdown with 4 options
- rating: Integer between 1-5
- relatedPages: Up to 5 article references
- featuredImage: Single image reference",
                            Hints = new List<string>
                            {
                                "Use 'localized: true' for content that needs translation",
                                "indexingType affects how content appears in Graph queries"
                            }
                        }
                    }
                },
                new Lesson
                {
                    Id = "cm-best-practices",
                    ModuleId = "content-modeling",
                    Title = "Content Modeling Best Practices",
                    Summary = "Learn strategies for designing effective and maintainable content models.",
                    Order = 4,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Design modular, reusable content types",
                        "Plan for scalability and maintenance",
                        "Avoid common content modeling mistakes"
                    },
                    Content = @"
<h2>Content Modeling Best Practices</h2>
<p>A well-designed content model is crucial for a successful CMS implementation. Follow these best practices to create maintainable, scalable content structures.</p>

<h3>1. Start with User Needs</h3>
<ul>
    <li>Identify what content editors need to create</li>
    <li>Map out the content workflows</li>
    <li>Consider the frontend requirements</li>
</ul>

<h3>2. Design for Reusability</h3>
<ul>
    <li>Create components that can be used across multiple pages</li>
    <li>Use base types appropriately</li>
    <li>Avoid duplicating properties across content types</li>
</ul>

<h3>3. Keep It Simple</h3>
<ul>
    <li>Don't over-engineer the model</li>
    <li>Start with essential properties, add more later</li>
    <li>Use clear, descriptive names</li>
</ul>

<h3>4. Plan for Indexing</h3>
<p>Configure indexing appropriately for Graph:</p>
<ul>
    <li><code>default</code> - Indexed but not filterable/searchable</li>
    <li><code>queryable</code> - Allows filtering and sorting</li>
    <li><code>searchable</code> - Full-text search enabled</li>
    <li><code>disabled</code> - Excluded from Graph</li>
</ul>

<h3>5. Reserved Names</h3>
<p>Avoid these reserved content type names:</p>
<ul>
    <li>String, Int, Float, DateTime, JSON, Boolean</li>
    <li>RichText, SearchableRichText, Link</li>
    <li>ContentReference, ContentUrl</li>
</ul>

<h3>6. Documentation</h3>
<ul>
    <li>Use description fields to guide editors</li>
    <li>Document the purpose of each content type</li>
    <li>Maintain a content model diagram</li>
</ul>
"
                }
            }
        };
    }

    #endregion

    #region Module 3: Visual Builder Essentials

    private LearningModule BuildVisualBuilderEssentialsModule()
    {
        return new LearningModule
        {
            Id = "visual-builder-essentials",
            Title = "Visual Builder Essentials",
            Description = "Master the fundamentals of Visual Builder for creating dynamic page layouts.",
            Icon = "paint-brush",
            Order = 3,
            Difficulty = ModuleDifficulty.Beginner,
            Prerequisites = new[] { "content-modeling" },
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "vb-introduction",
                    ModuleId = "visual-builder-essentials",
                    Title = "Introduction to Visual Builder",
                    Summary = "Discover Visual Builder and its role in content creation.",
                    Order = 1,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Understand what Visual Builder is",
                        "Know the key concepts and terminology",
                        "Navigate the Visual Builder interface"
                    },
                    Content = @"
<h2>What is Visual Builder?</h2>
<p>Visual Builder is the editor interface in CMS (SaaS) that makes <strong>content creation and layout building intuitive and accessible</strong> to non-technical users. It provides a drag-and-drop experience with real-time previews.</p>

<h3>Why Visual Builder?</h3>
<p>Visual Builder bridges the gap between developers and content creators:</p>
<ul>
    <li>Developers define the building blocks (content types, styles)</li>
    <li>Content managers assemble layouts without coding</li>
    <li>Changes are immediately visible in preview</li>
</ul>

<h3>Key Concepts</h3>

<h4>Building Blocks</h4>
<p>Visual Builder uses three hierarchical building blocks:</p>
<ol>
    <li><strong>Experiences</strong> - The page-level container</li>
    <li><strong>Sections</strong> - Major content areas within experiences</li>
    <li><strong>Elements</strong> - Individual content components</li>
</ol>

<h4>Layouts</h4>
<p>Two layout types organize content:</p>
<ul>
    <li><strong>Outline</strong> - A flat list of sections (used by experiences)</li>
    <li><strong>Grid</strong> - Rows, columns, and elements (used by sections)</li>
</ul>

<h3>Interface Overview</h3>
<ul>
    <li><strong>Outline Panel</strong> - View and reorder sections</li>
    <li><strong>Properties Panel</strong> - Edit selected item properties</li>
    <li><strong>Preview Area</strong> - See changes in real-time</li>
    <li><strong>Toolbar</strong> - Access tools and settings</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "vb-experiences",
                    ModuleId = "visual-builder-essentials",
                    Title = "Experiences: The Routable Entry Point",
                    Summary = "Learn how experiences serve as the foundation for Visual Builder pages.",
                    Order = 2,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Understand what experiences are",
                        "Know how experiences relate to pages",
                        "Create and configure experiences"
                    },
                    Content = @"
<h2>Understanding Experiences</h2>
<p>An experience is the <strong>main routable entry point</strong> of Visual Builder. It extends the traditional page concept with access to the layout system through compositions.</p>

<h3>Experience vs Page</h3>
<table class=""min-w-full divide-y divide-gray-200 dark:divide-gray-700 my-4"">
    <thead>
        <tr>
            <th class=""px-4 py-2 text-left"">Feature</th>
            <th class=""px-4 py-2 text-left"">Traditional Page</th>
            <th class=""px-4 py-2 text-left"">Experience</th>
        </tr>
    </thead>
    <tbody>
        <tr><td class=""px-4 py-2"">URL Routing</td><td class=""px-4 py-2"">Yes</td><td class=""px-4 py-2"">Yes</td></tr>
        <tr><td class=""px-4 py-2"">Visual Builder</td><td class=""px-4 py-2"">No</td><td class=""px-4 py-2"">Yes</td></tr>
        <tr><td class=""px-4 py-2"">Layout System</td><td class=""px-4 py-2"">Fixed</td><td class=""px-4 py-2"">Dynamic (Outline)</td></tr>
        <tr><td class=""px-4 py-2"">Sections Support</td><td class=""px-4 py-2"">No</td><td class=""px-4 py-2"">Yes</td></tr>
    </tbody>
</table>

<h3>Outline Layout</h3>
<p>Experiences use the <strong>outline layout type</strong>, which is a flat, ordered list of sections. The outline provides:</p>
<ul>
    <li>Easy section reordering via drag-and-drop</li>
    <li>Clear visual hierarchy</li>
    <li>Simple section management</li>
</ul>

<h3>Experience Properties</h3>
<p>Experiences can have their own properties in addition to sections:</p>
<ul>
    <li>SEO metadata (title, description)</li>
    <li>Page-level settings</li>
    <li>Custom fields defined in the content type</li>
</ul>

<h3>Saving as Blueprints</h3>
<p>Entire experiences can be saved as <strong>blueprints</strong> - reusable templates that content managers can use to quickly create new pages with predefined layouts.</p>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "vb-experience-type",
                            Title = "Experience Content Type Definition",
                            Description = "Define an experience type for Visual Builder.",
                            Type = ExampleType.Configuration,
                            ExampleContent = @"{
  ""key"": ""landingPageExperience"",
  ""baseType"": ""_experience"",
  ""displayName"": ""Landing Page"",
  ""description"": ""A flexible landing page with Visual Builder support"",
  ""properties"": {
    ""metaTitle"": {
      ""type"": ""string"",
      ""displayName"": ""SEO Title"",
      ""maxLength"": 60
    },
    ""metaDescription"": {
      ""type"": ""string"",
      ""displayName"": ""SEO Description"",
      ""format"": ""textarea"",
      ""maxLength"": 160
    }
  }
}",
                            SampleResponse = @"Experience type created successfully.

This experience will appear in Visual Builder with:
- Full outline layout support
- Section management capabilities
- Custom SEO properties",
                            Hints = new List<string>
                            {
                                "Use _experience base type for Visual Builder pages",
                                "Keep page-level properties minimal - use sections for content"
                            }
                        }
                    }
                },
                new Lesson
                {
                    Id = "vb-sections",
                    ModuleId = "visual-builder-essentials",
                    Title = "Sections: Grid Layouts",
                    Summary = "Master sections and their grid-based layout system.",
                    Order = 3,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Understand section structure",
                        "Work with rows and columns",
                        "Configure section layouts"
                    },
                    Content = @"
<h2>Understanding Sections</h2>
<p>Sections are <strong>vertical content chunks</strong> within an experience. They extend block functionality with access to the layout system through a grid composition.</p>

<h3>Grid Layout System</h3>
<p>Sections use the <strong>grid layout type</strong>, which provides a hierarchical structure:</p>
<ol>
    <li><strong>Rows</strong> - Horizontal containers</li>
    <li><strong>Columns</strong> - Vertical divisions within rows</li>
    <li><strong>Elements</strong> - Content within columns</li>
</ol>

<h3>Working with the Grid</h3>
<p>The grid system allows flexible layouts:</p>
<ul>
    <li>Add multiple rows per section</li>
    <li>Configure column widths (equal, 2/3-1/3, etc.)</li>
    <li>Place multiple elements per column</li>
    <li>Apply styles at any level</li>
</ul>

<h3>Section Types</h3>
<p>Developers can create custom section types with:</p>
<ul>
    <li>Pre-defined layouts</li>
    <li>Custom properties</li>
    <li>Specific styling options</li>
</ul>

<h3>Section Behaviors</h3>
<p>Sections can be configured to:</p>
<ul>
    <li>Allow specific element types only</li>
    <li>Have required elements</li>
    <li>Include section-level properties (background, spacing)</li>
</ul>

<h3>Saving as Blueprints</h3>
<p>Frequently used section configurations can be saved as blueprints for reuse across pages.</p>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "vb-section-structure",
                            Title = "Section Grid Structure",
                            Description = "Example of a section with rows, columns, and elements.",
                            Type = ExampleType.Code,
                            ExampleContent = @"// Section Structure in Visual Builder

Section: ""Hero Section""
├── Row 1 (Full Width)
│   └── Column 1 (100%)
│       └── Element: Hero Image
│
├── Row 2 (Two Column)
│   ├── Column 1 (60%)
│   │   └── Element: Heading
│   │   └── Element: Rich Text
│   │
│   └── Column 2 (40%)
│       └── Element: Call-to-Action Button
│
└── Row 3 (Three Column)
    ├── Column 1 (33%)
    │   └── Element: Feature Card
    ├── Column 2 (33%)
    │   └── Element: Feature Card
    └── Column 3 (33%)
        └── Element: Feature Card",
                            SampleResponse = @"This section layout demonstrates:
- Multiple rows with different configurations
- Variable column widths
- Nested elements within columns
- Common landing page pattern",
                            Hints = new List<string>
                            {
                                "Column widths are typically percentages that add up to 100%",
                                "Elements are the leaf nodes - they cannot contain other elements"
                            }
                        }
                    }
                },
                new Lesson
                {
                    Id = "vb-elements",
                    ModuleId = "visual-builder-essentials",
                    Title = "Elements: Building Blocks",
                    Summary = "Learn about elements, the smallest content units in Visual Builder.",
                    Order = 4,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Understand what elements are",
                        "Know element limitations",
                        "Create element content types"
                    },
                    Content = @"
<h2>Understanding Elements</h2>
<p>Elements are the <strong>smallest building blocks</strong> in Visual Builder. They contain the actual content data and are terminal nodes - they cannot have children.</p>

<h3>Element Characteristics</h3>
<ul>
    <li>Extension of block content type</li>
    <li>Restricted property types (no nested compositions)</li>
    <li>Leaf nodes in the experience tree</li>
    <li>Direct content containers</li>
</ul>

<h3>Common Element Types</h3>
<ul>
    <li><strong>Heading</strong> - Title text with level options</li>
    <li><strong>Rich Text</strong> - Formatted content with WYSIWYG editor</li>
    <li><strong>Image</strong> - Single image with alt text</li>
    <li><strong>Button</strong> - Call-to-action with link</li>
    <li><strong>Video</strong> - Embedded video content</li>
    <li><strong>Testimonial</strong> - Quote with attribution</li>
</ul>

<h3>Creating Elements</h3>
<p>To create an element type:</p>
<ol>
    <li>Go to Settings > Content Types</li>
    <li>Create a new Block Type</li>
    <li>Add your properties</li>
    <li>Enable ""Available for composition in Visual Builder""</li>
    <li>Enable ""Display as Element""</li>
</ol>

<h3>Element vs Section-Enabled Block</h3>
<p>Blocks can be configured as:</p>
<ul>
    <li><strong>Elements</strong> - No grid layout, simple content</li>
    <li><strong>Sections</strong> - Have grid layout for complex compositions</li>
</ul>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "vb-element-definition",
                            Title = "Element Content Type",
                            Description = "Define a simple element for Visual Builder.",
                            Type = ExampleType.Configuration,
                            ExampleContent = @"{
  ""key"": ""testimonialElement"",
  ""baseType"": ""_component"",
  ""displayName"": ""Testimonial"",
  ""description"": ""Customer testimonial with quote and attribution"",
  ""compositionBehaviors"": [""element""],
  ""properties"": {
    ""quote"": {
      ""type"": ""string"",
      ""displayName"": ""Quote"",
      ""format"": ""textarea"",
      ""required"": true
    },
    ""authorName"": {
      ""type"": ""string"",
      ""displayName"": ""Author Name"",
      ""required"": true
    },
    ""authorTitle"": {
      ""type"": ""string"",
      ""displayName"": ""Author Title""
    },
    ""authorImage"": {
      ""type"": ""contentReference"",
      ""displayName"": ""Author Photo"",
      ""allowedTypes"": [""_image""]
    }
  }
}",
                            SampleResponse = @"Element type created successfully.

This testimonial element will be available in Visual Builder
to drag into any section column.",
                            Hints = new List<string>
                            {
                                "compositionBehaviors: ['element'] marks it as an element",
                                "Keep elements focused on a single purpose"
                            }
                        }
                    }
                }
            }
        };
    }

    #endregion

    #region Module 4: Visual Builder Advanced

    private LearningModule BuildVisualBuilderAdvancedModule()
    {
        return new LearningModule
        {
            Id = "visual-builder-advanced",
            Title = "Working with Visual Builder",
            Description = "Advanced Visual Builder techniques including templates, styles, and blueprints.",
            Icon = "adjustments-horizontal",
            Order = 4,
            Difficulty = ModuleDifficulty.Intermediate,
            Prerequisites = new[] { "visual-builder-essentials" },
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "vba-display-templates",
                    ModuleId = "visual-builder-advanced",
                    Title = "Display Templates & Styles",
                    Summary = "Configure display options and styling for Visual Builder content.",
                    Order = 1,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Understand display templates",
                        "Configure style settings",
                        "Apply styles at different levels"
                    },
                    Content = @"
<h2>Display Templates</h2>
<p>Display templates define the <strong>visual rendering options</strong> available to content managers. Developers create templates that content managers can select in Visual Builder.</p>

<h3>Template Association</h3>
<p>Templates can be associated with:</p>
<ul>
    <li><strong>Base types</strong> - experience, section, component</li>
    <li><strong>Content types</strong> - Specific content type keys</li>
    <li><strong>Node types</strong> - row, column</li>
</ul>

<h3>Style Settings</h3>
<p>Each template can expose settings that content managers can configure:</p>

<h4>Setting Editor Types</h4>
<ul>
    <li><strong>Select (dropdown)</strong> - Single selection from predefined options</li>
    <li><strong>Checkbox</strong> - Toggle true/false values</li>
</ul>

<h3>Applying Styles</h3>
<p>Styles can be applied at multiple levels:</p>
<ul>
    <li><strong>Experience level</strong> - Page-wide settings</li>
    <li><strong>Section level</strong> - Section background, spacing</li>
    <li><strong>Row/Column level</strong> - Layout adjustments</li>
    <li><strong>Element level</strong> - Component-specific styling</li>
</ul>

<h3>Style Inheritance</h3>
<p>Styles cascade from parent to child, allowing for:</p>
<ul>
    <li>Consistent theming across sections</li>
    <li>Override capability at lower levels</li>
    <li>Reduced configuration repetition</li>
</ul>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "vba-style-config",
                            Title = "Display Template with Styles",
                            Description = "Configure a display template with style settings.",
                            Type = ExampleType.Configuration,
                            ExampleContent = @"{
  ""key"": ""heroSectionTemplate"",
  ""displayName"": ""Hero Section"",
  ""baseType"": ""section"",
  ""contentTypes"": [""heroSection""],
  ""settings"": [
    {
      ""key"": ""backgroundColor"",
      ""displayName"": ""Background Color"",
      ""editor"": ""select"",
      ""options"": [
        { ""value"": ""white"", ""label"": ""White"" },
        { ""value"": ""gray"", ""label"": ""Light Gray"" },
        { ""value"": ""primary"", ""label"": ""Brand Primary"" },
        { ""value"": ""dark"", ""label"": ""Dark"" }
      ],
      ""default"": ""white""
    },
    {
      ""key"": ""fullWidth"",
      ""displayName"": ""Full Width"",
      ""editor"": ""checkbox"",
      ""default"": false
    },
    {
      ""key"": ""paddingSize"",
      ""displayName"": ""Vertical Padding"",
      ""editor"": ""select"",
      ""options"": [
        { ""value"": ""small"", ""label"": ""Small"" },
        { ""value"": ""medium"", ""label"": ""Medium"" },
        { ""value"": ""large"", ""label"": ""Large"" }
      ],
      ""default"": ""medium""
    }
  ]
}",
                            SampleResponse = @"Display template settings will appear in Visual Builder
when editors select this section, allowing them to:
- Choose background color from brand palette
- Toggle full-width display
- Select padding size",
                            Hints = new List<string>
                            {
                                "Keep style options aligned with your design system",
                                "Default values ensure consistent baseline appearance"
                            }
                        }
                    }
                },
                new Lesson
                {
                    Id = "vba-blueprints",
                    ModuleId = "visual-builder-advanced",
                    Title = "Creating & Using Blueprints",
                    Summary = "Learn to create and manage reusable layout templates.",
                    Order = 2,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Understand blueprint functionality",
                        "Create blueprints from existing content",
                        "Use blueprints effectively"
                    },
                    Content = @"
<h2>Understanding Blueprints</h2>
<p>Blueprints are <strong>reusable layout templates</strong> that content managers can create directly in the CMS. They enable rapid content creation with consistent layouts.</p>

<h3>How Blueprints Work</h3>
<p>When you use a blueprint to create content:</p>
<ol>
    <li>Visual Builder copies the entire layout structure</li>
    <li>A new, independent content instance is created</li>
    <li>The new content is <strong>not connected</strong> to the original blueprint</li>
    <li>Changes to the blueprint don't affect existing content</li>
</ol>

<h3>Blueprint Types</h3>
<ul>
    <li><strong>Experience blueprints</strong> - Complete page templates</li>
    <li><strong>Section blueprints</strong> - Reusable section layouts</li>
</ul>

<h3>Creating Blueprints</h3>
<p>To save content as a blueprint:</p>
<ol>
    <li>Design your experience or section layout</li>
    <li>Configure all settings and styles</li>
    <li>Add placeholder content if needed</li>
    <li>Use ""Save as Blueprint"" option</li>
    <li>Give it a descriptive name</li>
</ol>

<h3>Blueprint Best Practices</h3>
<ul>
    <li>Create blueprints for commonly used layouts</li>
    <li>Use clear naming conventions</li>
    <li>Include example content to guide editors</li>
    <li>Document when each blueprint should be used</li>
    <li>Review and update blueprints regularly</li>
</ul>

<h3>Blueprint Storage</h3>
<p>Blueprints are stored in a dedicated blueprints folder within the CMS, making them easy to find and manage.</p>
"
                },
                new Lesson
                {
                    Id = "vba-composition-queries",
                    ModuleId = "visual-builder-advanced",
                    Title = "Visual Builder Composition Queries",
                    Summary = "Query Visual Builder content using Optimizely Graph.",
                    Order = 3,
                    EstimatedMinutes = 15,
                    LearningObjectives = new List<string>
                    {
                        "Understand composition structure in Graph",
                        "Query experiences with nested content",
                        "Use explicit and recursive queries"
                    },
                    Content = @"
<h2>Querying Visual Builder Content</h2>
<p>Visual Builder content is indexed in Optimizely Graph as <strong>composition models</strong>. Understanding the structure is key to building effective queries.</p>

<h3>Composition Structure</h3>
<p>When indexed, experiences become queryable structures containing:</p>
<ul>
    <li><strong>CompositionNode</strong> - Base type for all nodes</li>
    <li><strong>CompositionStructureNode</strong> - Structural elements with children</li>
    <li><strong>CompositionComponentNode</strong> - Actual content components</li>
</ul>

<h3>Node Properties</h3>
<p>Each CompositionNode includes:</p>
<ul>
    <li><code>type</code> - Content type (e.g., HeroSection)</li>
    <li><code>nodeType</code> - Category (experience, section, row, column, component)</li>
    <li><code>displayName</code> - The node's label</li>
    <li><code>key</code> - Unique identifier</li>
</ul>

<h3>Query Approaches</h3>

<h4>Explicit Queries</h4>
<p>Define fixed nested levels when structure depth is known:</p>
<pre>experience → section → row → column → component</pre>

<h4>Recursive Queries</h4>
<p>Use GraphQL's <code>@recursive</code> directive for variable-depth structures, avoiding repetitive nesting.</p>

<h3>Accessing Component Data</h3>
<p>Use GraphQL fragments to access component-specific fields like headings, rich text, images, and custom properties.</p>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "vba-composition-query",
                            Title = "Experience Composition Query",
                            Description = "Query an experience with its nested sections and elements.",
                            Type = ExampleType.Query,
                            ExampleContent = @"{
  LandingPageExperience {
    items {
      Name
      _metadata {
        url {
          default
        }
      }
      composition {
        nodeType
        nodes {
          ... on CompositionStructureNode {
            type
            nodeType
            nodes {
              ... on CompositionStructureNode {
                type
                nodes {
                  ... on CompositionComponentNode {
                    type
                    component {
                      ... on HeadingElement {
                        text
                        level
                      }
                      ... on RichTextElement {
                        content {
                          html
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
    }
  }
}",
                            SampleResponse = @"{
  ""data"": {
    ""LandingPageExperience"": {
      ""items"": [
        {
          ""Name"": ""Homepage"",
          ""_metadata"": {
            ""url"": {
              ""default"": ""/""
            }
          },
          ""composition"": {
            ""nodeType"": ""experience"",
            ""nodes"": [
              {
                ""type"": ""HeroSection"",
                ""nodeType"": ""section"",
                ""nodes"": [...]
              }
            ]
          }
        }
      ]
    }
  }
}",
                            Hints = new List<string>
                            {
                                "Use fragments to keep queries maintainable",
                                "@recursive directive simplifies deep structures"
                            }
                        }
                    }
                },
                new Lesson
                {
                    Id = "vba-developer-config",
                    ModuleId = "visual-builder-advanced",
                    Title = "Developer Configuration",
                    Summary = "Configure Visual Builder for your frontend application.",
                    Order = 4,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Set up Visual Builder preview",
                        "Configure applications and hosts",
                        "Troubleshoot common issues"
                    },
                    Content = @"
<h2>Developer Configuration</h2>
<p>To fully enable Visual Builder capabilities, developers need to configure several settings in both the CMS and the frontend application.</p>

<h3>Preview Configuration</h3>
<p>Live preview allows content managers to see changes in real-time:</p>
<ol>
    <li>Navigate to <strong>Settings > Applications</strong></li>
    <li>Configure your frontend application URL</li>
    <li>Enable preview token settings</li>
    <li>Set up the preview endpoint in your frontend</li>
</ol>

<h3>Website Configuration</h3>
<p>In <strong>Settings > Manage Websites</strong>:</p>
<ol>
    <li>Create a new website entry</li>
    <li>Set the frontend URL</li>
    <li>Configure the start page</li>
    <li>Add host configurations</li>
</ol>

<h3>Graph Synchronization</h3>
<p>After configuration changes:</p>
<ol>
    <li>Run ""Optimizely Graph Full Synchronization""</li>
    <li>Verify content appears in Graph</li>
    <li>Test queries in GraphQL playground</li>
</ol>

<h3>Troubleshooting</h3>
<p>Common issues and solutions:</p>
<ul>
    <li><strong>404 in preview</strong> - Enable preview tokens in Applications settings</li>
    <li><strong>Content not in Graph</strong> - Run full synchronization</li>
    <li><strong>Styles not applying</strong> - Verify display template configuration</li>
    <li><strong>Elements not available</strong> - Check composition behavior settings</li>
</ul>
"
                }
            }
        };
    }

    #endregion

    #region Module 5: REST API Fundamentals

    private LearningModule BuildRestApiModule()
    {
        return new LearningModule
        {
            Id = "rest-api",
            Title = "REST API Fundamentals",
            Description = "Learn to manage content programmatically using the CMS REST API.",
            Icon = "code-bracket-square",
            Order = 5,
            Difficulty = ModuleDifficulty.Intermediate,
            Prerequisites = new[] { "content-modeling" },
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "api-introduction",
                    ModuleId = "rest-api",
                    Title = "Introduction to the REST API",
                    Summary = "Overview of the CMS REST API and its capabilities.",
                    Order = 1,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Understand REST API purpose and scope",
                        "Know when to use REST API vs Graph",
                        "Learn API conventions"
                    },
                    Content = @"
<h2>CMS REST API Overview</h2>
<p>The CMS (SaaS) REST API enables <strong>programmatic management</strong> of your CMS resources. It's designed for resource management operations, not content delivery.</p>

<h3>When to Use REST API</h3>
<ul>
    <li>Creating and managing content types</li>
    <li>CRUD operations on content items</li>
    <li>System configuration and setup</li>
    <li>Automation and integration scenarios</li>
    <li>Building custom admin tools</li>
</ul>

<h3>When to Use Optimizely Graph</h3>
<ul>
    <li>High-performance content delivery</li>
    <li>Frontend data fetching</li>
    <li>Complex content queries</li>
    <li>Read-heavy operations</li>
</ul>

<h3>API Base URL</h3>
<p>All API calls use the base URL:</p>
<pre>https://api.cms.optimizely.com</pre>

<h3>API Conventions</h3>
<ul>
    <li><strong>Content-Type</strong>: <code>application/json</code></li>
    <li><strong>PATCH requests</strong>: Use <code>application/merge-patch+json</code></li>
    <li><strong>Rate limiting</strong>: 100 requests per 10 seconds per IP</li>
</ul>

<h3>Available Resources</h3>
<ul>
    <li>Content types (definitions)</li>
    <li>Content items</li>
    <li>Display templates</li>
    <li>Property formats and groups</li>
    <li>OAuth tokens</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "api-authentication",
                    ModuleId = "rest-api",
                    Title = "Authentication & API Keys",
                    Summary = "Learn how to authenticate with the CMS REST API.",
                    Order = 2,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Create API keys in the CMS",
                        "Obtain access tokens",
                        "Use bearer authentication"
                    },
                    Content = @"
<h2>API Authentication</h2>
<p>The CMS REST API requires <strong>OAuth 2.0 authentication</strong> using JSON Web Tokens (JWT).</p>

<h3>Creating API Keys</h3>
<ol>
    <li>Go to <strong>Settings > API Keys</strong></li>
    <li>Click <strong>Create API Key</strong></li>
    <li>Enter a name (letters, numbers, hyphens, underscores only)</li>
    <li>Optionally enable Impersonation</li>
    <li>Click <strong>Create API Key</strong></li>
    <li>Save the Client ID and Secret securely</li>
</ol>

<div class=""bg-yellow-50 dark:bg-yellow-900/20 border-l-4 border-yellow-400 p-4 my-4"">
    <p class=""text-yellow-800 dark:text-yellow-200""><strong>Important:</strong> The secret is only shown once. Store it securely.</p>
</div>

<h3>Obtaining Access Tokens</h3>
<p>Request a token from the OAuth endpoint:</p>
<pre>POST https://api.cms.optimizely.com/oauth/token</pre>

<h3>Required Scope</h3>
<p>The API requires the <code>api:admin</code> scope, which is included by default in access tokens.</p>

<h3>Using the Token</h3>
<p>Include the token in the Authorization header:</p>
<pre>Authorization: Bearer &lt;your-access-token&gt;</pre>

<h3>Token Expiration</h3>
<p>Tokens expire after a set period. Your application should handle token refresh.</p>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "api-auth-request",
                            Title = "Token Request",
                            Description = "Request an access token using client credentials.",
                            Type = ExampleType.Code,
                            ExampleContent = @"POST https://api.cms.optimizely.com/oauth/token
Content-Type: application/x-www-form-urlencoded

grant_type=client_credentials
&client_id=YOUR_CLIENT_ID
&client_secret=YOUR_CLIENT_SECRET",
                            SampleResponse = @"{
  ""access_token"": ""eyJhbGciOiJSUzI1NiIsInR5cCI..."",
  ""token_type"": ""Bearer"",
  ""expires_in"": 3600,
  ""scope"": ""api:admin""
}",
                            Hints = new List<string>
                            {
                                "Store tokens securely, never in client-side code",
                                "Implement token refresh before expiration"
                            }
                        }
                    }
                },
                new Lesson
                {
                    Id = "api-content-types",
                    ModuleId = "rest-api",
                    Title = "Managing Content Types",
                    Summary = "Create and manage content types via the REST API.",
                    Order = 3,
                    EstimatedMinutes = 15,
                    LearningObjectives = new List<string>
                    {
                        "Create content types via API",
                        "Update existing content types",
                        "Manage properties and validation"
                    },
                    Content = @"
<h2>Content Type Management</h2>
<p>The REST API allows you to programmatically manage content types, enabling automation and version control of your content model.</p>

<h3>API Endpoint</h3>
<pre>https://api.cms.optimizely.com/preview3/contenttypes</pre>

<h3>Operations</h3>
<ul>
    <li><strong>GET</strong> - List or retrieve content types</li>
    <li><strong>POST</strong> - Create new content types</li>
    <li><strong>PATCH</strong> - Update existing content types</li>
    <li><strong>DELETE</strong> - Remove content types</li>
</ul>

<h3>Creating a Content Type</h3>
<p>Send a POST request with the content type definition:</p>
<ul>
    <li><code>key</code> - Unique identifier</li>
    <li><code>baseType</code> - Base type to inherit from</li>
    <li><code>displayName</code> - UI display name</li>
    <li><code>properties</code> - Property definitions</li>
</ul>

<h3>Updating Content Types</h3>
<p>Use PATCH with JSON Merge Patch format. Only include changed properties:</p>
<ul>
    <li>Set a value to update it</li>
    <li>Set a value to <code>null</code> to remove it</li>
    <li>Omit properties to leave unchanged</li>
</ul>

<h3>Concurrency Control</h3>
<p>Use ETags for optimistic locking:</p>
<ol>
    <li>Get the ETag from a GET response</li>
    <li>Include <code>If-Match: ""etag-value""</code> in PATCH/DELETE</li>
    <li>Handle 412 Precondition Failed for conflicts</li>
</ol>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "api-create-content-type",
                            Title = "Create Content Type",
                            Description = "Create a new page content type via REST API.",
                            Type = ExampleType.Code,
                            ExampleContent = @"POST https://api.cms.optimizely.com/preview3/contenttypes
Authorization: Bearer <token>
Content-Type: application/json

{
  ""key"": ""blogPost"",
  ""baseType"": ""_page"",
  ""displayName"": ""Blog Post"",
  ""description"": ""A blog post with author and categories"",
  ""properties"": {
    ""title"": {
      ""type"": ""string"",
      ""displayName"": ""Title"",
      ""required"": true,
      ""indexingType"": ""searchable""
    },
    ""author"": {
      ""type"": ""string"",
      ""displayName"": ""Author Name""
    },
    ""content"": {
      ""type"": ""richText"",
      ""displayName"": ""Content"",
      ""indexingType"": ""searchable""
    },
    ""publishDate"": {
      ""type"": ""dateTime"",
      ""displayName"": ""Publish Date""
    },
    ""featuredImage"": {
      ""type"": ""contentReference"",
      ""displayName"": ""Featured Image"",
      ""allowedTypes"": [""_image""]
    }
  }
}",
                            SampleResponse = @"HTTP/1.1 201 Created
Location: /preview3/contenttypes/blogPost
ETag: ""abc123""

{
  ""key"": ""blogPost"",
  ""baseType"": ""_page"",
  ""displayName"": ""Blog Post"",
  ...
}",
                            Hints = new List<string>
                            {
                                "Save the ETag for future update operations",
                                "Content types can't be renamed - delete and recreate instead"
                            }
                        }
                    }
                },
                new Lesson
                {
                    Id = "api-content-crud",
                    ModuleId = "rest-api",
                    Title = "Content CRUD Operations",
                    Summary = "Create, read, update, and delete content items.",
                    Order = 4,
                    EstimatedMinutes = 15,
                    LearningObjectives = new List<string>
                    {
                        "Create content items via API",
                        "Retrieve and query content",
                        "Update and delete content"
                    },
                    Content = @"
<h2>Content Operations</h2>
<p>The Content API allows full CRUD operations on content items within your CMS.</p>

<h3>API Endpoint</h3>
<pre>https://api.cms.optimizely.com/preview3/experimental/content</pre>

<h3>Creating Content</h3>
<p>POST request with content data:</p>
<ul>
    <li><code>key</code> - UUID for the content item</li>
    <li><code>contentType</code> - Type of content to create</li>
    <li><code>locale</code> - Language code (e.g., ""en"")</li>
    <li><code>container</code> - Parent container UUID</li>
    <li><code>status</code> - ""draft"" or ""published""</li>
    <li><code>displayName</code> - Name shown in the tree</li>
    <li><code>properties</code> - Content values</li>
</ul>

<h3>Reading Content</h3>
<p>GET request with optional parameters:</p>
<ul>
    <li>By key: <code>/content/{key}</code></li>
    <li>With locale: <code>?locale=en</code></li>
    <li>With version: <code>?version=draft</code></li>
</ul>

<h3>Updating Content</h3>
<p>PATCH request with changed properties only:</p>
<ul>
    <li>Include <code>If-Match</code> header with ETag</li>
    <li>Use <code>application/merge-patch+json</code> content type</li>
</ul>

<h3>Publishing Content</h3>
<p>Special endpoint for publishing:</p>
<pre>POST /content/{key}:publish</pre>

<h3>Deleting Content</h3>
<p>DELETE request removes content:</p>
<ul>
    <li>Include ETag for concurrency</li>
    <li>Consider soft-delete workflows</li>
</ul>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "api-create-content",
                            Title = "Create Content Item",
                            Description = "Create and publish a blog post.",
                            Type = ExampleType.Code,
                            ExampleContent = @"POST https://api.cms.optimizely.com/preview3/experimental/content
Authorization: Bearer <token>
Content-Type: application/json

{
  ""key"": ""019003fe597f70c8b9b5f6231c74ed96"",
  ""contentType"": ""blogPost"",
  ""locale"": ""en"",
  ""container"": ""43f936c99b234ea397b261c538ad07c9"",
  ""status"": ""published"",
  ""displayName"": ""Getting Started with Headless CMS"",
  ""properties"": {
    ""title"": ""Getting Started with Headless CMS"",
    ""author"": ""Jane Developer"",
    ""content"": ""<p>Welcome to our guide...</p>"",
    ""publishDate"": ""2024-01-15T10:00:00Z""
  }
}",
                            SampleResponse = @"HTTP/1.1 201 Created
Location: /preview3/experimental/content/019003fe597f70c8b9b5f6231c74ed96

{
  ""key"": ""019003fe597f70c8b9b5f6231c74ed96"",
  ""contentType"": ""blogPost"",
  ""locale"": ""en"",
  ""status"": ""published"",
  ...
}",
                            Hints = new List<string>
                            {
                                "The container is the parent folder UUID - find it in Settings",
                                "UUIDs should have hyphens removed when used in queries"
                            }
                        }
                    }
                }
            }
        };
    }

    #endregion

    #region Module 6: Graph Integration

    private LearningModule BuildGraphIntegrationModule()
    {
        return new LearningModule
        {
            Id = "graph-integration",
            Title = "Content Delivery with Graph",
            Description = "Learn to deliver CMS content through Optimizely Graph.",
            Icon = "bolt",
            Order = 6,
            Difficulty = ModuleDifficulty.Intermediate,
            Prerequisites = new[] { "visual-builder-essentials" },
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "gi-overview",
                    ModuleId = "graph-integration",
                    Title = "Connecting SaaS to Graph",
                    Summary = "Understand how CMS (SaaS) content flows to Optimizely Graph.",
                    Order = 1,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Understand the content indexing process",
                        "Know how content types map to Graph",
                        "Configure Graph synchronization"
                    },
                    Content = @"
<h2>CMS to Graph Integration</h2>
<p>Optimizely Graph is the recommended way to <strong>deliver content</strong> from CMS (SaaS) to your frontend applications. Content is automatically indexed in Graph for efficient querying.</p>

<h3>How Content Flows to Graph</h3>
<ol>
    <li>Content is created/edited in CMS</li>
    <li>On publish, content is indexed in Graph</li>
    <li>Graph schema is automatically updated for new types</li>
    <li>Frontend queries Graph for content</li>
</ol>

<h3>Content Type Mapping</h3>
<p>Each CMS content type gets a corresponding Graph type:</p>
<ul>
    <li>Page types become queryable at their type name</li>
    <li>Components register as both standalone and with ""Property"" suffix</li>
    <li>Standard fields are available on all types</li>
</ul>

<h3>Synchronization</h3>
<p>Content sync happens automatically, but you can trigger manual sync:</p>
<ul>
    <li>Navigate to Scheduled Jobs</li>
    <li>Run ""Optimizely Graph Full Synchronization""</li>
    <li>Use after bulk changes or configuration updates</li>
</ul>

<h3>Graph Schema</h3>
<p>The Graph schema reflects your content model:</p>
<ul>
    <li>Each content type is a GraphQL type</li>
    <li>Properties become fields</li>
    <li>References resolve to related content</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "gi-indexing",
                    ModuleId = "graph-integration",
                    Title = "Content Indexing Configuration",
                    Summary = "Configure how content properties are indexed in Graph.",
                    Order = 2,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Understand indexing types",
                        "Configure property indexing",
                        "Control what content appears in Graph"
                    },
                    Content = @"
<h2>Indexing Configuration</h2>
<p>Control how your content properties are indexed in Graph to optimize query performance and search capabilities.</p>

<h3>Indexing Types</h3>
<p>Properties support four indexing levels:</p>

<h4>Default</h4>
<p>Property is indexed but:</p>
<ul>
    <li>Cannot be used for filtering</li>
    <li>Cannot be used for sorting</li>
    <li>Not included in full-text search</li>
</ul>
<p><em>Use for: Properties only needed in output</em></p>

<h4>Queryable</h4>
<p>Enhanced indexing that allows:</p>
<ul>
    <li>Filtering in where clauses</li>
    <li>Sorting in orderBy</li>
    <li>Not in full-text search</li>
</ul>
<p><em>Use for: Properties used to filter/sort (dates, categories)</em></p>

<h4>Searchable</h4>
<p>Full indexing that enables:</p>
<ul>
    <li>Filtering and sorting</li>
    <li>Full-text search inclusion</li>
    <li>Relevance scoring</li>
</ul>
<p><em>Use for: Content that should be found via search (titles, body text)</em></p>

<h4>Disabled</h4>
<p>Property is excluded from Graph entirely:</p>
<ul>
    <li>Not queryable</li>
    <li>Not returned in results</li>
</ul>
<p><em>Use for: Internal properties, sensitive data</em></p>

<h3>Access Control</h3>
<p>The <strong>SearchIndexer</strong> role determines what gets indexed. Remove Read access for this role on specific content to exclude it from Graph.</p>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "gi-indexing-config",
                            Title = "Indexing Configuration Example",
                            Description = "Configure indexing types for different properties.",
                            Type = ExampleType.Configuration,
                            ExampleContent = @"{
  ""properties"": {
    ""title"": {
      ""type"": ""string"",
      ""displayName"": ""Title"",
      ""indexingType"": ""searchable""
    },
    ""category"": {
      ""type"": ""string"",
      ""displayName"": ""Category"",
      ""indexingType"": ""queryable""
    },
    ""viewCount"": {
      ""type"": ""integer"",
      ""displayName"": ""View Count"",
      ""indexingType"": ""default""
    },
    ""internalNotes"": {
      ""type"": ""string"",
      ""displayName"": ""Internal Notes"",
      ""indexingType"": ""disabled""
    }
  }
}",
                            SampleResponse = @"Indexing Configuration:
- title: Full-text searchable, can filter/sort
- category: Can filter and sort, not in search
- viewCount: Output only, no filtering
- internalNotes: Excluded from Graph",
                            Hints = new List<string>
                            {
                                "Searchable indexing has higher storage/compute cost",
                                "Use queryable for enum-like values used in filters"
                            }
                        }
                    }
                },
                new Lesson
                {
                    Id = "gi-querying",
                    ModuleId = "graph-integration",
                    Title = "Querying CMS Content",
                    Summary = "Build effective GraphQL queries for CMS content.",
                    Order = 3,
                    EstimatedMinutes = 15,
                    LearningObjectives = new List<string>
                    {
                        "Write queries for CMS content types",
                        "Use filtering and sorting",
                        "Handle content references"
                    },
                    Content = @"
<h2>Querying CMS Content</h2>
<p>With content indexed in Graph, you can build powerful queries to retrieve exactly what your frontend needs.</p>

<h3>Basic Query Structure</h3>
<p>Each content type is queryable by name:</p>
<pre>
{
  BlogPost {
    items {
      title
      author
      publishDate
    }
  }
}
</pre>

<h3>Filtering</h3>
<p>Use the <code>where</code> clause for filtering:</p>
<ul>
    <li>Equality: <code>status: { eq: ""published"" }</code></li>
    <li>Contains: <code>title: { contains: ""guide"" }</code></li>
    <li>Date ranges: <code>publishDate: { gte: ""2024-01-01"" }</code></li>
    <li>In list: <code>category: { in: [""News"", ""Blog""] }</code></li>
</ul>

<h3>Sorting</h3>
<p>Use <code>orderBy</code> to control result order:</p>
<pre>orderBy: { publishDate: DESC }</pre>

<h3>Pagination</h3>
<p>Use <code>limit</code> and <code>skip</code> for pagination:</p>
<pre>limit: 10, skip: 20</pre>

<h3>Content References</h3>
<p>Expand references to get related content:</p>
<pre>
featuredImage {
  url
  altText
}
</pre>

<h3>Localization</h3>
<p>Query specific locales:</p>
<pre>locale: { eq: ""en"" }</pre>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "gi-query-example",
                            Title = "Blog Post Query",
                            Description = "Query blog posts with filtering, sorting, and references.",
                            Type = ExampleType.Query,
                            ExampleContent = @"{
  BlogPost(
    where: {
      _and: [
        { status: { eq: ""published"" } }
        { publishDate: { lte: ""2024-12-31"" } }
      ]
    }
    orderBy: { publishDate: DESC }
    limit: 10
    locale: en
  ) {
    items {
      _metadata {
        key
        url {
          default
        }
      }
      title
      author
      publishDate
      content {
        html
      }
      featuredImage {
        url {
          default
        }
      }
    }
    total
  }
}",
                            SampleResponse = @"{
  ""data"": {
    ""BlogPost"": {
      ""items"": [
        {
          ""_metadata"": {
            ""key"": ""abc123"",
            ""url"": { ""default"": ""/blog/headless-cms-guide"" }
          },
          ""title"": ""Getting Started with Headless CMS"",
          ""author"": ""Jane Developer"",
          ""publishDate"": ""2024-01-15T10:00:00Z"",
          ""content"": { ""html"": ""<p>Welcome...</p>"" },
          ""featuredImage"": {
            ""url"": { ""default"": ""/media/hero.jpg"" }
          }
        }
      ],
      ""total"": 42
    }
  }
}",
                            Hints = new List<string>
                            {
                                "_metadata provides system fields like URL and key",
                                "Use total for pagination calculations"
                            }
                        }
                    }
                },
                new Lesson
                {
                    Id = "gi-recursive-queries",
                    ModuleId = "graph-integration",
                    Title = "Explicit vs Recursive Queries",
                    Summary = "Master different approaches for querying nested Visual Builder content.",
                    Order = 4,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Choose the right query approach",
                        "Use the @recursive directive",
                        "Handle complex compositions"
                    },
                    Content = @"
<h2>Query Approaches for Visual Builder</h2>
<p>Visual Builder content creates nested structures that require special query techniques.</p>

<h3>Explicit Queries</h3>
<p>Define each level of nesting explicitly:</p>
<ul>
    <li>Best when structure depth is known</li>
    <li>More verbose but precise control</li>
    <li>Easier to understand and debug</li>
</ul>

<h3>Recursive Queries</h3>
<p>Use GraphQL's <code>@recursive</code> directive:</p>
<ul>
    <li>Handles variable-depth structures</li>
    <li>More concise query syntax</li>
    <li>Useful for deeply nested content</li>
</ul>

<h3>When to Use Each</h3>

<h4>Use Explicit When:</h4>
<ul>
    <li>Structure is fixed and known</li>
    <li>You need precise control over fields</li>
    <li>Performance optimization is needed</li>
</ul>

<h4>Use Recursive When:</h4>
<ul>
    <li>Structure depth varies</li>
    <li>Content managers can create arbitrary nesting</li>
    <li>You want simpler query maintenance</li>
</ul>

<h3>Performance Considerations</h3>
<ul>
    <li>Limit recursion depth when possible</li>
    <li>Be mindful of query complexity</li>
    <li>Cache results appropriately</li>
</ul>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "gi-recursive-query",
                            Title = "Recursive Composition Query",
                            Description = "Use @recursive to query nested Visual Builder structures.",
                            Type = ExampleType.Query,
                            ExampleContent = @"{
  LandingPageExperience {
    items {
      Name
      composition {
        ... CompositionFields @recursive(depth: 10)
      }
    }
  }
}

fragment CompositionFields on CompositionNode {
  type
  nodeType
  key
  ... on CompositionStructureNode {
    nodes {
      ... CompositionFields
    }
  }
  ... on CompositionComponentNode {
    component {
      ... on HeadingElement {
        text
        level
      }
      ... on ButtonElement {
        text
        url
      }
    }
  }
}",
                            SampleResponse = @"The @recursive directive automatically
expands nested structures up to 10 levels deep,
returning all composition nodes with their
component data in a single query.",
                            Hints = new List<string>
                            {
                                "Fragments keep recursive queries maintainable",
                                "Set depth limit to prevent excessive nesting"
                            }
                        }
                    }
                }
            }
        };
    }

    #endregion

    #region Module 7: Access Rights & Administration

    private LearningModule BuildAccessRightsModule()
    {
        return new LearningModule
        {
            Id = "access-rights",
            Title = "Access Rights & Administration",
            Description = "Learn to configure permissions and manage users in CMS (SaaS).",
            Icon = "shield-check",
            Order = 7,
            Difficulty = ModuleDifficulty.Intermediate,
            Prerequisites = new[] { "getting-started" },
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "ar-permissions",
                    ModuleId = "access-rights",
                    Title = "Understanding Permissions",
                    Summary = "Learn the six permission types and how they control access.",
                    Order = 1,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Know all six permission types",
                        "Understand permission inheritance",
                        "Apply appropriate permissions"
                    },
                    Content = @"
<h2>Permission Types</h2>
<p>CMS (SaaS) uses six permission levels to control what users can do with content.</p>

<h3>The Six Permissions</h3>

<h4>Read</h4>
<p>View content as a reader. Without Read permission, content is invisible to the user.</p>

<h4>Create</h4>
<p>Generate new content under a content item. Users need Create on the parent to add children.</p>

<h4>Change</h4>
<p>Modify existing content. Allows editing but not publishing or deleting.</p>

<h4>Delete</h4>
<p>Remove content permanently. Use carefully - deleted content may not be recoverable.</p>

<h4>Publish</h4>
<p>Make content live. Essential for content to appear on the public site.</p>

<h4>Administer</h4>
<p>Full control including:</p>
<ul>
    <li>Create approval sequences</li>
    <li>Set access rights on the item</li>
    <li>Manage language properties</li>
</ul>

<h3>Permission Inheritance</h3>
<p>Permissions cascade down the content tree:</p>
<ul>
    <li>Child items inherit parent permissions by default</li>
    <li>You can break inheritance on any item</li>
    <li>Explicit permissions override inherited ones</li>
</ul>

<h3>Permission Combinations</h3>
<p>Common combinations:</p>
<ul>
    <li><strong>Viewer</strong>: Read only</li>
    <li><strong>Editor</strong>: Read, Create, Change</li>
    <li><strong>Publisher</strong>: Read, Create, Change, Publish</li>
    <li><strong>Full Control</strong>: All permissions</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "ar-user-groups",
                    ModuleId = "access-rights",
                    Title = "Built-in User Groups",
                    Summary = "Understand the default user groups and their roles.",
                    Order = 2,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Know the built-in groups",
                        "Understand each group's purpose",
                        "Assign users appropriately"
                    },
                    Content = @"
<h2>Default User Groups</h2>
<p>CMS (SaaS) includes four built-in groups with predefined permission sets.</p>

<h3>Administrators</h3>
<p>Full system access for developers and technical admins:</p>
<ul>
    <li>All permissions on all content</li>
    <li>System configuration access</li>
    <li>Content type management</li>
    <li>API key management</li>
</ul>

<h3>Content Admins</h3>
<p>Administrative functions without editing:</p>
<ul>
    <li>Manage settings and configuration</li>
    <li>Set up access rights</li>
    <li>Configure languages and workflows</li>
    <li>No direct content editing</li>
</ul>

<h3>Content Editors</h3>
<p>Day-to-day content work:</p>
<ul>
    <li>Create and edit content</li>
    <li>Publish content</li>
    <li>Manage assets</li>
    <li>Use Visual Builder</li>
</ul>

<h3>Everyone</h3>
<p>Anonymous/public access:</p>
<ul>
    <li>Read access to published content</li>
    <li>Used for public website visitors</li>
    <li>No editing capabilities</li>
</ul>

<h3>Group Best Practices</h3>
<ul>
    <li>Assign users to groups rather than individual permissions</li>
    <li>Create custom groups for specific needs</li>
    <li>Keep Administrator membership limited</li>
    <li>Document group purposes</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "ar-custom-roles",
                    ModuleId = "access-rights",
                    Title = "Creating Custom Roles",
                    Summary = "Build custom roles for specific organizational needs.",
                    Order = 3,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Create roles in Opti ID Admin Center",
                        "Configure role attributes",
                        "Sync roles with CMS"
                    },
                    Content = @"
<h2>Custom Role Management</h2>
<p>When built-in groups don't fit your needs, create custom roles in the Opti ID Admin Center.</p>

<h3>Creating a Role</h3>
<ol>
    <li>Access the Opti ID Admin Center</li>
    <li>Navigate to Roles management</li>
    <li>Click Create New Role</li>
    <li>Configure:
        <ul>
            <li>Role name</li>
            <li>Description (optional)</li>
            <li>Product: Optimizely CMS</li>
            <li>Target instances</li>
        </ul>
    </li>
    <li>Save the role</li>
</ol>

<h3>Assigning Roles</h3>
<p>Roles can be assigned:</p>
<ul>
    <li><strong>To individuals</strong> - Direct assignment in Opti ID</li>
    <li><strong>To groups</strong> - Assign role to a group, all members inherit</li>
</ul>

<h3>Role Syncing</h3>
<p>Roles sync to CMS when users authenticate. The sync process:</p>
<ol>
    <li>User logs into CMS</li>
    <li>Opti ID validates credentials</li>
    <li>User's roles are synced</li>
    <li>Permissions apply immediately</li>
</ol>

<h3>Role Strategy</h3>
<ul>
    <li>Group-based assignment simplifies management</li>
    <li>Create roles for job functions, not individuals</li>
    <li>Document the purpose of each custom role</li>
    <li>Review role assignments periodically</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "ar-configuration",
                    ModuleId = "access-rights",
                    Title = "Configuring Access Rights",
                    Summary = "Set up access rights on content and configure language permissions.",
                    Order = 4,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Configure access rights on content",
                        "Set up language-specific permissions",
                        "Manage inheritance settings"
                    },
                    Content = @"
<h2>Configuring Access Rights</h2>
<p>Administrators manage permissions through the Settings interface or directly on content items.</p>

<h3>Via Settings</h3>
<ol>
    <li>Navigate to <strong>Settings > Set Access Rights</strong></li>
    <li>Select a content item in the tree</li>
    <li>Configure permissions:
        <ul>
            <li>Add/remove user groups</li>
            <li>Set permission levels</li>
            <li>Control inheritance</li>
        </ul>
    </li>
    <li>Apply to subitems if needed</li>
</ol>

<h3>Via Content Context Menu</h3>
<p>Editors with Administer rights can:</p>
<ol>
    <li>Select content in the tree</li>
    <li>Open Publish Changes menu</li>
    <li>Access Set Access Rights</li>
    <li>Configure for that item only</li>
</ol>

<h3>Inheritance Options</h3>
<ul>
    <li><strong>Inherit from parent</strong> - Use parent's permissions</li>
    <li><strong>Break inheritance</strong> - Set custom permissions</li>
    <li><strong>Apply to subitems</strong> - Push changes down the tree</li>
</ul>

<h3>Language-Specific Access</h3>
<p>Configure in <strong>Settings > Manage Website Languages</strong>:</p>
<ul>
    <li>Enable languages for the site</li>
    <li>Assign language permissions to groups</li>
    <li>Users only see languages they have access to</li>
</ul>

<h3>Graph Indexing Access</h3>
<p>The <strong>SearchIndexer</strong> role controls what appears in Graph:</p>
<ul>
    <li>Remove Read for SearchIndexer to exclude content</li>
    <li>Useful for internal or draft content</li>
</ul>
"
                }
            }
        };
    }

    #endregion

    #region Module 8: Advanced Topics & Best Practices

    private LearningModule BuildAdvancedTopicsModule()
    {
        return new LearningModule
        {
            Id = "advanced-topics",
            Title = "Advanced Topics & Best Practices",
            Description = "Master advanced concepts and learn best practices for CMS (SaaS) implementations.",
            Icon = "rocket-launch",
            Order = 8,
            Difficulty = ModuleDifficulty.Advanced,
            Prerequisites = new[] { "rest-api", "graph-integration", "access-rights" },
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "adv-multisite",
                    ModuleId = "advanced-topics",
                    Title = "Multi-site Configuration",
                    Summary = "Configure multiple websites within a single CMS instance.",
                    Order = 1,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Understand multi-site architecture",
                        "Configure multiple websites",
                        "Manage hosts and domains"
                    },
                    Content = @"
<h2>Multi-site Configuration</h2>
<p>CMS (SaaS) supports running multiple websites from a single instance, sharing content and resources while maintaining separate identities.</p>

<h3>Multi-site Benefits</h3>
<ul>
    <li>Share content types across sites</li>
    <li>Reuse assets and components</li>
    <li>Centralized management</li>
    <li>Cost-effective scaling</li>
</ul>

<h3>Configuration Steps</h3>
<ol>
    <li>Navigate to <strong>Settings > Manage Websites</strong></li>
    <li>Create a new website entry</li>
    <li>Configure:
        <ul>
            <li>Website name</li>
            <li>Start page</li>
            <li>URL structure</li>
        </ul>
    </li>
    <li>Add host configurations</li>
</ol>

<h3>Host Configuration</h3>
<p>Each site can have multiple hosts:</p>
<ul>
    <li><strong>Production host</strong> - Live site URL</li>
    <li><strong>Preview host</strong> - Editor preview URL</li>
    <li><strong>Edit host</strong> - CMS edit mode URL</li>
</ul>

<h3>Content Sharing</h3>
<p>Sites can share content through:</p>
<ul>
    <li>Global content folders</li>
    <li>Shared assets library</li>
    <li>Cross-site references</li>
</ul>

<h3>Considerations</h3>
<ul>
    <li>Plan content structure carefully</li>
    <li>Define clear ownership</li>
    <li>Consider SEO implications</li>
    <li>Manage permissions per site</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "adv-sync-jobs",
                    ModuleId = "advanced-topics",
                    Title = "Content Synchronization",
                    Summary = "Understand and manage content sync between CMS and Graph.",
                    Order = 2,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Understand sync processes",
                        "Run synchronization jobs",
                        "Troubleshoot sync issues"
                    },
                    Content = @"
<h2>Content Synchronization</h2>
<p>Content synchronization ensures your content in CMS is properly indexed and available in Optimizely Graph.</p>

<h3>Automatic Sync</h3>
<p>Content is automatically synced when:</p>
<ul>
    <li>Content is published</li>
    <li>Content is unpublished</li>
    <li>Content is deleted</li>
</ul>

<h3>Manual Sync Jobs</h3>
<p>Run full sync through Scheduled Jobs:</p>
<ul>
    <li><strong>Optimizely Graph Full Synchronization</strong> - Re-indexes all content</li>
</ul>

<h3>When to Run Manual Sync</h3>
<ul>
    <li>After bulk content imports</li>
    <li>After content type changes</li>
    <li>After configuration updates</li>
    <li>When troubleshooting missing content</li>
</ul>

<h3>Sync Troubleshooting</h3>
<p>Content not appearing in Graph? Check:</p>
<ol>
    <li>Content is published, not draft</li>
    <li>SearchIndexer has Read access</li>
    <li>Content type is not excluded</li>
    <li>Indexing type is not ""disabled""</li>
    <li>Run full synchronization</li>
</ol>

<h3>Sync Performance</h3>
<ul>
    <li>Full sync can take time for large sites</li>
    <li>Run during off-peak hours if possible</li>
    <li>Monitor sync job status</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "adv-performance",
                    ModuleId = "advanced-topics",
                    Title = "Performance Optimization",
                    Summary = "Optimize your CMS (SaaS) implementation for best performance.",
                    Order = 3,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Optimize content model design",
                        "Improve query performance",
                        "Implement caching strategies"
                    },
                    Content = @"
<h2>Performance Optimization</h2>
<p>Optimize your CMS (SaaS) implementation for the best user experience and efficient resource usage.</p>

<h3>Content Model Optimization</h3>
<ul>
    <li><strong>Keep content types focused</strong> - Don't overload with properties</li>
    <li><strong>Use appropriate indexing</strong> - Only ""searchable"" when needed</li>
    <li><strong>Normalize references</strong> - Avoid deep nesting</li>
</ul>

<h3>Query Optimization</h3>
<ul>
    <li><strong>Request only needed fields</strong> - Don't over-fetch</li>
    <li><strong>Use pagination</strong> - Limit result sets</li>
    <li><strong>Apply filters early</strong> - Reduce data processing</li>
    <li><strong>Avoid n+1 queries</strong> - Fetch related content in one query</li>
</ul>

<h3>Caching Strategies</h3>
<ul>
    <li><strong>CDN caching</strong> - Cache Graph responses at edge</li>
    <li><strong>Application caching</strong> - Cache in your frontend app</li>
    <li><strong>Cache invalidation</strong> - Refresh on content changes</li>
</ul>

<h3>Visual Builder Performance</h3>
<ul>
    <li>Keep section nesting reasonable</li>
    <li>Optimize element complexity</li>
    <li>Limit elements per page</li>
</ul>

<h3>Image Optimization</h3>
<ul>
    <li>Use appropriate image sizes</li>
    <li>Leverage image transformation URLs</li>
    <li>Implement lazy loading</li>
</ul>

<h3>API Rate Limiting</h3>
<p>Remember the limit: 100 requests per 10 seconds per IP</p>
<ul>
    <li>Batch operations when possible</li>
    <li>Implement request queuing</li>
    <li>Handle 429 responses gracefully</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "adv-migration",
                    ModuleId = "advanced-topics",
                    Title = "Migration to CMS (SaaS)",
                    Summary = "Plan and execute a migration from CMS 12 to CMS (SaaS).",
                    Order = 4,
                    EstimatedMinutes = 15,
                    LearningObjectives = new List<string>
                    {
                        "Understand migration considerations",
                        "Plan a migration strategy",
                        "Execute content migration"
                    },
                    Content = @"
<h2>Migration to CMS (SaaS)</h2>
<p>Migrating from CMS 12 (PaaS) to CMS (SaaS) requires careful planning and execution.</p>

<h3>Key Differences</h3>
<table class=""min-w-full divide-y divide-gray-200 dark:divide-gray-700 my-4"">
    <thead>
        <tr>
            <th class=""px-4 py-2 text-left"">Aspect</th>
            <th class=""px-4 py-2 text-left"">CMS 12 (PaaS)</th>
            <th class=""px-4 py-2 text-left"">CMS (SaaS)</th>
        </tr>
    </thead>
    <tbody>
        <tr><td class=""px-4 py-2"">Hosting</td><td class=""px-4 py-2"">Self-managed</td><td class=""px-4 py-2"">Optimizely-managed</td></tr>
        <tr><td class=""px-4 py-2"">Content Types</td><td class=""px-4 py-2"">C# code</td><td class=""px-4 py-2"">JSON definitions</td></tr>
        <tr><td class=""px-4 py-2"">Rendering</td><td class=""px-4 py-2"">Server-side</td><td class=""px-4 py-2"">Headless/API-based</td></tr>
        <tr><td class=""px-4 py-2"">Customization</td><td class=""px-4 py-2"">Full code access</td><td class=""px-4 py-2"">Configuration/API</td></tr>
    </tbody>
</table>

<h3>Migration Planning</h3>
<ol>
    <li><strong>Audit existing content</strong>
        <ul>
            <li>Content types and properties</li>
            <li>Content volume and complexity</li>
            <li>Custom functionality</li>
        </ul>
    </li>
    <li><strong>Design new content model</strong>
        <ul>
            <li>Map C# types to JSON definitions</li>
            <li>Plan Visual Builder structure</li>
            <li>Identify customization gaps</li>
        </ul>
    </li>
    <li><strong>Plan frontend rebuild</strong>
        <ul>
            <li>Choose frontend framework</li>
            <li>Design Graph queries</li>
            <li>Implement rendering components</li>
        </ul>
    </li>
</ol>

<h3>Content Migration</h3>
<ul>
    <li>Export content from CMS 12</li>
    <li>Transform to SaaS format</li>
    <li>Import via REST API</li>
    <li>Verify and validate</li>
</ul>

<h3>Gradual Migration</h3>
<p>Consider a phased approach:</p>
<ol>
    <li>Start with new content in SaaS</li>
    <li>Migrate section by section</li>
    <li>Maintain both systems during transition</li>
    <li>Full cutover when ready</li>
</ol>

<h3>Migration Tools</h3>
<ul>
    <li>REST API for content type creation</li>
    <li>Content API for content import</li>
    <li>Custom scripts for transformation</li>
</ul>
"
                }
            }
        };
    }

    #endregion
}
