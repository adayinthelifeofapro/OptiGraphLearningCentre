using OptimizelyLearningCentre.Client.Models.Learning;
using OptimizelyLearningCentre.Client.Services;

namespace OptimizelyLearningCentre.Client.Courses.DXP;

/// <summary>
/// Content provider for the Optimizely DXP (Digital Experience Platform) course
/// </summary>
public class DXPContentProvider : ILearningContentProvider
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
            BuildArchitectureModule(),
            BuildEnvironmentsModule(),
            BuildDeploymentModule(),
            BuildMonitoringModule(),
            BuildSecurityModule(),
            BuildBestPracticesModule(),
            BuildTroubleshootingModule()
        };
    }

    #region Module 1: Getting Started with DXP

    private LearningModule BuildGettingStartedModule()
    {
        return new LearningModule
        {
            Id = "getting-started",
            Title = "Getting Started with DXP",
            Description = "Learn the fundamentals of Optimizely's Digital Experience Platform and cloud hosting.",
            Icon = "academic-cap",
            Order = 1,
            Difficulty = ModuleDifficulty.Beginner,
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "gs-what-is-dxp",
                    ModuleId = "getting-started",
                    Title = "What is Optimizely DXP?",
                    Summary = "Discover Optimizely DXP and how it provides enterprise cloud hosting for digital experiences.",
                    Order = 1,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Understand what Optimizely DXP is and its purpose",
                        "Learn the benefits of cloud-hosted digital experiences",
                        "Know what products can run on DXP"
                    },
                    Content = @"
<h2>Introduction to Optimizely DXP</h2>
<p>Optimizely Digital Experience Platform (DXP) is an <strong>end-to-end, full-stack digital platform service package</strong> that includes content management, digital marketing, enterprise search, and digital commerce in a single cloud service.</p>

<h3>What Does DXP Provide?</h3>
<p>DXP is Optimizely's Platform-as-a-Service (PaaS) offering built on Microsoft Azure. It handles the infrastructure complexity so you can focus on building great digital experiences:</p>
<ul>
    <li><strong>High availability</strong> - Enterprise-grade uptime and reliability</li>
    <li><strong>Performance optimization</strong> - CDN, caching, and auto-scaling</li>
    <li><strong>Easy connectivity</strong> - Integration with Azure services and existing systems</li>
    <li><strong>Elastic scaling</strong> - Handle traffic spikes seamlessly</li>
    <li><strong>Managed updates</strong> - Platform updates handled by Optimizely</li>
</ul>

<h3>Products Supported on DXP</h3>
<p>DXP can host multiple Optimizely products:</p>
<table class=""min-w-full divide-y divide-gray-200 dark:divide-gray-700 my-4"">
    <thead>
        <tr>
            <th class=""px-4 py-2 text-left"">Product</th>
            <th class=""px-4 py-2 text-left"">Description</th>
        </tr>
    </thead>
    <tbody>
        <tr><td class=""px-4 py-2"">Optimizely CMS</td><td class=""px-4 py-2"">Content management system (CMS 12+)</td></tr>
        <tr><td class=""px-4 py-2"">Commerce Connect</td><td class=""px-4 py-2"">E-commerce and catalog management</td></tr>
        <tr><td class=""px-4 py-2"">Search & Navigation</td><td class=""px-4 py-2"">Enterprise search powered by Elasticsearch</td></tr>
        <tr><td class=""px-4 py-2"">Marketing Automation</td><td class=""px-4 py-2"">Campaign and visitor tracking connectors</td></tr>
    </tbody>
</table>

<h3>Cloud-First Approach</h3>
<p>DXP takes a cloud-first approach, meaning:</p>
<ul>
    <li>Infrastructure is fully managed by Optimizely</li>
    <li>You deploy code packages, not manage servers</li>
    <li>Scaling, backups, and monitoring are built-in</li>
    <li>Security patches and platform updates are automatic</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "gs-requirements",
                    ModuleId = "getting-started",
                    Title = "DXP Requirements",
                    Summary = "Understand the technical requirements and prerequisites for DXP development.",
                    Order = 2,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Know the required software versions",
                        "Understand supported and unsupported features",
                        "Prepare your development environment"
                    },
                    Content = @"
<h2>DXP Technical Requirements</h2>
<p>Before developing for DXP, ensure your environment meets the required specifications.</p>

<h3>Required Software Versions</h3>
<h4>Optimizely Packages (Minimum)</h4>
<table class=""min-w-full divide-y divide-gray-200 dark:divide-gray-700 my-4"">
    <thead>
        <tr>
            <th class=""px-4 py-2 text-left"">Package</th>
            <th class=""px-4 py-2 text-left"">Minimum Version</th>
        </tr>
    </thead>
    <tbody>
        <tr><td class=""px-4 py-2"">EPiServer.CMS.Core</td><td class=""px-4 py-2"">12.0.3</td></tr>
        <tr><td class=""px-4 py-2"">EPiServer.CMS.UI.Core</td><td class=""px-4 py-2"">12.0.2</td></tr>
        <tr><td class=""px-4 py-2"">EPiServer.Find</td><td class=""px-4 py-2"">14.0.1</td></tr>
        <tr><td class=""px-4 py-2"">EPiServer.Commerce</td><td class=""px-4 py-2"">1.1.1</td></tr>
        <tr><td class=""px-4 py-2"">EPiServer.CloudPlatform.Cms</td><td class=""px-4 py-2"">1.0.1</td></tr>
    </tbody>
</table>

<h4>Development Tools</h4>
<ul>
    <li><strong>Visual Studio</strong> - 2019 or later (2022 recommended)</li>
    <li><strong>SQL Server</strong> - Version 16</li>
    <li><strong>SQL Server Management Studio</strong> - 16.x (2022)</li>
    <li><strong>.NET</strong> - .NET 6 or later</li>
</ul>

<h3>Key Constraints</h3>
<div class=""bg-yellow-50 dark:bg-yellow-900/20 border-l-4 border-yellow-400 p-4 my-4"">
    <p class=""text-yellow-800 dark:text-yellow-200""><strong>Important:</strong> You can run only <strong>one Web Application</strong> (ASP.NET Web Application project) in your DXP solution. However, you can have unlimited number of <strong>sites</strong> in that application.</p>
</div>

<h3>Unsupported Features</h3>
<p>The following features are <strong>not supported</strong> on DXP:</p>
<ul>
    <li>Optimizely Mirroring</li>
    <li>Windows Workflow Foundation (WF)</li>
    <li>Optimizely Search (default search) - use Search & Navigation instead</li>
    <li>Solr Search Provider</li>
    <li>Legacy products: CMO, Mail, Relate</li>
</ul>

<h3>Supported Add-ons</h3>
<ul>
    <li>Optimizely Languages</li>
    <li>Marketing Automation connectors (HubSpot, Salesforce, Marketo, Dynamics CRM, and more)</li>
</ul>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "gs-cloud-package",
                            Title = "Cloud Platform Package",
                            Description = "Required NuGet package for DXP deployment.",
                            Type = ExampleType.Code,
                            ExampleContent = @"<!-- Add to your .csproj file -->
<PackageReference Include=""EPiServer.CloudPlatform.Cms"" Version=""1.0.1"" />

<!-- For Commerce projects, also add: -->
<PackageReference Include=""EPiServer.CloudPlatform.Commerce"" Version=""1.0.1"" />",
                            SampleResponse = @"The EPiServer.CloudPlatform packages configure your application
for proper operation in the DXP environment, including:
- Application Insights integration
- Azure Blob Storage configuration
- Proper logging redirection
- Environment-specific settings",
                            Hints = new List<string>
                            {
                                "Always use the latest stable version of CloudPlatform packages",
                                "CMS projects only need the Cms package, Commerce projects need both"
                            }
                        }
                    }
                },
                new Lesson
                {
                    Id = "gs-onboarding",
                    ModuleId = "getting-started",
                    Title = "Onboarding Process",
                    Summary = "Learn about the DXP onboarding process and initial setup steps.",
                    Order = 3,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Understand the onboarding workflow",
                        "Know what information you'll receive",
                        "Learn the initial configuration steps"
                    },
                    Content = @"
<h2>DXP Onboarding Process</h2>
<p>When your DXP subscription is activated, you'll go through a structured onboarding process.</p>

<h3>Step 1: Welcome Email</h3>
<p>Upon order activation, your technical contact receives an email containing:</p>
<ul>
    <li>Default URLs for all provisioned environments</li>
    <li>Optimizely support contact information</li>
    <li>Access to the DXP Management Portal</li>
    <li>Links to documentation and resources</li>
</ul>

<h3>Step 2: Initial Configuration</h3>
<p>The setup involves three key actions:</p>
<ol>
    <li><strong>Install packages</strong> - Add required NuGet packages and configure Opti ID authentication</li>
    <li><strong>Configure settings</strong> - Add solution-specific configurations and establish authentication options</li>
    <li><strong>Enable Opti ID</strong> - Configure identity management in the Opti ID Admin Center</li>
</ol>

<h3>Step 3: Environment Access</h3>
<p>You'll have access to three environments:</p>
<ul>
    <li><strong>Integration</strong> - For development and continuous integration</li>
    <li><strong>Preproduction</strong> - For staging and testing</li>
    <li><strong>Production</strong> - For live site operation</li>
</ul>

<h3>Cloud Accelerator Package</h3>
<p>The Cloud Accelerator Package provides comprehensive onboarding support:</p>
<ul>
    <li><strong>Technical Overview</strong> - Kickoff meeting with Customer Success</li>
    <li><strong>Training</strong> - One-day course with two seats</li>
    <li><strong>Load Testing</strong> - Access to performance testing tools</li>
    <li><strong>Go Live Certification</strong> - Pre-production checklist</li>
    <li><strong>Go Live Support</strong> - Ongoing assistance from Customer Success</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "gs-portal-access",
                    ModuleId = "getting-started",
                    Title = "DXP Management Portal",
                    Summary = "Learn to navigate and use the DXP Management Portal for self-service operations.",
                    Order = 4,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Navigate the DXP Management Portal",
                        "Understand available self-service features",
                        "Know how to access support resources"
                    },
                    Content = @"
<h2>DXP Management Portal</h2>
<p>The DXP Management Portal (also known as PaaS Portal) provides self-service capabilities for managing your DXP environments.</p>

<h3>Accessing the Portal</h3>
<p>You need an <strong>Optimizely cloud account</strong> to access the self-service capabilities. Your account is created during onboarding.</p>

<h3>Portal Features</h3>

<h4>Deployment Management</h4>
<ul>
    <li>One-click deployment to environments</li>
    <li>View deployment history and status</li>
    <li>Configuration transforms</li>
    <li>Deployment validation and smoke testing</li>
</ul>

<h4>Environment Management</h4>
<ul>
    <li>Restart web applications</li>
    <li>Manage app settings and connection strings</li>
    <li>Export databases to backup files</li>
    <li>Synchronize content between environments</li>
</ul>

<h4>Monitoring & Usage</h4>
<ul>
    <li>Page views (monthly and year-to-date)</li>
    <li>Environment availability metrics</li>
    <li>Resource consumption tracking</li>
</ul>

<h4>Portal Menu Options</h4>
<table class=""min-w-full divide-y divide-gray-200 dark:divide-gray-700 my-4"">
    <thead>
        <tr>
            <th class=""px-4 py-2 text-left"">Menu Item</th>
            <th class=""px-4 py-2 text-left"">Purpose</th>
        </tr>
    </thead>
    <tbody>
        <tr><td class=""px-4 py-2"">Management Portal</td><td class=""px-4 py-2"">Deployment and configuration tools</td></tr>
        <tr><td class=""px-4 py-2"">Forums</td><td class=""px-4 py-2"">Optimizely World developer community</td></tr>
        <tr><td class=""px-4 py-2"">Support</td><td class=""px-4 py-2"">Help center and knowledge base</td></tr>
        <tr><td class=""px-4 py-2"">Trust Center</td><td class=""px-4 py-2"">Security documentation</td></tr>
        <tr><td class=""px-4 py-2"">Service Health</td><td class=""px-4 py-2"">Operational status overview</td></tr>
    </tbody>
</table>

<h3>Self-Service Operations</h3>
<p>Key operations you can perform without contacting support:</p>
<ul>
    <li>Deploy code packages</li>
    <li>Purge CDN cache</li>
    <li>Restart applications</li>
    <li>Export/import databases</li>
    <li>Configure maintenance pages</li>
    <li>View logs and metrics</li>
</ul>
"
                }
            }
        };
    }

    #endregion

    #region Module 2: DXP Architecture

    private LearningModule BuildArchitectureModule()
    {
        return new LearningModule
        {
            Id = "architecture",
            Title = "DXP Architecture",
            Description = "Understand the Azure-based architecture that powers Optimizely DXP.",
            Icon = "server-stack",
            Order = 2,
            Difficulty = ModuleDifficulty.Beginner,
            Prerequisites = new[] { "getting-started" },
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "arch-azure-foundation",
                    ModuleId = "architecture",
                    Title = "Azure Foundation",
                    Summary = "Learn about the Microsoft Azure services that power DXP.",
                    Order = 1,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Identify the Azure services used by DXP",
                        "Understand how services work together",
                        "Know the benefits of Azure-based hosting"
                    },
                    Content = @"
<h2>Azure Foundation</h2>
<p>Optimizely DXP is built on Microsoft Azure infrastructure, leveraging enterprise-grade cloud services for reliability and performance.</p>

<h3>Core Azure Services</h3>

<h4>Azure Web Apps</h4>
<p>Your Optimizely solution runs as an <strong>Azure Web App</strong>, which provides:</p>
<ul>
    <li>Managed hosting environment</li>
    <li>Automatic scaling (Premium V3 App Service Plans)</li>
    <li>Built-in load balancing</li>
    <li>SSL certificate management</li>
    <li>Deployment slots for zero-downtime updates</li>
</ul>

<h4>Azure SQL Database</h4>
<p>Content and configuration data is stored in <strong>Azure SQL Database</strong>:</p>
<ul>
    <li>Fully managed relational database</li>
    <li>Automatic backups (up to 35-day point-in-time restore)</li>
    <li>High availability with geo-redundancy options</li>
    <li>Performance monitoring and tuning recommendations</li>
</ul>

<h4>Azure Blob Storage</h4>
<p>Media files and assets are stored in <strong>Azure Blob Storage</strong>:</p>
<ul>
    <li>Scalable object storage</li>
    <li>CDN integration for fast delivery</li>
    <li>Redundant storage for durability</li>
    <li>Direct access from your application</li>
</ul>

<h4>Content Delivery Network (CDN)</h4>
<p>DXP includes <strong>Cloudflare CDN</strong> for:</p>
<ul>
    <li>Global content distribution</li>
    <li>DDoS protection</li>
    <li>SSL/TLS encryption</li>
    <li>Edge caching</li>
    <li>Web Application Firewall (WAF)</li>
</ul>

<h3>Architecture Diagram</h3>
<div class=""bg-gray-100 dark:bg-gray-800 p-4 rounded-lg my-4 font-mono text-sm"">
<pre>
Internet
    |
[Cloudflare CDN] - Caching, DDoS protection, WAF
    |
[Azure Web App] - Your Optimizely application
    |
    +-- [Azure SQL] - Content and config database
    +-- [Azure Blob] - Media and asset storage
    +-- [Search & Navigation] - Elasticsearch index
</pre>
</div>
"
                },
                new Lesson
                {
                    Id = "arch-web-app-structure",
                    ModuleId = "architecture",
                    Title = "Web App Structure",
                    Summary = "Understand how DXP web apps are structured and configured.",
                    Order = 2,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Understand web app components",
                        "Know the single web app constraint",
                        "Learn about multi-site configurations"
                    },
                    Content = @"
<h2>Web App Structure</h2>
<p>A DXP web app is a complete solution built on the Optimizely platform, including all necessary Azure resources.</p>

<h3>Web App Components</h3>
<p>Each DXP web app includes:</p>
<ul>
    <li><strong>Azure Web App</strong> - The ASP.NET Core application</li>
    <li><strong>SQL Database</strong> - Dedicated database instance</li>
    <li><strong>Blob Storage</strong> - Media and asset container</li>
    <li><strong>Search Index</strong> - Search & Navigation index</li>
</ul>

<h3>Single Web App Constraint</h3>
<div class=""bg-yellow-50 dark:bg-yellow-900/20 border-l-4 border-yellow-400 p-4 my-4"">
    <p class=""text-yellow-800 dark:text-yellow-200""><strong>Important:</strong> You can run only <strong>one Web Application project</strong> in your DXP solution. However, you can have an unlimited number of <strong>sites</strong> within that application.</p>
</div>

<h3>Single vs. Multiple Web Apps</h3>
<h4>Single Web App Benefits</h4>
<ul>
    <li>Reduced cost</li>
    <li>Less complexity</li>
    <li>Shared content and users</li>
    <li>Code reuse across sites</li>
    <li>Simplified testing and deployment</li>
</ul>

<h4>When to Use Multiple Web Apps</h4>
<p>Only pursue multiple web apps when you have specific requirements for:</p>
<ul>
    <li>Completely separate codebases</li>
    <li>Different technology stacks</li>
    <li>Isolated failure domains</li>
</ul>
<p>Note: Multiple web apps still count toward your total page views and content items.</p>

<h3>Multi-Site Configuration</h3>
<p>Within a single web app, you can configure multiple sites that:</p>
<ul>
    <li>Share the same codebase</li>
    <li>Have different domains and URLs</li>
    <li>Use separate content trees</li>
    <li>Can share or isolate content as needed</li>
</ul>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "arch-site-config",
                            Title = "Multi-Site Configuration",
                            Description = "Example appsettings.json for multi-site setup.",
                            Type = ExampleType.Configuration,
                            ExampleContent = @"{
  ""EPiServer"": {
    ""Cms"": {
      ""HostMappings"": [
        {
          ""SiteId"": ""site-a"",
          ""Hosts"": [
            { ""Name"": ""www.site-a.com"", ""Type"": ""Primary"" },
            { ""Name"": ""site-a.com"", ""Type"": ""Redirect"" }
          ]
        },
        {
          ""SiteId"": ""site-b"",
          ""Hosts"": [
            { ""Name"": ""www.site-b.com"", ""Type"": ""Primary"" }
          ]
        }
      ]
    }
  }
}",
                            SampleResponse = @"Multiple sites configured within single DXP web app:
- Site A: www.site-a.com (with redirect from non-www)
- Site B: www.site-b.com

Both sites share the same codebase and deployment.",
                            Hints = new List<string>
                            {
                                "Each site needs unique SiteId values",
                                "Configure DNS to point all domains to your DXP environment"
                            }
                        }
                    }
                },
                new Lesson
                {
                    Id = "arch-search-navigation",
                    ModuleId = "architecture",
                    Title = "Search & Navigation",
                    Summary = "Learn about the Search & Navigation service integrated with DXP.",
                    Order = 3,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Understand Search & Navigation architecture",
                        "Know how indexing works in DXP",
                        "Learn configuration best practices"
                    },
                    Content = @"
<h2>Search & Navigation in DXP</h2>
<p>Search & Navigation (formerly Optimizely Find) provides powerful search functionality powered by Elasticsearch.</p>

<h3>Key Features</h3>
<ul>
    <li><strong>Full-text search</strong> - Search across all content types</li>
    <li><strong>Faceted navigation</strong> - Filter results by properties</li>
    <li><strong>Auto-suggest</strong> - Real-time search suggestions</li>
    <li><strong>Language support</strong> - Multi-language indexing</li>
    <li><strong>Customizable relevance</strong> - Boost and filter results</li>
</ul>

<h3>Indexing in DXP</h3>
<p>Search & Navigation indexing is <strong>real-time</strong> and code-managed:</p>
<ul>
    <li>Content changes are indexed automatically on publish</li>
    <li>Production continuously updates the index</li>
    <li>Other environments require manual indexing after convention changes</li>
</ul>

<div class=""bg-yellow-50 dark:bg-yellow-900/20 border-l-4 border-yellow-400 p-4 my-4"">
    <p class=""text-yellow-800 dark:text-yellow-200""><strong>Important:</strong> Don't use Commerce Manager catalog indexing or CMS scheduled jobs for Search & Navigation. Those apply only to alternative search providers.</p>
</div>

<h3>Environment Considerations</h3>
<ul>
    <li>Each environment has its own Search index</li>
    <li>Index configuration is shared via code</li>
    <li>Content must be indexed in each environment separately</li>
</ul>

<h3>IP Ranges</h3>
<p>Search & Navigation uses <strong>dynamic IP ranges</strong>. If you have firewall rules, configure them using domain-based approval lists rather than IP addresses.</p>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "arch-find-config",
                            Title = "Search & Navigation Configuration",
                            Description = "Configuration for Search & Navigation in DXP.",
                            Type = ExampleType.Configuration,
                            ExampleContent = @"// In Startup.cs or Program.cs
services.AddFind();

// In appsettings.json
{
  ""EPiServer"": {
    ""Find"": {
      ""ServiceUrl"": ""https://your-index.find.episerver.net"",
      ""DefaultIndex"": ""your_index_name"",
      ""TrackingEnabled"": true
    }
  }
}",
                            SampleResponse = @"Search & Navigation is configured and ready.
- ServiceUrl: Points to your Elasticsearch cluster
- DefaultIndex: Your assigned index name
- TrackingEnabled: Enables search statistics",
                            Hints = new List<string>
                            {
                                "Service URL and index name are provided during onboarding",
                                "Don't share credentials between environments"
                            }
                        }
                    }
                },
                new Lesson
                {
                    Id = "arch-cdn-cloudflare",
                    ModuleId = "architecture",
                    Title = "CDN and Cloudflare",
                    Summary = "Understand the CDN layer and security features provided by Cloudflare.",
                    Order = 4,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Know the CDN capabilities",
                        "Understand caching behavior",
                        "Learn about security protections"
                    },
                    Content = @"
<h2>CDN and Cloudflare</h2>
<p>DXP includes Cloudflare as the Content Delivery Network (CDN) layer, providing performance and security benefits.</p>

<h3>Performance Features</h3>
<ul>
    <li><strong>Edge caching</strong> - Static assets cached at global edge locations</li>
    <li><strong>Image optimization</strong> - Automatic format conversion and resizing</li>
    <li><strong>Minification</strong> - CSS and JavaScript minification</li>
    <li><strong>HTTP/2</strong> - Modern protocol for faster loading</li>
    <li><strong>Compression</strong> - Gzip and Brotli compression</li>
</ul>

<h3>Security Features</h3>
<ul>
    <li><strong>DDoS protection</strong> - Automatic mitigation of attacks</li>
    <li><strong>Web Application Firewall</strong> - OWASP rule sets</li>
    <li><strong>SSL/TLS</strong> - Automatic HTTPS encryption</li>
    <li><strong>Bot management</strong> - Detection and filtering of malicious bots</li>
</ul>

<h3>Cache Management</h3>
<p>You can manage CDN cache through the DXP Portal:</p>
<ul>
    <li><strong>Purge all</strong> - Clear entire cache</li>
    <li><strong>Purge by URL</strong> - Clear specific pages</li>
    <li><strong>Purge by tag</strong> - Clear tagged content</li>
</ul>

<h3>Integration Environment</h3>
<p>Note that the Integration environment <strong>bypasses Cloudflare</strong> and uses a direct hostname (<code>*inte.dxcloud.episerver.net</code>). This means:</p>
<ul>
    <li>No CDN caching in Integration</li>
    <li>Direct access to the web app</li>
    <li>Useful for testing without cache interference</li>
</ul>
"
                }
            }
        };
    }

    #endregion

    #region Module 3: Environments

    private LearningModule BuildEnvironmentsModule()
    {
        return new LearningModule
        {
            Id = "environments",
            Title = "DXP Environments",
            Description = "Learn about the different DXP environments and how to work with them.",
            Icon = "square-3-stack-3d",
            Order = 3,
            Difficulty = ModuleDifficulty.Intermediate,
            Prerequisites = new[] { "architecture" },
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "env-overview",
                    ModuleId = "environments",
                    Title = "Environment Overview",
                    Summary = "Understand the three standard DXP environments and their purposes.",
                    Order = 1,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Know the three standard environments",
                        "Understand each environment's purpose",
                        "Learn the deployment flow between environments"
                    },
                    Content = @"
<h2>DXP Environment Overview</h2>
<p>DXP provides three standard environments for development, testing, and production use.</p>

<h3>Integration Environment</h3>
<p>The Integration environment is for <strong>development and continuous integration</strong>:</p>
<ul>
    <li>Deploy daily builds or continuous releases</li>
    <li>Functional testing and initial content setup</li>
    <li>Fixed configuration (no auto-scaling)</li>
    <li>Bypasses Cloudflare CDN</li>
    <li>Hostname: <code>*inte.dxcloud.episerver.net</code></li>
</ul>

<h3>Preproduction Environment</h3>
<p>The Preproduction environment is for <strong>staging and validation</strong>:</p>
<ul>
    <li>User Acceptance Testing (UAT)</li>
    <li>Performance and load testing</li>
    <li>Approved penetration testing</li>
    <li>Production-like configuration</li>
    <li>Automatic scaling enabled</li>
</ul>

<h3>Production Environment</h3>
<p>The Production environment is for <strong>live site operation</strong>:</p>
<ul>
    <li>Content authoring and publishing</li>
    <li>Public visitor access</li>
    <li>Full CDN and security features</li>
    <li>Automatic scaling</li>
    <li>Continuous Search & Navigation indexing</li>
</ul>

<h3>Deployment Flow</h3>
<div class=""bg-gray-100 dark:bg-gray-800 p-4 rounded-lg my-4 font-mono text-sm"">
<pre>
Development -> Integration -> Preproduction -> Production
     |              |               |               |
   Local       CI/CD builds      Staging         Live
   testing     and testing      & UAT           site
</pre>
</div>

<div class=""bg-red-50 dark:bg-red-900/20 border-l-4 border-red-400 p-4 my-4"">
    <p class=""text-red-800 dark:text-red-200""><strong>Critical:</strong> Never use credentials, tokens, or endpoints from a Preproduction environment in a Production environment. Keep environment configurations strictly separated.</p>
</div>
"
                },
                new Lesson
                {
                    Id = "env-integration",
                    ModuleId = "environments",
                    Title = "Integration Environment",
                    Summary = "Deep dive into the Integration environment for development teams.",
                    Order = 2,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Understand Integration environment capabilities",
                        "Know the configuration options",
                        "Learn best practices for CI/CD"
                    },
                    Content = @"
<h2>Integration Environment</h2>
<p>The Integration environment is designed for continuous development and testing.</p>

<h3>Key Characteristics</h3>
<ul>
    <li><strong>No CDN</strong> - Direct access without Cloudflare caching</li>
    <li><strong>Fixed resources</strong> - No auto-scaling (predictable performance)</li>
    <li><strong>Flexible configuration</strong> - Can modify logging settings independently</li>
    <li><strong>Reset capability</strong> - Database can be reset from Preproduction</li>
</ul>

<h3>Configuration Flexibility</h3>
<p>In the Integration environment, you can modify:</p>
<ul>
    <li>Logging levels and settings</li>
    <li>App settings and connection strings</li>
    <li>Debug configurations</li>
</ul>
<p>For Preproduction and Production, contact Optimizely support for these changes.</p>

<h3>CI/CD Integration</h3>
<p>Integration environment is ideal for automated deployments:</p>
<ul>
    <li>Connect your build pipeline (Azure DevOps, GitHub Actions, etc.)</li>
    <li>Deploy on every commit or merge</li>
    <li>Run automated tests after deployment</li>
    <li>Use deployment API for automation</li>
</ul>

<h3>Testing Considerations</h3>
<ul>
    <li>Test without CDN cache interference</li>
    <li>Verify functionality before promoting to Preproduction</li>
    <li>Performance testing should be done in Preproduction (with scaling)</li>
</ul>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "env-deploy-api",
                            Title = "Deployment API Call",
                            Description = "Example of deploying to Integration via API.",
                            Type = ExampleType.Code,
                            ExampleContent = @"# Deploy to Integration environment using Deployment API
curl -X POST ""https://paasportal.episerver.net/api/v1/deploy"" \
  -H ""Authorization: Bearer YOUR_API_TOKEN"" \
  -H ""Content-Type: application/json"" \
  -d '{
    ""environment"": ""Integration"",
    ""packageUrl"": ""https://your-storage/package.nupkg"",
    ""directDeploy"": true
  }'",
                            SampleResponse = @"{
  ""deploymentId"": ""dep-12345"",
  ""status"": ""InProgress"",
  ""environment"": ""Integration"",
  ""startedAt"": ""2024-01-15T10:30:00Z""
}",
                            Hints = new List<string>
                            {
                                "API tokens are generated in the DXP Portal",
                                "DirectDeploy skips the staging slot for faster deployments"
                            }
                        }
                    }
                },
                new Lesson
                {
                    Id = "env-ade",
                    ModuleId = "environments",
                    Title = "Additional Deployment Environments",
                    Summary = "Learn about Additional Deployment Environments (ADEs) for extended testing.",
                    Order = 3,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Understand what ADEs are",
                        "Know when to use ADEs",
                        "Learn ADE capabilities and limitations"
                    },
                    Content = @"
<h2>Additional Deployment Environments (ADEs)</h2>
<p>For deployments after July 2022, ADEs provide extended testing capabilities beyond the standard three environments.</p>

<h3>What Are ADEs?</h3>
<p>ADEs are reusable alternative environments that extend your standard DXP setup. They're useful for:</p>
<ul>
    <li>Parallel feature development</li>
    <li>Extended UAT periods</li>
    <li>Customer demos</li>
    <li>Training environments</li>
</ul>

<h3>ADE Capabilities</h3>
<ul>
    <li><strong>Deployment API</strong> - Full API support for deployments</li>
    <li><strong>Content synchronization</strong> - Copy content from other environments</li>
    <li><strong>Portal features</strong> - Database exports, CDN cache purging</li>
    <li><strong>Independent configuration</strong> - Separate settings from main environments</li>
</ul>

<h3>Using ADEs</h3>
<ol>
    <li>Add API credentials in your deployment tool</li>
    <li>Upload NuGet packages to your storage</li>
    <li>Set target environment to your ADE</li>
    <li>Execute deployment via API or portal</li>
</ol>

<h3>Content Synchronization</h3>
<p>You can copy content between environments using the Deployments interface:</p>
<ul>
    <li>Copy database from Production to ADE</li>
    <li>Sync media files (BLOBs)</li>
    <li>Refresh test data as needed</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "env-config-management",
                    ModuleId = "environments",
                    Title = "Configuration Management",
                    Summary = "Learn how to manage environment-specific configurations in DXP.",
                    Order = 4,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Understand configuration transforms",
                        "Learn to manage secrets",
                        "Know environment-specific settings patterns"
                    },
                    Content = @"
<h2>Configuration Management</h2>
<p>Proper configuration management is essential for successful DXP deployments across environments.</p>

<h3>Configuration Sources</h3>
<p>DXP supports multiple configuration sources:</p>
<ol>
    <li><strong>appsettings.json</strong> - Base configuration in code</li>
    <li><strong>appsettings.{Environment}.json</strong> - Environment-specific overrides</li>
    <li><strong>Azure App Settings</strong> - Portal-managed settings</li>
    <li><strong>Environment variables</strong> - Runtime configuration</li>
</ol>

<h3>Configuration Transforms</h3>
<p>Use environment-specific JSON files for different configurations:</p>
<ul>
    <li><code>appsettings.Integration.json</code></li>
    <li><code>appsettings.Preproduction.json</code></li>
    <li><code>appsettings.Production.json</code></li>
</ul>

<h3>Managing Secrets</h3>
<div class=""bg-red-50 dark:bg-red-900/20 border-l-4 border-red-400 p-4 my-4"">
    <p class=""text-red-800 dark:text-red-200""><strong>Warning:</strong> Never store secrets in source code or appsettings.json files. Use Azure App Settings or Key Vault for sensitive data.</p>
</div>

<h3>Azure App Settings</h3>
<p>Custom settings are managed through client code, not the Azure Portal:</p>
<ul>
    <li>Connection strings for external services</li>
    <li>API keys and secrets</li>
    <li>Feature flags</li>
</ul>

<div class=""bg-yellow-50 dark:bg-yellow-900/20 border-l-4 border-yellow-400 p-4 my-4"">
    <p class=""text-yellow-800 dark:text-yellow-200""><strong>Note:</strong> Do not create additional Azure resources using your DXP account. Custom resources could be deleted during deployment cleanup.</p>
</div>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "env-config-example",
                            Title = "Environment Configuration",
                            Description = "Example of environment-specific configuration.",
                            Type = ExampleType.Configuration,
                            ExampleContent = @"// appsettings.Production.json
{
  ""Logging"": {
    ""LogLevel"": {
      ""Default"": ""Warning"",
      ""Microsoft"": ""Warning""
    }
  },
  ""ExternalServices"": {
    ""CrmApiUrl"": ""https://api.crm.example.com/v2"",
    ""EnableCaching"": true,
    ""CacheTimeoutMinutes"": 60
  }
}

// appsettings.Integration.json
{
  ""Logging"": {
    ""LogLevel"": {
      ""Default"": ""Debug"",
      ""Microsoft"": ""Information""
    }
  },
  ""ExternalServices"": {
    ""CrmApiUrl"": ""https://api-test.crm.example.com/v2"",
    ""EnableCaching"": false
  }
}",
                            SampleResponse = @"Configuration loaded based on environment:
- Production: Warning logs, production CRM API, caching enabled
- Integration: Debug logs, test CRM API, caching disabled",
                            Hints = new List<string>
                            {
                                "Keep connection strings and API keys in Azure App Settings",
                                "Use environment detection in code for runtime decisions"
                            }
                        }
                    }
                }
            }
        };
    }

    #endregion

    #region Module 4: Deployment

    private LearningModule BuildDeploymentModule()
    {
        return new LearningModule
        {
            Id = "deployment",
            Title = "Deploying to DXP",
            Description = "Master the deployment process for Optimizely DXP.",
            Icon = "cloud-arrow-up",
            Order = 4,
            Difficulty = ModuleDifficulty.Intermediate,
            Prerequisites = new[] { "environments" },
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "deploy-overview",
                    ModuleId = "deployment",
                    Title = "Deployment Overview",
                    Summary = "Understand the DXP deployment process and available methods.",
                    Order = 1,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Understand the deployment workflow",
                        "Know the available deployment methods",
                        "Learn the deployment stages"
                    },
                    Content = @"
<h2>Deployment Overview</h2>
<p>DXP supports deploying solutions from development environments to cloud environments as code packages.</p>

<h3>Deployment Methods</h3>

<h4>1. Deployment API (Recommended)</h4>
<p>The preferred approach for publishing application code to DXP environments:</p>
<ul>
    <li>Programmatic deployment via REST API</li>
    <li>Integration with CI/CD pipelines</li>
    <li>Supports automation and scripting</li>
</ul>

<h4>2. DXP Portal</h4>
<p>Manual deployment through the web interface:</p>
<ul>
    <li>Upload NuGet packages directly</li>
    <li>Trigger deployments manually</li>
    <li>Monitor deployment status</li>
</ul>

<h4>3. Version-Controlled Tools</h4>
<p>Integration with popular deployment tools:</p>
<ul>
    <li>Azure DevOps</li>
    <li>Octopus Deploy</li>
    <li>TeamCity</li>
    <li>GitHub Actions</li>
</ul>

<h3>Deployment Stages</h3>
<p>When deploying code changes, DXP's automation engine executes these stages:</p>
<ol>
    <li><strong>Code Packaging</strong> - Creates deployment package</li>
    <li><strong>Slot Creation</strong> - Staging slot with production config</li>
    <li><strong>Code Deployment</strong> - Transfers package to staging</li>
    <li><strong>Monitoring Setup</strong> - Installs performance monitoring</li>
    <li><strong>Configuration</strong> - Applies environment transforms</li>
    <li><strong>Warmup</strong> - Prepares site initialization</li>
    <li><strong>Slot Activation</strong> - Starts staging slot</li>
    <li><strong>Validation</strong> - Smoke testing</li>
    <li><strong>Go-Live</strong> - Swaps staging to production (or aborts)</li>
</ol>
"
                },
                new Lesson
                {
                    Id = "deploy-packaging",
                    ModuleId = "deployment",
                    Title = "Creating Deployment Packages",
                    Summary = "Learn how to create proper deployment packages for DXP.",
                    Order = 2,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Understand NuGet package requirements",
                        "Know what to include and exclude",
                        "Create deployment-ready packages"
                    },
                    Content = @"
<h2>Creating Deployment Packages</h2>
<p>DXP deployments use NuGet packages containing your compiled application.</p>

<h3>Package Requirements</h3>
<ul>
    <li><strong>Format</strong> - Standard NuGet package (.nupkg)</li>
    <li><strong>Content</strong> - Published web application files</li>
    <li><strong>Version</strong> - Semantic versioning recommended</li>
</ul>

<h3>What to Include</h3>
<ul>
    <li>Compiled application (bin folder)</li>
    <li>Static content (wwwroot)</li>
    <li>Configuration files (appsettings.*.json)</li>
    <li><strong>modules</strong> folder - Add-on modules</li>
    <li><strong>modulesbin</strong> folder - Add-on binaries</li>
</ul>

<h3>What to Exclude</h3>
<ul>
    <li>Development files (*.ts, *.scss source)</li>
    <li>Test projects and files</li>
    <li>Local configuration (appsettings.Development.json)</li>
    <li>Build artifacts (.obj, .pdb)</li>
</ul>

<h3>Version Increments</h3>
<div class=""bg-blue-50 dark:bg-blue-900/20 border-l-4 border-blue-400 p-4 my-4"">
    <p class=""text-blue-800 dark:text-blue-200""><strong>Tip:</strong> Increment major/minor versions to trigger initialization module updates. Patch versions may not trigger all startup routines.</p>
</div>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "deploy-nuspec",
                            Title = "NuGet Package Specification",
                            Description = "Example .nuspec file for DXP deployment.",
                            Type = ExampleType.Configuration,
                            ExampleContent = @"<?xml version=""1.0""?>
<package>
  <metadata>
    <id>MyCompany.Website</id>
    <version>1.2.0</version>
    <authors>My Company</authors>
    <description>Corporate website deployment package</description>
  </metadata>
  <files>
    <file src=""publish\**\*.*"" target=""\"" />
    <file src=""modules\**\*.*"" target=""modules"" />
    <file src=""modulesbin\**\*.*"" target=""modulesbin"" />
  </files>
</package>",
                            SampleResponse = @"Package created: MyCompany.Website.1.2.0.nupkg
Contents:
- Application binaries and views
- Static assets (wwwroot)
- Configuration files
- Optimizely modules",
                            Hints = new List<string>
                            {
                                "Use dotnet publish to create deployment-ready files",
                                "Include modules and modulesbin for add-ons to work"
                            }
                        }
                    }
                },
                new Lesson
                {
                    Id = "deploy-api",
                    ModuleId = "deployment",
                    Title = "Deployment API",
                    Summary = "Learn to use the DXP Deployment API for automated deployments.",
                    Order = 3,
                    EstimatedMinutes = 15,
                    LearningObjectives = new List<string>
                    {
                        "Understand the Deployment API",
                        "Create and use API credentials",
                        "Implement automated deployments"
                    },
                    Content = @"
<h2>Deployment API</h2>
<p>The Deployment API enables programmatic deployment to DXP environments.</p>

<h3>API Credentials</h3>
<p>To use the Deployment API:</p>
<ol>
    <li>Log in to the DXP Portal</li>
    <li>Navigate to API Credentials section</li>
    <li>Generate a new API key and secret</li>
    <li>Store credentials securely (never in source code)</li>
</ol>

<h3>Deployment Process</h3>
<ol>
    <li><strong>Upload package</strong> - Upload .nupkg to accessible storage</li>
    <li><strong>Start deployment</strong> - POST to deployment endpoint</li>
    <li><strong>Monitor status</strong> - Poll for deployment completion</li>
    <li><strong>Verify</strong> - Validate deployment succeeded</li>
</ol>

<h3>DirectDeploy Option</h3>
<p>For faster deployments to Integration:</p>
<ul>
    <li>Skips staging slot creation</li>
    <li>Deploys directly to main slot</li>
    <li>Faster but causes brief downtime</li>
    <li>Recommended for CI/CD to Integration only</li>
</ul>

<h3>Database Limitations</h3>
<div class=""bg-red-50 dark:bg-red-900/20 border-l-4 border-red-400 p-4 my-4"">
    <p class=""text-red-800 dark:text-red-200""><strong>Important:</strong> After going live, you can no longer overwrite the production database using the deployment API. Content editing must occur in the production environment.</p>
</div>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "deploy-api-script",
                            Title = "PowerShell Deployment Script",
                            Description = "Example script for deploying via API.",
                            Type = ExampleType.Code,
                            ExampleContent = @"# DXP Deployment Script
$apiKey = $env:DXP_API_KEY
$apiSecret = $env:DXP_API_SECRET
$projectId = ""your-project-id""
$environment = ""Integration""
$packageUrl = ""https://storage.blob.core.windows.net/packages/MyApp.1.2.0.nupkg""

# Get access token
$tokenResponse = Invoke-RestMethod -Uri ""https://paasportal.episerver.net/api/v1/auth/token"" `
    -Method Post `
    -Body @{ apiKey = $apiKey; apiSecret = $apiSecret } `
    -ContentType ""application/x-www-form-urlencoded""

$token = $tokenResponse.access_token

# Start deployment
$headers = @{ Authorization = ""Bearer $token"" }
$body = @{
    projectId = $projectId
    targetEnvironment = $environment
    packageUrl = $packageUrl
    directDeploy = $true
} | ConvertTo-Json

$deployment = Invoke-RestMethod -Uri ""https://paasportal.episerver.net/api/v1/deployments"" `
    -Method Post `
    -Headers $headers `
    -Body $body `
    -ContentType ""application/json""

Write-Host ""Deployment started: $($deployment.id)""",
                            SampleResponse = @"Deployment started: dep-abc123
Status: InProgress
Target: Integration
Package: MyApp.1.2.0.nupkg",
                            Hints = new List<string>
                            {
                                "Store API credentials in environment variables or secret managers",
                                "Use DirectDeploy only for non-production environments"
                            }
                        }
                    }
                },
                new Lesson
                {
                    Id = "deploy-best-practices",
                    ModuleId = "deployment",
                    Title = "Deployment Best Practices",
                    Summary = "Learn best practices for successful DXP deployments.",
                    Order = 4,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Know deployment best practices",
                        "Avoid common pitfalls",
                        "Handle breaking changes properly"
                    },
                    Content = @"
<h2>Deployment Best Practices</h2>
<p>Follow these best practices for reliable, successful deployments.</p>

<h3>Before Deployment</h3>
<ul>
    <li><strong>Test locally</strong> - Verify functionality in development</li>
    <li><strong>Check compatibility</strong> - Ensure software supports Azure Web Apps</li>
    <li><strong>Version increment</strong> - Use proper semantic versioning</li>
    <li><strong>Backup planning</strong> - Know your rollback strategy</li>
</ul>

<h3>Breaking Changes</h3>
<p>Use maintenance pages when deploying:</p>
<ul>
    <li>Database schema updates</li>
    <li>Content type modifications</li>
    <li>Major structural changes</li>
</ul>

<h3>Wildcard Binding</h3>
<div class=""bg-yellow-50 dark:bg-yellow-900/20 border-l-4 border-yellow-400 p-4 my-4"">
    <p class=""text-yellow-800 dark:text-yellow-200""><strong>Important:</strong> Add wildcard hostname bindings before deployment to prevent URL-dependent code failures in staging slots.</p>
</div>

<h3>Continuous Deployment Flow</h3>
<p>After initial production deployment:</p>
<ol>
    <li>Content editing occurs in production</li>
    <li>Code changes deploy through Integration → Preproduction → Production</li>
    <li>Sync content back to lower environments for testing</li>
</ol>

<h3>Common Pitfalls to Avoid</h3>
<ul>
    <li>Deploying with development configuration</li>
    <li>Missing modules or modulesbin folders</li>
    <li>Hardcoded connection strings</li>
    <li>Using production credentials in testing</li>
    <li>Skipping smoke tests after deployment</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "deploy-go-live",
                    ModuleId = "deployment",
                    Title = "Go-Live Certification",
                    Summary = "Prepare for production launch with the Go-Live certification checklist.",
                    Order = 5,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Understand Go-Live requirements",
                        "Complete the certification checklist",
                        "Prepare for production launch"
                    },
                    Content = @"
<h2>Go-Live Certification</h2>
<p>Before launching to production, complete the Go-Live certification checklist.</p>

<h3>Pre-Production Checklist</h3>

<h4>Partner/Developer Tasks</h4>
<ul>
    <li>All functionality tested in Preproduction</li>
    <li>Performance testing completed</li>
    <li>Security review passed</li>
    <li>All required integrations working</li>
    <li>Content migration completed</li>
</ul>

<h4>DNS Configuration</h4>
<ul>
    <li>DNS records prepared (but not switched)</li>
    <li>SSL certificates configured</li>
    <li>TTL values reduced for faster switchover</li>
</ul>

<h4>Backup & Recovery</h4>
<ul>
    <li>Backup procedures documented</li>
    <li>Recovery process tested</li>
    <li>Point-in-time restore understood (up to 35 days)</li>
</ul>

<h3>Go-Live Support</h3>
<p>Customer Success team assists with:</p>
<ul>
    <li>Final deployment to production</li>
    <li>CDN configuration</li>
    <li>IP whitelisting requests</li>
    <li>VPN configuration if needed</li>
    <li>DNS cutover coordination</li>
</ul>

<h3>Post Go-Live</h3>
<ul>
    <li>Monitor error logs and performance</li>
    <li>Verify CDN caching behavior</li>
    <li>Confirm Search & Navigation indexing</li>
    <li>Test all critical user journeys</li>
</ul>
"
                }
            }
        };
    }

    #endregion

    #region Module 5: Monitoring and Logging

    private LearningModule BuildMonitoringModule()
    {
        return new LearningModule
        {
            Id = "monitoring",
            Title = "Monitoring and Logging",
            Description = "Learn to monitor, log, and optimize your DXP applications.",
            Icon = "chart-bar",
            Order = 5,
            Difficulty = ModuleDifficulty.Intermediate,
            Prerequisites = new[] { "deployment" },
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "mon-app-insights",
                    ModuleId = "monitoring",
                    Title = "Application Insights",
                    Summary = "Understand Application Insights integration in DXP.",
                    Order = 1,
                    EstimatedMinutes = 15,
                    LearningObjectives = new List<string>
                    {
                        "Understand Application Insights capabilities",
                        "Configure logging levels",
                        "Access telemetry data"
                    },
                    Content = @"
<h2>Application Insights</h2>
<p>DXP uses Microsoft Application Insights for application performance management and monitoring.</p>

<h3>Automatic Configuration</h3>
<p>Application Insights is <strong>configured automatically</strong> for all DXP sites via the <code>EPiServer.CloudPlatform.Cms</code> package. No additional setup required for basic monitoring.</p>

<h3>What's Monitored</h3>
<ul>
    <li><strong>Request performance</strong> - Response times and throughput</li>
    <li><strong>Dependencies</strong> - Database, external API calls</li>
    <li><strong>Exceptions</strong> - Unhandled errors and stack traces</li>
    <li><strong>Custom events</strong> - Business metrics you define</li>
    <li><strong>Page views</strong> - Client-side navigation (with beacon)</li>
</ul>

<h3>Logging Levels</h3>
<p>By default, only <strong>Warning logs and higher</strong> are captured. To change this:</p>

<h4>ILogger Integration</h4>
<p>The standard <code>ILogger</code> automatically logs to Application Insights. Override the default level in configuration.</p>

<h3>Runtime vs Build-time Instrumentation</h3>
<ul>
    <li><strong>Runtime</strong> (default) - Basic monitoring, logs in DXP Dashboard</li>
    <li><strong>Build-time</strong> - Full Application Insights features, requires SDK packages</li>
</ul>

<p>For full features, install these packages:</p>
<ul>
    <li><code>Microsoft.ApplicationInsights.Web</code></li>
    <li><code>Microsoft.ApplicationInsights.TraceListener</code></li>
</ul>

<h3>API Key Access</h3>
<p>Request Application Insights API keys from <a href=""mailto:support@optimizely.com"">support@optimizely.com</a>. You receive one key per environment with read-only permissions.</p>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "mon-logging-config",
                            Title = "Logging Configuration",
                            Description = "Configure Application Insights logging levels.",
                            Type = ExampleType.Configuration,
                            ExampleContent = @"// appsettings.json
{
  ""Logging"": {
    ""LogLevel"": {
      ""Default"": ""Warning""
    },
    ""ApplicationInsights"": {
      ""LogLevel"": {
        ""Default"": ""Information"",
        ""Microsoft.EntityFrameworkCore"": ""Warning""
      }
    }
  }
}

// Enable profiler in Program.cs (.NET 5+)
services.AddServiceProfiler();",
                            SampleResponse = @"Logging configured:
- General logs: Warning and above
- Application Insights: Information and above
- EF Core: Warning only (reduces noise)
- Profiler: Enabled for performance analysis",
                            Hints = new List<string>
                            {
                                "For .NET 4.x, ask support to activate profiling",
                                "Too verbose logging can impact performance and costs"
                            }
                        }
                    }
                },
                new Lesson
                {
                    Id = "mon-logging",
                    ModuleId = "monitoring",
                    Title = "Logging in DXP",
                    Summary = "Learn logging configuration and best practices for DXP.",
                    Order = 2,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Understand DXP logging infrastructure",
                        "Access logs in the portal",
                        "Configure custom logging"
                    },
                    Content = @"
<h2>Logging in DXP</h2>
<p>DXP provides built-in logging infrastructure with centralized access through the Management Portal.</p>

<h3>Default Configuration</h3>
<ul>
    <li><strong>Application logging</strong> - Enabled by default</li>
    <li><strong>Storage</strong> - Logs stored in Azure Blob Storage</li>
    <li><strong>Retention</strong> - 90-day retention period</li>
</ul>

<h3>Accessing Logs</h3>
<p>From the DXP Management Portal:</p>
<ul>
    <li><strong>Live stream</strong> - Real-time log viewing</li>
    <li><strong>Download</strong> - Export logs for offline analysis</li>
</ul>

<h3>Environment-Specific Changes</h3>
<table class=""min-w-full divide-y divide-gray-200 dark:divide-gray-700 my-4"">
    <thead>
        <tr>
            <th class=""px-4 py-2 text-left"">Environment</th>
            <th class=""px-4 py-2 text-left"">Can Modify</th>
        </tr>
    </thead>
    <tbody>
        <tr><td class=""px-4 py-2"">Integration</td><td class=""px-4 py-2"">Yes - self-service</td></tr>
        <tr><td class=""px-4 py-2"">Preproduction</td><td class=""px-4 py-2"">Contact support</td></tr>
        <tr><td class=""px-4 py-2"">Production</td><td class=""px-4 py-2"">Contact support</td></tr>
    </tbody>
</table>

<h3>Available Modifications (via Support)</h3>
<ul>
    <li>Enable web server log access</li>
    <li>Temporarily adjust logging level</li>
    <li>Enable detailed error messages</li>
    <li>Activate failed request tracing</li>
</ul>

<h3>Recommended Logging Framework</h3>
<p>Use <strong>Microsoft's logging abstraction</strong> (<code>ILogger</code>) for DXP compatibility:</p>
<ul>
    <li>Replace log4net with <code>EPiServer.Logging.LogManager</code></li>
    <li>Logs are redirected to .NET Diagnostic Trace</li>
    <li>Full integration with Application Insights</li>
</ul>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "mon-ilogger-usage",
                            Title = "Using ILogger",
                            Description = "Example of proper logging in DXP.",
                            Type = ExampleType.Code,
                            ExampleContent = @"using Microsoft.Extensions.Logging;

public class MyService
{
    private readonly ILogger<MyService> _logger;

    public MyService(ILogger<MyService> logger)
    {
        _logger = logger;
    }

    public void ProcessOrder(int orderId)
    {
        _logger.LogInformation(""Processing order {OrderId}"", orderId);

        try
        {
            // Process order
            _logger.LogDebug(""Order {OrderId} validation passed"", orderId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ""Failed to process order {OrderId}"", orderId);
            throw;
        }
    }
}",
                            SampleResponse = @"Log output (at Information level):
[INF] Processing order 12345
[ERR] Failed to process order 12345
      System.InvalidOperationException: ...",
                            Hints = new List<string>
                            {
                                "Use structured logging with named parameters",
                                "Don't log sensitive data (passwords, PII)"
                            }
                        }
                    }
                },
                new Lesson
                {
                    Id = "mon-consumption",
                    ModuleId = "monitoring",
                    Title = "Consumption Metrics",
                    Summary = "Understand and monitor your DXP consumption metrics.",
                    Order = 3,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Understand consumption-based pricing",
                        "Monitor page views and usage",
                        "Track SPA applications correctly"
                    },
                    Content = @"
<h2>Consumption Metrics</h2>
<p>DXP uses consumption-based pricing, making it important to understand and monitor your usage.</p>

<h3>Key Metrics</h3>
<ul>
    <li><strong>Page views</strong> - Monthly and year-to-date counts</li>
    <li><strong>Content items</strong> - Total content in the system</li>
    <li><strong>Storage</strong> - Blob storage usage</li>
    <li><strong>Database size</strong> - SQL database consumption</li>
</ul>

<h3>Tracking Page Views</h3>
<p>Application Insights tracks server telemetry automatically. For accurate client-side tracking:</p>
<ul>
    <li>JavaScript beacon installed on pages</li>
    <li>Session information collected</li>
    <li>User navigation patterns tracked</li>
</ul>

<h3>Single Page Applications (SPAs)</h3>
<div class=""bg-yellow-50 dark:bg-yellow-900/20 border-l-4 border-yellow-400 p-4 my-4"">
    <p class=""text-yellow-800 dark:text-yellow-200""><strong>Important:</strong> SPAs require specific setup since pages don't reload during navigation. Configure page view tracking parameters to ensure accurate traffic volume monitoring.</p>
</div>

<h3>Viewing Consumption</h3>
<p>Access consumption data in:</p>
<ul>
    <li><strong>DXP Dashboard</strong> - Overview and trends</li>
    <li><strong>Azure Portal</strong> - Detailed resource usage</li>
    <li><strong>Application Insights</strong> - Traffic analytics</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "mon-performance",
                    ModuleId = "monitoring",
                    Title = "Performance Optimization",
                    Summary = "Learn to optimize performance of your DXP applications.",
                    Order = 4,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Identify performance bottlenecks",
                        "Use Application Insights Profiler",
                        "Implement optimization strategies"
                    },
                    Content = @"
<h2>Performance Optimization</h2>
<p>Use DXP's monitoring tools to identify and resolve performance issues.</p>

<h3>Application Insights Profiler</h3>
<p>Azure Application Insights Profiler provides production performance traces:</p>
<ul>
    <li>Captures data automatically at scale</li>
    <li>No impact on end users</li>
    <li>Identifies ""hot"" code paths</li>
    <li>Shows time spent in each method</li>
</ul>

<h3>Common Performance Issues</h3>

<h4>Database Queries</h4>
<ul>
    <li>N+1 query problems</li>
    <li>Missing indexes</li>
    <li>Large result sets</li>
</ul>

<h4>External Dependencies</h4>
<ul>
    <li>Slow API calls</li>
    <li>Missing timeouts</li>
    <li>Lack of caching</li>
</ul>

<h4>Content Delivery</h4>
<ul>
    <li>Large images not optimized</li>
    <li>Missing CDN caching headers</li>
    <li>Uncached dynamic content</li>
</ul>

<h3>Optimization Strategies</h3>
<ul>
    <li><strong>Output caching</strong> - Cache rendered pages</li>
    <li><strong>CDN caching</strong> - Configure proper cache headers</li>
    <li><strong>Async operations</strong> - Use async/await throughout</li>
    <li><strong>Database optimization</strong> - Indexes and query tuning</li>
    <li><strong>Image optimization</strong> - Use ImageResizer or CDN transforms</li>
</ul>

<h3>Source Maps for JavaScript</h3>
<p>For debugging client-side issues:</p>
<ul>
    <li>Place <code>.js.map</code> files alongside JavaScript</li>
    <li>Use versioned folders for multiple versions</li>
    <li>Maps translate minified code for debugging</li>
</ul>
"
                }
            }
        };
    }

    #endregion

    #region Module 6: Security

    private LearningModule BuildSecurityModule()
    {
        return new LearningModule
        {
            Id = "security",
            Title = "Security and Compliance",
            Description = "Understand security features and compliance in DXP.",
            Icon = "shield-check",
            Order = 6,
            Difficulty = ModuleDifficulty.Intermediate,
            Prerequisites = new[] { "architecture" },
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "sec-authentication",
                    ModuleId = "security",
                    Title = "Authentication Options",
                    Summary = "Learn about authentication and identity management in DXP.",
                    Order = 1,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Understand Opti ID",
                        "Know Entra ID integration options",
                        "Configure authentication properly"
                    },
                    Content = @"
<h2>Authentication in DXP</h2>
<p>DXP supports multiple authentication providers for editor access and visitor identity.</p>

<h3>Opti ID</h3>
<p>Optimizely's native identity solution provides:</p>
<ul>
    <li><strong>Single sign-on</strong> - Access all Optimizely products</li>
    <li><strong>User management</strong> - Centralized user administration</li>
    <li><strong>Role-based access</strong> - Fine-grained permissions</li>
    <li><strong>Multi-factor authentication</strong> - Enhanced security</li>
</ul>

<h3>Entra ID (Azure AD)</h3>
<p>Microsoft's cloud directory service provides:</p>
<ul>
    <li><strong>Enterprise SSO</strong> - Integrate with corporate identity</li>
    <li><strong>Conditional access</strong> - Policy-based security</li>
    <li><strong>B2C support</strong> - Customer identity management</li>
    <li><strong>Group synchronization</strong> - Map AD groups to CMS roles</li>
</ul>

<h3>Configuration Steps</h3>
<ol>
    <li>Install required NuGet packages</li>
    <li>Configure authentication in <code>Program.cs</code></li>
    <li>Set up identity provider in admin center</li>
    <li>Map roles and permissions</li>
</ol>

<h3>Best Practices</h3>
<ul>
    <li>Always use HTTPS for authentication</li>
    <li>Implement proper session management</li>
    <li>Use strong password policies</li>
    <li>Enable MFA for editor accounts</li>
    <li>Regularly audit user access</li>
</ul>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "sec-opti-id-config",
                            Title = "Opti ID Configuration",
                            Description = "Configure Opti ID for your DXP site.",
                            Type = ExampleType.Configuration,
                            ExampleContent = @"// In Program.cs
builder.Services.AddCmsHost()
    .AddCmsUI()
    .AddOptiIdCms(options =>
    {
        options.ClientId = ""your-client-id"";
        options.Authority = ""https://id.optimizely.com"";
        options.AdminClientId = ""your-admin-client-id"";
    });

// In appsettings.json
{
  ""OptiId"": {
    ""ClientId"": ""your-client-id"",
    ""ClientSecret"": ""[from-app-settings]"",
    ""Authority"": ""https://id.optimizely.com"",
    ""CallbackPath"": ""/signin-optimizely""
  }
}",
                            SampleResponse = @"Opti ID configured successfully.
Users can now sign in via Optimizely's identity service.
MFA and enterprise policies will apply based on tenant settings.",
                            Hints = new List<string>
                            {
                                "ClientSecret should be in Azure App Settings, not in code",
                                "Configure callback URLs in Opti ID admin center"
                            }
                        }
                    }
                },
                new Lesson
                {
                    Id = "sec-cdn-protection",
                    ModuleId = "security",
                    Title = "CDN Security Features",
                    Summary = "Understand the security features provided by Cloudflare CDN.",
                    Order = 2,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Know CDN security capabilities",
                        "Understand DDoS protection",
                        "Learn about WAF rules"
                    },
                    Content = @"
<h2>CDN Security Features</h2>
<p>Cloudflare CDN provides multiple layers of security protection for DXP sites.</p>

<h3>DDoS Protection</h3>
<ul>
    <li>Automatic attack detection and mitigation</li>
    <li>Layer 3/4 and Layer 7 protection</li>
    <li>Traffic scrubbing at edge locations</li>
    <li>Rate limiting for suspicious traffic</li>
</ul>

<h3>Web Application Firewall (WAF)</h3>
<ul>
    <li><strong>OWASP rules</strong> - Protection against common vulnerabilities</li>
    <li><strong>SQL injection</strong> - Block malicious database queries</li>
    <li><strong>XSS protection</strong> - Prevent cross-site scripting</li>
    <li><strong>Custom rules</strong> - Available upon request</li>
</ul>

<h3>SSL/TLS Encryption</h3>
<ul>
    <li>Automatic HTTPS for all traffic</li>
    <li>TLS 1.2+ required</li>
    <li>Free SSL certificates</li>
    <li>Custom certificates supported</li>
</ul>

<h3>Bot Management</h3>
<ul>
    <li>Known bot detection</li>
    <li>Malicious bot blocking</li>
    <li>Challenge pages for suspicious traffic</li>
    <li>Legitimate bot allowlisting</li>
</ul>

<h3>Additional Protections</h3>
<ul>
    <li><strong>IP filtering</strong> - Geo-blocking and IP restrictions</li>
    <li><strong>Page Rules</strong> - Custom security rules per URL</li>
    <li><strong>Access Controls</strong> - Additional authentication layers</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "sec-best-practices",
                    ModuleId = "security",
                    Title = "Security Best Practices",
                    Summary = "Learn security best practices for DXP development.",
                    Order = 3,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Implement secure coding practices",
                        "Manage secrets properly",
                        "Protect against common vulnerabilities"
                    },
                    Content = @"
<h2>Security Best Practices</h2>
<p>Follow these security best practices when developing for DXP.</p>

<h3>Secrets Management</h3>
<div class=""bg-red-50 dark:bg-red-900/20 border-l-4 border-red-400 p-4 my-4"">
    <p class=""text-red-800 dark:text-red-200""><strong>Critical:</strong> Never store secrets in source code, configuration files, or version control.</p>
</div>
<ul>
    <li>Use Azure App Settings for sensitive values</li>
    <li>Consider Azure Key Vault for highly sensitive data</li>
    <li>Rotate credentials regularly</li>
    <li>Use separate credentials per environment</li>
</ul>

<h3>Environment Separation</h3>
<ul>
    <li>Never use production credentials in lower environments</li>
    <li>Keep environment configurations strictly separated</li>
    <li>Use different API keys per environment</li>
    <li>Sanitize data when copying to lower environments</li>
</ul>

<h3>Input Validation</h3>
<ul>
    <li>Validate all user input on the server</li>
    <li>Use parameterized queries for database access</li>
    <li>Sanitize output to prevent XSS</li>
    <li>Implement proper error handling (don't leak info)</li>
</ul>

<h3>Access Control</h3>
<ul>
    <li>Follow principle of least privilege</li>
    <li>Regularly audit user permissions</li>
    <li>Remove access for departed team members</li>
    <li>Use role-based access control</li>
</ul>

<h3>Penetration Testing</h3>
<p>Penetration testing is allowed on the <strong>Preproduction environment</strong> with approval:</p>
<ul>
    <li>Contact Optimizely support before testing</li>
    <li>Provide testing schedule and scope</li>
    <li>Do not test on Production</li>
</ul>
"
                }
            }
        };
    }

    #endregion

    #region Module 7: Best Practices

    private LearningModule BuildBestPracticesModule()
    {
        return new LearningModule
        {
            Id = "best-practices",
            Title = "Development Best Practices",
            Description = "Learn best practices for developing solutions on DXP.",
            Icon = "light-bulb",
            Order = 7,
            Difficulty = ModuleDifficulty.Advanced,
            Prerequisites = new[] { "deployment", "monitoring" },
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "bp-development",
                    ModuleId = "best-practices",
                    Title = "Development Considerations",
                    Summary = "Key considerations for developing DXP solutions.",
                    Order = 1,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Understand DXP-specific development patterns",
                        "Avoid common development pitfalls",
                        "Write cloud-ready code"
                    },
                    Content = @"
<h2>Development Considerations</h2>
<p>DXP development requires understanding cloud-specific patterns and constraints.</p>

<h3>Azure Resource Constraints</h3>
<div class=""bg-yellow-50 dark:bg-yellow-900/20 border-l-4 border-yellow-400 p-4 my-4"">
    <p class=""text-yellow-800 dark:text-yellow-200""><strong>Warning:</strong> Do not create additional Azure resources using your DXP account. Custom resources could be deleted during deployment cleanup.</p>
</div>

<h3>Configuration Through Code</h3>
<p>Custom application settings and connection strings are only supported through client code:</p>
<ul>
    <li>Use <code>IConfiguration</code> in your application</li>
    <li>Settings stored in appsettings.json or App Settings</li>
    <li>Do not modify settings via Azure Portal directly</li>
</ul>

<h3>Session State</h3>
<p>In a scaled environment, session state must be distributed:</p>
<ul>
    <li>Avoid in-process session state</li>
    <li>Use distributed cache (Redis, SQL)</li>
    <li>Design for stateless operation where possible</li>
</ul>

<h3>File System Access</h3>
<ul>
    <li>Local file storage is not persistent across instances</li>
    <li>Use Azure Blob Storage for persistent files</li>
    <li>Temp files may disappear on app restart</li>
</ul>

<h3>Logging Changes</h3>
<p>Replace log4net with DXP-compatible logging:</p>
<ul>
    <li>Use <code>EPiServer.Logging.LogManager</code> abstraction</li>
    <li>Logs redirect to .NET Diagnostic Trace</li>
    <li>Integrated with Application Insights</li>
</ul>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "bp-blob-storage",
                            Title = "Using Blob Storage",
                            Description = "Example of accessing Blob Storage in DXP.",
                            Type = ExampleType.Code,
                            ExampleContent = @"using EPiServer.Framework.Blobs;

public class AssetService
{
    private readonly IBlobFactory _blobFactory;

    public AssetService(IBlobFactory blobFactory)
    {
        _blobFactory = blobFactory;
    }

    public async Task<string> StoreFileAsync(Stream content, string fileName)
    {
        // Create blob in default container
        var blob = _blobFactory.CreateBlob(
            Blob.DefaultProvider,
            ""."" + Path.GetExtension(fileName));

        using (var stream = blob.OpenWrite())
        {
            await content.CopyToAsync(stream);
        }

        return blob.ID.ExternalURL;
    }
}",
                            SampleResponse = @"File stored in Azure Blob Storage.
URL: https://youraccount.blob.core.windows.net/container/file.pdf

The blob is automatically served through the CDN for optimal performance.",
                            Hints = new List<string>
                            {
                                "Blob Storage is automatically configured by CloudPlatform package",
                                "Use IBlobFactory for DXP-compatible file operations"
                            }
                        }
                    }
                },
                new Lesson
                {
                    Id = "bp-scalability",
                    ModuleId = "best-practices",
                    Title = "Designing for Scale",
                    Summary = "Build applications that scale effectively on DXP.",
                    Order = 2,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Understand auto-scaling behavior",
                        "Design stateless applications",
                        "Handle multi-instance scenarios"
                    },
                    Content = @"
<h2>Designing for Scale</h2>
<p>DXP automatically scales your application based on demand. Design your code to work correctly across multiple instances.</p>

<h3>Auto-Scaling</h3>
<p>Premium V3 App Service Plans provide:</p>
<ul>
    <li>Automatic scale-out under load</li>
    <li>Scale-in when demand decreases</li>
    <li>Multiple instances running simultaneously</li>
</ul>

<h3>Stateless Design</h3>
<p>With multiple instances, avoid instance-specific state:</p>
<ul>
    <li><strong>No in-memory caching</strong> of instance-specific data</li>
    <li><strong>No local file storage</strong> for persistent data</li>
    <li><strong>No in-process session</strong> state</li>
</ul>

<h3>Distributed Caching</h3>
<ul>
    <li>Use Redis or SQL Server for distributed cache</li>
    <li>Optimizely's output cache is distributed-aware</li>
    <li>Configure cache invalidation properly</li>
</ul>

<h3>Background Jobs</h3>
<ul>
    <li>Scheduled jobs run on all instances</li>
    <li>Use distributed locks for single-execution jobs</li>
    <li>Consider Azure Functions for heavy processing</li>
</ul>

<h3>Database Connections</h3>
<ul>
    <li>Use connection pooling</li>
    <li>Keep connections short-lived</li>
    <li>Handle transient failures with retry logic</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "bp-caching",
                    ModuleId = "best-practices",
                    Title = "Caching Strategies",
                    Summary = "Implement effective caching strategies for DXP.",
                    Order = 3,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Understand caching layers in DXP",
                        "Configure CDN caching",
                        "Implement application-level caching"
                    },
                    Content = @"
<h2>Caching Strategies</h2>
<p>Effective caching is crucial for DXP performance. Multiple caching layers work together.</p>

<h3>Caching Layers</h3>
<ol>
    <li><strong>CDN (Cloudflare)</strong> - Edge caching globally</li>
    <li><strong>Output Cache</strong> - Full page caching</li>
    <li><strong>Object Cache</strong> - Content and query results</li>
    <li><strong>Database</strong> - Query caching</li>
</ol>

<h3>CDN Caching</h3>
<p>Configure cache headers for optimal CDN behavior:</p>
<ul>
    <li>Static assets: Long cache duration (1 year)</li>
    <li>Dynamic pages: Short or no cache</li>
    <li>API responses: Based on data freshness needs</li>
</ul>

<h3>Output Caching</h3>
<p>Optimizely's output cache works with distributed environments:</p>
<ul>
    <li>Cache entire rendered pages</li>
    <li>Automatic invalidation on content publish</li>
    <li>VaryBy support for personalization</li>
</ul>

<h3>Cache Invalidation</h3>
<ul>
    <li>Content changes automatically invalidate cache</li>
    <li>Use DXP Portal to purge CDN cache</li>
    <li>Implement cache dependencies properly</li>
</ul>

<h3>Cache Headers Example</h3>
<p>Set appropriate headers for different content types:</p>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "bp-cache-headers",
                            Title = "Cache Header Configuration",
                            Description = "Configure caching headers in middleware.",
                            Type = ExampleType.Code,
                            ExampleContent = @"// In Program.cs - Add caching middleware
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        // Cache static files for 1 year
        ctx.Context.Response.Headers.Append(
            ""Cache-Control"", ""public,max-age=31536000"");
    }
});

// For dynamic content, use response caching
[ResponseCache(Duration = 300, VaryByQueryKeys = new[] { ""page"" })]
public IActionResult List(int page = 1)
{
    // Content cached for 5 minutes, varying by page
}

// Disable caching for personalized content
[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public IActionResult PersonalizedContent()
{
    // No caching for user-specific content
}",
                            SampleResponse = @"Cache configuration applied:
- Static files: Cached 1 year at CDN and browser
- List page: Cached 5 minutes, varies by page parameter
- Personalized content: No caching",
                            Hints = new List<string>
                            {
                                "Use versioned URLs for static assets to enable long caching",
                                "Purge CDN cache after deploying new assets"
                            }
                        }
                    }
                },
                new Lesson
                {
                    Id = "bp-migrations",
                    ModuleId = "best-practices",
                    Title = "Database Migrations",
                    Summary = "Handle database migrations safely in DXP.",
                    Order = 4,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Understand migration strategies",
                        "Minimize deployment downtime",
                        "Handle breaking changes"
                    },
                    Content = @"
<h2>Database Migrations</h2>
<p>Database schema changes require careful planning in DXP to minimize disruption.</p>

<h3>Migration Approaches</h3>

<h4>Non-Breaking Changes</h4>
<p>Changes that don't require downtime:</p>
<ul>
    <li>Adding new columns (with defaults)</li>
    <li>Adding new tables</li>
    <li>Adding new indexes</li>
</ul>

<h4>Breaking Changes</h4>
<p>Changes requiring maintenance mode:</p>
<ul>
    <li>Dropping columns or tables</li>
    <li>Renaming columns</li>
    <li>Changing data types</li>
    <li>Content type modifications</li>
</ul>

<h3>Safe Migration Pattern</h3>
<ol>
    <li><strong>Add new</strong> - Add new columns/tables</li>
    <li><strong>Deploy code</strong> - Update application to use new schema</li>
    <li><strong>Migrate data</strong> - Copy data to new structure</li>
    <li><strong>Remove old</strong> - Drop unused columns (later deployment)</li>
</ol>

<h3>Using Maintenance Pages</h3>
<p>For breaking changes:</p>
<ul>
    <li>Enable maintenance page in DXP Portal</li>
    <li>Deploy code and run migrations</li>
    <li>Verify functionality</li>
    <li>Disable maintenance page</li>
</ul>

<h3>Entity Framework Migrations</h3>
<ul>
    <li>Run migrations at application startup (carefully)</li>
    <li>Consider separate migration deployment step</li>
    <li>Test migrations in Preproduction first</li>
</ul>
"
                }
            }
        };
    }

    #endregion

    #region Module 8: Troubleshooting

    private LearningModule BuildTroubleshootingModule()
    {
        return new LearningModule
        {
            Id = "troubleshooting",
            Title = "Troubleshooting DXP",
            Description = "Learn to diagnose and resolve common DXP issues.",
            Icon = "wrench-screwdriver",
            Order = 8,
            Difficulty = ModuleDifficulty.Advanced,
            Prerequisites = new[] { "monitoring" },
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "ts-common-issues",
                    ModuleId = "troubleshooting",
                    Title = "Common Issues",
                    Summary = "Diagnose and resolve frequently encountered DXP issues.",
                    Order = 1,
                    EstimatedMinutes = 15,
                    LearningObjectives = new List<string>
                    {
                        "Identify common DXP issues",
                        "Know troubleshooting approaches",
                        "Resolve typical problems"
                    },
                    Content = @"
<h2>Common DXP Issues</h2>
<p>Learn to diagnose and resolve frequently encountered issues in DXP.</p>

<h3>Deployment Failures</h3>
<h4>Symptoms</h4>
<ul>
    <li>Deployment stays in ""In Progress"" state</li>
    <li>Smoke tests fail after deployment</li>
    <li>Application doesn't start</li>
</ul>
<h4>Solutions</h4>
<ul>
    <li>Check application logs for startup errors</li>
    <li>Verify all dependencies are included in package</li>
    <li>Ensure configuration files are correct</li>
    <li>Check for missing connection strings</li>
</ul>

<h3>Performance Issues</h3>
<h4>Symptoms</h4>
<ul>
    <li>Slow page load times</li>
    <li>High CPU or memory usage</li>
    <li>Database connection timeouts</li>
</ul>
<h4>Solutions</h4>
<ul>
    <li>Review Application Insights for bottlenecks</li>
    <li>Check for inefficient database queries</li>
    <li>Verify CDN caching is working</li>
    <li>Review for memory leaks</li>
</ul>

<h3>Authentication Problems</h3>
<h4>Symptoms</h4>
<ul>
    <li>Login redirect loops</li>
    <li>""Access denied"" errors</li>
    <li>Token validation failures</li>
</ul>
<h4>Solutions</h4>
<ul>
    <li>Verify callback URLs in identity provider</li>
    <li>Check token expiration settings</li>
    <li>Ensure proper SSL configuration</li>
    <li>Review cookie settings for multi-site</li>
</ul>

<h3>Search Issues</h3>
<h4>Symptoms</h4>
<ul>
    <li>Content not appearing in search results</li>
    <li>Search returns outdated content</li>
    <li>Index errors in logs</li>
</ul>
<h4>Solutions</h4>
<ul>
    <li>Trigger manual reindex</li>
    <li>Check Search & Navigation service status</li>
    <li>Verify content is published</li>
    <li>Review indexing conventions</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "ts-debugging",
                    ModuleId = "troubleshooting",
                    Title = "Debugging Techniques",
                    Summary = "Learn effective debugging techniques for DXP applications.",
                    Order = 2,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Use Application Insights effectively",
                        "Analyze logs and traces",
                        "Debug production issues safely"
                    },
                    Content = @"
<h2>Debugging Techniques</h2>
<p>Effective debugging in DXP requires using the right tools and techniques.</p>

<h3>Application Insights</h3>
<h4>Live Metrics</h4>
<ul>
    <li>Real-time request and failure rates</li>
    <li>Current server performance</li>
    <li>Live log stream</li>
</ul>

<h4>Transaction Search</h4>
<ul>
    <li>Find specific requests by properties</li>
    <li>View full request details and dependencies</li>
    <li>Correlate related telemetry</li>
</ul>

<h4>Failure Analysis</h4>
<ul>
    <li>Group exceptions by type</li>
    <li>View stack traces and context</li>
    <li>Track failure trends</li>
</ul>

<h3>Log Analysis</h3>
<h4>DXP Portal Logs</h4>
<ul>
    <li>Access application logs via portal</li>
    <li>Download for offline analysis</li>
    <li>Stream live logs during debugging</li>
</ul>

<h4>Log Levels</h4>
<p>Temporarily increase log levels for debugging:</p>
<ul>
    <li>Integration: Self-service change</li>
    <li>Higher environments: Contact support</li>
</ul>

<h3>Remote Debugging</h3>
<div class=""bg-yellow-50 dark:bg-yellow-900/20 border-l-4 border-yellow-400 p-4 my-4"">
    <p class=""text-yellow-800 dark:text-yellow-200""><strong>Caution:</strong> Remote debugging is available but should be used carefully in production as it can impact performance.</p>
</div>

<h3>Diagnostic Settings</h3>
<p>Request from support (for higher environments):</p>
<ul>
    <li>Enable web server logs</li>
    <li>Enable failed request tracing</li>
    <li>Enable detailed error pages</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "ts-support",
                    ModuleId = "troubleshooting",
                    Title = "Getting Support",
                    Summary = "Learn how to effectively engage Optimizely support.",
                    Order = 3,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Know when to contact support",
                        "Provide effective problem reports",
                        "Understand support resources"
                    },
                    Content = @"
<h2>Getting Support</h2>
<p>Know when and how to engage Optimizely support effectively.</p>

<h3>Support Channels</h3>
<ul>
    <li><strong>Support Portal</strong> - <a href=""https://support.optimizely.com"">support.optimizely.com</a></li>
    <li><strong>Community Forums</strong> - <a href=""https://world.optimizely.com"">Optimizely World</a></li>
    <li><strong>Documentation</strong> - Developer docs and user guides</li>
</ul>

<h3>When to Contact Support</h3>
<h4>Immediately</h4>
<ul>
    <li>Production site is down</li>
    <li>Security incident suspected</li>
    <li>Data loss or corruption</li>
</ul>

<h4>Standard Request</h4>
<ul>
    <li>Configuration changes (Preprod/Prod logging)</li>
    <li>Environment provisioning</li>
    <li>Feature questions</li>
</ul>

<h3>Effective Problem Reports</h3>
<p>Include in your support ticket:</p>
<ul>
    <li><strong>Environment</strong> - Which environment is affected</li>
    <li><strong>Reproduction steps</strong> - How to reproduce the issue</li>
    <li><strong>Expected vs actual</strong> - What should happen vs what happens</li>
    <li><strong>Timing</strong> - When did the issue start</li>
    <li><strong>Changes</strong> - What changed recently</li>
    <li><strong>Logs/screenshots</strong> - Supporting evidence</li>
</ul>

<h3>Self-Service First</h3>
<p>Before contacting support, check:</p>
<ul>
    <li>Service Health dashboard</li>
    <li>Known issues on Optimizely World</li>
    <li>Application logs for obvious errors</li>
    <li>Recent deployment or configuration changes</li>
</ul>
"
                }
            }
        };
    }

    #endregion
}
