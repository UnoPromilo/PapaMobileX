using Android.App;
using Android.Runtime;

namespace PapaMobileX.App;

[Application(NetworkSecurityConfig = "@xml/network_security_config")]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership) { }

    protected override MauiApp CreateMauiApp()
    {
        return MauiProgram.CreateMauiApp();
    }
}