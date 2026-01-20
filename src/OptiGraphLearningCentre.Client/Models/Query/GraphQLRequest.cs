using System.Text.Json.Serialization;

namespace OptiGraphLearningCentre.Client.Models.Query;

/// <summary>
/// Represents a GraphQL request to be sent to the server
/// </summary>
public class GraphQLRequest
{
    /// <summary>
    /// The GraphQL query string
    /// </summary>
    [JsonPropertyName("query")]
    public string Query { get; set; } = string.Empty;

    /// <summary>
    /// Optional operation name for queries with multiple operations
    /// </summary>
    [JsonPropertyName("operationName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? OperationName { get; set; }

    /// <summary>
    /// Query variables
    /// </summary>
    [JsonPropertyName("variables")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, object>? Variables { get; set; }
}

/// <summary>
/// Represents a GraphQL response from the server
/// </summary>
/// <typeparam name="T">The type of the data payload</typeparam>
public class GraphQLResponse<T>
{
    /// <summary>
    /// The data returned by the query
    /// </summary>
    [JsonPropertyName("data")]
    public T? Data { get; set; }

    /// <summary>
    /// Any errors that occurred during execution
    /// </summary>
    [JsonPropertyName("errors")]
    public List<GraphQLError>? Errors { get; set; }

    /// <summary>
    /// Extension data from the server
    /// </summary>
    [JsonPropertyName("extensions")]
    public Dictionary<string, object>? Extensions { get; set; }

    /// <summary>
    /// Whether the response contains errors
    /// </summary>
    [JsonIgnore]
    public bool HasErrors => Errors?.Count > 0;
}

/// <summary>
/// Represents a GraphQL error
/// </summary>
public class GraphQLError
{
    /// <summary>
    /// Error message
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Locations in the query where the error occurred
    /// </summary>
    [JsonPropertyName("locations")]
    public List<GraphQLErrorLocation>? Locations { get; set; }

    /// <summary>
    /// Path to the field that caused the error
    /// </summary>
    [JsonPropertyName("path")]
    public List<object>? Path { get; set; }

    /// <summary>
    /// Additional error extensions
    /// </summary>
    [JsonPropertyName("extensions")]
    public Dictionary<string, object>? Extensions { get; set; }
}

/// <summary>
/// Location in the query where an error occurred
/// </summary>
public class GraphQLErrorLocation
{
    [JsonPropertyName("line")]
    public int Line { get; set; }

    [JsonPropertyName("column")]
    public int Column { get; set; }
}
