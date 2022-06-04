using PapaMobileX.App.Foundation.Enums;
using PapaMobileX.App.Foundation.Interfaces;

namespace PapaMobileX.App.Foundation.Concrete;

public partial class OrientationService : IOrientationService
{
    public partial void LockOrientation(Orientation orientation);

    public partial void UnlockOrientation();
}