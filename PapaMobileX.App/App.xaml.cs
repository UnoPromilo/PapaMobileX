using PapaMobileX.App.Foundation.Services;
using PapaMobileX.App.Views;

namespace PapaMobileX.App;

public partial class App : Application
{
    public App(INavigationService navigationService)
    {
        InitializeComponent();
        AccentColor = Resources["PrimaryColor"] as Color;
        MainPage = new NavigationPage();
        _ = navigationService.NavigateToPage<LoginPage>();
    }
}