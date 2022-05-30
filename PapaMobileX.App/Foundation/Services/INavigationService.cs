namespace PapaMobileX.App.Foundation.Services;

public interface INavigationService
{
    Task NavigateToPage<T>(object? parameter = null) where T : Page;

    Task NavigateBack();
}