using Android.App;
using Android.Content.PM;
using Android.OS;

namespace PapaMobileX.App;

[Activity(Theme = "@style/Maui.SplashTheme",
          MainLauncher = true,
          ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
                                 ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
public class MainActivity : MauiAppCompatActivity
{
    public static Activity Context = null!;

    public MainActivity()
    {
        Context = this;
    }

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
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