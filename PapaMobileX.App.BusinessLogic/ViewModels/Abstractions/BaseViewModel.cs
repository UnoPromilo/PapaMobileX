using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PapaMobileX.App.BusinessLogic.ViewModels.Abstractions;

public abstract class BaseViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private readonly IDictionary<string, IList<string>> _errorsByPropertyName =
        new Dictionary<string, IList<string>>();

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;


    IEnumerable INotifyDataErrorInfo.GetErrors(string? propertyName)
    {
        return _errorsByPropertyName.Where(e => e.Key == propertyName).SelectMany(e => e.Value);
    }
    
    public IList GetErrors([CallerMemberName]string? propertyName = null)
    {
        return _errorsByPropertyName.Where(e => e.Key == propertyName).SelectMany(e => e.Value).ToList();
    }

    public bool HasErrors => _errorsByPropertyName.Any(e => e.Key != String.Empty);
    
    public virtual string ErrorMessage => _errorsByPropertyName.SelectMany(e=>e.Value).FirstOrDefault() ?? String.Empty;

    protected void AddError(string error = "", [CallerMemberName] string propertyName = "")
    {
        if (!_errorsByPropertyName.ContainsKey(propertyName))
            _errorsByPropertyName[propertyName] = new List<string>();

        if (!_errorsByPropertyName[propertyName].Contains(error))
        {
            _errorsByPropertyName[propertyName].Add(error);
            OnErrorsChanged(propertyName);
        }
    }
    
    protected void ClearErrors(string? propertyName = null)
    {
        if (propertyName is null)
            _errorsByPropertyName.Clear();
        
        else
            _errorsByPropertyName.Remove(propertyName);
        OnErrorsChanged(propertyName);
    }
    
    protected virtual void OnErrorsChanged(string? propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        OnPropertyChanged(nameof(ErrorMessage));
    }

    public virtual void Validate([CallerMemberName] string propertyName = "")
    {
        ClearErrors(propertyName);
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;
        field = value;
        Validate(propertyName);
        OnPropertyChanged(propertyName);
        return true;
    }
}