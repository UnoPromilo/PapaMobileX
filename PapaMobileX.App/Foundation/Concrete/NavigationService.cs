using PapaMobileX.App.BusinessLogic.Services.Interfaces;
using PapaMobileX.App.BusinessLogic.ViewModels;
using PapaMobileX.App.Views;

namespace PapaMobileX.App.Foundation.Concrete;

public class NavigationService : INavigationService
{
    private static readonly Dictionary<Type, Type> ViewViewModelDictionary = new()
    {
        { typeof(LoginViewModel), typeof(LoginPage) },
        { typeof(SteeringViewModel), typeof(SteeringPage) }
    };

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

    public async Task NavigateToPageByViewModelAsync<T>(T? viewModel = null) where T : class
    {
        if (ViewViewModelDictionary.ContainsKey(typeof(T)))
        {
            Type pageType = ViewViewModelDictionary[typeof(T)];
            var toPage = ResolvePage(pageType) as Page;
            if (viewModel is not null && toPage is not null)
                toPage.BindingContext = viewModel;
            await Navigation.PushAsync(toPage, true);
        }
    }

    public void PopLastPageByViewModel<T>(T viewModel) where T : class
    {
        Page? page = Navigation.NavigationStack.First(p => p.BindingContext == viewModel);
        Navigation.RemovePage(page);
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