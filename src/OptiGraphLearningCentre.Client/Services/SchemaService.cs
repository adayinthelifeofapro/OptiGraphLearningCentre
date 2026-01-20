using System.Text.Json;
using OptiGraphLearningCentre.Client.Models.Query;
using OptiGraphLearningCentre.Client.Models.Schema;

namespace OptiGraphLearningCentre.Client.Services;

/// <summary>
/// Service for discovering GraphQL schema through introspection
/// </summary>
public class SchemaService : ISchemaService
{
    private readonly IGraphQLClient _graphQLClient;
    private readonly ILogger<SchemaService> _logger;

    private SchemaInfo? _cachedSchema;
    private readonly SemaphoreSlim _lock = new(1, 1);

    // Standard GraphQL introspection query
    private const string IntrospectionQuery = @"
        query IntrospectionQuery {
            __schema {
                queryType { name }
                types {
                    kind
                    name
                    description
                    fields(includeDeprecated: false) {
                        name
                        description
                        type {
                            kind
                            name
                            ofType {
                                kind
                                name
                                ofType {
                                    kind
                                    name
                                    ofType {
                                        kind
                                        name
                                    }
                                }
                            }
                        }
                    }
                    interfaces {
                        name
                    }
                }
            }
        }";

    // Types to exclude from queryable content types
    private static readonly HashSet<string> ExcludedTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "Query", "Mutation", "Subscription",
        "__Schema", "__Type", "__Field", "__InputValue", "__EnumValue", "__Directive",
        "String", "Int", "Float", "Boolean", "ID",
        "DateTime", "Date", "Time", "DateTimeOffset",
        "Decimal", "Long", "Short", "Byte",
        "Uri", "Guid", "TimeSpan"
    };

    // Prefixes that indicate internal types
    private static readonly string[] ExcludedPrefixes = { "__", "Query", "Mutation" };

    public bool IsSchemaLoaded => _cachedSchema != null;

    public event Action? OnSchemaRefreshed;

    public SchemaService(IGraphQLClient graphQLClient, ILogger<SchemaService> logger)
    {
        _graphQLClient = graphQLClient;
        _logger = logger;
    }

    public async Task<List<ContentTypeInfo>> GetContentTypesAsync(CancellationToken cancellationToken = default)
    {
        var schema = await GetSchemaInfoAsync(cancellationToken);
        return schema?.ContentTypes ?? new List<ContentTypeInfo>();
    }

    public async Task<List<string>> GetQueryableTypeNamesAsync(CancellationToken cancellationToken = default)
    {
        var schema = await GetSchemaInfoAsync(cancellationToken);
        return schema?.QueryableTypeNames ?? new List<string>();
    }

    public async Task<ContentTypeInfo?> GetContentTypeAsync(string typeName, CancellationToken cancellationToken = default)
    {
        var schema = await GetSchemaInfoAsync(cancellationToken);
        return schema?.ContentTypes.FirstOrDefault(t =>
            t.Name.Equals(typeName, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<SchemaInfo?> GetSchemaInfoAsync(CancellationToken cancellationToken = default)
    {
        if (_cachedSchema != null)
            return _cachedSchema;

        await _lock.WaitAsync(cancellationToken);
        try
        {
            if (_cachedSchema != null)
                return _cachedSchema;

            await LoadSchemaAsync(cancellationToken);
            return _cachedSchema;
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task RefreshSchemaAsync(CancellationToken cancellationToken = default)
    {
        await _lock.WaitAsync(cancellationToken);
        try
        {
            _cachedSchema = null;
            await LoadSchemaAsync(cancellationToken);
            OnSchemaRefreshed?.Invoke();
        }
        finally
        {
            _lock.Release();
        }
    }

    private async Task LoadSchemaAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Loading GraphQL schema via introspection");

        var request = new GraphQLRequest { Query = IntrospectionQuery };
        var response = await _graphQLClient.ExecuteAsync(request, cancellationToken);

        if (response.HasErrors)
        {
            var errors = string.Join("; ", response.Errors!.Select(e => e.Message));
            _logger.LogError("Schema introspection failed: {Errors}", errors);
            return;
        }

        try
        {
            var schema = ParseIntrospectionResult(response.Data);
            _cachedSchema = schema;
            _logger.LogInformation("Loaded {Count} content types from schema", schema.ContentTypes.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to parse introspection result");
        }
    }

    private SchemaInfo ParseIntrospectionResult(JsonElement? data)
    {
        var schema = new SchemaInfo
        {
            FetchedAt = DateTime.UtcNow,
            ContentTypes = new List<ContentTypeInfo>(),
            QueryableTypeNames = new List<string>()
        };

        if (!data.HasValue)
            return schema;

        var schemaElement = data.Value.GetProperty("__schema");
        var queryTypeName = schemaElement.GetProperty("queryType").GetProperty("name").GetString();

        var types = schemaElement.GetProperty("types");

        // First pass: Get all OBJECT types that could be content types
        var queryType = types.EnumerateArray()
            .FirstOrDefault(t => t.GetProperty("name").GetString() == queryTypeName);

        if (queryType.ValueKind == JsonValueKind.Undefined)
            return schema;

        // Get the fields on the Query type - these are our queryable content types
        var queryFields = queryType.GetProperty("fields");
        var queryableTypeNames = new HashSet<string>();

        foreach (var field in queryFields.EnumerateArray())
        {
            var fieldName = field.GetProperty("name").GetString() ?? "";

            // Skip GraphQL introspection fields (double underscore)
            if (fieldName.StartsWith("__"))
                continue;

            // Add to the list of queryable type names for the dropdown
            schema.QueryableTypeNames.Add(fieldName);

            // For content type matching, skip single underscore prefixed names
            if (fieldName.StartsWith("_"))
                continue;

            // The field name is often the content type name
            queryableTypeNames.Add(fieldName);
        }

        // Sort the queryable type names alphabetically
        schema.QueryableTypeNames.Sort();

        // Second pass: Build content type info for each type
        foreach (var type in types.EnumerateArray())
        {
            var typeName = type.GetProperty("name").GetString() ?? "";
            var typeKind = type.GetProperty("kind").GetString() ?? "";

            // Only process OBJECT types
            if (typeKind != "OBJECT")
                continue;

            // Skip excluded types
            if (ExcludedTypes.Contains(typeName))
                continue;

            if (ExcludedPrefixes.Any(p => typeName.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
                continue;

            // Skip types that end with common suffixes for input/output/connection types
            if (typeName.EndsWith("Input") || typeName.EndsWith("Output") ||
                typeName.EndsWith("Connection") || typeName.EndsWith("Edge") ||
                typeName.EndsWith("WhereInput") || typeName.EndsWith("OrderByInput") ||
                typeName.EndsWith("Facet") || typeName.EndsWith("Autocomplete"))
                continue;

            var contentType = new ContentTypeInfo
            {
                Name = typeName,
                Description = type.TryGetProperty("description", out var desc) && desc.ValueKind != JsonValueKind.Null
                    ? desc.GetString()
                    : null,
                IsQueryable = queryableTypeNames.Contains(typeName),
                Fields = new List<FieldInfo>(),
                Interfaces = new List<string>()
            };

            // Parse interfaces
            if (type.TryGetProperty("interfaces", out var interfaces))
            {
                foreach (var iface in interfaces.EnumerateArray())
                {
                    var ifaceName = iface.GetProperty("name").GetString();
                    if (!string.IsNullOrEmpty(ifaceName))
                        contentType.Interfaces.Add(ifaceName);
                }
            }

            // Parse fields
            if (type.TryGetProperty("fields", out var fields) && fields.ValueKind != JsonValueKind.Null)
            {
                foreach (var field in fields.EnumerateArray())
                {
                    var fieldInfo = ParseField(field);
                    if (fieldInfo != null)
                        contentType.Fields.Add(fieldInfo);
                }
            }

            // Only add if it has fields and looks like a content type
            if (contentType.Fields.Count > 0)
            {
                schema.ContentTypes.Add(contentType);
            }
        }

        // Sort by queryable first, then by name
        schema.ContentTypes = schema.ContentTypes
            .OrderByDescending(t => t.IsQueryable)
            .ThenBy(t => t.Name)
            .ToList();

        return schema;
    }

    private FieldInfo? ParseField(JsonElement field)
    {
        var name = field.GetProperty("name").GetString() ?? "";

        // Skip internal fields
        if (name.StartsWith("__"))
            return null;

        var typeInfo = ParseTypeInfo(field.GetProperty("type"));

        return new FieldInfo
        {
            Name = name,
            Description = field.TryGetProperty("description", out var desc) && desc.ValueKind != JsonValueKind.Null
                ? desc.GetString()
                : null,
            Type = typeInfo.TypeName,
            UnderlyingType = typeInfo.UnderlyingType,
            IsNullable = typeInfo.IsNullable,
            IsList = typeInfo.IsList,
            IsScalar = IsScalarType(typeInfo.UnderlyingType),
            IsFilterable = true, // Assume all fields are filterable
            IsSortable = IsScalarType(typeInfo.UnderlyingType),
            IsSearchable = typeInfo.UnderlyingType == "String",
            AvailableOperators = GetOperatorsForType(typeInfo.UnderlyingType)
        };
    }

    private (string TypeName, string UnderlyingType, bool IsNullable, bool IsList) ParseTypeInfo(JsonElement type)
    {
        var kind = type.GetProperty("kind").GetString() ?? "";
        var name = type.TryGetProperty("name", out var n) && n.ValueKind != JsonValueKind.Null
            ? n.GetString()
            : null;

        return kind switch
        {
            "NON_NULL" => ParseNonNullType(type),
            "LIST" => ParseListType(type),
            _ => (name ?? "Unknown", name ?? "Unknown", true, false)
        };
    }

    private (string TypeName, string UnderlyingType, bool IsNullable, bool IsList) ParseNonNullType(JsonElement type)
    {
        var ofType = type.GetProperty("ofType");
        var inner = ParseTypeInfo(ofType);
        return ($"{inner.TypeName}!", inner.UnderlyingType, false, inner.IsList);
    }

    private (string TypeName, string UnderlyingType, bool IsNullable, bool IsList) ParseListType(JsonElement type)
    {
        var ofType = type.GetProperty("ofType");
        var inner = ParseTypeInfo(ofType);
        return ($"[{inner.TypeName}]", inner.UnderlyingType, true, true);
    }

    private static bool IsScalarType(string typeName)
    {
        return typeName switch
        {
            "String" or "Int" or "Float" or "Boolean" or "ID" or
            "DateTime" or "Date" or "Time" or "DateTimeOffset" or
            "Decimal" or "Long" or "Short" or "Byte" or
            "Uri" or "Guid" => true,
            _ => false
        };
    }

    private static List<string> GetOperatorsForType(string typeName)
    {
        return typeName switch
        {
            "String" => new List<string> { "eq", "notEq", "like", "startsWith", "endsWith", "in", "notIn", "exist" },
            "Int" or "Float" or "Decimal" or "Long" or "Short" =>
                new List<string> { "eq", "notEq", "gt", "gte", "lt", "lte", "in", "notIn", "exist" },
            "DateTime" or "Date" or "DateTimeOffset" =>
                new List<string> { "eq", "notEq", "gt", "gte", "lt", "lte", "exist" },
            "Boolean" => new List<string> { "eq", "exist" },
            _ => new List<string> { "eq", "exist" }
        };
    }
}
