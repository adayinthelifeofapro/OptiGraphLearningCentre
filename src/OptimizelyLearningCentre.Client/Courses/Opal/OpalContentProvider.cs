using OptimizelyLearningCentre.Client.Models.Learning;
using OptimizelyLearningCentre.Client.Services;

namespace OptimizelyLearningCentre.Client.Courses.Opal;

/// <summary>
/// Content provider for the Optimizely Opal course
/// </summary>
public class OpalContentProvider : ILearningContentProvider
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
            BuildOpalChatModule(),
            BuildCanvasModule(),
            BuildContentGenerationModule(),
            BuildImageGenerationModule(),
            BuildInstructionsModule(),
            BuildWorkingWithAgentsModule(),
            BuildSpecializedAgentsModule(),
            BuildToolsIntegrationsModule(),
            BuildExperimentationModule()
        };
    }

    #region Module 1: Getting Started with Opal

    private LearningModule BuildGettingStartedModule()
    {
        return new LearningModule
        {
            Id = "getting-started",
            Title = "Getting Started with Opal",
            Description = "Learn the fundamentals of Optimizely Opal, the AI-powered agent orchestration platform.",
            Icon = "academic-cap",
            Order = 1,
            Difficulty = ModuleDifficulty.Beginner,
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "gs-what-is-opal",
                    ModuleId = "getting-started",
                    Title = "What is Optimizely Opal?",
                    Summary = "Discover Optimizely Opal and how it transforms your marketing workflows with AI.",
                    Order = 1,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Understand what Optimizely Opal is and its purpose",
                        "Learn how Opal integrates across Optimizely One",
                        "Discover the key benefits of using Opal"
                    },
                    Content = @"
<h2>Introduction to Optimizely Opal</h2>
<p>Optimizely Opal is an <strong>agent orchestration platform</strong> that helps you work smarter across Optimizely One. Whether you create content, manage experiments, or analyze data, you can use Opal to automate tasks, surface insights, and guide decision-making.</p>

<h3>What Makes Opal Special?</h3>
<p>Opal is more than just an AI assistant. It's a flexible system of AI agents that are intelligent software components that understand your intent and help you reach your goal. Key characteristics include:</p>
<ul>
    <li><strong>Directly connected to your workflows</strong> - Opal integrates natively with all Optimizely products</li>
    <li><strong>Fully aware of your brand</strong> - Configure Opal to understand your brand guidelines, tone, and style</li>
    <li><strong>Powered by hundreds of tools</strong> - From content creation to analytics, Opal has tools for every task</li>
</ul>

<h3>Privacy and Security</h3>
<p>Opal runs on Google Gemini through a secure business integration. This means:</p>
<ul>
    <li>Your data stays private</li>
    <li>Your information isn't used to train public models</li>
    <li>No user profiles are built from your interactions</li>
</ul>

<h3>Key Benefits</h3>
<p>According to the Optimizely Opal AI Benchmark Report:</p>
<ul>
    <li><strong>17.1%</strong> increase in campaign volume</li>
    <li><strong>12.8%</strong> increase in task volume</li>
    <li><strong>53.7%</strong> reduction in campaign completion time</li>
    <li><strong>7.4%</strong> improvement in content engagement</li>
</ul>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "gs-example-1",
                            Title = "Asking Opal About Itself",
                            Description = "Try asking Opal to introduce itself and explain its capabilities.",
                            Type = ExampleType.Prompt,
                            ExampleContent ="What can you help me do in Optimizely?",
                            SampleResponse = "I can help you with many tasks across Optimizely One! Here are some things I can assist with:\n\n• **Content Creation**: Generate marketing copy, product descriptions, blog posts, and more\n• **Image Generation**: Create and edit images for your campaigns\n• **Campaign Management**: Help plan, create, and optimize campaigns\n• **Experimentation**: Design A/B tests and analyze results\n• **Analytics**: Generate reports and extract insights from your data\n• **Workflows**: Automate repetitive tasks and streamline processes\n\nJust ask me what you need help with!",
                            Hints = new List<string>
                            {
                                "Opal can explain any Optimizely feature or concept",
                                "Try being specific about what product or feature you want to learn about"
                            }
                        }
                    }
                },
                new Lesson
                {
                    Id = "gs-architecture",
                    ModuleId = "getting-started",
                    Title = "Opal Architecture",
                    Summary = "Understand how Opal's agent orchestration platform works.",
                    Order = 2,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Understand the concept of agent orchestration",
                        "Learn about the different types of agents",
                        "Understand how tools, instructions, and agents work together"
                    },
                    Content = @"
<h2>Agent Orchestration Platform</h2>
<p>At its core, Opal is an <strong>agent orchestration platform</strong>. This means it coordinates multiple AI agents to accomplish complex tasks that would be difficult for a single AI to handle.</p>

<h3>Core Components</h3>

<h4>1. Opal Chat</h4>
<p>The primary interface for interacting with Opal. Click ""Ask Opal"" to open Opal Chat, where you can:</p>
<ul>
    <li>Have natural language conversations</li>
    <li>Request help with tasks</li>
    <li>Call specialized agents using @mentions</li>
</ul>

<h4>2. Agents</h4>
<p>Intelligent software components that understand your intent. There are several types:</p>
<ul>
    <li><strong>Default Agents</strong> - Pre-built by Optimizely for common tasks</li>
    <li><strong>Specialized Agents</strong> - Custom agents you create for specific tasks</li>
    <li><strong>Workflow Agents</strong> - Multi-step agents for complex processes</li>
</ul>

<h4>3. Tools</h4>
<p>Individual capabilities that agents use to complete tasks. Think of tools like attachments on a Swiss Army knife - each one has a distinct purpose. Examples include:</p>
<ul>
    <li>Create a campaign</li>
    <li>Upload files</li>
    <li>Generate images</li>
    <li>Search the web</li>
</ul>

<h4>4. Instructions</h4>
<p>Customizable AI behaviors that guide how Opal responds. Instructions let you:</p>
<ul>
    <li>Define your brand voice and tone</li>
    <li>Set content guidelines</li>
    <li>Configure response patterns</li>
</ul>

<h4>5. Canvas</h4>
<p>An interactive workspace where you and Opal collaboratively edit content. Canvases support version control and can hold various content types from documentation to code.</p>

<h3>How It All Works Together</h3>
<p>When you make a request to Opal:</p>
<ol>
    <li>Your request is analyzed to understand intent</li>
    <li>Relevant instructions shape the response approach</li>
    <li>Appropriate agents are activated</li>
    <li>Tools are used to complete the task</li>
    <li>Results are delivered in your Canvas or Chat</li>
</ol>
"
                },
                new Lesson
                {
                    Id = "gs-accessing-opal",
                    ModuleId = "getting-started",
                    Title = "Accessing Opal",
                    Summary = "Learn how to access Opal and understand the credits system.",
                    Order = 3,
                    EstimatedMinutes = 7,
                    LearningObjectives = new List<string>
                    {
                        "Understand Opti ID requirements",
                        "Learn about the credits-based billing model",
                        "Know where to find Opal across Optimizely products"
                    },
                    Content = @"
<h2>Accessing Optimizely Opal</h2>

<h3>Opti ID Requirement</h3>
<p>To access Opal features, you must use <strong>Opti ID</strong> - Optimizely's unified identity system. This ensures:</p>
<ul>
    <li>Secure authentication across all Optimizely products</li>
    <li>Proper tracking of your Opal usage</li>
    <li>Access to your personalized settings and history</li>
</ul>

<h3>Credits-Based Usage Model</h3>
<p>As of May 2025, Opal uses a <strong>credit-based usage and billing model</strong>. Key points:</p>
<ul>
    <li>Usage of Opal features incurs credit charges</li>
    <li>Credits apply across Content Marketing Platform, Web Experimentation, Feature Experimentation, Personalization, CMS (SaaS), Collaboration, and Optimizely Data Platform</li>
    <li>You can monitor your credit usage in the Opal admin interface</li>
</ul>

<h3>Where to Find Opal</h3>
<p>Opal is available across Optimizely One products. Look for the <strong>""Ask Opal""</strong> button, typically found in:</p>
<ul>
    <li>The main navigation or toolbar</li>
    <li>Content editing interfaces</li>
    <li>Campaign management screens</li>
    <li>Experimentation dashboards</li>
</ul>

<h3>Opal in Slack</h3>
<p>Opal is also available in Slack, allowing you to:</p>
<ul>
    <li>Create and manage campaigns</li>
    <li>Generate and edit images</li>
    <li>Analyze experiment results</li>
    <li>Execute AI agents</li>
</ul>
<p>All from within your team's communication platform!</p>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "gs-access-example",
                            Title = "Check Your Credits",
                            Description = "Ask Opal about your credit usage.",
                            Type = ExampleType.Prompt,
                            ExampleContent ="How can I check my Opal credit usage?",
                            SampleResponse = "You can check your Opal credit usage in the admin interface. Navigate to the Tools page where you'll see the total count of tools enabled and your credit usage per agent. Your administrator can also generate Agent Usage Reports to monitor credit consumption across your organization."
                        }
                    }
                },
                new Lesson
                {
                    Id = "gs-first-interaction",
                    ModuleId = "getting-started",
                    Title = "Your First Opal Interaction",
                    Summary = "Start using Opal with your first prompts and interactions.",
                    Order = 4,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Open and navigate Opal Chat",
                        "Write effective prompts",
                        "Understand response patterns"
                    },
                    Content = @"
<h2>Your First Interaction with Opal</h2>

<h3>Opening Opal Chat</h3>
<p>To start using Opal:</p>
<ol>
    <li>Click the <strong>""Ask Opal""</strong> button in your Optimizely interface</li>
    <li>The Opal Chat panel will open</li>
    <li>Type your request in the input field</li>
    <li>Press Enter or click Send</li>
</ol>

<h3>Writing Effective Prompts</h3>
<p>The better your prompt, the better Opal's response. Follow these guidelines:</p>

<h4>Be Specific</h4>
<p>Instead of: ""Write some content""<br/>
Try: ""Write a product description for a blue running shoe targeting fitness enthusiasts, emphasizing comfort and durability""</p>

<h4>Provide Context</h4>
<p>Include relevant information like:</p>
<ul>
    <li>Target audience</li>
    <li>Tone and style preferences</li>
    <li>Length requirements</li>
    <li>Format specifications</li>
</ul>

<h4>Use Clear Instructions</h4>
<p>Start with an action verb:</p>
<ul>
    <li>""Create..."", ""Generate..."", ""Analyze...""</li>
    <li>""Summarize..."", ""Compare..."", ""Explain...""</li>
    <li>""Translate..."", ""Rewrite..."", ""Optimize...""</li>
</ul>

<h3>Understanding Responses</h3>
<p>Opal's responses may include:</p>
<ul>
    <li><strong>Direct answers</strong> - Information you requested</li>
    <li><strong>Generated content</strong> - Text, images, or other assets</li>
    <li><strong>Follow-up questions</strong> - Clarifications to improve results</li>
    <li><strong>Tool actions</strong> - Operations performed on your behalf</li>
</ul>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "gs-first-prompt",
                            Title = "A Good First Prompt",
                            Description = "Try this example prompt to see Opal in action.",
                            Type = ExampleType.Prompt,
                            ExampleContent ="Create 3 email subject lines for a summer sale campaign targeting young professionals. The tone should be exciting but professional.",
                            SampleResponse = "Here are 3 email subject lines for your summer sale campaign:\n\n1. **Your Summer Upgrade Awaits** - Score 40% Off Professional Essentials\n\n2. **Hot Deals, Cool Style** - Exclusive Summer Savings for Career Builders\n\n3. **Summer's Here & So Are the Savings** - Don't Miss This Limited-Time Offer\n\nWould you like me to create more variations or adjust the tone?"
                        }
                    }
                }
            }
        };
    }

    #endregion

    #region Module 2: Opal Chat Fundamentals

    private LearningModule BuildOpalChatModule()
    {
        return new LearningModule
        {
            Id = "opal-chat",
            Title = "Opal Chat Fundamentals",
            Description = "Master the primary interface for interacting with Optimizely Opal.",
            Icon = "chat-bubble-left-right",
            Order = 2,
            Difficulty = ModuleDifficulty.Beginner,
            Prerequisites = new[] { "getting-started" },
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "chat-intro",
                    ModuleId = "opal-chat",
                    Title = "Introduction to Opal Chat",
                    Summary = "Learn the basics of the Opal Chat interface and its capabilities.",
                    Order = 1,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Navigate the Opal Chat interface",
                        "Understand the different capabilities available",
                        "Learn how to access chat history"
                    },
                    Content = @"
<h2>Opal Chat Interface</h2>
<p>Opal Chat is your primary way to interact with Optimizely's AI capabilities. It provides a natural language interface for accomplishing tasks across the platform.</p>

<h3>Chat Interface Elements</h3>
<ul>
    <li><strong>Message Input</strong> - Where you type your requests</li>
    <li><strong>Chat History</strong> - Previous conversations and responses</li>
    <li><strong>Agent Mentions</strong> - @mention to call specific agents</li>
    <li><strong>File Attachments</strong> - Upload documents for analysis</li>
</ul>

<h3>Core Capabilities</h3>
<p>With Opal Chat, you can:</p>
<ul>
    <li><strong>Ideate and prompt</strong> - Research, brainstorm, create, summarize</li>
    <li><strong>Use tools</strong> - Create campaigns, draft content, analyze images, suggest variations</li>
    <li><strong>Analyze files</strong> - Upload documents, images, or spreadsheets and ask questions</li>
    <li><strong>Answer questions</strong> - Get help with Optimizely features and concepts</li>
    <li><strong>Translate content</strong> - Convert text to different languages or tones</li>
    <li><strong>Research online</strong> - Search the web and summarize content</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "chat-prompting",
                    ModuleId = "opal-chat",
                    Title = "Prompting Best Practices",
                    Summary = "Learn how to write effective prompts for optimal results.",
                    Order = 2,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Write clear, effective prompts",
                        "Use prompt techniques to improve results",
                        "Iterate on prompts for better outcomes"
                    },
                    Content = @"
<h2>Writing Effective Prompts</h2>
<p>The quality of your prompts directly impacts the quality of Opal's responses. Here are proven techniques for better results.</p>

<h3>The CRAFT Framework</h3>
<ul>
    <li><strong>C</strong>ontext - Provide background information</li>
    <li><strong>R</strong>ole - Define what perspective Opal should take</li>
    <li><strong>A</strong>ction - Clearly state what you want done</li>
    <li><strong>F</strong>ormat - Specify the desired output format</li>
    <li><strong>T</strong>one - Indicate the style or voice</li>
</ul>

<h3>Prompt Techniques</h3>

<h4>Be Specific</h4>
<p>Include details about length, format, audience, and purpose.</p>

<h4>Use Examples</h4>
<p>Show Opal what you're looking for with sample outputs.</p>

<h4>Break Down Complex Tasks</h4>
<p>Divide large requests into smaller, manageable steps.</p>

<h4>Iterate and Refine</h4>
<p>Use follow-up prompts to refine results: ""Make it shorter"", ""Add more detail about..."", ""Change the tone to...""</p>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "chat-craft-example",
                            Title = "Using the CRAFT Framework",
                            Description = "See how the CRAFT framework improves prompt quality.",
                            Type = ExampleType.Prompt,
                            ExampleContent =@"Context: We're launching a new sustainable clothing line called ""EcoWear"" targeting millennials.
Role: Act as an experienced marketing copywriter.
Action: Write a hero section headline and subheadline for our landing page.
Format: Provide 3 options, each with a headline (max 8 words) and subheadline (max 20 words).
Tone: Inspiring, eco-conscious, but not preachy.",
                            SampleResponse = @"Here are 3 hero section options for EcoWear:

**Option 1**
Headline: Wear Your Values, Literally
Subheadline: Premium sustainable fashion that looks good, feels good, and does good for the planet.

**Option 2**
Headline: Fashion Forward, Earth First
Subheadline: Join the movement where style meets sustainability. Because your wardrobe choices matter.

**Option 3**
Headline: Good Clothes for Good People
Subheadline: Ethically made, beautifully designed clothing for those who care about tomorrow."
                        }
                    }
                },
                new Lesson
                {
                    Id = "chat-research",
                    ModuleId = "opal-chat",
                    Title = "Research & Web Search",
                    Summary = "Use Opal to research topics and extract information from the web.",
                    Order = 3,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Use Opal for web research",
                        "Extract key information from URLs",
                        "Summarize and analyze online content"
                    },
                    Content = @"
<h2>Research with Opal</h2>
<p>Opal can search the web, summarize content, and extract key information from provided URLs.</p>

<h3>Web Search Capabilities</h3>
<ul>
    <li>Search for current information on any topic</li>
    <li>Find competitor information and market trends</li>
    <li>Research industry best practices</li>
    <li>Discover relevant statistics and data</li>
</ul>

<h3>URL Analysis</h3>
<p>Provide a URL and ask Opal to:</p>
<ul>
    <li>Summarize the page content</li>
    <li>Extract specific information</li>
    <li>Analyze the content structure</li>
    <li>Compare with other pages</li>
</ul>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "chat-research-example",
                            Title = "Research Request",
                            Description = "Ask Opal to research a topic.",
                            Type = ExampleType.Prompt,
                            ExampleContent ="Research the top 5 trends in email marketing for 2025. Provide a brief summary of each trend.",
                            SampleResponse = @"Here are the top 5 email marketing trends for 2025:

1. **AI-Powered Personalization** - Using AI to create hyper-personalized content based on individual behavior patterns and preferences.

2. **Interactive Emails** - Embedding forms, carousels, and mini-apps directly in emails for engagement without clicking through.

3. **Privacy-First Strategies** - Adapting to stricter data regulations with first-party data collection and transparent practices.

4. **Predictive Send Times** - AI optimization of delivery times based on individual recipient behavior patterns.

5. **AMP for Email** - Dynamic, real-time content updates within emails for live inventory, pricing, and content."
                        }
                    }
                },
                new Lesson
                {
                    Id = "chat-files",
                    ModuleId = "opal-chat",
                    Title = "File Analysis & Uploads",
                    Summary = "Learn how to upload and analyze files with Opal.",
                    Order = 4,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Upload files to Opal Chat",
                        "Analyze different file types",
                        "Extract insights from documents"
                    },
                    Content = @"
<h2>Working with Files</h2>
<p>Opal can analyze various file types to help you extract insights, answer questions, and work with your content.</p>

<h3>Supported File Types</h3>
<p>Opal supports many file types including:</p>
<ul>
    <li><strong>Documents</strong> - PDF, Word, text files</li>
    <li><strong>Spreadsheets</strong> - Excel, CSV files</li>
    <li><strong>Images</strong> - PNG, JPEG, GIF</li>
    <li><strong>Code</strong> - Various programming languages</li>
</ul>

<h3>What You Can Do</h3>
<ul>
    <li>Ask questions about document content</li>
    <li>Summarize lengthy documents</li>
    <li>Extract data from spreadsheets</li>
    <li>Analyze images and describe their content</li>
    <li>Review and explain code</li>
</ul>

<h3>How to Upload</h3>
<ol>
    <li>Click the attachment icon in Opal Chat</li>
    <li>Select your file</li>
    <li>Add your question or request about the file</li>
    <li>Send your message</li>
</ol>
"
                },
                new Lesson
                {
                    Id = "chat-help",
                    ModuleId = "opal-chat",
                    Title = "Getting Help with Optimizely Features",
                    Summary = "Use Opal to learn about Optimizely features and get guidance.",
                    Order = 5,
                    EstimatedMinutes = 6,
                    LearningObjectives = new List<string>
                    {
                        "Ask Opal about Optimizely features",
                        "Get step-by-step guidance",
                        "Find relevant documentation"
                    },
                    Content = @"
<h2>Opal as Your Guide</h2>
<p>Opal can explain Optimizely features, walk you through configuration steps, and point you to relevant documentation.</p>

<h3>What You Can Ask</h3>
<ul>
    <li>How to use specific features</li>
    <li>Best practices for configuration</li>
    <li>Troubleshooting common issues</li>
    <li>Feature comparisons and recommendations</li>
</ul>

<h3>Getting Step-by-Step Help</h3>
<p>Ask Opal to guide you through processes like:</p>
<ul>
    <li>Setting up a new campaign</li>
    <li>Creating an A/B test</li>
    <li>Configuring personalization rules</li>
    <li>Managing content workflows</li>
</ul>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "chat-help-example",
                            Title = "Ask for Feature Help",
                            Description = "Get help with an Optimizely feature.",
                            Type = ExampleType.Prompt,
                            ExampleContent ="How do I create my first A/B test in Web Experimentation?",
                            SampleResponse = @"Here's how to create your first A/B test:

1. **Navigate to Experiments** - Go to the Experiments section in your project

2. **Create New Experiment** - Click ""Create New"" and select ""A/B Test""

3. **Name Your Experiment** - Give it a descriptive name

4. **Set the URL** - Enter the page URL where you want to run the test

5. **Create Variations** - Use the Visual Editor to make changes:
   - Click elements to modify
   - Change text, colors, or layouts
   - Add or remove elements

6. **Configure Goals** - Set up what you want to measure (clicks, conversions, etc.)

7. **Set Traffic Allocation** - Decide what percentage of visitors see each variation

8. **Start the Experiment** - Review and launch!

Would you like more details on any of these steps?"
                        }
                    }
                }
            }
        };
    }

    #endregion

    #region Module 3: Canvas & Collaboration

    private LearningModule BuildCanvasModule()
    {
        return new LearningModule
        {
            Id = "canvas",
            Title = "Canvas & Collaboration",
            Description = "Learn to use Canvas for collaborative content creation with Opal.",
            Icon = "document-text",
            Order = 3,
            Difficulty = ModuleDifficulty.Beginner,
            Prerequisites = new[] { "opal-chat" },
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "canvas-overview",
                    ModuleId = "canvas",
                    Title = "Canvas Overview",
                    Summary = "Understand what Canvas is and how it enables collaborative editing.",
                    Order = 1,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Understand what Canvas is",
                        "Learn about the active workspace concept",
                        "Know when to use Canvas vs Chat"
                    },
                    Content = @"
<h2>Introduction to Canvas</h2>
<p>Canvas is an interactive AI workspace where you and Opal collaboratively edit content in real-time. It's version-controlled and can hold various types of content, from documentation to code.</p>

<h3>What is Canvas?</h3>
<p>Think of Canvas as a shared document where:</p>
<ul>
    <li>You and Opal can both make edits</li>
    <li>Changes are tracked with version history</li>
    <li>Content persists across sessions</li>
    <li>Multiple canvases can be created and managed</li>
</ul>

<h3>Canvas vs Chat</h3>
<table class=""min-w-full divide-y divide-gray-200 dark:divide-gray-700"">
    <thead>
        <tr>
            <th class=""px-4 py-2 text-left"">Feature</th>
            <th class=""px-4 py-2 text-left"">Chat</th>
            <th class=""px-4 py-2 text-left"">Canvas</th>
        </tr>
    </thead>
    <tbody>
        <tr><td class=""px-4 py-2"">Quick responses</td><td class=""px-4 py-2"">✓</td><td class=""px-4 py-2""></td></tr>
        <tr><td class=""px-4 py-2"">Long-form content</td><td class=""px-4 py-2""></td><td class=""px-4 py-2"">✓</td></tr>
        <tr><td class=""px-4 py-2"">Version history</td><td class=""px-4 py-2""></td><td class=""px-4 py-2"">✓</td></tr>
        <tr><td class=""px-4 py-2"">Collaborative editing</td><td class=""px-4 py-2""></td><td class=""px-4 py-2"">✓</td></tr>
        <tr><td class=""px-4 py-2"">Persistent storage</td><td class=""px-4 py-2""></td><td class=""px-4 py-2"">✓</td></tr>
    </tbody>
</table>
"
                },
                new Lesson
                {
                    Id = "canvas-creating",
                    ModuleId = "canvas",
                    Title = "Creating & Managing Canvases",
                    Summary = "Learn how to create, organize, and manage your canvases.",
                    Order = 2,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Create new canvases",
                        "Organize multiple canvases",
                        "Access and manage existing canvases"
                    },
                    Content = @"
<h2>Working with Canvases</h2>

<h3>Creating a Canvas</h3>
<p>You can create a canvas by:</p>
<ul>
    <li>Asking Opal: ""Create a canvas for [topic]""</li>
    <li>Using the create_canvas tool directly</li>
    <li>Having Opal generate content that automatically creates a canvas</li>
</ul>

<h3>Canvas Tools</h3>
<p>Opal has specific tools for canvas management:</p>
<ul>
    <li><strong>create_canvas</strong> - Creates a new interactive canvas</li>
    <li><strong>update_canvas</strong> - Modifies existing canvas content</li>
    <li><strong>list_canvases</strong> - Shows all your canvases</li>
    <li><strong>delete_canvas</strong> - Removes a canvas</li>
</ul>

<h3>Best Practices</h3>
<ul>
    <li>Use descriptive names for easy reference</li>
    <li>One canvas per project or document</li>
    <li>Regularly review and clean up old canvases</li>
</ul>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "canvas-create-example",
                            Title = "Create a Marketing Brief Canvas",
                            Description = "Ask Opal to create a canvas for a marketing brief.",
                            Type = ExampleType.Prompt,
                            ExampleContent ="Create a canvas for our Q2 product launch marketing brief. Include sections for objectives, target audience, key messages, and timeline.",
                            SampleResponse = @"I've created a new canvas called ""Q2 Product Launch Marketing Brief"". Here's the initial structure:

# Q2 Product Launch Marketing Brief

## Objectives
[Define your primary marketing goals for the launch]

## Target Audience
### Primary Audience
- Demographics:
- Psychographics:
- Pain points:

### Secondary Audience
- Demographics:
- Psychographics:
- Pain points:

## Key Messages
1. Primary message:
2. Supporting messages:

## Timeline
| Phase | Dates | Activities |
|-------|-------|------------|
| Pre-launch | | |
| Launch | | |
| Post-launch | | |

Would you like me to help fill in any of these sections?"
                        }
                    }
                },
                new Lesson
                {
                    Id = "canvas-collaboration",
                    ModuleId = "canvas",
                    Title = "Collaborative Editing",
                    Summary = "Work together with Opal to refine and improve content.",
                    Order = 3,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Edit canvas content collaboratively",
                        "Request specific changes from Opal",
                        "Iterate on content efficiently"
                    },
                    Content = @"
<h2>Collaborating with Opal on Canvas</h2>

<h3>How Collaboration Works</h3>
<p>Canvas enables a back-and-forth workflow:</p>
<ol>
    <li>You create or open a canvas</li>
    <li>Make your own edits directly</li>
    <li>Ask Opal to enhance, expand, or modify</li>
    <li>Review and iterate</li>
</ol>

<h3>Effective Collaboration Requests</h3>
<ul>
    <li>""Expand the section on [topic]""</li>
    <li>""Rewrite paragraph 3 in a more casual tone""</li>
    <li>""Add examples to the best practices section""</li>
    <li>""Make the introduction more compelling""</li>
    <li>""Fix any grammar or spelling issues""</li>
</ul>

<h3>Tips for Better Collaboration</h3>
<ul>
    <li>Be specific about what you want changed</li>
    <li>Reference specific sections or paragraphs</li>
    <li>Provide context for why changes are needed</li>
    <li>Use iteration - small changes are easier to review</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "canvas-versions",
                    ModuleId = "canvas",
                    Title = "Version Control in Canvas",
                    Summary = "Track changes and restore previous versions of your content.",
                    Order = 4,
                    EstimatedMinutes = 6,
                    LearningObjectives = new List<string>
                    {
                        "View version history",
                        "Restore previous versions",
                        "Compare different versions"
                    },
                    Content = @"
<h2>Version Control</h2>
<p>Canvas automatically tracks all changes, allowing you to review history and restore previous versions.</p>

<h3>What's Tracked</h3>
<ul>
    <li>All content changes</li>
    <li>Who made the change (you or Opal)</li>
    <li>Timestamps for each change</li>
</ul>

<h3>Using Version History</h3>
<p>You can:</p>
<ul>
    <li>View the history of changes</li>
    <li>Compare different versions</li>
    <li>Restore any previous version</li>
    <li>See what Opal changed</li>
</ul>

<h3>Best Practices</h3>
<ul>
    <li>Review changes before accepting them</li>
    <li>Save important milestones</li>
    <li>Don't be afraid to try things - you can always revert</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "canvas-use-cases",
                    ModuleId = "canvas",
                    Title = "Canvas Use Cases",
                    Summary = "Explore practical applications of Canvas in marketing workflows.",
                    Order = 5,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Identify good use cases for Canvas",
                        "Apply Canvas to marketing workflows",
                        "Maximize productivity with Canvas"
                    },
                    Content = @"
<h2>Practical Canvas Use Cases</h2>

<h3>Marketing Briefs</h3>
<p>Create comprehensive campaign briefs with:</p>
<ul>
    <li>Objectives and KPIs</li>
    <li>Target audience profiles</li>
    <li>Messaging frameworks</li>
    <li>Timeline and milestones</li>
</ul>

<h3>Content Creation</h3>
<p>Use Canvas for:</p>
<ul>
    <li>Blog posts and articles</li>
    <li>Landing page copy</li>
    <li>Email sequences</li>
    <li>Social media content calendars</li>
</ul>

<h3>Documentation</h3>
<p>Build and maintain:</p>
<ul>
    <li>Process documentation</li>
    <li>Style guides</li>
    <li>Standard operating procedures</li>
    <li>Training materials</li>
</ul>

<h3>Strategy Development</h3>
<p>Collaborate on:</p>
<ul>
    <li>Campaign strategies</li>
    <li>Competitive analysis</li>
    <li>Audience research</li>
    <li>Performance reports</li>
</ul>
"
                }
            }
        };
    }

    #endregion

    #region Module 4: Content Generation

    private LearningModule BuildContentGenerationModule()
    {
        return new LearningModule
        {
            Id = "content-generation",
            Title = "Content Generation",
            Description = "Master AI-powered content creation for marketing channels.",
            Icon = "pencil-square",
            Order = 4,
            Difficulty = ModuleDifficulty.Intermediate,
            Prerequisites = new[] { "opal-chat" },
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "content-marketing",
                    ModuleId = "content-generation",
                    Title = "Generating Marketing Content",
                    Summary = "Create compelling marketing content with Opal.",
                    Order = 1,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Generate various types of marketing content",
                        "Use Opal for copywriting tasks",
                        "Create content for multiple channels"
                    },
                    Content = @"
<h2>Marketing Content with Opal</h2>
<p>Opal excels at generating marketing content across channels including web, email, and social media. It adapts outputs to suit different formats and platforms.</p>

<h3>Types of Content Opal Can Generate</h3>
<ul>
    <li>Headlines and taglines</li>
    <li>Product descriptions</li>
    <li>Marketing emails</li>
    <li>Blog posts and articles</li>
    <li>Social media posts</li>
    <li>Ad copy</li>
    <li>Landing page content</li>
    <li>Newsletter content</li>
</ul>

<h3>Content Generation Process</h3>
<ol>
    <li>Define your goal and audience</li>
    <li>Provide context about brand and tone</li>
    <li>Request specific content type</li>
    <li>Review and iterate</li>
    <li>Refine with follow-up prompts</li>
</ol>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "content-email-example",
                            Title = "Generate an Email Campaign",
                            Description = "Create a promotional email with Opal.",
                            Type = ExampleType.Prompt,
                            ExampleContent ="Write a promotional email for our annual Black Friday sale. We're offering 40% off all products. Target audience is existing customers. Keep it concise and urgent.",
                            SampleResponse = @"**Subject:** Your 40% OFF Awaits - Black Friday Starts NOW

Hi [First Name],

The wait is over. Our biggest sale of the year is LIVE.

**40% OFF EVERYTHING**
No exceptions. No minimum purchase.

As one of our valued customers, you get first access before we open the doors to everyone else.

**[SHOP THE SALE]**

This is the lowest our prices will be all year. Once it's gone, it's gone.

Sale ends Monday at midnight.

See you there,
The [Brand] Team

P.S. Use code BLACKFRIDAY40 at checkout. Your exclusive access expires in 24 hours."
                        }
                    }
                },
                new Lesson
                {
                    Id = "content-seo",
                    ModuleId = "content-generation",
                    Title = "SEO-Optimized Headlines & Descriptions",
                    Summary = "Create content optimized for search engines.",
                    Order = 2,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Generate SEO-friendly content",
                        "Optimize headlines and meta descriptions",
                        "Understand SEO best practices for content"
                    },
                    Content = @"
<h2>SEO Content Generation</h2>
<p>Opal can generate SEO-optimized content including headlines, meta descriptions, and product descriptions that rank well in search engines.</p>

<h3>SEO Elements Opal Can Help With</h3>
<ul>
    <li><strong>Page Titles</strong> - Compelling, keyword-rich titles under 60 characters</li>
    <li><strong>Meta Descriptions</strong> - Engaging summaries under 160 characters</li>
    <li><strong>H1/H2 Headlines</strong> - Structured headings with target keywords</li>
    <li><strong>Product Descriptions</strong> - Unique, detailed descriptions for e-commerce</li>
    <li><strong>Blog Content</strong> - In-depth articles optimized for target keywords</li>
</ul>

<h3>Tips for SEO Content</h3>
<ul>
    <li>Include your target keywords in prompts</li>
    <li>Specify character limits for meta content</li>
    <li>Request multiple variations to A/B test</li>
    <li>Ask Opal to include related keywords naturally</li>
</ul>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "content-seo-example",
                            Title = "Generate SEO Meta Content",
                            Description = "Create optimized title and meta description.",
                            Type = ExampleType.Prompt,
                            ExampleContent ="Create an SEO-optimized page title (under 60 characters) and meta description (under 160 characters) for a page about 'best running shoes for beginners'. Include a call to action.",
                            SampleResponse = @"**Page Title (58 characters):**
Best Running Shoes for Beginners 2025 | Top Picks & Guide

**Meta Description (156 characters):**
Discover the best running shoes for beginners. Expert reviews, comfort ratings & buying tips to find your perfect first pair. Shop our top picks today!"
                        }
                    }
                },
                new Lesson
                {
                    Id = "content-multichannel",
                    ModuleId = "content-generation",
                    Title = "Multi-Channel Content Adaptation",
                    Summary = "Adapt content for different marketing channels.",
                    Order = 3,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Adapt content for different platforms",
                        "Understand channel-specific requirements",
                        "Create cohesive multi-channel campaigns"
                    },
                    Content = @"
<h2>Adapting Content Across Channels</h2>
<p>Opal can take a single piece of content and adapt it for multiple channels, maintaining your message while optimizing for each platform's requirements.</p>

<h3>Channel Considerations</h3>
<ul>
    <li><strong>Social Media</strong> - Character limits, hashtags, platform tone</li>
    <li><strong>Email</strong> - Subject lines, preview text, body length</li>
    <li><strong>Web</strong> - SEO, readability, CTAs</li>
    <li><strong>Ads</strong> - Character limits, headlines, descriptions</li>
</ul>

<h3>Adaptation Workflow</h3>
<ol>
    <li>Create your core message or content</li>
    <li>Ask Opal to adapt for specific channels</li>
    <li>Provide channel-specific requirements</li>
    <li>Review and adjust for brand consistency</li>
</ol>
"
                },
                new Lesson
                {
                    Id = "content-tone",
                    ModuleId = "content-generation",
                    Title = "Tone & Style Translation",
                    Summary = "Transform content tone and style for different audiences.",
                    Order = 4,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Change content tone and style",
                        "Adapt for different audiences",
                        "Maintain message while changing voice"
                    },
                    Content = @"
<h2>Tone & Style Transformation</h2>
<p>Opal can rewrite content to fit a specific tone or audience while preserving the core message.</p>

<h3>Common Tone Transformations</h3>
<ul>
    <li><strong>Formal ↔ Casual</strong> - Professional vs. conversational</li>
    <li><strong>Technical ↔ Simple</strong> - Expert vs. beginner-friendly</li>
    <li><strong>Serious ↔ Playful</strong> - Corporate vs. fun</li>
    <li><strong>Long-form ↔ Concise</strong> - Detailed vs. brief</li>
</ul>

<h3>Audience Adaptation</h3>
<p>Adapt content for:</p>
<ul>
    <li>Different age groups</li>
    <li>Industry expertise levels</li>
    <li>Cultural contexts</li>
    <li>B2B vs B2C audiences</li>
</ul>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "content-tone-example",
                            Title = "Transform Content Tone",
                            Description = "Convert formal content to casual.",
                            Type = ExampleType.Prompt,
                            ExampleContent =@"Rewrite this formal product description in a casual, friendly tone for a younger audience:

""Our premium ergonomic office chair features adjustable lumbar support, breathable mesh backing, and precision-engineered armrests to provide optimal comfort during extended work sessions.""",
                            SampleResponse = @"Meet your new work-from-home bestie! This chair's got your back (literally) with adjustable lumbar support that actually works. The breathable mesh keeps you cool during those marathon Zoom calls, and the armrests? *Chef's kiss* - they adjust every which way so you can find your perfect chill position. Say goodbye to that achy feeling after a long day. Your spine will thank you! 🪑✨"
                        }
                    }
                },
                new Lesson
                {
                    Id = "content-localization",
                    ModuleId = "content-generation",
                    Title = "Content Localization",
                    Summary = "Translate and localize content for different markets.",
                    Order = 5,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Translate content to other languages",
                        "Understand localization vs translation",
                        "Adapt content for different markets"
                    },
                    Content = @"
<h2>Content Localization</h2>
<p>Opal can translate content into other languages and adapt it for different cultural contexts.</p>

<h3>Translation vs Localization</h3>
<ul>
    <li><strong>Translation</strong> - Converting text from one language to another</li>
    <li><strong>Localization</strong> - Adapting content for a specific market, including cultural references, idioms, and local conventions</li>
</ul>

<h3>Localization Considerations</h3>
<ul>
    <li>Date and number formats</li>
    <li>Currency symbols</li>
    <li>Cultural references and idioms</li>
    <li>Local regulations and sensitivities</li>
    <li>Measurement units</li>
</ul>

<h3>Best Practices</h3>
<ul>
    <li>Specify the target market, not just language</li>
    <li>Provide context about your audience</li>
    <li>Review with native speakers when possible</li>
    <li>Consider cultural appropriateness</li>
</ul>
"
                }
            }
        };
    }

    #endregion

    #region Module 5: Image Generation & Editing

    private LearningModule BuildImageGenerationModule()
    {
        return new LearningModule
        {
            Id = "image-generation",
            Title = "Image Generation & Editing",
            Description = "Create and edit images using Opal's AI capabilities.",
            Icon = "photo",
            Order = 5,
            Difficulty = ModuleDifficulty.Intermediate,
            Prerequisites = new[] { "opal-chat" },
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "image-creating",
                    ModuleId = "image-generation",
                    Title = "Creating Original Images",
                    Summary = "Generate new images from text descriptions.",
                    Order = 1,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Generate images from prompts",
                        "Write effective image prompts",
                        "Request multiple variations"
                    },
                    Content = @"
<h2>Creating Images with Opal</h2>
<p>You can use Opal Chat to create original images by sending a request with a prompt describing what you want to create.</p>

<h3>How to Generate Images</h3>
<ol>
    <li>Open Opal Chat</li>
    <li>Describe the image you want to create</li>
    <li>Optionally specify settings like aspect ratio</li>
    <li>Send your request</li>
    <li>Review and iterate</li>
</ol>

<h3>Writing Effective Image Prompts</h3>
<p>More detail leads to better results. Include:</p>
<ul>
    <li><strong>Subject</strong> - What should be in the image</li>
    <li><strong>Style</strong> - Photo, illustration, minimalist, etc.</li>
    <li><strong>Mood</strong> - Bright, dramatic, peaceful, etc.</li>
    <li><strong>Colors</strong> - Specific color palette or preferences</li>
    <li><strong>Composition</strong> - How elements should be arranged</li>
</ul>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "image-create-example",
                            Title = "Generate a Hero Image",
                            Description = "Create a marketing image for a website.",
                            Type = ExampleType.Prompt,
                            ExampleContent ="Create a hero image for a sustainable fashion brand. Show a diverse group of young professionals in eco-friendly clothing in an urban park setting. Style: modern, bright, aspirational. Aspect ratio: 16:9",
                            SampleResponse = "I've generated your hero image showing a diverse group of young professionals wearing sustainable fashion in a sunny urban park. The image has a modern, aspirational feel with bright natural lighting. You can download it or request variations with different compositions or styling."
                        }
                    }
                },
                new Lesson
                {
                    Id = "image-formats",
                    ModuleId = "image-generation",
                    Title = "Image Aspect Ratios & Formats",
                    Summary = "Understand and use different image formats for various purposes.",
                    Order = 2,
                    EstimatedMinutes = 6,
                    LearningObjectives = new List<string>
                    {
                        "Choose appropriate aspect ratios",
                        "Understand format requirements for different platforms",
                        "Request specific image dimensions"
                    },
                    Content = @"
<h2>Image Formats and Aspect Ratios</h2>
<p>Choosing the right aspect ratio is crucial for how your image displays across different platforms and use cases.</p>

<h3>Available Aspect Ratios</h3>
<ul>
    <li><strong>1:1 (Square)</strong> - Profile pictures, Instagram posts</li>
    <li><strong>4:3 (Standard landscape)</strong> - Traditional photos, presentations</li>
    <li><strong>16:9 (Widescreen landscape)</strong> - YouTube thumbnails, hero images, banners</li>
    <li><strong>9:16 (Vertical portrait)</strong> - Instagram Stories, TikTok, mobile ads</li>
</ul>

<h3>Platform Recommendations</h3>
<ul>
    <li><strong>Instagram Feed</strong> - 1:1 or 4:5</li>
    <li><strong>Instagram Stories</strong> - 9:16</li>
    <li><strong>Facebook</strong> - 16:9 or 1:1</li>
    <li><strong>LinkedIn</strong> - 16:9</li>
    <li><strong>Website Hero</strong> - 16:9</li>
    <li><strong>Email</strong> - 3:2 or 16:9</li>
</ul>

<h3>Requesting Specific Formats</h3>
<p>Specify the aspect ratio in your prompt:</p>
<ul>
    <li>""Create a square image for Instagram...""</li>
    <li>""Generate a 16:9 banner image...""</li>
    <li>""Make a vertical 9:16 image for Stories...""</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "image-editing",
                    ModuleId = "image-generation",
                    Title = "Editing Existing Images",
                    Summary = "Modify and enhance existing images with Opal.",
                    Order = 3,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Edit images using prompts",
                        "Make targeted modifications",
                        "Enhance image quality and composition"
                    },
                    Content = @"
<h2>Editing Images with Opal</h2>
<p>You can edit existing images by linking to their URL and describing the changes you want. Opal can make various modifications while preserving the original style.</p>

<h3>What You Can Edit</h3>
<ul>
    <li>Add or remove objects</li>
    <li>Change the background</li>
    <li>Adjust colors, lighting, or composition</li>
    <li>Change the style or mood</li>
    <li>Modify the aspect ratio</li>
</ul>

<h3>How to Edit</h3>
<ol>
    <li>Share the image URL or upload the image</li>
    <li>Describe what you want to change</li>
    <li>Review the result</li>
    <li>Request additional adjustments if needed</li>
</ol>

<h3>Requesting Variations</h3>
<p>You can request up to four variations of an edited image to choose from.</p>
"
                },
                new Lesson
                {
                    Id = "image-objects",
                    ModuleId = "image-generation",
                    Title = "Object Addition & Removal",
                    Summary = "Add new elements to or remove unwanted elements from images.",
                    Order = 4,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Add elements to existing images",
                        "Remove unwanted objects",
                        "Maintain image coherence"
                    },
                    Content = @"
<h2>Adding and Removing Objects</h2>
<p>Opal can intelligently add new elements to images or remove unwanted objects while maintaining visual coherence.</p>

<h3>Adding Objects</h3>
<p>You can add:</p>
<ul>
    <li>Products or items</li>
    <li>People or characters</li>
    <li>Text overlays</li>
    <li>Decorative elements</li>
    <li>Backgrounds or scenery</li>
</ul>

<h3>Removing Objects</h3>
<p>You can remove:</p>
<ul>
    <li>Unwanted people or objects</li>
    <li>Distracting background elements</li>
    <li>Text or watermarks</li>
    <li>Imperfections or blemishes</li>
</ul>

<h3>Tips for Best Results</h3>
<ul>
    <li>Be specific about what to add/remove</li>
    <li>Describe the location in the image</li>
    <li>Consider lighting and perspective consistency</li>
    <li>Review carefully for artifacts</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "image-style",
                    ModuleId = "image-generation",
                    Title = "Style & Mood Adjustments",
                    Summary = "Transform the visual style and atmosphere of images.",
                    Order = 5,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Change image styles",
                        "Adjust mood and atmosphere",
                        "Apply consistent brand aesthetics"
                    },
                    Content = @"
<h2>Style and Mood Transformations</h2>
<p>Opal can transform the visual style and mood of images to match your brand or campaign requirements.</p>

<h3>Style Adjustments</h3>
<ul>
    <li>Photo to illustration</li>
    <li>Realistic to stylized</li>
    <li>Modern to vintage</li>
    <li>Minimalist to detailed</li>
</ul>

<h3>Mood Adjustments</h3>
<ul>
    <li>Bright and cheerful</li>
    <li>Dark and dramatic</li>
    <li>Warm and cozy</li>
    <li>Cool and professional</li>
    <li>Energetic and dynamic</li>
</ul>

<h3>Color Adjustments</h3>
<ul>
    <li>Apply brand colors</li>
    <li>Change color temperature</li>
    <li>Convert to monochrome</li>
    <li>Enhance or mute saturation</li>
</ul>
"
                }
            }
        };
    }

    #endregion

    #region Module 6: Instructions

    private LearningModule BuildInstructionsModule()
    {
        return new LearningModule
        {
            Id = "instructions",
            Title = "Instructions",
            Description = "Learn to customize Opal's behavior with instructions.",
            Icon = "clipboard-document-list",
            Order = 6,
            Difficulty = ModuleDifficulty.Intermediate,
            Prerequisites = new[] { "opal-chat" },
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "instructions-understanding",
                    ModuleId = "instructions",
                    Title = "Understanding Instructions",
                    Summary = "Learn what instructions are and how they shape Opal's behavior.",
                    Order = 1,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Understand what instructions are",
                        "Learn how instructions affect Opal's responses",
                        "Know when to use instructions"
                    },
                    Content = @"
<h2>What Are Instructions?</h2>
<p>Instructions are Opal's first version of customizable AI agents. They are predefined, reusable prompt behaviors that guide AI-powered content creation.</p>

<h3>How Instructions Work</h3>
<p>Instructions define:</p>
<ul>
    <li>How Opal should behave in specific contexts</li>
    <li>What tone and style to use</li>
    <li>What information to include or exclude</li>
    <li>How to format responses</li>
</ul>

<h3>Benefits of Instructions</h3>
<ul>
    <li><strong>Consistency</strong> - Get predictable results every time</li>
    <li><strong>Efficiency</strong> - Don't repeat yourself with every prompt</li>
    <li><strong>Brand Alignment</strong> - Ensure all content matches your guidelines</li>
    <li><strong>Team Collaboration</strong> - Share instructions across your organization</li>
</ul>

<h3>Instructions vs. Prompts</h3>
<ul>
    <li><strong>Prompts</strong> - One-time requests for specific tasks</li>
    <li><strong>Instructions</strong> - Reusable rules that shape how Opal responds to any prompt</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "instructions-prebuilt",
                    ModuleId = "instructions",
                    Title = "Prebuilt Instructions",
                    Summary = "Explore and use instructions that come ready to use.",
                    Order = 2,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Access prebuilt instructions",
                        "Understand available instruction types",
                        "Apply prebuilt instructions effectively"
                    },
                    Content = @"
<h2>Prebuilt Instructions</h2>
<p>Optimizely provides prebuilt instructions that you can use immediately or customize for your needs.</p>

<h3>Types of Prebuilt Instructions</h3>
<ul>
    <li><strong>Content Style</strong> - Define writing tone and voice</li>
    <li><strong>Brand Guidelines</strong> - Enforce brand standards</li>
    <li><strong>Format Templates</strong> - Structure content output</li>
    <li><strong>Task-Specific</strong> - Optimize for specific use cases</li>
</ul>

<h3>Using Prebuilt Instructions</h3>
<ol>
    <li>Browse available instructions</li>
    <li>Select one that matches your need</li>
    <li>Apply it to your Opal instance</li>
    <li>Customize if needed</li>
</ol>

<h3>Customizing Prebuilt Instructions</h3>
<p>You can use prebuilt instructions as starting points:</p>
<ul>
    <li>Copy and modify existing instructions</li>
    <li>Adjust parameters for your use case</li>
    <li>Add your brand-specific details</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "instructions-custom",
                    ModuleId = "instructions",
                    Title = "Creating Custom Instructions",
                    Summary = "Build your own instructions for specific needs.",
                    Order = 3,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Create custom instructions",
                        "Define instruction parameters",
                        "Test and refine instructions"
                    },
                    Content = @"
<h2>Creating Custom Instructions</h2>
<p>Custom instructions let you define exactly how Opal should behave for your specific needs.</p>

<h3>Instruction Components</h3>
<ul>
    <li><strong>Name</strong> - Clear, descriptive identifier</li>
    <li><strong>Description</strong> - What the instruction does</li>
    <li><strong>Prompt Template</strong> - The core behavior definition</li>
    <li><strong>Conditions</strong> - When the instruction should apply</li>
</ul>

<h3>Writing Effective Instructions</h3>
<p>Follow these principles:</p>
<ul>
    <li>Be specific and detailed</li>
    <li>Include examples of desired output</li>
    <li>Define what to avoid</li>
    <li>Specify format requirements</li>
</ul>

<h3>Testing Instructions</h3>
<ol>
    <li>Create a draft instruction</li>
    <li>Test with various prompts</li>
    <li>Review outputs for consistency</li>
    <li>Refine based on results</li>
    <li>Get feedback from team members</li>
</ol>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "instructions-example",
                            Title = "Brand Voice Instruction",
                            Description = "Example of a custom instruction for brand voice.",
                            Type = ExampleType.Configuration,
                            ExampleContent =@"Name: TechStartup Brand Voice

Description: Ensures all content matches our startup's friendly, innovative voice.

Prompt Template:
When creating content, follow these guidelines:
- Tone: Friendly, approachable, and slightly informal
- Avoid: Corporate jargon, overly technical language, passive voice
- Include: Contractions, direct address (""you""), action verbs
- Emoji usage: Occasional, professional contexts only
- Sentence length: Mix of short and medium, no long complex sentences
- Always convey enthusiasm without being over-the-top",
                            SampleResponse = "This instruction will now be applied to all content generation requests, ensuring consistent brand voice across your marketing materials."
                        }
                    }
                },
                new Lesson
                {
                    Id = "instructions-best-practices",
                    ModuleId = "instructions",
                    Title = "Instruction Best Practices",
                    Summary = "Learn tips for creating and managing effective instructions.",
                    Order = 4,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Follow best practices for instructions",
                        "Avoid common mistakes",
                        "Manage instructions effectively"
                    },
                    Content = @"
<h2>Best Practices for Instructions</h2>

<h3>Writing Guidelines</h3>
<ul>
    <li><strong>Be Specific</strong> - Vague instructions lead to inconsistent results</li>
    <li><strong>Use Examples</strong> - Show what you want, not just tell</li>
    <li><strong>Keep It Focused</strong> - One instruction should have one clear purpose</li>
    <li><strong>Test Thoroughly</strong> - Try many different prompts</li>
</ul>

<h3>Common Mistakes to Avoid</h3>
<ul>
    <li>Making instructions too broad</li>
    <li>Conflicting rules within an instruction</li>
    <li>Forgetting to specify exceptions</li>
    <li>Not updating instructions as needs change</li>
</ul>

<h3>Management Tips</h3>
<ul>
    <li>Name instructions clearly</li>
    <li>Document what each instruction does</li>
    <li>Review and update regularly</li>
    <li>Archive unused instructions</li>
    <li>Share best practices with your team</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "instructions-library",
                    ModuleId = "instructions",
                    Title = "Sample Instructions Library",
                    Summary = "Explore examples of useful instructions for common scenarios.",
                    Order = 5,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Access sample instructions",
                        "Adapt examples for your needs",
                        "Build an instruction library"
                    },
                    Content = @"
<h2>Sample Instructions Library</h2>
<p>Here are examples of instructions for common marketing scenarios.</p>

<h3>Email Subject Line Writer</h3>
<p>Creates attention-grabbing email subjects with A/B test variations.</p>

<h3>Social Media Adapter</h3>
<p>Transforms content for different social platforms with proper hashtags and formatting.</p>

<h3>SEO Content Optimizer</h3>
<p>Ensures content includes keywords naturally and follows SEO best practices.</p>

<h3>Brand Consistency Checker</h3>
<p>Reviews content for adherence to brand guidelines and suggests improvements.</p>

<h3>Accessibility Reviewer</h3>
<p>Checks content for accessibility issues and suggests improvements.</p>
"
                }
            }
        };
    }

    #endregion

    #region Module 7: Working with Agents

    private LearningModule BuildWorkingWithAgentsModule()
    {
        return new LearningModule
        {
            Id = "agents",
            Title = "Working with Agents",
            Description = "Learn to use AI agents for complex tasks.",
            Icon = "cpu-chip",
            Order = 7,
            Difficulty = ModuleDifficulty.Intermediate,
            Prerequisites = new[] { "instructions" },
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "agents-overview",
                    ModuleId = "agents",
                    Title = "Agent Architecture Overview",
                    Summary = "Understand how agents work within Opal.",
                    Order = 1,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Understand agent architecture",
                        "Know the different types of agents",
                        "Learn how agents use tools"
                    },
                    Content = @"
<h2>Understanding Opal Agents</h2>
<p>Agents are what make Opal powerful and flexible. They are intelligent software components that understand your intent and help you reach your goal.</p>

<h3>What Agents Do</h3>
<p>Instead of knowing which tool to run or how to structure a request, you simply describe what you want in natural language. The agent:</p>
<ul>
    <li>Interprets your request</li>
    <li>Selects appropriate tools</li>
    <li>Executes the task</li>
    <li>Returns consistent, accurate results</li>
</ul>

<h3>Types of Agents</h3>
<ul>
    <li><strong>Default Agents</strong> - Pre-built by Optimizely for common tasks</li>
    <li><strong>Specialized Agents</strong> - Custom single-task agents you create</li>
    <li><strong>Workflow Agents</strong> - Multi-step agents for complex processes</li>
</ul>

<h3>Agent Components</h3>
<ul>
    <li><strong>Instructions</strong> - Define agent behavior</li>
    <li><strong>Tools</strong> - Capabilities the agent can use</li>
    <li><strong>Inputs</strong> - Data the agent needs</li>
    <li><strong>Outputs</strong> - What the agent produces</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "agents-default",
                    ModuleId = "agents",
                    Title = "Default Agents & Agent Directory",
                    Summary = "Explore pre-built agents available in Opal.",
                    Order = 2,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Browse the Agent Directory",
                        "Understand default agent capabilities",
                        "Add default agents to your instance"
                    },
                    Content = @"
<h2>Default Agents</h2>
<p>Default agents are pre-built by Optimizely and come ready to use. They handle common tasks and can be added from the Agent Directory.</p>

<h3>Agent Directory</h3>
<p>The Agent Directory is where you browse, discover, and add agents:</p>
<ul>
    <li>Search for agents by name or capability</li>
    <li>View agent descriptions and capabilities</li>
    <li>Add agents to your Opal instance</li>
    <li>Manage your active agents</li>
</ul>

<h3>Common Default Agents</h3>
<ul>
    <li><strong>Content Adaptation</strong> - Tailors content for different contexts</li>
    <li><strong>GA4 Web Traffic Report</strong> - Generates analytics reports</li>
    <li><strong>UTM Creation</strong> - Creates tracking parameters</li>
    <li><strong>LinkedIn InMail Copy</strong> - Generates messaging content</li>
    <li><strong>Blog Post Generation</strong> - Creates blog content</li>
    <li><strong>Technical SEO Auditor</strong> - Analyzes site optimization</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "agents-calling",
                    ModuleId = "agents",
                    Title = "Calling Agents in Chat",
                    Summary = "Learn how to invoke agents using @mentions.",
                    Order = 3,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Use @mentions to call agents",
                        "Provide inputs to agents",
                        "Understand agent responses"
                    },
                    Content = @"
<h2>Calling Agents</h2>
<p>To call an agent in Opal, use the @mention syntax.</p>

<h3>How to Call an Agent</h3>
<ol>
    <li>Click ""Mention Agent"" or type @</li>
    <li>A list of available agents displays</li>
    <li>Select an agent or type the agent's ID</li>
    <li>Provide any required inputs</li>
    <li>Send your message</li>
</ol>

<h3>Agent Mention Syntax</h3>
<p>Use: <code>@AGENT_ID [your request]</code></p>

<h3>Example</h3>
<p><code>@content-adapter Adapt this product description for LinkedIn</code></p>

<h3>Tips for Agent Calls</h3>
<ul>
    <li>Be clear about what you want the agent to do</li>
    <li>Provide all necessary context</li>
    <li>Include any required parameters</li>
    <li>Review the agent's output carefully</li>
</ul>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "agents-mention-example",
                            Title = "Call the UTM Creation Agent",
                            Description = "Use @mention to create UTM parameters.",
                            Type = ExampleType.Prompt,
                            ExampleContent ="@utm-creator Create UTM parameters for our spring email campaign linking to the homepage",
                            SampleResponse = @"I've created UTM parameters for your spring email campaign:

**Full URL:**
https://yoursite.com/?utm_source=email&utm_medium=newsletter&utm_campaign=spring-2025&utm_content=homepage-link

**Parameters:**
- utm_source: email
- utm_medium: newsletter
- utm_campaign: spring-2025
- utm_content: homepage-link

Would you like me to create variations for different email segments?"
                        }
                    }
                },
                new Lesson
                {
                    Id = "agents-inputs-outputs",
                    ModuleId = "agents",
                    Title = "Agent Inputs & Outputs",
                    Summary = "Understand how to provide inputs and interpret outputs.",
                    Order = 4,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Understand agent input requirements",
                        "Format inputs correctly",
                        "Interpret and use agent outputs"
                    },
                    Content = @"
<h2>Agent Inputs and Outputs</h2>

<h3>Understanding Inputs</h3>
<p>Each agent has specific input requirements:</p>
<ul>
    <li><strong>Required inputs</strong> - Must be provided for the agent to work</li>
    <li><strong>Optional inputs</strong> - Enhance results but aren't mandatory</li>
    <li><strong>Context</strong> - Background information that improves output</li>
</ul>

<h3>Providing Good Inputs</h3>
<ul>
    <li>Check the agent's documentation for requirements</li>
    <li>Be specific and detailed</li>
    <li>Include relevant context</li>
    <li>Format data as expected</li>
</ul>

<h3>Understanding Outputs</h3>
<p>Agents return structured outputs that may include:</p>
<ul>
    <li>Generated content</li>
    <li>Data and reports</li>
    <li>Recommendations</li>
    <li>Actions taken</li>
    <li>Follow-up suggestions</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "agents-evaluation",
                    ModuleId = "agents",
                    Title = "Agent Evaluation & Monitoring",
                    Summary = "Monitor agent performance and ensure quality.",
                    Order = 5,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Monitor agent performance",
                        "Evaluate output quality",
                        "Use agent metrics effectively"
                    },
                    Content = @"
<h2>Agent Evaluation and Monitoring</h2>

<h3>Agent Evaluations</h3>
<p>Opal provides live monitoring and performance reviews to verify quality and output consistency:</p>
<ul>
    <li><strong>Output quality checks</strong> - Verify against sample outputs</li>
    <li><strong>Consistency monitoring</strong> - Ensure predictable results</li>
    <li><strong>Performance tracking</strong> - Monitor speed and reliability</li>
</ul>

<h3>Execution Guardrails</h3>
<p>Opal automatically modifies agent behavior when breaching operational boundaries, ensuring:</p>
<ul>
    <li>Outputs stay within acceptable ranges</li>
    <li>Errors are handled gracefully</li>
    <li>Quality standards are maintained</li>
</ul>

<h3>Agent Usage Reporting</h3>
<p>Track credit usage per agent to:</p>
<ul>
    <li>Understand which agents are used most</li>
    <li>Optimize agent usage</li>
    <li>Plan for capacity</li>
</ul>

<h3>Agent Activity Streaming</h3>
<p>See real-time progress of agent activities in Chat for transparency into what the agent is doing.</p>
"
                }
            }
        };
    }

    #endregion

    #region Module 8: Specialized Agents

    private LearningModule BuildSpecializedAgentsModule()
    {
        return new LearningModule
        {
            Id = "specialized-agents",
            Title = "Specialized Agents",
            Description = "Create and configure custom agents for specific tasks.",
            Icon = "cog-6-tooth",
            Order = 8,
            Difficulty = ModuleDifficulty.Advanced,
            Prerequisites = new[] { "agents" },
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "specialized-overview",
                    ModuleId = "specialized-agents",
                    Title = "Specialized Agents Overview",
                    Summary = "Learn what specialized agents are and when to use them.",
                    Order = 1,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Understand specialized agents",
                        "Know when to create a specialized agent",
                        "Understand the single-shot model"
                    },
                    Content = @"
<h2>What Are Specialized Agents?</h2>
<p>Specialized agents are purpose-built AI agents you create that complete a single, well-defined task. They operate in a <strong>single-shot model</strong>: you provide inputs, the agent runs once, and returns a result. There is no multi-turn conversation.</p>

<h3>Characteristics</h3>
<ul>
    <li><strong>Single Purpose</strong> - One task, done well</li>
    <li><strong>Single-Shot</strong> - Run once, return result</li>
    <li><strong>Targeted Tools</strong> - Only the tools needed</li>
    <li><strong>Adjustable Settings</strong> - Fine-tune reasoning and creativity</li>
    <li><strong>Defined Inputs</strong> - Clear parameters for consistent results</li>
</ul>

<h3>When to Create a Specialized Agent</h3>
<ul>
    <li>You have a repetitive task with consistent inputs/outputs</li>
    <li>Default agents don't meet your specific needs</li>
    <li>You want precise control over behavior</li>
    <li>You need to automate a workflow step</li>
</ul>

<h3>Benefits</h3>
<ul>
    <li>Consistency in output</li>
    <li>Efficiency in execution</li>
    <li>Easy to share with team</li>
    <li>Reduced prompt engineering</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "specialized-creating",
                    ModuleId = "specialized-agents",
                    Title = "Creating Specialized Agents",
                    Summary = "Step-by-step guide to creating your own agent.",
                    Order = 2,
                    EstimatedMinutes = 15,
                    LearningObjectives = new List<string>
                    {
                        "Create a new specialized agent",
                        "Configure agent settings",
                        "Define inputs and outputs"
                    },
                    Content = @"
<h2>Creating a Specialized Agent</h2>

<h3>Step 1: Define the Task</h3>
<p>Before creating, clearly define:</p>
<ul>
    <li>What does the agent do?</li>
    <li>What inputs does it need?</li>
    <li>What output should it produce?</li>
    <li>What tools will it use?</li>
</ul>

<h3>Step 2: Configure Basic Settings</h3>
<ul>
    <li><strong>Name</strong> - Clear, descriptive identifier</li>
    <li><strong>Description</strong> - What the agent does</li>
    <li><strong>Icon</strong> - Visual identifier</li>
</ul>

<h3>Step 3: Define Inputs</h3>
<ul>
    <li>Name each input parameter</li>
    <li>Specify data types</li>
    <li>Mark required vs optional</li>
    <li>Add descriptions for clarity</li>
</ul>

<h3>Step 4: Write the Prompt Template</h3>
<p>The core instructions that define agent behavior.</p>

<h3>Step 5: Select Tools</h3>
<p>Choose which tools the agent can access.</p>

<h3>Step 6: Configure Settings</h3>
<ul>
    <li>Reasoning level</li>
    <li>Creativity setting</li>
    <li>Output format</li>
</ul>

<h3>Step 7: Test and Refine</h3>
<p>Test with various inputs and adjust as needed.</p>
"
                },
                new Lesson
                {
                    Id = "specialized-prompts",
                    ModuleId = "specialized-agents",
                    Title = "Prompt Templates",
                    Summary = "Write effective prompt templates for agents.",
                    Order = 3,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Write effective prompt templates",
                        "Use template variables",
                        "Structure prompts for consistency"
                    },
                    Content = @"
<h2>Prompt Templates</h2>
<p>Prompt templates are the core of your specialized agent. They define exactly how the agent should behave.</p>

<h3>Template Structure</h3>
<ul>
    <li><strong>Context</strong> - Background information</li>
    <li><strong>Task</strong> - What to do</li>
    <li><strong>Variables</strong> - Dynamic inputs</li>
    <li><strong>Format</strong> - Output structure</li>
    <li><strong>Constraints</strong> - Rules and limitations</li>
</ul>

<h3>Using Variables</h3>
<p>Reference inputs using curly braces: <code>{variable_name}</code></p>

<h3>Best Practices</h3>
<ul>
    <li>Be specific and detailed</li>
    <li>Include examples when helpful</li>
    <li>Define what NOT to do</li>
    <li>Specify output format clearly</li>
    <li>Keep instructions organized</li>
</ul>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "prompt-template-example",
                            Title = "Product Description Generator Template",
                            Description = "Example prompt template for generating product descriptions.",
                            Type = ExampleType.Configuration,
                            ExampleContent =@"You are a product copywriter creating SEO-optimized product descriptions.

Product Name: {product_name}
Product Category: {category}
Key Features: {features}
Target Audience: {audience}
Tone: {tone}

Create a product description that:
1. Opens with a compelling hook
2. Highlights key benefits (not just features)
3. Includes the primary keyword naturally 2-3 times
4. Uses sensory and emotional language
5. Ends with a clear call to action

Format:
- Headline (max 10 words)
- Main description (100-150 words)
- Bullet points (3-5 key benefits)
- Call to action (1 sentence)

Avoid: Generic phrases, technical jargon, exaggeration"
                        }
                    }
                },
                new Lesson
                {
                    Id = "specialized-tools",
                    ModuleId = "specialized-agents",
                    Title = "Tool Configuration",
                    Summary = "Select and configure tools for your agent.",
                    Order = 4,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Select appropriate tools for agents",
                        "Configure tool access",
                        "Understand tool limitations"
                    },
                    Content = @"
<h2>Configuring Agent Tools</h2>
<p>Tools are the capabilities your agent can use. Selecting the right tools is crucial for agent effectiveness.</p>

<h3>Selecting Tools</h3>
<ul>
    <li>Only include tools the agent needs</li>
    <li>More tools = more complexity</li>
    <li>Consider what actions the task requires</li>
</ul>

<h3>Common Tool Categories</h3>
<ul>
    <li><strong>Content Tools</strong> - Create, edit, translate content</li>
    <li><strong>Campaign Tools</strong> - Create and manage campaigns</li>
    <li><strong>Image Tools</strong> - Generate and edit images</li>
    <li><strong>Data Tools</strong> - Search, analyze, report</li>
    <li><strong>Integration Tools</strong> - Connect to external services</li>
</ul>

<h3>Tool Best Practices</h3>
<ul>
    <li>Start with minimal tools, add as needed</li>
    <li>Test tool combinations</li>
    <li>Document why each tool is included</li>
    <li>Review tool access regularly</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "specialized-rag",
                    ModuleId = "specialized-agents",
                    Title = "RAG Integration in Agents",
                    Summary = "Use Retrieval-Augmented Generation in your agents.",
                    Order = 5,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Understand RAG in Opal",
                        "Include RAG data in agents",
                        "Query knowledge sources"
                    },
                    Content = @"
<h2>RAG in Specialized Agents</h2>
<p>Retrieval-Augmented Generation (RAG) lets Opal access, understand, and query your knowledge sources for more contextual, accurate responses.</p>

<h3>What RAG Provides</h3>
<ul>
    <li><strong>Unified Content Graph</strong> - Campaigns, tasks, assets, permissions</li>
    <li><strong>Multi-modal Retrieval</strong> - Text, PDFs, images, video, metadata</li>
    <li><strong>Natural Language Queries</strong> - Ask questions, get relevant data</li>
</ul>

<h3>Including RAG in Prompt Templates</h3>
<p>Use special syntax to inject RAG data:</p>
<ul>
    <li><code>{retrieval: query}</code> - Returns text information</li>
    <li><code>{retrieval+: query}</code> - Returns text and file information</li>
</ul>

<h3>Using the search_application_data Tool</h3>
<p>Add the tool to your agent and specify usage in the prompt template.</p>

<h3>Security</h3>
<p>All RAG operations respect user roles and permissions - you only see content you're authorized to access.</p>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "rag-template-example",
                            Title = "RAG in a Prompt Template",
                            Description = "How to include RAG data in an agent template.",
                            Type = ExampleType.Configuration,
                            ExampleContent =@"You are a brand consistency checker reviewing content against our brand guidelines.

Content to review:
{content}

Brand guidelines from our knowledge base:
{retrieval: brand guidelines and style guide}

Review the content and:
1. Check for brand voice consistency
2. Verify terminology usage
3. Confirm visual style alignment (if applicable)
4. Identify any guideline violations

Provide a structured report with:
- Overall compliance score (1-10)
- Specific issues found
- Recommendations for fixes"
                        }
                    }
                },
                new Lesson
                {
                    Id = "specialized-versioning",
                    ModuleId = "specialized-agents",
                    Title = "Agent Versioning & Management",
                    Summary = "Manage agent versions and maintain quality over time.",
                    Order = 6,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Version your agents",
                        "Restore previous versions",
                        "Manage agent lifecycle"
                    },
                    Content = @"
<h2>Agent Versioning</h2>
<p>Opal provides detailed history tracking with version restore capabilities for your specialized agents.</p>

<h3>Version Tracking</h3>
<ul>
    <li>All changes are automatically tracked</li>
    <li>View history of modifications</li>
    <li>See who made changes and when</li>
    <li>Compare different versions</li>
</ul>

<h3>Restoring Versions</h3>
<p>You can restore any previous version if:</p>
<ul>
    <li>A change caused issues</li>
    <li>You want to test different approaches</li>
    <li>You need to roll back updates</li>
</ul>

<h3>Agent Lifecycle Management</h3>
<ul>
    <li><strong>Development</strong> - Building and testing</li>
    <li><strong>Active</strong> - In use by team</li>
    <li><strong>Deprecated</strong> - Being phased out</li>
    <li><strong>Archived</strong> - No longer in use</li>
</ul>

<h3>Best Practices</h3>
<ul>
    <li>Document changes when updating</li>
    <li>Test thoroughly before deploying</li>
    <li>Communicate changes to users</li>
    <li>Archive rather than delete unused agents</li>
</ul>
"
                }
            }
        };
    }

    #endregion

    #region Module 9: Tools & Integrations

    private LearningModule BuildToolsIntegrationsModule()
    {
        return new LearningModule
        {
            Id = "tools-integrations",
            Title = "Tools & Integrations",
            Description = "Master Opal's tools and connect to external services.",
            Icon = "wrench-screwdriver",
            Order = 9,
            Difficulty = ModuleDifficulty.Advanced,
            Prerequisites = new[] { "agents" },
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "tools-overview",
                    ModuleId = "tools-integrations",
                    Title = "System Tools Overview",
                    Summary = "Understand the built-in tools available in Opal.",
                    Order = 1,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Understand what system tools are",
                        "Know the main tool categories",
                        "Access the tools interface"
                    },
                    Content = @"
<h2>System Tools</h2>
<p>System tools are built-in features that help Opal take action. Each tool performs a specific task, like creating a campaign, uploading files, or generating images.</p>

<h3>How Tools Work</h3>
<p>Think of tools like attachments on a Swiss Army knife. Each one has a distinct purpose that helps you get work done. Tools connect natural language requests to specific actions, so you don't need to know the underlying systems or commands.</p>

<h3>Tool Count</h3>
<p>An Opal instance supports a maximum of <strong>128 active tools</strong>. You can monitor tool count on the Tools page.</p>

<h3>Tool Categories</h3>
<ul>
    <li><strong>Content Tools</strong> - Create, edit, manage content</li>
    <li><strong>Campaign Tools</strong> - Build and manage campaigns</li>
    <li><strong>Image Tools</strong> - Generate and edit images</li>
    <li><strong>Analytics Tools</strong> - Report and analyze data</li>
    <li><strong>Integration Tools</strong> - Connect external services</li>
    <li><strong>Workflow Tools</strong> - Automate processes</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "tools-categories",
                    ModuleId = "tools-integrations",
                    Title = "Tool Categories & Capabilities",
                    Summary = "Explore the different categories of tools and what they do.",
                    Order = 2,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Know the major tool categories",
                        "Understand tool capabilities",
                        "Choose the right tools for tasks"
                    },
                    Content = @"
<h2>Tool Categories</h2>

<h3>Content Management Tools</h3>
<ul>
    <li><strong>create_content</strong> - Create new content items</li>
    <li><strong>edit_content</strong> - Modify existing content</li>
    <li><strong>translate_content</strong> - Translate to other languages</li>
    <li><strong>create_canvas</strong> - Create collaborative workspaces</li>
</ul>

<h3>Campaign Tools</h3>
<ul>
    <li><strong>create_campaign</strong> - Build new campaigns</li>
    <li><strong>create_task</strong> - Add campaign tasks</li>
    <li><strong>upload_files</strong> - Add assets to campaigns</li>
</ul>

<h3>Image Tools</h3>
<ul>
    <li><strong>generate_image</strong> - Create new images</li>
    <li><strong>edit_image</strong> - Modify existing images</li>
    <li><strong>analyze_image</strong> - Describe and understand images</li>
</ul>

<h3>Analytics & Data Tools</h3>
<ul>
    <li><strong>search_application_data</strong> - Query knowledge sources</li>
    <li><strong>generate_report</strong> - Create data reports</li>
    <li><strong>analyze_data</strong> - Extract insights</li>
</ul>

<h3>Web & Research Tools</h3>
<ul>
    <li><strong>search_web</strong> - Search the internet</li>
    <li><strong>fetch_url</strong> - Get content from URLs</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "tools-custom",
                    ModuleId = "tools-integrations",
                    Title = "Custom Tools",
                    Summary = "Create custom tools for specialized needs.",
                    Order = 3,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Understand when to create custom tools",
                        "Know the custom tool creation process",
                        "Manage custom tools"
                    },
                    Content = @"
<h2>Custom Tools</h2>
<p>Administrators can create custom tools to extend Opal's capabilities beyond the built-in system tools.</p>

<h3>When to Create Custom Tools</h3>
<ul>
    <li>Integrate with proprietary systems</li>
    <li>Automate unique business processes</li>
    <li>Connect to industry-specific services</li>
    <li>Extend default capabilities</li>
</ul>

<h3>Custom Tool Components</h3>
<ul>
    <li><strong>Name & Description</strong> - Clear identification</li>
    <li><strong>Input Parameters</strong> - What the tool needs</li>
    <li><strong>Output Format</strong> - What the tool returns</li>
    <li><strong>Integration Logic</strong> - How it connects to systems</li>
</ul>

<h3>Managing Custom Tools</h3>
<ul>
    <li>Test thoroughly before deployment</li>
    <li>Document usage and limitations</li>
    <li>Monitor performance and errors</li>
    <li>Update as external systems change</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "tools-integrations-external",
                    ModuleId = "tools-integrations",
                    Title = "Third-Party Integrations",
                    Summary = "Connect Opal to external services and platforms.",
                    Order = 4,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Connect external services to Opal",
                        "Use popular integrations",
                        "Manage integration data"
                    },
                    Content = @"
<h2>Third-Party Integrations</h2>
<p>Opal can connect to external services to extend its capabilities.</p>

<h3>Available Integrations</h3>
<ul>
    <li><strong>Looker</strong> - Business intelligence and analytics</li>
    <li><strong>Google Analytics</strong> - Web analytics data</li>
    <li><strong>Profound</strong> - Research platform</li>
    <li><strong>Fullstory</strong> - Digital experience analytics</li>
    <li><strong>Zapier MCP</strong> - Connect to thousands of apps</li>
</ul>

<h3>Integration Benefits</h3>
<ul>
    <li>Access data from multiple sources</li>
    <li>Automate cross-platform workflows</li>
    <li>Enhance AI responses with external data</li>
    <li>Streamline marketing operations</li>
</ul>

<h3>Setting Up Integrations</h3>
<ol>
    <li>Navigate to integration settings</li>
    <li>Authorize the external service</li>
    <li>Configure data access permissions</li>
    <li>Test the connection</li>
    <li>Enable in Opal</li>
</ol>
"
                },
                new Lesson
                {
                    Id = "tools-permissions",
                    ModuleId = "tools-integrations",
                    Title = "Managing Tool Permissions",
                    Summary = "Control tool access and permissions.",
                    Order = 5,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Manage tool access",
                        "Configure permissions",
                        "Ensure security best practices"
                    },
                    Content = @"
<h2>Tool Permissions</h2>
<p>Administrators control which tools are available and who can use them.</p>

<h3>Permission Levels</h3>
<ul>
    <li><strong>Enabled/Disabled</strong> - Whether the tool is active</li>
    <li><strong>User Access</strong> - Which users can use the tool</li>
    <li><strong>Agent Access</strong> - Which agents can use the tool</li>
</ul>

<h3>Managing Tool Count</h3>
<p>Monitor the Tools page to see:</p>
<ul>
    <li>Total enabled tools out of 128 maximum</li>
    <li>Tool usage metrics</li>
    <li>Error rates and performance</li>
</ul>

<h3>Security Best Practices</h3>
<ul>
    <li>Only enable tools that are needed</li>
    <li>Regularly audit tool access</li>
    <li>Review integration permissions</li>
    <li>Monitor for unusual activity</li>
</ul>
"
                }
            }
        };
    }

    #endregion

    #region Module 10: AI for Experimentation

    private LearningModule BuildExperimentationModule()
    {
        return new LearningModule
        {
            Id = "experimentation",
            Title = "AI for Experimentation",
            Description = "Use Opal to enhance A/B testing and experimentation.",
            Icon = "beaker",
            Order = 10,
            Difficulty = ModuleDifficulty.Advanced,
            Prerequisites = new[] { "agents" },
            Lessons = new List<Lesson>
            {
                new Lesson
                {
                    Id = "exp-chat",
                    ModuleId = "experimentation",
                    Title = "Opal Chat for Experimentation",
                    Summary = "Use Opal to enhance your experimentation workflow.",
                    Order = 1,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Use Opal for experimentation tasks",
                        "Generate test ideas",
                        "Analyze experiment results"
                    },
                    Content = @"
<h2>Experimentation with Opal</h2>
<p>Opal integrates with Optimizely's experimentation products to help you design, run, and analyze tests more effectively.</p>

<h3>What Opal Can Help With</h3>
<ul>
    <li><strong>Test Ideation</strong> - Generate A/B test ideas</li>
    <li><strong>Variation Creation</strong> - Create test variations</li>
    <li><strong>Hypothesis Writing</strong> - Craft clear hypotheses</li>
    <li><strong>Result Analysis</strong> - Interpret experiment outcomes</li>
    <li><strong>Optimization Suggestions</strong> - Recommend next steps</li>
</ul>

<h3>Getting Started</h3>
<p>Ask Opal questions like:</p>
<ul>
    <li>""What should I test on my checkout page?""</li>
    <li>""Help me write a hypothesis for this test""</li>
    <li>""Analyze my experiment results""</li>
    <li>""What variations should I create?""</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "exp-variation-agent",
                    ModuleId = "experimentation",
                    Title = "AI Variation Development Agent",
                    Summary = "Use AI to create and modify test variations.",
                    Order = 2,
                    EstimatedMinutes = 12,
                    LearningObjectives = new List<string>
                    {
                        "Understand the AI Variation Development Agent",
                        "Generate variations automatically",
                        "Maintain brand consistency"
                    },
                    Content = @"
<h2>AI Variation Development Agent</h2>
<p>The AI variation development agent helps you modify and update existing website elements, create new ones, and generate enhancement suggestions while maintaining brand consistency.</p>

<h3>Capabilities</h3>
<ul>
    <li>Modify existing website elements</li>
    <li>Create new elements</li>
    <li>Generate enhancement suggestions</li>
    <li>Automatically retrieve page styles for brand consistency</li>
    <li>Generate custom code for complex changes</li>
</ul>

<h3>How to Use</h3>
<ol>
    <li>Select the element you want to edit</li>
    <li>Enter what you would like to change</li>
    <li>Click ""Build with AI""</li>
    <li>A chat with Opal opens with your request</li>
    <li>Click ""Preview changes"" to view modifications</li>
</ol>

<h3>Example Requests</h3>
<ul>
    <li>""Improve this button to increase clicks""</li>
    <li>""Make this headline more compelling""</li>
    <li>""Redesign this section based on [theme]""</li>
    <li>""Generate different variants to test""</li>
</ul>
",
                    Examples = new List<LessonExample>
                    {
                        new LessonExample
                        {
                            Id = "exp-variation-example",
                            Title = "Generate Button Variations",
                            Description = "Ask the AI to create button variations for testing.",
                            Type = ExampleType.Prompt,
                            ExampleContent ="Generate 3 variations of this CTA button to test for higher click-through rate. Current button text is 'Buy Now'. Keep the button style consistent with the page.",
                            SampleResponse = @"I've generated 3 button variations for your A/B test:

**Variation A: Urgency Focus**
Text: ""Buy Now - Limited Stock""
Style: Same button, added urgency indicator

**Variation B: Value Proposition**
Text: ""Get Yours Today""
Style: Same button, friendlier language

**Variation C: Action + Benefit**
Text: ""Add to Cart - Free Shipping""
Style: Same button, added benefit

All variations maintain your page's existing button styles. Would you like me to apply these to your experiment?"
                        }
                    }
                },
                new Lesson
                {
                    Id = "exp-variations",
                    ModuleId = "experimentation",
                    Title = "Generating Test Variations",
                    Summary = "Create compelling test variations with AI assistance.",
                    Order = 3,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Generate text variations",
                        "Create visual variations",
                        "Apply best practices for variations"
                    },
                    Content = @"
<h2>Generating Test Variations</h2>
<p>Optimizely Opal lets you generate AI-powered suggested content in the Visual Editor for any existing copy on a page.</p>

<h3>Text Variations</h3>
<p>Generate alternatives for:</p>
<ul>
    <li>Headlines and titles</li>
    <li>Body copy</li>
    <li>Call-to-action buttons</li>
    <li>Product descriptions</li>
    <li>Navigation labels</li>
</ul>

<h3>Visual Variations</h3>
<p>Create variations of:</p>
<ul>
    <li>Layout arrangements</li>
    <li>Color schemes</li>
    <li>Image placements</li>
    <li>Form designs</li>
</ul>

<h3>Variation Best Practices</h3>
<ul>
    <li>Test one element at a time for clear results</li>
    <li>Make variations meaningfully different</li>
    <li>Ensure all variations are brand-consistent</li>
    <li>Consider your hypothesis when creating</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "exp-review-agent",
                    ModuleId = "experimentation",
                    Title = "Experiment Review Agent",
                    Summary = "Get AI-powered recommendations for better experiments.",
                    Order = 4,
                    EstimatedMinutes = 8,
                    LearningObjectives = new List<string>
                    {
                        "Use the Experiment Review Agent",
                        "Get configuration recommendations",
                        "Optimize for statistical significance"
                    },
                    Content = @"
<h2>Experiment Review Agent</h2>
<p>The Experiment Review Agent reviews your experiment configuration and recommends changes to maximize your odds of reaching statistical significance.</p>

<h3>What It Reviews</h3>
<ul>
    <li>Traffic allocation</li>
    <li>Sample size requirements</li>
    <li>Goal configuration</li>
    <li>Duration estimates</li>
    <li>Variation differences</li>
</ul>

<h3>Recommendations May Include</h3>
<ul>
    <li>Adjusting traffic allocation</li>
    <li>Modifying test duration</li>
    <li>Changing primary metrics</li>
    <li>Adding or removing variations</li>
    <li>Improving hypothesis clarity</li>
</ul>

<h3>Using the Agent</h3>
<p>Call the agent after setting up your experiment:</p>
<ul>
    <li>@experiment-review Review my checkout page test</li>
    <li>Ask: ""How can I improve this experiment?""</li>
</ul>
"
                },
                new Lesson
                {
                    Id = "exp-autonomous",
                    ModuleId = "experimentation",
                    Title = "Autonomous Experimentation Workflows",
                    Summary = "Explore the future of AI-powered autonomous testing.",
                    Order = 5,
                    EstimatedMinutes = 10,
                    LearningObjectives = new List<string>
                    {
                        "Understand autonomous experimentation",
                        "Learn about workflow agents",
                        "Explore the future of AI testing"
                    },
                    Content = @"
<h2>Autonomous Experimentation</h2>
<p>The future of experimentation involves agent workflows that can run autonomously to continuously optimize your digital experiences.</p>

<h3>The Vision</h3>
<p>Imagine a workflow that runs autonomously every day or week:</p>
<ol>
    <li>Analyzes your goals</li>
    <li>Generates test ideas</li>
    <li>Builds 2-3 experiments</li>
    <li>Runs the tests</li>
    <li>Picks the winner</li>
    <li>Pushes to production</li>
</ol>
<p>This is a six or seven step workflow that Opal can run autonomously versus a marketer manually pushing buttons.</p>

<h3>Workflow Agents</h3>
<p>Workflow agents are designed to process natural language input to accomplish complex, multi-step processes within Optimizely.</p>

<h3>Getting Started</h3>
<ul>
    <li>Start with assisted experimentation</li>
    <li>Build confidence with AI recommendations</li>
    <li>Gradually automate more steps</li>
    <li>Work with your team to define automation boundaries</li>
</ul>

<h3>The Future</h3>
<p>As predicted by Optimizely: ""Platforms like Optimizely Opal are already pioneering this approach"" of multi-agent systems orchestrating complex marketing workflows.</p>
"
                }
            }
        };
    }

    #endregion
}
