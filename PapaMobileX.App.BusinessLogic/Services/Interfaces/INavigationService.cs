namespace PapaMobileX.App.BusinessLogic.Services.Interfaces;

public interface INavigationService
{
    Task NavigateToPageByViewModelAsync<T>(T? viewModel = null) where T : class;

    void PopLastPageByViewModel<T>(T viewModel) where T : class;

    Task NavigateBack();
}