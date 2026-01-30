using System.Text.Json;
using OptimizelyLearningCentre.Client.Models.Query;

namespace OptimizelyLearningCentre.Client.Services;

/// <summary>
/// Client for executing GraphQL queries against Optimizely Graph
/// </summary>
public interface IGraphQLClient
{
    /// <summary>
    /// Executes a GraphQL query and returns the raw JSON response
    /// </summary>
    Task<GraphQLResponse<JsonElement>> ExecuteAsync(
        GraphQLRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes a GraphQL query with a typed response
    /// </summary>
    Task<GraphQLResponse<T>> ExecuteAsync<T>(
        GraphQLRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Tests the connection to the configured endpoint
    /// </summary>
    Task<(bool Success, string Message)> TestConnectionAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the raw HTTP response details from the last request
    /// </summary>
    LastRequestInfo? LastRequest { get; }
}

/// <summary>
/// Information about the last HTTP request made
/// </summary>
public class LastRequestInfo
{
    public string Url { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public Dictionary<string, string> RequestHeaders { get; set; } = new();
    public string RequestBody { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public Dictionary<string, string> ResponseHeaders { get; set; } = new();
    public string ResponseBody { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }
}
