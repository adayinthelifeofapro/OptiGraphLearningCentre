namespace OptimizelyLearningCentre.Client.Models.Query;

/// <summary>
/// Structured representation of a GraphQL query for Optimizely Graph
/// </summary>
public class QueryDefinition
{
    /// <summary>
    /// The content type to query (e.g., "BlogPost", "StandardPage")
    /// </summary>
    public string ContentType { get; set; } = string.Empty;

    /// <summary>
    /// Fields to include in the query response
    /// </summary>
    public List<string> SelectedFields { get; set; } = new();

    /// <summary>
    /// Filter conditions for the where clause
    /// </summary>
    public List<FilterDefinition> Filters { get; set; } = new();

    /// <summary>
    /// Sorting configuration
    /// </summary>
    public List<SortDefinition> Sorts { get; set; } = new();

    /// <summary>
    /// Pagination options
    /// </summary>
    public PaginationOptions Pagination { get; set; } = new();

    /// <summary>
    /// Content locale to query
    /// </summary>
    public string? Locale { get; set; }

    /// <summary>
    /// Full-text search term
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Facet definitions for aggregations
    /// </summary>
    public List<FacetDefinition> Facets { get; set; } = new();

    /// <summary>
    /// Whether to include total count
    /// </summary>
    public bool IncludeTotal { get; set; } = true;

    /// <summary>
    /// Creates a deep clone of this definition
    /// </summary>
    public QueryDefinition Clone()
    {
        return new QueryDefinition
        {
            ContentType = ContentType,
            SelectedFields = new List<string>(SelectedFields),
            Filters = Filters.Select(f => f.Clone()).ToList(),
            Sorts = Sorts.Select(s => s.Clone()).ToList(),
            Pagination = Pagination.Clone(),
            Locale = Locale,
            SearchTerm = SearchTerm,
            Facets = Facets.Select(f => f.Clone()).ToList(),
            IncludeTotal = IncludeTotal
        };
    }
}

/// <summary>
/// Represents a filter condition in the where clause
/// </summary>
public class FilterDefinition
{
    /// <summary>
    /// Unique identifier for this filter
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// The field to filter on
    /// </summary>
    public string Field { get; set; } = string.Empty;

    /// <summary>
    /// The filter operator to use
    /// </summary>
    public FilterOperator Operator { get; set; } = FilterOperator.Eq;

    /// <summary>
    /// The value to filter by
    /// </summary>
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Multiple values for 'in' and 'notIn' operators
    /// </summary>
    public List<string> Values { get; set; } = new();

    /// <summary>
    /// How this filter combines with previous filters
    /// </summary>
    public FilterLogic Logic { get; set; } = FilterLogic.And;

    /// <summary>
    /// Nested filters for complex conditions
    /// </summary>
    public List<FilterDefinition> NestedFilters { get; set; } = new();

    public FilterDefinition Clone()
    {
        return new FilterDefinition
        {
            Id = Id,
            Field = Field,
            Operator = Operator,
            Value = Value,
            Values = new List<string>(Values),
            Logic = Logic,
            NestedFilters = NestedFilters.Select(f => f.Clone()).ToList()
        };
    }
}

/// <summary>
/// Filter operators supported by Optimizely Graph
/// </summary>
public enum FilterOperator
{
    /// <summary>Equals - exact match</summary>
    Eq,
    /// <summary>Not equals</summary>
    NotEq,
    /// <summary>Pattern matching with wildcards (use * for wildcard, e.g. *text*)</summary>
    Like,
    /// <summary>Greater than</summary>
    Gt,
    /// <summary>Greater than or equal</summary>
    Gte,
    /// <summary>Less than</summary>
    Lt,
    /// <summary>Less than or equal</summary>
    Lte,
    /// <summary>Field exists and is not null</summary>
    Exist,
    /// <summary>Starts with string</summary>
    StartsWith,
    /// <summary>Ends with string</summary>
    EndsWith,
    /// <summary>Value is in array</summary>
    In,
    /// <summary>Value is not in array</summary>
    NotIn,
    /// <summary>Boost relevance score</summary>
    Boost,
    /// <summary>Enable synonym expansion</summary>
    Synonyms
}

/// <summary>
/// Logical operator for combining filters
/// </summary>
public enum FilterLogic
{
    And,
    Or
}

/// <summary>
/// Represents a sort definition
/// </summary>
public class SortDefinition
{
    /// <summary>
    /// The field to sort by
    /// </summary>
    public string Field { get; set; } = string.Empty;

    /// <summary>
    /// Sort direction
    /// </summary>
    public SortDirection Direction { get; set; } = SortDirection.Asc;

    /// <summary>
    /// Sort order (for multiple sort fields)
    /// </summary>
    public int Order { get; set; }

    public SortDefinition Clone()
    {
        return new SortDefinition
        {
            Field = Field,
            Direction = Direction,
            Order = Order
        };
    }
}

/// <summary>
/// Sort direction
/// </summary>
public enum SortDirection
{
    Asc,
    Desc
}

/// <summary>
/// Pagination options
/// </summary>
public class PaginationOptions
{
    /// <summary>
    /// Number of items to skip
    /// </summary>
    public int? Skip { get; set; }

    /// <summary>
    /// Maximum number of items to return
    /// </summary>
    public int? Limit { get; set; } = 10;

    /// <summary>
    /// Cursor for cursor-based pagination
    /// </summary>
    public string? Cursor { get; set; }

    /// <summary>
    /// Whether to use cursor-based pagination
    /// </summary>
    public bool UseCursorPagination { get; set; }

    public PaginationOptions Clone()
    {
        return new PaginationOptions
        {
            Skip = Skip,
            Limit = Limit,
            Cursor = Cursor,
            UseCursorPagination = UseCursorPagination
        };
    }
}

/// <summary>
/// Facet definition for aggregations
/// </summary>
public class FacetDefinition
{
    /// <summary>
    /// The field to facet on
    /// </summary>
    public string Field { get; set; } = string.Empty;

    /// <summary>
    /// Maximum number of facet values to return
    /// </summary>
    public int? Limit { get; set; }

    /// <summary>
    /// Order facets by count or value
    /// </summary>
    public FacetOrderBy OrderBy { get; set; } = FacetOrderBy.Count;

    public FacetDefinition Clone()
    {
        return new FacetDefinition
        {
            Field = Field,
            Limit = Limit,
            OrderBy = OrderBy
        };
    }
}

/// <summary>
/// How to order facet results
/// </summary>
public enum FacetOrderBy
{
    Count,
    Value
}
