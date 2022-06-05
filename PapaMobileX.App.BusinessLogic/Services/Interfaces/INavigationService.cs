namespace PapaMobileX.App.BusinessLogic.Services.Interfaces;

public interface INavigationService
{
    Task NavigateToPageByViewModelAsync<T>(object? parameter = null);

    Task NavigateBack();
}