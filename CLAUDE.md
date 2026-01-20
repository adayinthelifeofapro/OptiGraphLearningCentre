# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Optimizely Graph Learning Centre is a **Blazor WebAssembly (WASM)** interactive learning platform for Optimizely Graph, a GraphQL API service for content management. This is a client-side only application with no backend server.

## Build Commands

```bash
# Build the application
dotnet build src/OptiGraphLearningCentre.Client/OptiGraphLearningCentre.Client.csproj

# Run the application (HTTP: localhost:5000, HTTPS: localhost:7168)
dotnet run --project src/OptiGraphLearningCentre.Client/OptiGraphLearningCentre.Client.csproj

# Build Tailwind CSS (minified for production)
cd src/OptiGraphLearningCentre.Client && npm run css:build

# Watch Tailwind CSS (development mode)
cd src/OptiGraphLearningCentre.Client && npm run css:watch
```

**No test framework is currently configured.**

## Architecture

### Technology Stack
- **.NET 10.0** with **Blazor WebAssembly**
- **C# 13+** with nullable reference types enabled
- **Tailwind CSS 3.4** for styling
- **Blazored.LocalStorage** for browser persistence

### State Management
Uses property-based state stores implementing `INotifyPropertyChanged`:
- **AppState** (`State/AppState.cs`) - Global app state: connection status, sidebar toggle, loading indicators, notifications
- **QueryState** (`State/QueryState.cs`) - Query builder state: current query definition, raw query, variables, execution history

### Core Services (all registered as Scoped in DI)
- **IGraphQLClient/GraphQLClient** - Executes GraphQL queries with HMAC, SingleKey, or no authentication
- **ISettingsService/LocalStorageSettingsService** - Persists GraphQL connection settings to browser localStorage
- **ISchemaService/SchemaService** - GraphQL schema introspection with caching
- **IQueryBuilderService/QueryBuilderService** - Constructs GraphQL queries from structured definitions
- **ILearningService/LearningService** - Manages learning modules and lessons (contains substantial embedded content)

### Authentication Modes
The GraphQL client supports three authentication modes defined in `Models/Configuration/GraphSettings.cs`:
1. **HMAC** - AppKey + Secret for secured queries
2. **SingleKey** - Public single authentication key
3. **None** - No authentication

### Key Pages
- `Pages/Home.razor` - Landing page
- `Pages/Playground.razor` - GraphQL query sandbox
- `Pages/QueryBuilder.razor` - Visual query builder
- `Pages/Settings.razor` - Connection configuration
- `Pages/Learn/` - Learning modules and lessons

### Component Organization
- `Components/Common/` - Reusable UI components (Badge, Card, JsonViewer, LoadingSpinner)
- `Components/Learning/` - Learning-specific components (TryItPanel)
- `Layout/` - MainLayout and NavMenu

## Styling

Tailwind is configured with custom Optimizely brand colors in `tailwind.config.js`:
- `opti-blue`: #0037FF
- `opti-dark`: #1a1a2e
- `opti-accent`: #00D4AA
- `opti-light`: #f8fafc

Source CSS is in `Styles/app.css`, compiled output goes to `wwwroot/css/app.css`.
