using PapaMobileX.App.Foundation.Enums;

namespace PapaMobileX.App.Foundation.Services;

public interface IOrientationService
{
    public void LockOrientation(Orientation orientation);
    public void UnlockOrientation();
}