using PapaMobileX.App.Foundation.Enums;
using PapaMobileX.App.Foundation.Services;

namespace PapaMobileX.App.Services.Concrete;

public partial class OrientationService : IOrientationService
{
    public partial void LockOrientation(Orientation orientation);

    public partial void UnlockOrientation();
}