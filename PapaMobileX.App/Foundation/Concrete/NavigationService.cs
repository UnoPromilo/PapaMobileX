using PapaMobileX.App.Foundation.Interfaces;

namespace PapaMobileX.App.Foundation.Concrete;

public class NavigationService : INavigationService
{
    private readonly IServiceProvider _services;

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

    public async Task NavigateToPage<T>(object? parameter = null) where T : Page
    {
        var toPage = ResolvePage<T>();
        await Navigation.PushAsync(toPage, true);
    }

    public Task NavigateBack()
    {
        if (Navigation.NavigationStack.Count > 1)
            return Navigation.PopAsync();
        throw new InvalidOperationException("No pages to navigate back to!");
    }

    private T ResolvePage<T>() where T : Page
    {
        return (T)_services.GetService(typeof(T))!;
    }
}