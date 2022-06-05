using PapaMobileX.App.BusinessLogic.Models;
using PapaMobileX.App.BusinessLogic.ResourceDictionary;
using PapaMobileX.App.BusinessLogic.Services.Concrete;
using PapaMobileX.App.BusinessLogic.Services.Interfaces;
using PapaMobileX.App.BusinessLogic.ViewModels.Abstractions;
using PapaMobileX.App.Shared.Commands.Concrete;
using PapaMobileX.App.Shared.Enums;
using PapaMobileX.App.Shared.Errors;
using PapaMobileX.Shared.Results;

namespace PapaMobileX.App.BusinessLogic.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private readonly ILoginService _loginService;
    private readonly ILoginDataService _loginDataService;
    private readonly ITokenService _tokenService;
    private readonly INavigationService _navigationService;
    private readonly IApiClientService _apiClientService;
    private readonly ILogoutService _logoutService;
    private readonly IRandomJokeService _randomJokeService;
    private string _userName = String.Empty;
    private string _password = String.Empty;
    private string _serverAddress = String.Empty;
    private string _joke = Resources.Loading;
    private bool _loginInProgress = false;

    public LoginViewModel(IRandomJokeService randomJokeService, ILoginService loginService, ILoginDataService loginDataService, ITokenService tokenService, INavigationService navigationService, IApiClientService apiClientService, ILogoutService logoutService)
    {
        _randomJokeService = randomJokeService;
        _loginService = loginService;
        _loginDataService = loginDataService;
        _tokenService = tokenService;
        _navigationService = navigationService;
        _apiClientService = apiClientService;
        _logoutService = logoutService;
        _ = RefreshJokeAsync();
        _ = LoadLoginDataAsync();
        LoginCommand = new AsyncCommand(Login, CanExecuteLogin);
        if (tokenService.IsTokenValid)
        {
            _ = TryToReconnect();
        }
    }

    public string Joke
    {
        get => _joke;
        private set => SetField(ref _joke, value);
    }

    public string ServerAddress
    {
        get => _serverAddress;
        set => SetField(ref _serverAddress, value);
    }
    
    public string UserName
    {
        get => _userName;
        set => SetField(ref _userName, value);
    }
    
    public string Password
    {
        get => _password;
        set => SetField(ref _password, value);
    }

    public bool IsServerAddressValid => GetErrors(nameof(ServerAddress)).Count == 0;
    public bool IsUserNameValid => GetErrors(nameof(UserName)).Count == 0;
    public bool IsPasswordValid => GetErrors(nameof(Password)).Count == 0;

    public AsyncCommand LoginCommand { get; init; }
    
    
    protected override void OnErrorsChanged(string? propertyName)
    {
        base.OnErrorsChanged(propertyName);
        OnPropertyChanged(nameof(IsServerAddressValid));
        OnPropertyChanged(nameof(IsUserNameValid));
        OnPropertyChanged(nameof(IsUserNameValid));
    }

    private async Task RefreshJokeAsync()
    {
        string joke = await _randomJokeService.GetRandomJoke();
        Joke = joke;
    }
    
    private async Task LoadLoginDataAsync()
    {
        var model = await _loginDataService.ReadSavedLoginModelAsync();
        if (model is not null)
        {
            ServerAddress = model.Address;
            UserName = model.UserName;
            Password = model.Password;
        }
    }

    private async Task Login()
    {
        UpdateLoginInProgress(true);
        var model = new LoginModel
        {
            Address = ServerAddress,
            Password = Password,
            UserName = UserName
        };
        ClearErrors();
        Result<LoginError> result = await _loginService.Login(model);
        if (result.IsFailed)
        {
            switch (result.Error.LoginErrorType)
            {
                case LoginErrorType.InvalidUri:
                    AddError(Resources.InvalidUriLoginError, nameof(ServerAddress));
                    break;
                case LoginErrorType.ServerNotFound:
                    AddError(Resources.ServerNotFoundLoginError, nameof(ServerAddress));
                    break;
                case LoginErrorType.InvalidCredentials:
                    AddError(Resources.InvalidCredentialsLoginError, nameof(UserName));
                    AddError(Resources.InvalidCredentialsLoginError, nameof(Password));
                    break;
                case LoginErrorType.Timeoout:
                    AddError(Resources.TimeoutLoginError, String.Empty);
                    break;
                case LoginErrorType.Other:
                    AddError(Resources.OtherLoginError, String.Empty); ;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return;
        }
        _ = _loginDataService.SaveLoginModelAsync(model);
        await _navigationService.NavigateToPageByViewModelAsync<SteeringViewModel>();
        UpdateLoginInProgress(false);
    }

    private async Task TryToReconnect()
    {
        UpdateLoginInProgress(true);
        Result<HttpError> result = await _apiClientService.TestConnectionAsync();
        if (result.IsSuccess)
        {
            await _loginService.InitializeConnectionAsync();
            await _navigationService.NavigateToPageByViewModelAsync<SteeringViewModel>();
        }
        else
        {
            _logoutService.Logout();
        }
        UpdateLoginInProgress(false);
    }
    
    
    private bool CanExecuteLogin()
    {
        return _loginInProgress;
    }

    private void UpdateLoginInProgress(bool newValue)
    {
        if (newValue == _loginInProgress)
            return;
        
        _loginInProgress = newValue;
        LoginCommand.RaiseCanExecuteChanged();
    }
}