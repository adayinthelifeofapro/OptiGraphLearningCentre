using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using OptimizelyLearningCentre.Client;
using OptimizelyLearningCentre.Client.Services;
using OptimizelyLearningCentre.Client.State;
using OptimizelyLearningCentre.Client.Courses.Graph;
using OptimizelyLearningCentre.Client.Courses.Opal;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// HTTP Client
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

// Blazored LocalStorage
builder.Services.AddBlazoredLocalStorage();

// State Management
builder.Services.AddScoped<AppState>();
builder.Services.AddScoped<QueryState>();

// Course Infrastructure
builder.Services.AddSingleton<ICourseRegistry>(sp =>
{
    var registry = new CourseRegistry();
    registry.RegisterCourse(GraphCourse.Definition);
    registry.RegisterCourse(OpalCourse.Definition);
    return registry;
});
builder.Services.AddScoped<ICourseContext, CourseContext>();

// Content Providers
builder.Services.AddScoped<GraphContentProvider>();
builder.Services.AddScoped<OpalContentProvider>();

// Core Services
builder.Services.AddScoped<ISettingsService, LocalStorageSettingsService>();
builder.Services.AddScoped<ILearningService, LearningService>();

// Graph-specific Services
builder.Services.AddScoped<IGraphQLClient, GraphQLClient>();
builder.Services.AddScoped<ISchemaService, SchemaService>();
builder.Services.AddScoped<IQueryBuilderService, QueryBuilderService>();

// Logging
builder.Logging.SetMinimumLevel(LogLevel.Information);

await builder.Build().RunAsync();
