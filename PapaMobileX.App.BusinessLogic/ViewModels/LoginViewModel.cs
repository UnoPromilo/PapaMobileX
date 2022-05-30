using System.ComponentModel;
using System.Runtime.CompilerServices;
using PapaMobileX.App.BusinessLogic.Models;
using PapaMobileX.App.BusinessLogic.ResourceDictionary;
using PapaMobileX.App.BusinessLogic.Services.Interfaces;
using PapaMobileX.App.Shared.Commands.Concrete;
using PapaMobileX.App.Shared.Enums;
using PapaMobileX.App.Shared.Errors;
using PapaMobileX.Shared.Results;

namespace PapaMobileX.App.BusinessLogic.ViewModels;

public class LoginViewModel : INotifyPropertyChanged
{
    private readonly ILoginService _loginService;
    private readonly IRandomJokeService _randomJokeService;
    private string _errorMessage = String.Empty;
    private string _joke = Resources.Loading;

    public LoginViewModel(IRandomJokeService randomJokeService, ILoginService loginService)
    {
        _randomJokeService = randomJokeService;
        _loginService = loginService;
        _ = RefreshJoke();
        LoginCommand = new AsyncCommand(Login);
    }

    public string Joke
    {
        get => _joke;
        private set => SetField(ref _joke, value);
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        private set => SetField(ref _errorMessage, value);
    }

    public AsyncCommand LoginCommand { get; init; }

    public LoginModel FormModel { get; init; } = new();

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    private async Task RefreshJoke()
    {
        string joke = await _randomJokeService.GetRandomJoke();
        Joke = joke;
    }

    private async Task Login()
    {
        Result<LoginError> result = await _loginService.Login(FormModel);
        if (result.IsFailed)
        {
            switch (result.Error.LoginErrorType)
            {
                case LoginErrorType.InvalidUri:
                    ErrorMessage = Resources.InvalidUriLoginError;
                    break;
                case LoginErrorType.ServerNotFound:
                    ErrorMessage = Resources.ServerNotFoundLoginError;
                    break;
                case LoginErrorType.InvalidCredentials:
                    ErrorMessage = Resources.InvalidCredentialsLoginError;
                    break;
                case LoginErrorType.Timeoout:
                    ErrorMessage = Resources.TimeoutLoginError;
                    break;
                case LoginErrorType.Other:
                    ErrorMessage = Resources.OtherLoginError;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}