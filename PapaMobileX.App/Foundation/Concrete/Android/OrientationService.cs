using Android.Content.PM;
using PapaMobileX.App.Foundation.Enums;
using PapaMobileX.App.Foundation.Services;

namespace PapaMobileX.App.Services.Concrete;

public partial class OrientationService
{
    public partial void LockOrientation(Orientation orientation)
    {
        var activity = Platform.CurrentActivity ?? MainActivity.Context;

        switch (orientation)
        { 
            case Orientation.Landscape:
                activity.RequestedOrientation = ScreenOrientation.Landscape;
                break;
            case Orientation.Portrait:
                activity.RequestedOrientation = ScreenOrientation.Portrait;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null);
        }
    }

    public partial void UnlockOrientation()
    {
        Platform.CurrentActivity.RequestedOrientation = ScreenOrientation.Unspecified;
    }
}