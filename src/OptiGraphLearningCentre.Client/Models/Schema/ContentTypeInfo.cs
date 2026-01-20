namespace OptiGraphLearningCentre.Client.Models.Schema;

/// <summary>
/// Information about a content type discovered from the GraphQL schema
/// </summary>
public class ContentTypeInfo
{
    /// <summary>
    /// The name of the content type (e.g., "BlogPost", "StandardPage")
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description from the schema
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Fields available on this content type
    /// </summary>
    public List<FieldInfo> Fields { get; set; } = new();

    /// <summary>
    /// Whether this type can be queried directly
    /// </summary>
    public bool IsQueryable { get; set; } = true;

    /// <summary>
    /// Interfaces this type implements
    /// </summary>
    public List<string> Interfaces { get; set; } = new();
}

/// <summary>
/// Information about a field on a content type
/// </summary>
public class FieldInfo
{
    /// <summary>
    /// Field name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// GraphQL type name (e.g., "String", "Int", "DateTime")
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Description from the schema
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Whether the field can be null
    /// </summary>
    public bool IsNullable { get; set; } = true;

    /// <summary>
    /// Whether this is a list/array type
    /// </summary>
    public bool IsList { get; set; }

    /// <summary>
    /// Whether this field can be filtered on
    /// </summary>
    public bool IsFilterable { get; set; } = true;

    /// <summary>
    /// Whether this field can be sorted on
    /// </summary>
    public bool IsSortable { get; set; } = true;

    /// <summary>
    /// Whether this field supports full-text search
    /// </summary>
    public bool IsSearchable { get; set; }

    /// <summary>
    /// Nested fields for complex types
    /// </summary>
    public List<FieldInfo>? NestedFields { get; set; }

    /// <summary>
    /// The underlying type for non-null and list wrappers
    /// </summary>
    public string UnderlyingType { get; set; } = string.Empty;

    /// <summary>
    /// Whether this is a scalar type (String, Int, etc.)
    /// </summary>
    public bool IsScalar { get; set; }

    /// <summary>
    /// Available filter operators for this field type
    /// </summary>
    public List<string> AvailableOperators { get; set; } = new();
}

/// <summary>
/// Schema introspection result
/// </summary>
public class SchemaInfo
{
    /// <summary>
    /// All queryable content types
    /// </summary>
    public List<ContentTypeInfo> ContentTypes { get; set; } = new();

    /// <summary>
    /// When the schema was last fetched
    /// </summary>
    public DateTime FetchedAt { get; set; }

    /// <summary>
    /// Available locales from the schema
    /// </summary>
    public List<string> AvailableLocales { get; set; } = new();
}
