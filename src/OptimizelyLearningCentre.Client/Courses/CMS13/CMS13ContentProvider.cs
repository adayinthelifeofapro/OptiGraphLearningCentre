using OptimizelyLearningCentre.Client.Models.Learning;
using OptimizelyLearningCentre.Client.Services;

namespace OptimizelyLearningCentre.Client.Courses.CMS13;

/// <summary>
/// Content provider for the Optimizely CMS 13 (Pre-Release) course
/// </summary>
public class CMS13ContentProvider : ILearningContentProvider
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
            BuildOverviewModule(),
            BuildVisualBuilderModule(),
            BuildContentManagerModule(),
            BuildEditExperienceModule(),
            BuildGraphIntegrationModule(),
            BuildLanguagesModule(),
            BuildContentVariationsModule(),
            BuildFrameworkModule()
        };
    }

    #region Module 1: Overview of CMS 13

    private LearningModule BuildOverviewModule()
    {
        return new LearningModule
        {
            Id = "overview",
            Title = "Overview of CMS 13 Pre-Release",
            Description = "Discover the new features and enhancements in Optimizely CMS 13 pre-release.",
            Icon = "rocket-launch",
            Order = 1,
            Difficulty = ModuleDifficulty.Beginner,
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "ov-introduction",
                    ModuleId = "overview",
                    Title = "What's New in CMS 13",
                    Summary = "Get an overview of the major features and enhancements in the CMS 13 pre-release.",
                    Order = 1,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Understand the key features introduced in CMS 13",
                        "Learn about the strong focus on Optimizely Graph integration",
                        "Discover improvements to content management and editing"
                    },
                    Content = @"
<h2>Welcome to CMS 13 Pre-Release</h2>
<p>Optimizely CMS 13 represents a significant evolution of the platform, with a <strong>strong focus on integration with Optimizely Graph</strong>. This pre-release provides an early look at the features and enhancements that will shape the future of content management with Optimizely.</p>

<h3>Key Feature Areas</h3>

<h4>Content Management &amp; Editing</h4>
<p>CMS 13 introduces a modernized content management experience:</p>
<ul>
    <li><strong>New Content Manager</strong> - A search-first approach powered by Optimizely Graph</li>
    <li><strong>Enhanced Visual Builder</strong> - Improved WYSIWYG editing with templates and blueprints</li>
    <li><strong>In-context editing</strong> - Edit content directly within Visual Builder</li>
    <li><strong>Customizable columns</strong> - Configure list views in Content Manager</li>
</ul>

<h4>Graph Integration</h4>
<p>Optimizely Graph is now a core component of CMS 13:</p>
<ul>
    <li><strong>Content retrieval</strong> - Graph is mandatory for content retrieval operations</li>
    <li><strong>External content</strong> - Surface external content sources in the CMS UI</li>
    <li><strong>Search capabilities</strong> - Full-text search, filtering, and faceted navigation</li>
    <li><strong>Global contracts</strong> - Consistent content indexing across sources</li>
</ul>

<h4>Multilingual Support</h4>
<p>Enhanced language management capabilities:</p>
<ul>
    <li><strong>Global fallback languages</strong> - Configure recursive fallback chains</li>
    <li><strong>Auto-translation</strong> - Machine-translate content while preserving structure</li>
    <li><strong>Language context switching</strong> - Dynamic UI updates based on language selection</li>
</ul>

<h4>Developer Experience</h4>
<p>Modernized development platform:</p>
<ul>
    <li><strong>.NET 10 runtime</strong> - Built on the latest .NET platform</li>
    <li><strong>Custom elements</strong> - Define custom elements in the Admin UI</li>
    <li><strong>Simplified plugin architecture</strong> - Plugin Manager removed in favor of modern patterns</li>
</ul>

<div class=""bg-yellow-50 dark:bg-yellow-900 border-l-4 border-yellow-400 p-4 my-4"">
    <p class=""text-yellow-800 dark:text-yellow-100""><strong>Pre-Release Notice:</strong> This is a pre-release version of CMS 13. Some features may change before the final release, and certain functionality may not yet be fully enabled.</p>
</div>
"
                },
                new Lesson
                {
                    Id = "ov-graph-mandatory",
                    ModuleId = "overview",
                    Title = "Optimizely Graph - Core Architecture",
                    Summary = "Understand why Optimizely Graph is mandatory in CMS 13 and its role in content delivery.",
                    Order = 2,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Understand why Graph is mandatory in CMS 13",
                        "Learn the difference between content retrieval and search",
                        "Understand how CMS 13 uses Graph internally"
                    },
                    Content = @"
<h2>Optimizely Graph as Core Architecture</h2>
<p>In CMS 13, <strong>Optimizely Graph is mandatory</strong>. It serves as the core architecture for content retrieval and is always used internally by the CMS.</p>

<h3>Why Graph is Required</h3>
<p>Unlike CMS 12 where Graph was optional, CMS 13 is built with Graph at its foundation:</p>
<ul>
    <li><strong>Content Manager</strong> - Requires Graph for its search-first navigation</li>
    <li><strong>Content retrieval</strong> - CMS 13 uses Graph internally to retrieve and structure content</li>
    <li><strong>Performance</strong> - Optimized queries through Graph's caching layer</li>
</ul>

<h3>Two Distinct Capabilities</h3>
<p>Graph operates with two separate capabilities in CMS 13:</p>

<table class=""min-w-full divide-y divide-gray-200 dark:divide-gray-700 my-4"">
    <thead>
        <tr>
            <th class=""px-4 py-2 text-left"">Capability</th>
            <th class=""px-4 py-2 text-left"">Status</th>
            <th class=""px-4 py-2 text-left"">Description</th>
        </tr>
    </thead>
    <tbody>
        <tr><td class=""px-4 py-2"">Content Retrieval</td><td class=""px-4 py-2""><span class=""text-green-600"">Required</span></td><td class=""px-4 py-2"">CMS 13 uses Graph internally for content operations</td></tr>
        <tr><td class=""px-4 py-2"">Search</td><td class=""px-4 py-2""><span class=""text-blue-600"">Optional</span></td><td class=""px-4 py-2"">Organizations can use Graph for search or implement their own provider</td></tr>
    </tbody>
</table>

<h3>Getting Started</h3>
<p>To integrate Graph into your CMS 13 application, add this to your startup file:</p>
<pre class=""bg-gray-100 dark:bg-gray-800 p-4 rounded-lg overflow-x-auto""><code>services.AddContentGraph()</code></pre>

<div class=""bg-blue-50 dark:bg-blue-900 border-l-4 border-blue-400 p-4 my-4"">
    <p class=""text-blue-800 dark:text-blue-100""><strong>Note:</strong> The .NET SDK for Graph is not yet included in the pre-release version but will be available in the final release.</p>
</div>
"
                },
                new Lesson
                {
                    Id = "ov-not-included",
                    ModuleId = "overview",
                    Title = "What's NOT in CMS 13 Pre-Release",
                    Summary = "Learn about features and functionality that are explicitly excluded from the CMS 13 pre-release.",
                    Order = 3,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Understand which major features are excluded from the pre-release",
                        "Know the implications for your evaluation and planning",
                        "Identify workarounds or alternative approaches"
                    },
                    Content = @"
<h2>Features NOT in CMS 13 Pre-Release</h2>
<p>The CMS 13 pre-release excludes several significant features. Understanding these exclusions is essential for planning your evaluation and migration strategy.</p>

<div class=""bg-red-50 dark:bg-red-900 border-l-4 border-red-400 p-4 my-4"">
    <p class=""text-red-800 dark:text-red-100""><strong>Important:</strong> The following features are explicitly NOT available in this pre-release version.</p>
</div>

<h3>Major Feature Exclusions</h3>

<h4>1. Opal Integration</h4>
<p><strong>Opal integration is not enabled in CMS 13 pre-release.</strong></p>
<p>The AI-powered content assistant and personalization capabilities provided by Opal are not available. If your workflows depend on Opal for content generation or optimization, you'll need to continue using your existing CMS version for those capabilities.</p>

<h4>2. DAM (Digital Asset Management)</h4>
<p><strong>DAM integration is not enabled in CMS 13 pre-release.</strong></p>
<p>Digital Asset Management functionality is not currently supported. Organizations relying on DAM integration for media management should plan accordingly.</p>

<h4>3. Projects Feature</h4>
<p><strong>Projects are not supported in CMS 13 pre-release and must be disabled to launch the CMS.</strong></p>
<p>The Projects feature for coordinating content workflows and releases is not available. In fact, Projects must be explicitly disabled for CMS 13 to function properly in this release.</p>

<h4>4. Forms for Visual Builder</h4>
<p><strong>Forms for Visual Builder is not supported in CMS 13 pre-release.</strong></p>
<p>Form creation and integration within Visual Builder is unavailable. If you need form capabilities, you'll need to implement alternative solutions or wait for a future release.</p>

<h4>5. REST API</h4>
<p><strong>REST API is not available in the CMS 13 pre-release.</strong></p>
<p>Programmatic access through REST endpoints is not supported. This affects any integrations or automation that rely on the REST API for content management operations.</p>

<h3>Additional Technical Limitations</h3>

<h4>Graph SDK</h4>
<p>The .NET SDK for Optimizely Graph is not yet fully included. Pending capabilities:</p>
<ul>
    <li>Full LINQ provider support</li>
    <li>Direct link support (use <code>IContentLoader.GetAncestors()</code> instead)</li>
    <li>Recursive directives (use <code>IContentLoader.Get()</code>)</li>
    <li>Routing support</li>
    <li>Single document retrieval by ID</li>
    <li>Multi-search capabilities</li>
</ul>

<h4>On-Page Edit (OPE)</h4>
<p>Traditional On-Page Edit is disabled in favor of the new Visual Builder experience.</p>

<h4>Smooth Rebuild</h4>
<p>The Smooth Rebuild feature for resetting Graph sources without downtime has not been enabled yet.</p>

<h4>Content Manager Features</h4>
<p>Some Content Manager capabilities are not yet available:</p>
<ul>
    <li>Custom columns</li>
    <li>Saved view configurations</li>
    <li>Session persistence</li>
    <li>Content Manager as content selector</li>
    <li>Content Sources/Contracts</li>
</ul>

<h4>Content Variations Limitations</h4>
<ul>
    <li>Only localizable properties are supported</li>
    <li>Softlinks aren't generated for published variations</li>
</ul>

<h3>Planning Your Evaluation</h3>
<p>When evaluating CMS 13 pre-release, consider:</p>
<ul>
    <li><strong>Feature dependencies</strong> - Identify if your current implementation relies on any excluded features</li>
    <li><strong>Timeline planning</strong> - Factor in that these features will arrive in future releases</li>
    <li><strong>Testing scope</strong> - Focus evaluation on features that ARE available in the pre-release</li>
    <li><strong>Hybrid approach</strong> - Consider running CMS 13 pre-release alongside existing systems for testing</li>
</ul>

<div class=""bg-gray-50 dark:bg-gray-800 border-l-4 border-gray-400 p-4 my-4"">
    <p class=""text-gray-800 dark:text-gray-200""><strong>Stay Updated:</strong> Check the <a href=""https://docs.developers.optimizely.com/content-management-system/v13-Pre-Release/docs/not-in-cms-13-pre-release"" target=""_blank"" class=""text-violet-600 hover:underline"">official documentation</a> regularly for updates on feature availability as the pre-release progresses toward general availability.</p>
</div>
"
                }
            }
        };
    }

    #endregion

    #region Module 2: Visual Builder

    private LearningModule BuildVisualBuilderModule()
    {
        return new LearningModule
        {
            Id = "visual-builder",
            Title = "Visual Builder Enhancements",
            Description = "Master the enhanced Visual Builder with templates, blueprints, and improved editing capabilities.",
            Icon = "paint-brush",
            Order = 2,
            Difficulty = ModuleDifficulty.Intermediate,
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "vb-layout-architecture",
                    ModuleId = "visual-builder",
                    Title = "Layout Architecture",
                    Summary = "Understand the data model for Visual Builder experiences and sections.",
                    Order = 1,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Understand the separation of layout structure from content data",
                        "Learn about key fields like AllowLayout and AllowAdditionalData",
                        "Understand experiences, sections, and elements"
                    },
                    Content = @"
<h2>Visual Builder Layout Architecture</h2>
<p>CMS 13 introduces a modernized Visual Builder with a clear separation between <strong>layout structure</strong> and <strong>content data</strong>. This separation enables flexible content management while keeping layout definitions distinct from actual content values.</p>

<h3>Key Structural Elements</h3>

<h4>AllowLayout Field</h4>
<p>The <code>AllowLayout</code> field controls whether a content type supports Visual Builder composition. When enabled, the content type can be used as an experience or section in Visual Builder.</p>

<h4>AllowAdditionalData Field</h4>
<p>The <code>AllowAdditionalData</code> field enables supplementary data elements for individual instances. This allows editors to add extra properties to specific content items without modifying the content type definition.</p>

<h4>Pre-defined Content Types</h4>
<p>CMS 13 includes blank experience and section templates that are enabled by default, providing a starting point for content creation.</p>

<h3>Layout Hierarchy</h3>
<p>Visual Builder organizes content in a clear hierarchy:</p>

<table class=""min-w-full divide-y divide-gray-200 dark:divide-gray-700 my-4"">
    <thead>
        <tr>
            <th class=""px-4 py-2 text-left"">Element</th>
            <th class=""px-4 py-2 text-left"">Description</th>
            <th class=""px-4 py-2 text-left"">Purpose</th>
        </tr>
    </thead>
    <tbody>
        <tr><td class=""px-4 py-2""><strong>Experiences</strong></td><td class=""px-4 py-2"">The main routable entry point</td><td class=""px-4 py-2"">Top-level pages that visitors navigate to</td></tr>
        <tr><td class=""px-4 py-2""><strong>Sections</strong></td><td class=""px-4 py-2"">Vertical content areas</td><td class=""px-4 py-2"">Organize content within experiences</td></tr>
        <tr><td class=""px-4 py-2""><strong>Elements</strong></td><td class=""px-4 py-2"">The smallest building blocks</td><td class=""px-4 py-2"">Contain the actual content</td></tr>
    </tbody>
</table>

<h3>Block Type Configuration</h3>
<p>Administrators can designate blocks for different contexts:</p>
<ul>
    <li><strong>Element-enabled</strong> - Available within sections</li>
    <li><strong>Section-enabled</strong> - Available within experiences</li>
    <li><strong>Dual-configured</strong> - Supporting both contexts</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "vb-templates-blueprints",
                    ModuleId = "visual-builder",
                    Title = "Templates and Blueprints",
                    Summary = "Learn how to create and manage templates and blueprints for efficient content creation.",
                    Order = 2,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Create templates from existing content",
                        "Manage blueprints with import/export capabilities",
                        "Understand automatic thumbnail generation"
                    },
                    Content = @"
<h2>Templates and Blueprints</h2>
<p>CMS 13 enhances content creation efficiency through improved template and blueprint management.</p>

<h3>Template System</h3>
<p>Templates allow editors to create reusable content patterns:</p>
<ul>
    <li><strong>Create from existing content</strong> - Turn any piece of content into a template</li>
    <li><strong>Create from content types</strong> - Generate templates from type definitions</li>
    <li><strong>Pre-populated properties</strong> - New instances inherit template values and layouts</li>
    <li><strong>Export/Import workflows</strong> - Share templates across environments</li>
    <li><strong>Automatic thumbnails</strong> - System generates preview images automatically</li>
</ul>

<h3>Blueprint Management</h3>
<p>Blueprints have been enhanced with dedicated management capabilities:</p>
<ul>
    <li><strong>Separate export/import</strong> - Blueprints are now distinct content types</li>
    <li><strong>Custom thumbnails</strong> - Support for auto-generated or custom preview images</li>
    <li><strong>Dedicated interface</strong> - Administrators can rename, delete, and organize blueprints</li>
</ul>

<h3>Creating a Template</h3>
<p>To create a template from existing content:</p>
<ol>
    <li>Navigate to the content item you want to use as a template</li>
    <li>Open the context menu and select ""Save as Template""</li>
    <li>Provide a name and optional thumbnail</li>
    <li>The template becomes available in the blueprint selector</li>
</ol>

<h3>Using Templates</h3>
<p>When creating new content:</p>
<ol>
    <li>Click ""Create New"" in Content Manager</li>
    <li>The blueprint selector modal appears with available templates</li>
    <li>Select a template to pre-populate the new content</li>
    <li>Modify as needed and publish</li>
</ol>
"
                },
                new Lesson
                {
                    Id = "vb-editing-features",
                    ModuleId = "visual-builder",
                    Title = "Enhanced Editing Features",
                    Summary = "Explore the new editing capabilities in Visual Builder including property highlighting and copy/paste.",
                    Order = 3,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Use direct property editing within Visual Builder",
                        "Understand property highlighting for visual feedback",
                        "Leverage copy/paste operations for rows and columns"
                    },
                    Content = @"
<h2>Enhanced Editing Features</h2>
<p>Visual Builder in CMS 13 includes several improvements to the editing experience.</p>

<h3>Direct Property Editing</h3>
<p>Experience and section properties are now editable inline within Visual Builder. You no longer need to switch to separate views to modify content properties.</p>

<h3>Property Highlighting</h3>
<p>Interactive visual feedback connects panel fields to their preview representations:</p>
<ul>
    <li><strong>Hover state</strong> - Pale blue highlighting shows which field is being referenced</li>
    <li><strong>Click state</strong> - Dark blue highlighting indicates the selected field</li>
    <li><strong>Field names</strong> - Displayed contextually to help identify properties</li>
</ul>

<h3>Media Asset Editing</h3>
<p>Full editing support for media properties:</p>
<ul>
    <li>Edit custom attributes directly</li>
    <li>Configure preview settings</li>
    <li>Autosave functionality preserves changes</li>
</ul>

<h3>Copy/Paste Operations</h3>
<p>Rows and columns support powerful duplication features:</p>
<ul>
    <li><strong>Complete duplication</strong> - Copy rows and columns with all nested content</li>
    <li><strong>Style preservation</strong> - Styling and asset references are maintained</li>
    <li><strong>User confirmation</strong> - Feedback messages confirm successful operations</li>
</ul>

<h3>Shared Block Integration</h3>
<p>Section-enabled shared blocks can be dragged into experiences with consistent editing workflows. Publishing experiences with unmodified shared blocks remains permitted.</p>

<h3>Bulk Actions</h3>
<p>Content operations preserve layout data:</p>
<ul>
    <li>Content type exports include layout data</li>
    <li>Imports recreate layout structures intact</li>
    <li>Copying content items duplicates complete layout information</li>
</ul>

<div class=""bg-yellow-50 dark:bg-yellow-900 border-l-4 border-yellow-400 p-4 my-4"">
    <p class=""text-yellow-800 dark:text-yellow-100""><strong>Limitation:</strong> TinyMCE editors within Visual Builder cannot receive dropped blocks of any type, preventing unintended composition conflicts.</p>
</div>
"
                },
                new Lesson
                {
                    Id = "vb-aspnet-mvc",
                    ModuleId = "visual-builder",
                    Title = "ASP.NET MVC Support",
                    Summary = "Learn how to use Visual Builder with ASP.NET MVC without requiring Optimizely Graph.",
                    Order = 4,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Understand tag helpers for Visual Builder rendering",
                        "Use HTML helpers for non-composable patterns",
                        "Implement automatic preview updates during editing"
                    },
                    Content = @"
<h2>ASP.NET MVC Support</h2>
<p>Visual Builder extends to ASP.NET MVC environments without requiring Optimizely Graph for rendering.</p>

<h3>Tag Helpers</h3>
<p>CMS 13 provides tag helpers for rendering Visual Builder compositions:</p>

<pre class=""bg-gray-100 dark:bg-gray-800 p-4 rounded-lg overflow-x-auto""><code>&lt;epi-outline&gt;
    &lt;epi-component /&gt;
    &lt;epi-grid&gt;
        &lt;epi-row&gt;
            &lt;epi-column /&gt;
        &lt;/epi-row&gt;
    &lt;/epi-grid&gt;
&lt;/epi-outline&gt;</code></pre>

<h4>Available Tag Helpers</h4>
<table class=""min-w-full divide-y divide-gray-200 dark:divide-gray-700 my-4"">
    <thead>
        <tr>
            <th class=""px-4 py-2 text-left"">Tag Helper</th>
            <th class=""px-4 py-2 text-left"">Purpose</th>
        </tr>
    </thead>
    <tbody>
        <tr><td class=""px-4 py-2""><code>epi-outline</code></td><td class=""px-4 py-2"">Container for Visual Builder content</td></tr>
        <tr><td class=""px-4 py-2""><code>epi-component</code></td><td class=""px-4 py-2"">Renders individual components</td></tr>
        <tr><td class=""px-4 py-2""><code>epi-grid</code></td><td class=""px-4 py-2"">Grid container for layout</td></tr>
        <tr><td class=""px-4 py-2""><code>epi-row</code></td><td class=""px-4 py-2"">Row within a grid</td></tr>
        <tr><td class=""px-4 py-2""><code>epi-column</code></td><td class=""px-4 py-2"">Column within a row</td></tr>
    </tbody>
</table>

<h3>HTML Helpers</h3>
<p>For non-composable patterns, HTML helpers follow established patterns from earlier CMS versions.</p>

<h3>Custom Tag Names</h3>
<p>Visual Builder supports custom tag names for flexible markup generation, allowing you to match your frontend framework's requirements.</p>

<h3>Automatic Preview Updates</h3>
<p>When editing content in Visual Builder, previews update automatically without manual refresh, providing immediate visual feedback.</p>

<h3>Custom Elements</h3>
<p>Citizen developers can define custom elements directly within the Admin UI using existing editors and fields. This enables non-developers to create new element types without writing code.</p>
"
                }
            }
        };
    }

    #endregion

    #region Module 3: Content Manager

    private LearningModule BuildContentManagerModule()
    {
        return new LearningModule
        {
            Id = "content-manager",
            Title = "Content Manager",
            Description = "Learn about the new Content Manager powered by Optimizely Graph.",
            Icon = "folder-open",
            Order = 3,
            Difficulty = ModuleDifficulty.Intermediate,
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "cm-setup",
                    ModuleId = "content-manager",
                    Title = "Setting Up Content Manager",
                    Summary = "Configure Content Manager with Optimizely Graph integration.",
                    Order = 1,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Enable Graph Service in DXP Portal",
                        "Configure authentication keys",
                        "Set up services in Startup.cs"
                    },
                    Content = @"
<h2>Setting Up Content Manager</h2>
<p>Content Manager requires Optimizely Graph integration. Without Graph, Content Manager cannot function.</p>

<h3>Setup Steps</h3>

<h4>1. Enable Graph Service</h4>
<p>In the DXP Portal, navigate to the API tab and enable the Graph Service for your environment.</p>

<h4>2. Retrieve Authentication Keys</h4>
<p>From the DXP Portal, obtain your authentication keys for Graph integration.</p>

<h4>3. Configure appSettings.json</h4>
<p>Add the gateway address and credentials to your application configuration:</p>

<pre class=""bg-gray-100 dark:bg-gray-800 p-4 rounded-lg overflow-x-auto""><code>{
  ""Optimizely"": {
    ""Graph"": {
      ""GatewayAddress"": ""https://graph.optimizely.com"",
      ""SingleKey"": ""your-single-key"",
      ""AppKey"": ""your-app-key"",
      ""Secret"": ""your-secret""
    }
  }
}</code></pre>

<h4>4. Register Services</h4>
<p>In your <code>Startup.cs</code> or <code>Program.cs</code>, register the required services:</p>

<pre class=""bg-gray-100 dark:bg-gray-800 p-4 rounded-lg overflow-x-auto""><code>services.AddContentGraph()
        .AddContentManager();</code></pre>

<div class=""bg-yellow-50 dark:bg-yellow-900 border-l-4 border-yellow-400 p-4 my-4"">
    <p class=""text-yellow-800 dark:text-yellow-100""><strong>Important:</strong> The order of service calls matters during the preview phase. Always call <code>AddContentGraph()</code> before <code>AddContentManager()</code>.</p>
</div>
"
                },
                new Lesson
                {
                    Id = "cm-search-navigation",
                    ModuleId = "content-manager",
                    Title = "Search-First Navigation",
                    Summary = "Explore Content Manager's search-first approach to finding and managing content.",
                    Order = 2,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Understand the search-first navigation paradigm",
                        "Use filtering capabilities effectively",
                        "Navigate content with Grid and List views"
                    },
                    Content = @"
<h2>Search-First Navigation</h2>
<p>Content Manager implements a <strong>search-first approach</strong> to content navigation, powered by Optimizely Graph. This is a fundamental shift from traditional tree-based navigation.</p>

<h3>Enhanced Search</h3>
<p>Search is at the core of Content Manager, making it easy to find content across your entire site:</p>
<ul>
    <li>Full-text search across all content</li>
    <li>Instant results as you type</li>
    <li>Search within specific content areas</li>
</ul>

<h3>Content Filtering</h3>
<p>Three filter categories help you narrow down content:</p>

<table class=""min-w-full divide-y divide-gray-200 dark:divide-gray-700 my-4"">
    <thead>
        <tr>
            <th class=""px-4 py-2 text-left"">Filter</th>
            <th class=""px-4 py-2 text-left"">Description</th>
        </tr>
    </thead>
    <tbody>
        <tr><td class=""px-4 py-2""><strong>Content Type</strong></td><td class=""px-4 py-2"">Select from types in the current list (multiple selections supported)</td></tr>
        <tr><td class=""px-4 py-2""><strong>Status</strong></td><td class=""px-4 py-2"">Filter by content status (published, draft, etc.)</td></tr>
        <tr><td class=""px-4 py-2""><strong>Language</strong></td><td class=""px-4 py-2"">Multi-language filtering with added language column</td></tr>
    </tbody>
</table>

<p>Server requests are debounced at 200ms per checkbox interaction for optimal performance.</p>

<h3>View Options</h3>

<h4>List View</h4>
<ul>
    <li>Toggle columns on/off</li>
    <li>Name column is required, others optional</li>
    <li>Horizontal scrolling accommodates many columns</li>
    <li>Column order matches selector arrangement</li>
</ul>

<h4>Grid View</h4>
<ul>
    <li>Image and PDF assets display with gray background borders</li>
    <li>Non-media content fills containers without borders</li>
    <li>Selected items show blue border indicators</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "cm-details-editing",
                    ModuleId = "content-manager",
                    Title = "Content Details and In-Context Editing",
                    Summary = "Use the details panel and in-context editing for efficient content management.",
                    Order = 3,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Access content details quickly",
                        "Use in-context editing for faster workflows",
                        "Navigate external content sources"
                    },
                    Content = @"
<h2>Content Details and Editing</h2>
<p>Content Manager provides efficient tools for viewing and editing content without leaving the main interface.</p>

<h3>Content Details Panel</h3>
<p>Access detailed information about any content item through the ""View details"" option in each item's menu:</p>
<ul>
    <li><strong>Asset thumbnail</strong> - Preview image or placeholder icon</li>
    <li><strong>Content title and icon</strong> - Quick identification</li>
    <li><strong>Type-specific properties</strong> - Relevant metadata for the content type</li>
    <li><strong>Edit option</strong> - Quick link to edit (external link for non-CMS content)</li>
</ul>
<p>The details panel is available in Grid view only and appears as a right-side drawer.</p>

<h3>In-Context Editing</h3>
<p>Click <strong>Edit</strong> to open an iframe-based dialog for editing content:</p>
<ul>
    <li>The iframe preloads for speed</li>
    <li>Persists when closed rather than being destroyed</li>
    <li>Improves subsequent access performance</li>
</ul>

<h3>Page Creation</h3>
<p>Create new pages directly from Content Manager:</p>
<ul>
    <li>Blueprint selector modal with contextual templates</li>
    <li>Page tree navigation with search functionality</li>
    <li>Permission-based access control</li>
    <li>Warning for nodes exceeding 10,000 direct children</li>
    <li>Auto-redirect to Edit UI after creation</li>
</ul>

<h3>Content Source Navigation</h3>
<p>The left-side panel displays available content sources:</p>
<ul>
    <li>CMS content appears by default</li>
    <li>External sources appear when configured</li>
    <li>External sources show ""shadow"" content types from Graph (e.g., <code>cmp_PublicImageAsset</code>)</li>
    <li>Filtering is not available for external source lists</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "cm-image-selector",
                    ModuleId = "content-manager",
                    Title = "Image Asset Selection",
                    Summary = "Use the streamlined image selector within Visual Builder.",
                    Order = 4,
                    EstimatedMinutes = 6,
                    LearningObjectives = new List<string>
                    {
                        "Use the focused image-only browser",
                        "Integrate with Optimizely CMP",
                        "Replace images efficiently"
                    },
                    Content = @"
<h2>Image Asset Selection</h2>
<p>Visual Builder includes a streamlined content selector specifically designed for image selection.</p>

<h3>Key Features</h3>
<ul>
    <li><strong>Focused browsing</strong> - Image-only view filters out non-image content</li>
    <li><strong>CMP integration</strong> - Access Optimizely CMP assets if available</li>
    <li><strong>Single image support</strong> - Designed for single image references</li>
    <li><strong>Previous image highlighting</strong> - Current image is highlighted during replacement</li>
</ul>

<h3>Streamlined Interface</h3>
<p>The image selector is intentionally simplified:</p>
<ul>
    <li>No multi-select functionality</li>
    <li>No create/upload buttons within the selector</li>
    <li>Quick selection and replacement workflow</li>
</ul>

<h3>Usage</h3>
<ol>
    <li>Click on an image property field in Visual Builder</li>
    <li>The image selector opens, showing available images</li>
    <li>If replacing, the current image is highlighted</li>
    <li>Select the new image</li>
    <li>The selector closes and the property updates</li>
</ol>

<div class=""bg-blue-50 dark:bg-blue-900 border-l-4 border-blue-400 p-4 my-4"">
    <p class=""text-blue-800 dark:text-blue-100""><strong>Tip:</strong> Upload new images through the standard media management interface before using them in Visual Builder.</p>
</div>
"
                }
            }
        };
    }

    #endregion

    #region Module 4: Edit Experience

    private LearningModule BuildEditExperienceModule()
    {
        return new LearningModule
        {
            Id = "edit-experience",
            Title = "Edit Experience Improvements",
            Description = "Explore the enhanced editing capabilities in CMS 13.",
            Icon = "pencil-square",
            Order = 4,
            Difficulty = ModuleDifficulty.Intermediate,
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "ed-visual-builder-default",
                    ModuleId = "edit-experience",
                    Title = "Visual Builder as Default",
                    Summary = "Understand the shift to Visual Builder as the primary editing interface.",
                    Order = 1,
                    EstimatedMinutes = 6,
                    LearningObjectives = new List<string>
                    {
                        "Understand why On-Page Edit is disabled",
                        "Learn the benefits of Visual Builder editing",
                        "Configure content types for Visual Builder"
                    },
                    Content = @"
<h2>Visual Builder as Default Editor</h2>
<p>In CMS 13 pre-release, <strong>On-Page Edit (OPE) is disabled</strong> in favor of Visual Builder. This represents a significant shift in the editing paradigm.</p>

<h3>Why This Change?</h3>
<p>Visual Builder provides a more consistent and powerful editing experience:</p>
<ul>
    <li><strong>Unified interface</strong> - All editing happens in one place</li>
    <li><strong>Layout control</strong> - Direct manipulation of page structure</li>
    <li><strong>Preview integration</strong> - Real-time preview as you edit</li>
    <li><strong>Mobile-friendly</strong> - Better support for responsive editing</li>
</ul>

<h3>Content Type Requirements</h3>
<p>To use Visual Builder's full feature set, your content types need to be configured as experiences. Non-experience content can still be edited but without dynamic layout capabilities.</p>

<h3>Interface Simplification</h3>
<p>The toolbar no longer displays redundant View and Preview buttons, as Visual Builder provides integrated preview functionality. This streamlines the interface and reduces confusion about which view to use.</p>

<div class=""bg-yellow-50 dark:bg-yellow-900 border-l-4 border-yellow-400 p-4 my-4"">
    <p class=""text-yellow-800 dark:text-yellow-100""><strong>Migration Note:</strong> If your existing workflows rely heavily on OPE, plan to adapt your editor training and content type configurations for Visual Builder.</p>
</div>
"
                },
                new Lesson
                {
                    Id = "ed-audience-viewing",
                    ModuleId = "edit-experience",
                    Title = "Audience Viewing",
                    Summary = "Preview content as different audience segments.",
                    Order = 2,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Preview pages as specific audience segments",
                        "Understand audience integration with visitor groups",
                        "Access audience viewing through the context menu"
                    },
                    Content = @"
<h2>Audience Viewing</h2>
<p>Content creators can preview pages as specific audience segments to understand how tailored content displays to different visitors.</p>

<h3>How It Works</h3>
<p>This feature reintroduces audience functionality similar to CMS 12's visitor groups:</p>
<ul>
    <li>Available for in-process sites on PaaS with audiences installed</li>
    <li>Accessible through a context menu located next to the Publish button</li>
    <li>Allows switching between different audience perspectives</li>
</ul>

<h3>Use Cases</h3>
<ul>
    <li><strong>Personalization testing</strong> - Verify that personalized content appears correctly</li>
    <li><strong>Segment preview</strong> - See how different user groups experience your content</li>
    <li><strong>QA workflows</strong> - Test audience targeting before publishing</li>
</ul>

<h3>Accessing Audience View</h3>
<ol>
    <li>Navigate to the content you want to preview</li>
    <li>Open the context menu next to the Publish button</li>
    <li>Select the audience segment to preview as</li>
    <li>The page refreshes to show the audience-specific view</li>
</ol>
"
                },
                new Lesson
                {
                    Id = "ed-file-uploads",
                    ModuleId = "edit-experience",
                    Title = "Enhanced File Uploads",
                    Summary = "Understand the improved file upload capabilities and security restrictions.",
                    Order = 3,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Understand increased upload size limits",
                        "Learn about file extension whitelisting",
                        "Handle upload errors effectively"
                    },
                    Content = @"
<h2>Enhanced File Uploads</h2>
<p>CMS 13 includes significant improvements to file upload functionality, including increased limits and better security.</p>

<h3>Increased Size Limits</h3>
<p>Default upload size limits have been increased to align with Microsoft's recommended Kestrel and Azure settings. This allows for larger media assets without custom configuration.</p>

<h3>Improved Error Messages</h3>
<p>When uploads fail, you now receive specific feedback about the issue:</p>
<ul>
    <li>Clear message: ""Upload failed because file exceeded the size limit of X""</li>
    <li>HTTP 413 status for size-related failures</li>
    <li>Actionable information for troubleshooting</li>
</ul>

<h3>Security: File Extension Whitelist</h3>
<p>File upload restrictions now enforce a <strong>whitelist of allowed extensions</strong> for enhanced security:</p>
<ul>
    <li>Applies to asset uploads</li>
    <li>Applies to property editors</li>
    <li>Applies to drag-and-drop functionality (including TinyMCE)</li>
</ul>

<p>Unsupported extensions trigger clear error messages, making it easy to understand why an upload was rejected.</p>

<h3>UI Terminology Changes</h3>
<p>""Blocks"" have been renamed to ""Shared Blocks"" in the user interface to better distinguish them from other content types.</p>

<h3>Additional Information</h3>
<p>Content GUID (Globally Unique Identifier) now displays alongside content ID, making it easier to reference specific content items in code or APIs.</p>
"
                },
                new Lesson
                {
                    Id = "ed-version-gadget",
                    ModuleId = "edit-experience",
                    Title = "Version Gadget and Scheduling",
                    Summary = "Learn about version gadget enhancements and scheduled publishing.",
                    Order = 4,
                    EstimatedMinutes = 6,
                    LearningObjectives = new List<string>
                    {
                        "View scheduled publication dates in the version gadget",
                        "Understand version tracking improvements",
                        "Configure translation support"
                    },
                    Content = @"
<h2>Version Gadget and Scheduling</h2>
<p>The Version gadget in CMS 13 includes enhancements for better visibility into content scheduling and versioning.</p>

<h3>Scheduled Publication Display</h3>
<p>The Version gadget now prominently displays scheduled publication dates:</p>
<ul>
    <li>Label changed from ""Scheduled for publish on"" to ""Scheduled to publish on""</li>
    <li>Clear visual indication of when content will go live</li>
    <li>Helps content teams coordinate publication timing</li>
</ul>

<h3>Version Tracking</h3>
<p>Improvements to version visibility help you understand your content's history:</p>
<ul>
    <li>Clear version numbering</li>
    <li>Publication date tracking</li>
    <li>Author information</li>
</ul>

<h3>Translation Support</h3>
<p>Translation functionality is now configured by default on DXP deployments:</p>
<ul>
    <li>Automatic content translation in deployed projects</li>
    <li>Integration with the languages system</li>
    <li>Streamlined localization workflows</li>
</ul>

<div class=""bg-blue-50 dark:bg-blue-900 border-l-4 border-blue-400 p-4 my-4"">
    <p class=""text-blue-800 dark:text-blue-100""><strong>Tip:</strong> Use scheduled publishing to coordinate content releases across time zones and ensure consistent go-live times for campaigns.</p>
</div>
"
                }
            }
        };
    }

    #endregion

    #region Module 5: Graph Integration

    private LearningModule BuildGraphIntegrationModule()
    {
        return new LearningModule
        {
            Id = "graph-integration",
            Title = "Optimizely Graph Integration",
            Description = "Deep dive into how CMS 13 integrates with Optimizely Graph for content delivery.",
            Icon = "circle-stack",
            Order = 5,
            Difficulty = ModuleDifficulty.Advanced,
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "gi-search-modes",
                    ModuleId = "graph-integration",
                    Title = "Search Modes and Capabilities",
                    Summary = "Explore the different search modes available through the Graph SDK.",
                    Order = 1,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Understand content search vs typed search vs untyped search",
                        "Choose the right search mode for your use case",
                        "Leverage advanced search features"
                    },
                    Content = @"
<h2>Search Modes and Capabilities</h2>
<p>The Graph SDK supports three distinct search approaches, each suited for different scenarios.</p>

<h3>1. Content Search</h3>
<p>Type-safe filtering that loads data through <code>IContentLoader</code>:</p>
<ul>
    <li>Returns <code>IContent</code> instances</li>
    <li>Includes permission handling</li>
    <li>Includes URL resolution</li>
    <li>Best for: CMS-integrated scenarios where you need full content objects</li>
</ul>

<h3>2. Untyped Search</h3>
<p>Accepts type and fragments as strings:</p>
<ul>
    <li>Returns raw <code>JsonElement</code> objects</li>
    <li>Maximum flexibility</li>
    <li>Best for: Dynamic queries or prototyping</li>
</ul>

<h3>3. Typed Search</h3>
<p>Deserializes responses to provided contracts:</p>
<ul>
    <li>Compile-time type safety</li>
    <li>Custom data contracts</li>
    <li>Best for: API responses or when you need specific data shapes</li>
</ul>

<h3>Advanced Search Features</h3>
<p>All search modes support powerful features:</p>
<table class=""min-w-full divide-y divide-gray-200 dark:divide-gray-700 my-4"">
    <thead>
        <tr>
            <th class=""px-4 py-2 text-left"">Feature</th>
            <th class=""px-4 py-2 text-left"">Description</th>
        </tr>
    </thead>
    <tbody>
        <tr><td class=""px-4 py-2"">Full-text search</td><td class=""px-4 py-2"">Search across all content with term highlighting</td></tr>
        <tr><td class=""px-4 py-2"">Filters &amp; operators</td><td class=""px-4 py-2"">Logical connectors and fuzzy matching</td></tr>
        <tr><td class=""px-4 py-2"">Locale support</td><td class=""px-4 py-2"">Filter by language and locale</td></tr>
        <tr><td class=""px-4 py-2"">Sorting</td><td class=""px-4 py-2"">Standard and semantic sorting options</td></tr>
        <tr><td class=""px-4 py-2"">Pagination</td><td class=""px-4 py-2"">Cursor-based pagination for large result sets</td></tr>
        <tr><td class=""px-4 py-2"">Autocomplete</td><td class=""px-4 py-2"">Suggestions as users type</td></tr>
        <tr><td class=""px-4 py-2"">Facets</td><td class=""px-4 py-2"">Aggregations for filtering UI</td></tr>
        <tr><td class=""px-4 py-2"">Boosting</td><td class=""px-4 py-2"">Prioritize certain results</td></tr>
    </tbody>
</table>
"
                },
                new Lesson
                {
                    Id = "gi-security",
                    ModuleId = "graph-integration",
                    Title = "Security and Authorization",
                    Summary = "Implement proper security and authorization with Graph integration.",
                    Order = 2,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Configure single-key and HMAC authentication",
                        "Use FilterForVisitor for authorization",
                        "Respect Do Not Track settings"
                    },
                    Content = @"
<h2>Security and Authorization</h2>
<p>Graph integration in CMS 13 includes robust security features to protect your content.</p>

<h3>Authentication Options</h3>

<h4>Single-Key Authentication</h4>
<p>Simple API key authentication suitable for public content:</p>
<pre class=""bg-gray-100 dark:bg-gray-800 p-4 rounded-lg overflow-x-auto""><code>{
  ""Optimizely"": {
    ""Graph"": {
      ""SingleKey"": ""your-public-single-key""
    }
  }
}</code></pre>

<h4>HMAC Authentication</h4>
<p>More secure option using AppKey and Secret for sensitive operations:</p>
<pre class=""bg-gray-100 dark:bg-gray-800 p-4 rounded-lg overflow-x-auto""><code>{
  ""Optimizely"": {
    ""Graph"": {
      ""AppKey"": ""your-app-key"",
      ""Secret"": ""your-secret""
    }
  }
}</code></pre>

<h3>Authorization</h3>
<p>Use <code>FilterForVisitor</code> to apply content permissions:</p>
<ul>
    <li>Accepts principal argument for user context</li>
    <li>Accepts locale argument for language filtering</li>
    <li>Respects CMS access rights</li>
</ul>

<h3>Privacy and Tracking</h3>
<p>Graph includes integrated tracking that respects user privacy:</p>
<ul>
    <li>Honors ""Do Not Track"" browser settings</li>
    <li>Built-in metrics and observability</li>
    <li>Configurable tracking behavior</li>
</ul>

<h3>Performance Features</h3>
<ul>
    <li><strong>Optional caching layer</strong> - Reduce redundant queries</li>
    <li><strong>Pinned results</strong> - Ensure specific content appears in search</li>
    <li><strong>Optimized synchronization</strong> - Prevents unnecessary uploads of unchanged content</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "gi-external-content",
                    ModuleId = "graph-integration",
                    Title = "External Content Integration",
                    Summary = "Connect external content sources to your CMS through Graph.",
                    Order = 3,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Surface external content types in the CMS UI",
                        "Create content type bindings",
                        "Use Visual Builder with external data sources"
                    },
                    Content = @"
<h2>External Content Integration</h2>
<p>CMS 13 enables connecting external content sources (particularly Optimizely Graph) to the CMS interface.</p>

<h3>Key Capabilities</h3>
<ul>
    <li><strong>Surface external content</strong> - Developers can display specific content types in the CMS UI</li>
    <li><strong>Content reuse</strong> - Editors can use external items when composing pages and blocks</li>
    <li><strong>Shadow types</strong> - External sources create ""shadow"" content types automatically</li>
</ul>

<h3>Content Type Binding</h3>
<p>The system includes APIs for creating bindings between content types:</p>
<ul>
    <li>Map properties between existing types including nested blocks</li>
    <li>Full CRUD operations on bindings</li>
    <li>Import/export capabilities</li>
    <li>UI management within content type editing</li>
</ul>

<h3>Visual Builder Integration</h3>
<p>External content integrates with Visual Builder:</p>
<ul>
    <li>Block property binding with data sources</li>
    <li>Drag external content into compositions</li>
    <li>Consistent editing experience</li>
</ul>

<h3>Global Contract Indexing</h3>
<p>Global contracts enable consistent searching across CMS and external sources:</p>
<ul>
    <li>CMS content is indexed to Graph using Global Contracts</li>
    <li>Experience, page, section, and block instances inherit the <code>Item</code> contract</li>
    <li>Media instances inherit both <code>AssetItem</code> and <code>ImageItem</code> contracts</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "gi-indexing",
                    ModuleId = "graph-integration",
                    Title = "Indexing and Performance",
                    Summary = "Optimize content indexing and understand performance improvements.",
                    Order = 4,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Monitor indexing with enhanced reporting",
                        "Understand optimized synchronization",
                        "Use Smooth Rebuild for maintenance"
                    },
                    Content = @"
<h2>Indexing and Performance</h2>
<p>CMS 13 includes significant improvements to content indexing performance and monitoring.</p>

<h3>Enhanced Indexing Reporting</h3>
<p>The indexing process now provides detailed metrics:</p>
<ul>
    <li>Real-time progress updates</li>
    <li>Content counts and status</li>
    <li>Error tracking and reporting</li>
    <li>Duration metrics</li>
</ul>

<h3>Optimized Synchronization</h3>
<p>Performance improvements for large datasets:</p>
<ul>
    <li><strong>Document hash values</strong> - Prevents unnecessary uploads of unchanged content</li>
    <li><strong>Incremental updates</strong> - Only changed content is re-indexed</li>
    <li><strong>Batch processing</strong> - Efficient handling of large content volumes</li>
</ul>

<h3>Smooth Rebuild</h3>
<p>A new feature for resetting Graph sources without downtime:</p>
<ul>
    <li>Reset CMS Optimizely Graph source while serving live traffic</li>
    <li>No impact during the reset process</li>
    <li>Verify changes before committing</li>
    <li>Option to abandon changes with no impact</li>
</ul>

<h4>Smooth Rebuild Controls</h4>
<ul>
    <li>Creating deployment slots</li>
    <li>Abandoning slots</li>
    <li>Committing slots</li>
    <li>Rebuilding Graph instances</li>
    <li>Progress monitoring</li>
</ul>

<div class=""bg-yellow-50 dark:bg-yellow-900 border-l-4 border-yellow-400 p-4 my-4"">
    <p class=""text-yellow-800 dark:text-yellow-100""><strong>Pre-Release Note:</strong> Smooth Rebuild has not been fully enabled yet and will be available following the Graph team's official release.</p>
</div>
"
                }
            }
        };
    }

    #endregion

    #region Module 6: Languages

    private LearningModule BuildLanguagesModule()
    {
        return new LearningModule
        {
            Id = "languages",
            Title = "Multilingual Enhancements",
            Description = "Master the enhanced multilingual capabilities in CMS 13.",
            Icon = "language",
            Order = 6,
            Difficulty = ModuleDifficulty.Intermediate,
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "lang-fallback",
                    ModuleId = "languages",
                    Title = "Global Fallback Languages",
                    Summary = "Configure global fallback languages for enhanced multi-language support.",
                    Order = 1,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Configure global fallback languages",
                        "Understand recursive fallback evaluation",
                        "Prevent circular fallback configurations"
                    },
                    Content = @"
<h2>Global Fallback Languages</h2>
<p>CMS 13 introduces the ability to configure global fallback languages, enhancing multi-language support for both SaaS and PaaS deployments with Optimizely Graph integration.</p>

<h3>Core Capabilities</h3>
<ul>
    <li><strong>Single fallback per language</strong> - Each language can have one designated fallback</li>
    <li><strong>Recursive evaluation</strong> - Fallbacks chain (e.g., fr-BE → nl-BE → nl)</li>
    <li><strong>Circular prevention</strong> - System validates to prevent circular fallbacks</li>
    <li><strong>Automatic cleanup</strong> - Fallback references are cleaned when languages are removed</li>
</ul>

<h3>Example Configuration</h3>
<p>Consider this fallback chain:</p>
<pre class=""bg-gray-100 dark:bg-gray-800 p-4 rounded-lg overflow-x-auto""><code>French (Belgium) → Dutch (Belgium) → Dutch
fr-BE            → nl-BE           → nl</code></pre>

<p>If content doesn't exist in <code>fr-BE</code>, the system looks for <code>nl-BE</code>. If that also doesn't exist, it falls back to <code>nl</code>.</p>

<h3>Admin UI Configuration</h3>
<p>Administrators can configure fallbacks through the admin interface:</p>
<ul>
    <li>Apply global fallback settings with user notifications</li>
    <li>Trigger re-indexing of affected content to Optimizely Graph</li>
    <li>Visualize content node configurations for re-indexing</li>
</ul>

<h3>Graph Indexing</h3>
<p>When content is published or deleted, the system manages fallback variants in Graph:</p>
<ul>
    <li>Indexes instances and fallback variants together</li>
    <li>Sets <code>locale</code> to the content instance's locale</li>
    <li>Indexes new fallback versions before deleting old ones (prevents data loss)</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "lang-translation",
                    ModuleId = "languages",
                    Title = "Translation Workflows",
                    Summary = "Use enhanced translation capabilities for efficient localization.",
                    Order = 2,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Duplicate content with structures and layouts",
                        "Use auto-translation for quick localization",
                        "Export content for external translation providers"
                    },
                    Content = @"
<h2>Translation Workflows</h2>
<p>CMS 13 provides enhanced tools for efficient content localization.</p>

<h3>Content Duplication Options</h3>

<h4>Full Content Copy</h4>
<p>Copy source content with all structures and layouts intact:</p>
<ul>
    <li>Preserves all content values</li>
    <li>Maintains layout structure</li>
    <li>Ideal for starting translations with existing content as reference</li>
</ul>

<h4>Structure-Only Copy</h4>
<p>Preserve layouts while removing content values:</p>
<ul>
    <li>Maintains page structure</li>
    <li>Clears translatable content fields</li>
    <li>Ready for fresh translation input</li>
</ul>

<h3>Auto-Translation</h3>
<p>Machine-translate content while maintaining structure:</p>
<ul>
    <li>Controlled by feature flag (disabled by default)</li>
    <li>Automatic language variant creation</li>
    <li>Structure preservation during translation</li>
    <li>Automatic context switching post-translation</li>
    <li>Error handling prevents version creation on failures</li>
    <li>Draft content can be translated without prior publication</li>
</ul>

<h3>JSON Export</h3>
<p>Export translatable content for external translation providers:</p>
<ul>
    <li>Standardized JSON format</li>
    <li>Includes all translatable fields</li>
    <li>Compatible with translation management systems</li>
</ul>

<h3>Translation Initiation Points</h3>
<p>Start translations from multiple locations:</p>
<ul>
    <li>Page tree context menus</li>
    <li>Toolbar's yellow ribbon notifications</li>
    <li>Language Selector dropdown</li>
</ul>
<p>The dialog auto-completes source/target languages based on context.</p>
"
                },
                new Lesson
                {
                    Id = "lang-context",
                    ModuleId = "languages",
                    Title = "Language Context Switching",
                    Summary = "Navigate languages efficiently with improved context switching.",
                    Order = 3,
                    EstimatedMinutes = 6,
                    LearningObjectives = new List<string>
                    {
                        "Use global language context switching",
                        "Understand dynamic UI updates",
                        "Navigate multi-language content efficiently"
                    },
                    Content = @"
<h2>Language Context Switching</h2>
<p>The CMS 13 UI dynamically reflects content availability based on the selected language.</p>

<h3>Dynamic UI Updates</h3>
<p>When you switch languages, the interface updates to show:</p>
<ul>
    <li>Content available in the selected language</li>
    <li>Visual indicators for missing translations</li>
    <li>Appropriate creation options based on language permissions</li>
</ul>

<h3>Content Availability</h3>
<p>The system prevents creation in languages not enabled for specific content items, ensuring you only work with valid language configurations.</p>

<h3>Improved Language Dropdown</h3>
<p>The language selector includes usability enhancements:</p>
<ul>
    <li><strong>Search functionality</strong> - Quickly find languages in large lists</li>
    <li><strong>Scrollbar support</strong> - Navigate long language lists easily</li>
    <li><strong>Clear visual hierarchy</strong> - Primary vs. fallback languages</li>
</ul>

<h3>Best Practices</h3>
<ul>
    <li>Set a primary language as your default view</li>
    <li>Use search to quickly switch between distant languages</li>
    <li>Pay attention to visual indicators for missing translations</li>
    <li>Use the yellow ribbon notifications to identify translation needs</li>
</ul>
"
                }
            }
        };
    }

    #endregion

    #region Module 7: Content Variations

    private LearningModule BuildContentVariationsModule()
    {
        return new LearningModule
        {
            Id = "content-variations",
            Title = "Content Variations",
            Description = "Learn how to create and manage multiple content variations for experimentation.",
            Icon = "document-duplicate",
            Order = 7,
            Difficulty = ModuleDifficulty.Advanced,
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "cv-overview",
                    ModuleId = "content-variations",
                    Title = "Understanding Content Variations",
                    Summary = "Learn about the content variations feature for experimentation and personalization.",
                    Order = 1,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Understand the purpose of content variations",
                        "Learn about delta-based storage architecture",
                        "Manage variations through the API and UI"
                    },
                    Content = @"
<h2>Understanding Content Variations</h2>
<p>Content Variations enables multiple published versions of the same content item within a single language. This is essential for experimentation and personalization.</p>

<h3>Key Capabilities</h3>

<h4>API Management</h4>
<ul>
    <li>List, create, and delete content variations</li>
    <li>Save draft changes and publish/unpublish variations</li>
    <li>Promote variations as the default published version</li>
</ul>

<h4>User Experience</h4>
<ul>
    <li>View and switch between variations in Edit view</li>
    <li>Create new variations from existing content or as empty versions</li>
    <li>Update and preview variations during editing</li>
    <li>Auto-save functionality applies to variations</li>
</ul>

<h3>Delta-Based Storage</h3>
<p>Variations use an efficient <strong>delta-based storage architecture</strong>:</p>
<ul>
    <li><strong>Initial state</strong> - Variations contain no property data initially</li>
    <li><strong>Only changes stored</strong> - Only explicitly modified fields are tracked</li>
    <li><strong>Complex properties</strong> - When modifying values within complex properties, the entire property data is copied</li>
</ul>

<h3>Independent Lifecycle</h3>
<p>Each variation has its own:</p>
<ul>
    <li>Language association</li>
    <li>Version history</li>
    <li>Publishing schedule</li>
</ul>
<p>Variations can be published independently from their source content, provided the source is published.</p>

<h3>Unique Identifiers</h3>
<p>Each variation receives a unique identifier following this format:</p>
<pre class=""bg-gray-100 dark:bg-gray-800 p-4 rounded-lg overflow-x-auto""><code>Guid_Status_Language_VariantKey</code></pre>
"
                },
                new Lesson
                {
                    Id = "cv-working",
                    ModuleId = "content-variations",
                    Title = "Working with Variations",
                    Summary = "Create, edit, and promote content variations in practice.",
                    Order = 2,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Create new content variations",
                        "Switch between variations while editing",
                        "Promote successful variations to the original"
                    },
                    Content = @"
<h2>Working with Variations</h2>
<p>Learn the practical workflows for managing content variations in CMS 13.</p>

<h3>Creating Variations</h3>
<p>You can create new variations in two ways:</p>
<ol>
    <li><strong>From existing content</strong> - Clone the current content as a starting point</li>
    <li><strong>Empty version</strong> - Start fresh with only the base structure</li>
</ol>

<h3>Editing Variations</h3>
<p>When editing variations:</p>
<ul>
    <li>Select the variation from the variation selector in Edit view</li>
    <li>Make changes to the content</li>
    <li>Auto-save preserves your work automatically</li>
    <li>Preview changes before publishing</li>
</ul>

<h3>Publishing Variations</h3>
<p>Variations can be published independently:</p>
<ul>
    <li>The source content must be published first</li>
    <li>Each variation has its own publication status</li>
    <li>Scheduled publishing is supported for variations</li>
</ul>

<h3>Promoting Variations</h3>
<p>When a variation proves successful, you can promote it using <strong>""Copy changes to Original""</strong>:</p>
<ul>
    <li>Merges variation modifications back into the primary content</li>
    <li>Creates a draft of the original if needed</li>
    <li>Preserves the original variation for reference</li>
</ul>

<h3>Graph Integration</h3>
<p>The system indexes all content variations to Optimizely Graph:</p>
<ul>
    <li>Includes unpublished drafts</li>
    <li>Enables use in previews and experiments</li>
    <li>Each variation is uniquely identifiable in queries</li>
</ul>

<div class=""bg-yellow-50 dark:bg-yellow-900 border-l-4 border-yellow-400 p-4 my-4"">
    <p class=""text-yellow-800 dark:text-yellow-100""><strong>Current Limitations:</strong></p>
    <ul class=""text-yellow-800 dark:text-yellow-100 mt-2"">
        <li>Only localizable properties are supported</li>
        <li>Softlinks aren't generated for published variations</li>
    </ul>
</div>
"
                }
            }
        };
    }

    #endregion

    #region Module 8: Framework Changes

    private LearningModule BuildFrameworkModule()
    {
        return new LearningModule
        {
            Id = "framework",
            Title = "Framework and Infrastructure",
            Description = "Understand the infrastructure changes and .NET 10 upgrade in CMS 13.",
            Icon = "cog",
            Order = 8,
            Difficulty = ModuleDifficulty.Advanced,
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "fw-net10",
                    ModuleId = "framework",
                    Title = ".NET 10 Runtime",
                    Summary = "Understand the upgrade to .NET 10 and its implications.",
                    Order = 1,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Understand the .NET 10 upgrade",
                        "Plan your migration path",
                        "Leverage new .NET features"
                    },
                    Content = @"
<h2>.NET 10 Runtime</h2>
<p>CMS 13 now operates on the <strong>.NET 10 runtime</strong>, representing a significant platform modernization.</p>

<h3>Why .NET 10?</h3>
<p>.NET 10 brings several benefits:</p>
<ul>
    <li><strong>Performance improvements</strong> - Faster startup and runtime performance</li>
    <li><strong>Security updates</strong> - Latest security patches and features</li>
    <li><strong>Modern language features</strong> - C# 13+ capabilities</li>
    <li><strong>Long-term support</strong> - Extended support timeline</li>
</ul>

<h3>Migration Considerations</h3>
<p>When upgrading from CMS 12 to CMS 13, consider:</p>
<ul>
    <li>Update your project target framework to .NET 10</li>
    <li>Review NuGet package compatibility</li>
    <li>Test third-party integrations</li>
    <li>Update CI/CD pipelines</li>
</ul>

<h3>Project File Updates</h3>
<pre class=""bg-gray-100 dark:bg-gray-800 p-4 rounded-lg overflow-x-auto""><code>&lt;Project Sdk=""Microsoft.NET.Sdk.Web""&gt;
  &lt;PropertyGroup&gt;
    &lt;TargetFramework&gt;net10.0&lt;/TargetFramework&gt;
    &lt;Nullable&gt;enable&lt;/Nullable&gt;
    &lt;ImplicitUsings&gt;enable&lt;/ImplicitUsings&gt;
  &lt;/PropertyGroup&gt;
&lt;/Project&gt;</code></pre>

<div class=""bg-blue-50 dark:bg-blue-900 border-l-4 border-blue-400 p-4 my-4"">
    <p class=""text-blue-800 dark:text-blue-100""><strong>Tip:</strong> Review the .NET 10 breaking changes documentation before upgrading to identify any code changes needed.</p>
</div>
"
                },
                new Lesson
                {
                    Id = "fw-admin-changes",
                    ModuleId = "framework",
                    Title = "Admin Interface Changes",
                    Summary = "Learn about changes to the administrative interface.",
                    Order = 2,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Navigate the relocated cloud license management",
                        "Understand the Plugin Manager removal",
                        "Adapt to new scheduled job patterns"
                    },
                    Content = @"
<h2>Admin Interface Changes</h2>
<p>CMS 13 includes several changes to the administrative interface.</p>

<h3>Cloud License Management</h3>
<p>The cloud license management interface has been relocated:</p>
<ul>
    <li><strong>Old location:</strong> Settings > Manage Websites > Cloud License</li>
    <li><strong>New location:</strong> Settings > Cloud License (dedicated menu item)</li>
</ul>

<p>The new interface is under the ""Admin (Framework)"" section and replicates previous functionality while adding quality-of-life improvements.</p>

<h3>Plugin Manager Removal</h3>
<p>The Plugin Manager has been discontinued:</p>
<ul>
    <li>The Plugin Manager UI and backend components have been removed</li>
    <li>The feature was underutilized and has been eliminated</li>
    <li>Scheduled jobs no longer depend on the obsoleted <code>EPiServer.PlugIn</code> system</li>
    <li>Custom property types no longer depend on the plugin system</li>
</ul>

<h3>Implications for Existing Code</h3>
<p>If you have code that relies on the plugin system:</p>
<ul>
    <li>Review scheduled jobs for plugin dependencies</li>
    <li>Update custom property type registrations</li>
    <li>Consider modern dependency injection patterns instead</li>
</ul>

<div class=""bg-yellow-50 dark:bg-yellow-900 border-l-4 border-yellow-400 p-4 my-4"">
    <p class=""text-yellow-800 dark:text-yellow-100""><strong>Migration Note:</strong> If you have custom scheduled jobs using the <code>[ScheduledPlugIn]</code> attribute, you'll need to update them to use the modern registration pattern.</p>
</div>
"
                },
                new Lesson
                {
                    Id = "fw-import-export",
                    ModuleId = "framework",
                    Title = "Import and Export Improvements",
                    Summary = "Explore the enhanced import and export capabilities for large sites.",
                    Order = 3,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Handle exports exceeding 2GB",
                        "Monitor export progress with client-side polling",
                        "Identify and resolve import failures"
                    },
                    Content = @"
<h2>Import and Export Improvements</h2>
<p>CMS 13 includes enhanced capabilities for handling large site exports and imports.</p>

<h3>Large File Support</h3>
<p>The system now handles files exceeding 2GB:</p>
<ul>
    <li>Surpasses previous file size limitations</li>
    <li>Enables full-site exports for large content repositories</li>
    <li>Background processing for long operations</li>
</ul>

<h3>Background Processing</h3>
<p>Export operations are no longer dependent on a single continuous request:</p>
<ul>
    <li>Works around Azure's 230-second timeout constraint</li>
    <li>Client-side polling monitors export progress</li>
    <li>Updated UI labels reflect operation status (e.g., ""Upload is in progress"")</li>
</ul>

<h3>Improved Error Visibility</h3>
<p>Import error handling has been enhanced:</p>
<ul>
    <li>Error messages remain visible on the page after import</li>
    <li>Administrators can identify and address import failures quickly</li>
    <li>Clear indication of which items failed and why</li>
</ul>

<h3>Compatibility</h3>
<p>The baseline import functionality carries forward from CMS 12, ensuring:</p>
<ul>
    <li>Export packages from CMS 12 can be imported (check migration guide for specifics)</li>
    <li>Familiar workflows for content migration</li>
    <li>Same core functionality with enhanced reliability</li>
</ul>

<div class=""bg-blue-50 dark:bg-blue-900 border-l-4 border-blue-400 p-4 my-4"">
    <p class=""text-blue-800 dark:text-blue-100""><strong>Tip:</strong> For very large exports, monitor the progress indicators and allow sufficient time for background processing to complete.</p>
</div>
"
                }
            }
        };
    }

    #endregion
}
