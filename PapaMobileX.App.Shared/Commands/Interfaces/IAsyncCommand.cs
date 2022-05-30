using System.Windows.Input;

namespace PapaMobileX.App.Shared.Commands.Interfaces;

public interface IAsyncCommand : ICommand
{
    Task ExecuteAsync();

    bool CanExecute();
}

public interface IAsyncCommand<T> : ICommand
{
    Task ExecuteAsync(T parameter);

    bool CanExecute(T parameter);
}