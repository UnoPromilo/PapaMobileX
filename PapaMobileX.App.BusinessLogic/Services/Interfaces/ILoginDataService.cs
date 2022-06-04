using PapaMobileX.App.BusinessLogic.Models;

namespace PapaMobileX.App.BusinessLogic.Services.Interfaces;

public interface ILoginDataService
{
    public Task SaveLoginModelAsync(LoginModel loginModel);

    public Task<LoginModel?> ReadSavedLoginModelAsync();
}