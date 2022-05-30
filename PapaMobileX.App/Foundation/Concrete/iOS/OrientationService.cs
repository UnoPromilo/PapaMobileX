using Foundation;
using PapaMobileX.App.Foundation.Enums;
using PapaMobileX.App.Foundation.Services;
using UIKit;

namespace PapaMobileX.App.Services.Concrete;

public partial class OrientationService
{
    public partial void LockOrientation(Orientation orientation)
    {
        switch (orientation)
        {
            case Orientation.Landscape:
                ForceLandscape();
                break;
            case Orientation.Portrait:
                ForcePortrait();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null);
        }
    }

    public partial void UnlockOrientation()
    {
        UIDevice.CurrentDevice.SetValueForKey(new NSNumber((int)UIInterfaceOrientation.Unknown), new NSString("orientation"));
    }
    
    private void ForceLandscape()
    {
        UIDevice.CurrentDevice.SetValueForKey(new NSNumber((int)UIInterfaceOrientation.LandscapeRight), new NSString("orientation"));
    }

    private void ForcePortrait()
    {
        UIDevice.CurrentDevice.SetValueForKey(new NSNumber((int)UIInterfaceOrientation.Portrait), new NSString("orientation"));
    }
}