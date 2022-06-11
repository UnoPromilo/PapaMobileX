using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace PapaMobileX.App;

[Activity(Theme = "@style/Maui.SplashTheme",
          MainLauncher = true,
          ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
                                 ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
public class MainActivity : MauiAppCompatActivity
{
    private const SystemUiFlags UiOptions = SystemUiFlags.HideNavigation |
                                            SystemUiFlags.LayoutHideNavigation |
                                            SystemUiFlags.LayoutFullscreen |
                                            SystemUiFlags.Fullscreen |
                                            SystemUiFlags.LayoutStable |
                                            SystemUiFlags.ImmersiveSticky;

    public static Activity Context = null!;

    public MainActivity()
    {
        Context = this;
    }

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        Window!.AddFlags(WindowManagerFlags.Fullscreen);
        Window.DecorView.SystemUiVisibility = (StatusBarVisibility)UiOptions;

        Platform.Init(this, savedInstanceState);
    }

    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
    {
        Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        #pragma warning disable CA1416
        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        #pragma warning restore CA1416
    }
}