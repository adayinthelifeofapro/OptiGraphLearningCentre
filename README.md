# Optimizely Graph Learning Centre

An interactive learning platform for [Optimizely Graph](https://docs.developers.optimizely.com/content-graph), a GraphQL API service for content management. Built with Blazor WebAssembly, this client-side application provides hands-on tutorials, a query playground, and visual query builder to help developers master Optimizely Graph.

## Features

- **Interactive Learning Modules** - Step-by-step tutorials covering Optimizely Graph concepts
- **GraphQL Playground** - Execute queries directly against your Optimizely Graph instance
- **Visual Query Builder** - Construct GraphQL queries without writing code
- **Schema Explorer** - Browse and understand your content model
- **Multiple Authentication Modes** - Support for HMAC, SingleKey, and public access

## Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download) or later
- [Node.js](https://nodejs.org/) (for Tailwind CSS compilation)
- An Optimizely Graph instance (for query execution)

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/your-org/opti-graph-learning-centre.git
cd opti-graph-learning-centre
```

### 2. Install npm dependencies

```bash
cd src/OptiGraphLearningCentre.Client
npm install
```

### 3. Build Tailwind CSS

```bash
npm run css:build
```

### 4. Run the application

```bash
dotnet run --project src/OptiGraphLearningCentre.Client/OptiGraphLearningCentre.Client.csproj
```

The application will be available at:
- HTTP: http://localhost:5000
- HTTPS: https://localhost:7168

## Development

### Build Commands

```bash
# Build the application
dotnet build src/OptiGraphLearningCentre.Client/OptiGraphLearningCentre.Client.csproj

# Run in development mode
dotnet run --project src/OptiGraphLearningCentre.Client/OptiGraphLearningCentre.Client.csproj

# Watch Tailwind CSS changes (run in a separate terminal)
cd src/OptiGraphLearningCentre.Client && npm run css:watch

# Build minified CSS for production
cd src/OptiGraphLearningCentre.Client && npm run css:build
```

## Project Structure

```
src/OptiGraphLearningCentre.Client/
├── Components/
│   ├── Common/           # Reusable UI components (Badge, Card, JsonViewer, LoadingSpinner)
│   └── Learning/         # Learning-specific components (TryItPanel)
├── Layout/               # MainLayout and NavMenu
├── Models/
│   ├── Configuration/    # GraphQL connection settings
│   ├── Learning/         # Learning module models
│   ├── Query/            # Query definition models
│   ├── Schema/           # Schema introspection models
│   └── UI/               # UI enumerations
├── Pages/
│   ├── Home.razor        # Landing page
│   ├── Playground.razor  # GraphQL query sandbox
│   ├── QueryBuilder.razor # Visual query builder
│   ├── Settings.razor    # Connection configuration
│   └── Learn/            # Learning modules and lessons
├── Services/             # Business logic services
├── State/                # Application state management
├── Styles/               # Source Tailwind CSS
└── wwwroot/              # Static assets and compiled CSS
```

## Technology Stack

- **.NET 10.0** with **Blazor WebAssembly**
- **C# 13+** with nullable reference types
- **Tailwind CSS 3.4** for styling
- **Blazored.LocalStorage** for browser persistence

## Authentication

The application supports three authentication modes for connecting to Optimizely Graph:

| Mode | Description | Use Case |
|------|-------------|----------|
| **HMAC** | App Key + Secret authentication | Secured queries, draft content access |
| **SingleKey** | Public single authentication key | Public read-only access |
| **None** | No authentication | Public endpoints only |

Configure your connection settings in the Settings page of the application.

## Architecture

### State Management

The application uses property-based state stores implementing `INotifyPropertyChanged`:

- **AppState** - Global app state: connection status, sidebar toggle, loading indicators, notifications
- **QueryState** - Query builder state: current query definition, raw query, variables, execution history

### Core Services

All services are registered as Scoped in the DI container:

- **IGraphQLClient** - Executes GraphQL queries with authentication
- **ISettingsService** - Persists connection settings to browser localStorage
- **ISchemaService** - GraphQL schema introspection with caching
- **IQueryBuilderService** - Constructs GraphQL queries from structured definitions
- **ILearningService** - Manages learning modules and lesson content

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

[MIT License](LICENSE)

## Resources

- [Optimizely Graph Documentation](https://docs.developers.optimizely.com/content-graph)
- [Optimizely Developer Portal](https://docs.developers.optimizely.com/)
- [Blazor WebAssembly Documentation](https://docs.microsoft.com/aspnet/core/blazor/)
