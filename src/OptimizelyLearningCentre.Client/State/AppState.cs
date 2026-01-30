using System.ComponentModel;
using System.Runtime.CompilerServices;
using OptimizelyLearningCentre.Client.Models.Configuration;

namespace OptimizelyLearningCentre.Client.State;

/// <summary>
/// Global application state
/// </summary>
public class AppState : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private ConnectionStatus _connectionStatus = ConnectionStatus.Unknown;
    public ConnectionStatus ConnectionStatus
    {
        get => _connectionStatus;
        set => SetProperty(ref _connectionStatus, value);
    }

    private bool _isSidebarOpen = true;
    public bool IsSidebarOpen
    {
        get => _isSidebarOpen;
        set => SetProperty(ref _isSidebarOpen, value);
    }

    private string? _currentModuleId;
    public string? CurrentModuleId
    {
        get => _currentModuleId;
        set => SetProperty(ref _currentModuleId, value);
    }

    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    private string? _errorMessage;
    public string? ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    private string? _successMessage;
    public string? SuccessMessage
    {
        get => _successMessage;
        set => SetProperty(ref _successMessage, value);
    }

    public void ShowError(string message)
    {
        ErrorMessage = message;
        SuccessMessage = null;
    }

    public void ShowSuccess(string message)
    {
        SuccessMessage = message;
        ErrorMessage = null;
    }

    public void ClearMessages()
    {
        ErrorMessage = null;
        SuccessMessage = null;
    }

    public void ToggleSidebar()
    {
        IsSidebarOpen = !IsSidebarOpen;
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
