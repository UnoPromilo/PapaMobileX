using PapaMobileX.App.BusinessLogic.ViewModels;
using PapaMobileX.App.Foundation.Enums;
using PapaMobileX.App.Foundation.Interfaces;

namespace PapaMobileX.App.Views;

public partial class SteeringPage
{
    private readonly IOrientationService _orientationService;

    public SteeringPage(SteeringViewModel steeringViewModel, IOrientationService orientationService)
    {
        BindingContext = steeringViewModel;
        _orientationService = orientationService;
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _orientationService.LockOrientation(Orientation.Landscape);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _orientationService.UnlockOrientation();
    }
}