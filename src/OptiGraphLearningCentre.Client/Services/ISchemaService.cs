using OptiGraphLearningCentre.Client.Models.Schema;

namespace OptiGraphLearningCentre.Client.Services;

/// <summary>
/// Service for discovering and caching GraphQL schema information
/// </summary>
public interface ISchemaService
{
    /// <summary>
    /// Gets all queryable content types from the schema
    /// </summary>
    Task<List<ContentTypeInfo>> GetContentTypesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets detailed information about a specific content type
    /// </summary>
    Task<ContentTypeInfo?> GetContentTypeAsync(string typeName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Forces a refresh of the cached schema
    /// </summary>
    Task RefreshSchemaAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the full schema info including metadata
    /// </summary>
    Task<SchemaInfo?> GetSchemaInfoAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Whether the schema has been loaded
    /// </summary>
    bool IsSchemaLoaded { get; }

    /// <summary>
    /// Event fired when the schema is refreshed
    /// </summary>
    event Action? OnSchemaRefreshed;
}
