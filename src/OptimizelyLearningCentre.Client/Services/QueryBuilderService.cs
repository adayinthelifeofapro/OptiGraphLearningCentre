using System.Text;
using OptimizelyLearningCentre.Client.Models.Query;

namespace OptimizelyLearningCentre.Client.Services;

/// <summary>
/// Service for building GraphQL query strings from structured definitions
/// </summary>
public class QueryBuilderService : IQueryBuilderService
{
    public string BuildQuery(QueryDefinition definition)
    {
        if (string.IsNullOrWhiteSpace(definition.ContentType))
            return "{\n  # Select a content type to begin\n}";

        var sb = new StringBuilder();
        sb.AppendLine("{");

        // Build the query with content type
        sb.Append($"  {definition.ContentType}");

        // Build arguments
        var args = BuildArguments(definition);
        if (args.Count > 0)
        {
            sb.AppendLine("(");
            for (var i = 0; i < args.Count; i++)
            {
                sb.Append($"    {args[i]}");
                if (i < args.Count - 1)
                    sb.AppendLine(",");
                else
                    sb.AppendLine();
            }
            sb.Append("  )");
        }

        sb.AppendLine(" {");

        // Add total count if requested
        if (definition.IncludeTotal)
        {
            sb.AppendLine("    total");
        }

        // Add cursor if using cursor pagination
        if (definition.Pagination.UseCursorPagination)
        {
            sb.AppendLine("    cursor");
        }

        // Build items selection
        sb.AppendLine("    items {");

        if (definition.SelectedFields.Count > 0)
        {
            foreach (var field in definition.SelectedFields)
            {
                sb.AppendLine($"      {field}");
            }
        }
        else
        {
            // Default fields when none selected
            sb.AppendLine("      _metadata {");
            sb.AppendLine("        key");
            sb.AppendLine("        displayName");
            sb.AppendLine("        types");
            sb.AppendLine("      }");
        }

        sb.AppendLine("    }");

        // Add facets if defined
        foreach (var facet in definition.Facets)
        {
            sb.AppendLine($"    {facet.Field}Facet {{");
            sb.AppendLine("      name");
            sb.AppendLine("      count");
            sb.AppendLine("    }");
        }

        sb.AppendLine("  }");
        sb.AppendLine("}");

        return sb.ToString();
    }

    private List<string> BuildArguments(QueryDefinition definition)
    {
        var args = new List<string>();

        // Locale - format as array for Optimizely Graph
        if (!string.IsNullOrEmpty(definition.Locale))
        {
            args.Add($"locale: [{definition.Locale}]");
        }

        // Where clause (filters) - only include filters with a field selected
        var validFilters = definition.Filters.Where(f => !string.IsNullOrWhiteSpace(f.Field)).ToList();
        if (validFilters.Count > 0)
        {
            var whereClause = BuildWhereClause(validFilters);
            args.Add($"where: {whereClause}");
        }

        // Full-text search
        if (!string.IsNullOrEmpty(definition.SearchTerm))
        {
            args.Add($"searchTerm: \"{EscapeString(definition.SearchTerm)}\"");
        }

        // OrderBy - only include sorts with a field selected
        var validSorts = definition.Sorts.Where(s => !string.IsNullOrWhiteSpace(s.Field)).ToList();
        if (validSorts.Count > 0)
        {
            var orderBy = BuildOrderByClause(validSorts);
            args.Add($"orderBy: {orderBy}");
        }

        // Pagination
        if (definition.Pagination.Skip.HasValue && definition.Pagination.Skip.Value > 0)
        {
            args.Add($"skip: {definition.Pagination.Skip}");
        }

        if (definition.Pagination.Limit.HasValue)
        {
            args.Add($"limit: {definition.Pagination.Limit}");
        }

        if (!string.IsNullOrEmpty(definition.Pagination.Cursor))
        {
            args.Add($"cursor: \"{definition.Pagination.Cursor}\"");
        }

        return args;
    }

    private string BuildWhereClause(List<FilterDefinition> filters)
    {
        if (filters.Count == 0)
            return "{}";

        var conditions = new List<string>();

        // Group filters by logic
        var andFilters = filters.Where(f => f.Logic == FilterLogic.And).ToList();
        var orFilters = filters.Where(f => f.Logic == FilterLogic.Or).ToList();

        // Build AND conditions
        foreach (var filter in andFilters)
        {
            conditions.Add(BuildFilterCondition(filter));
        }

        // Build OR conditions if any
        if (orFilters.Count > 0)
        {
            var orConditions = orFilters.Select(BuildFilterCondition);
            conditions.Add($"_or: [{{ {string.Join(" }}, {{ ", orConditions)} }}]");
        }

        return "{ " + string.Join(", ", conditions) + " }";
    }

    private string BuildFilterCondition(FilterDefinition filter)
    {
        var operatorName = GetOperatorName(filter.Operator);
        var value = FormatFilterValue(filter);

        return $"{filter.Field}: {{ {operatorName}: {value} }}";
    }

    private string GetOperatorName(FilterOperator op)
    {
        return op switch
        {
            FilterOperator.Eq => "eq",
            FilterOperator.NotEq => "notEq",
            FilterOperator.Like => "like",
            FilterOperator.Gt => "gt",
            FilterOperator.Gte => "gte",
            FilterOperator.Lt => "lt",
            FilterOperator.Lte => "lte",
            FilterOperator.Exist => "exist",
            FilterOperator.StartsWith => "startsWith",
            FilterOperator.EndsWith => "endsWith",
            FilterOperator.In => "in",
            FilterOperator.NotIn => "notIn",
            FilterOperator.Boost => "boost",
            FilterOperator.Synonyms => "synonyms",
            _ => "eq"
        };
    }

    private string FormatFilterValue(FilterDefinition filter)
    {
        // Handle multi-value operators
        if (filter.Operator == FilterOperator.In || filter.Operator == FilterOperator.NotIn)
        {
            var values = filter.Values.Count > 0 ? filter.Values : new List<string> { filter.Value };
            var formattedValues = values.Select(v => $"\"{EscapeString(v)}\"");
            return $"[{string.Join(", ", formattedValues)}]";
        }

        // Handle exist operator (boolean)
        if (filter.Operator == FilterOperator.Exist)
        {
            return filter.Value.ToLowerInvariant() == "true" ? "true" : "false";
        }

        // Handle boost operator (numeric)
        if (filter.Operator == FilterOperator.Boost)
        {
            return filter.Value;
        }

        // Try to parse as number for numeric operators
        if (filter.Operator is FilterOperator.Gt or FilterOperator.Gte or FilterOperator.Lt or FilterOperator.Lte)
        {
            if (double.TryParse(filter.Value, out _))
                return filter.Value;
        }

        // Default: string value
        return $"\"{EscapeString(filter.Value)}\"";
    }

    private string BuildOrderByClause(List<SortDefinition> sorts)
    {
        var sortFields = sorts
            .OrderBy(s => s.Order)
            .Select(s => $"{s.Field}: {s.Direction.ToString().ToUpperInvariant()}");

        return "{ " + string.Join(", ", sortFields) + " }";
    }

    private static string EscapeString(string value)
    {
        return value
            .Replace("\\", "\\\\")
            .Replace("\"", "\\\"")
            .Replace("\n", "\\n")
            .Replace("\r", "\\r")
            .Replace("\t", "\\t");
    }

    public string FormatQuery(string query)
    {
        // Simple formatter - proper GraphQL formatting
        var sb = new StringBuilder();
        var indent = 0;
        var inString = false;
        var lastChar = '\0';

        foreach (var c in query)
        {
            if (c == '"' && lastChar != '\\')
            {
                inString = !inString;
                sb.Append(c);
            }
            else if (inString)
            {
                sb.Append(c);
            }
            else if (c == '{')
            {
                sb.Append(c);
                sb.AppendLine();
                indent++;
                sb.Append(new string(' ', indent * 2));
            }
            else if (c == '}')
            {
                sb.AppendLine();
                indent = Math.Max(0, indent - 1);
                sb.Append(new string(' ', indent * 2));
                sb.Append(c);
            }
            else if (c == '\n' || c == '\r')
            {
                // Skip existing newlines
            }
            else if (c == ' ' && lastChar == ' ')
            {
                // Skip multiple spaces
            }
            else
            {
                sb.Append(c);
            }

            lastChar = c;
        }

        return sb.ToString().Trim();
    }

    public (bool IsValid, List<string> Errors) ValidateQuery(string query)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(query))
        {
            errors.Add("Query cannot be empty");
            return (false, errors);
        }

        // Basic brace matching
        var braceCount = 0;
        var inString = false;
        var lastChar = '\0';

        foreach (var c in query)
        {
            if (c == '"' && lastChar != '\\')
                inString = !inString;

            if (!inString)
            {
                if (c == '{') braceCount++;
                if (c == '}') braceCount--;

                if (braceCount < 0)
                {
                    errors.Add("Unexpected closing brace '}'");
                    return (false, errors);
                }
            }

            lastChar = c;
        }

        if (braceCount != 0)
        {
            errors.Add($"Mismatched braces: {Math.Abs(braceCount)} unclosed");
            return (false, errors);
        }

        if (inString)
        {
            errors.Add("Unterminated string");
            return (false, errors);
        }

        return (true, errors);
    }
}
