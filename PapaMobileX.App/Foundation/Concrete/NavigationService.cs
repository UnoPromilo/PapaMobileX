using PapaMobileX.App.BusinessLogic.Services.Interfaces;
using PapaMobileX.App.BusinessLogic.ViewModels;
using PapaMobileX.App.Foundation.Interfaces;
using PapaMobileX.App.Views;

namespace PapaMobileX.App.Foundation.Concrete;

public class NavigationService : INavigationService
{
    private readonly IServiceProvider _services;

    public static Dictionary<Type, Type> ViewViewModel = new()
    {
        { typeof(LoginViewModel), typeof(LoginPage) },
        { typeof(SteeringViewModel), typeof(SteeringPage) },
    };

    public NavigationService(IServiceProvider services)
    {
        _services = services;
    }

    private INavigation Navigation
    {
        get
        {
            INavigation? navigation = Application.Current?.MainPage?.Navigation;
            if (navigation is not null)
                return navigation;
            throw new Exception();
        }
    }

    public async Task NavigateToPageByViewModelAsync<T>(object? parameter = null)
    {
        if (ViewViewModel.ContainsKey(typeof(T)))
        {
            var pageType = ViewViewModel[typeof(T)];
            var toPage = ResolvePage(pageType);
            await Navigation.PushAsync(toPage as Page, true);
        }
    }

    public Task NavigateBack()
    {
        if (Navigation.NavigationStack.Count > 1)
            return Navigation.PopAsync();
        throw new InvalidOperationException("No pages to navigate back to!");
    }

    private object ResolvePage(Type type)
    {
        return _services.GetService(type)!;
    }
}