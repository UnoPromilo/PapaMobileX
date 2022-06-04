using PapaMobileX.App.Foundation.Interfaces;
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

    public static Color ErrorColor => Color.FromArgb("C82525");
    public static Color PrimaryColor => Color.FromArgb("FEB301");
}