using System.ComponentModel;
using System.Runtime.CompilerServices;
using OptimizelyLearningCentre.Client.Models.Query;

namespace OptimizelyLearningCentre.Client.State;

/// <summary>
/// State for the query builder and playground
/// </summary>
public class QueryState : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private QueryDefinition _currentDefinition = new();
    public QueryDefinition CurrentDefinition
    {
        get => _currentDefinition;
        set => SetProperty(ref _currentDefinition, value);
    }

    private string _rawQuery = string.Empty;
    public string RawQuery
    {
        get => _rawQuery;
        set => SetProperty(ref _rawQuery, value);
    }

    private string _variables = "{}";
    public string Variables
    {
        get => _variables;
        set => SetProperty(ref _variables, value);
    }

    private string _lastResponse = string.Empty;
    public string LastResponse
    {
        get => _lastResponse;
        set => SetProperty(ref _lastResponse, value);
    }

    private bool _isExecuting;
    public bool IsExecuting
    {
        get => _isExecuting;
        set => SetProperty(ref _isExecuting, value);
    }

    private List<QueryHistoryItem> _history = new();
    public List<QueryHistoryItem> History
    {
        get => _history;
        set => SetProperty(ref _history, value);
    }

    private int _maxHistoryItems = 50;
    public int MaxHistoryItems
    {
        get => _maxHistoryItems;
        set => SetProperty(ref _maxHistoryItems, value);
    }

    /// <summary>
    /// Adds a query execution to history
    /// </summary>
    public void AddToHistory(string query, string response, bool success)
    {
        var item = new QueryHistoryItem
        {
            Query = query,
            Response = response,
            ExecutedAt = DateTime.UtcNow,
            Success = success
        };

        var newHistory = new List<QueryHistoryItem> { item };
        newHistory.AddRange(_history.Take(MaxHistoryItems - 1));
        History = newHistory;
    }

    /// <summary>
    /// Clears the query history
    /// </summary>
    public void ClearHistory()
    {
        History = new List<QueryHistoryItem>();
    }

    /// <summary>
    /// Resets the query builder to initial state
    /// </summary>
    public void Reset()
    {
        CurrentDefinition = new QueryDefinition();
        RawQuery = string.Empty;
        Variables = "{}";
        LastResponse = string.Empty;
        IsExecuting = false;
    }

    /// <summary>
    /// Loads a query from history
    /// </summary>
    public void LoadFromHistory(QueryHistoryItem item)
    {
        RawQuery = item.Query;
        LastResponse = item.Response;
    }

    protected void SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (!EqualityComparer<T>.Default.Equals(field, value))
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public void NotifyStateChanged() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
}

/// <summary>
/// A single item in the query history
/// </summary>
public class QueryHistoryItem
{
    public string Query { get; set; } = string.Empty;
    public string Response { get; set; } = string.Empty;
    public DateTime ExecutedAt { get; set; }
    public bool Success { get; set; }

    public string ShortQuery => Query.Length > 100 ? Query[..100] + "..." : Query;
    public string TimeAgo => GetTimeAgo(ExecutedAt);

    private static string GetTimeAgo(DateTime dateTime)
    {
        var span = DateTime.UtcNow - dateTime;

        if (span.TotalMinutes < 1)
            return "just now";
        if (span.TotalMinutes < 60)
            return $"{(int)span.TotalMinutes}m ago";
        if (span.TotalHours < 24)
            return $"{(int)span.TotalHours}h ago";
        return $"{(int)span.TotalDays}d ago";
    }
}
