using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using OptiGraphLearningCentre.Client;
using OptiGraphLearningCentre.Client.Services;
using OptiGraphLearningCentre.Client.State;

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

// Services
builder.Services.AddScoped<ISettingsService, LocalStorageSettingsService>();
builder.Services.AddScoped<IGraphQLClient, GraphQLClient>();
builder.Services.AddScoped<ISchemaService, SchemaService>();
builder.Services.AddScoped<IQueryBuilderService, QueryBuilderService>();
builder.Services.AddScoped<ILearningService, LearningService>();

// Logging
builder.Logging.SetMinimumLevel(LogLevel.Information);

await builder.Build().RunAsync();
