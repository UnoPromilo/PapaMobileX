using System.Diagnostics;
using System.Windows.Input;
using PapaMobileX.App.Shared.Commands.Interfaces;

namespace PapaMobileX.App.Shared.Commands.Concrete;

public class AsyncCommand : IAsyncCommand
{
    private readonly Func<bool>? _canExecute;
    private readonly Func<Task> _execute;

    private bool _isExecuting;

    public AsyncCommand(Func<Task> execute,
                        Func<bool>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute()
    {
        return !_isExecuting && (_canExecute?.Invoke() ?? true);
    }

    public async Task ExecuteAsync()
    {
        if (CanExecute())
        {
            try
            {
                _isExecuting = true;
                await _execute();
            }
            #if DEBUG
            catch (Exception ex)
            {
                Debug.Write(ex);
            }
            #endif
            finally
            {
                _isExecuting = false;
            }
        }

        RaiseCanExecuteChanged();
    }

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    #region Explicit implementations

    bool ICommand.CanExecute(object? parameter)
    {
        return CanExecute();
    }

    void ICommand.Execute(object? parameter)
    {
        _ = ExecuteAsync();
    }

    #endregion
}

public class AsyncCommand<T> : IAsyncCommand<T>
{
    private readonly Func<T?, bool>? _canExecute;
    private readonly Func<T?, Task> _execute;
    private bool _isExecuting;

    public AsyncCommand(Func<T?, Task> execute, Func<T?, bool>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(T? parameter)
    {
        return !_isExecuting && (_canExecute?.Invoke(parameter) ?? true);
    }

    public async Task ExecuteAsync(T? parameter)
    {
        if (CanExecute(parameter))
        {
            try
            {
                _isExecuting = true;
                await _execute(parameter);
            }
            finally
            {
                _isExecuting = false;
            }
        }

        RaiseCanExecuteChanged();
    }

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    #region Explicit implementations

    bool ICommand.CanExecute(object? parameter)
    {
        return CanExecute((T?)parameter);
    }

    void ICommand.Execute(object? parameter)
    {
        _ = ExecuteAsync((T?)parameter);
    }

    #endregion
}