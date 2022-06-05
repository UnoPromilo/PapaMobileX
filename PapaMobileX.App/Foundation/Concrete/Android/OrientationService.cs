using Android.App;
using Android.Content.PM;
using PapaMobileX.App.Foundation.Enums;

namespace PapaMobileX.App.Foundation.Concrete;

public partial class OrientationService
{
    public partial void LockOrientation(Orientation orientation)
    {
        Activity activity = Platform.CurrentActivity ?? MainActivity.Context;

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
        Platform.CurrentActivity!.RequestedOrientation = ScreenOrientation.Unspecified;
    }
}