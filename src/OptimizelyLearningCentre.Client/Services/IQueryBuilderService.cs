using OptimizelyLearningCentre.Client.Models.Query;

namespace OptimizelyLearningCentre.Client.Services;

/// <summary>
/// Service for building GraphQL query strings from structured definitions
/// </summary>
public interface IQueryBuilderService
{
    /// <summary>
    /// Builds a GraphQL query string from a query definition
    /// </summary>
    string BuildQuery(QueryDefinition definition);

    /// <summary>
    /// Formats a GraphQL query string with proper indentation
    /// </summary>
    string FormatQuery(string query);

    /// <summary>
    /// Validates a GraphQL query string
    /// </summary>
    (bool IsValid, List<string> Errors) ValidateQuery(string query);
}
