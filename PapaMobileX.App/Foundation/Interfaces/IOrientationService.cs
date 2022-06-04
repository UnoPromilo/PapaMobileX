using PapaMobileX.App.Foundation.Enums;

namespace PapaMobileX.App.Foundation.Interfaces;

public interface IOrientationService
{
    public void LockOrientation(Orientation orientation);

    public void UnlockOrientation();
}