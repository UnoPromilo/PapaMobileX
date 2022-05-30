using PapaMobileX.App.BusinessLogic.ViewModels;
using PapaMobileX.App.Foundation.Enums;
using PapaMobileX.App.Foundation.Services;

namespace PapaMobileX.App.Views;

public partial class LoginPage : ContentPage
{
    private readonly IOrientationService _orientationService;

    public LoginPage(LoginViewModel viewModel, IOrientationService orientationService)
    {
        _orientationService = orientationService;
        BindingContext = viewModel;
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _orientationService.LockOrientation(Orientation.Portrait);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _orientationService.UnlockOrientation();
    }
}