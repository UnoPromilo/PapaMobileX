using System.Text.Json;
using PapaMobileX.App.BusinessLogic.Models;
using PapaMobileX.App.BusinessLogic.Services.Interfaces;

namespace PapaMobileX.App.Foundation.Concrete;

public class LoginDataService : ILoginDataService
{
    private const string LoginModelKey = $"{nameof(LoginDataService)}.LoginModel";
    public Task SaveLoginModelAsync(LoginModel loginModel)
    {
        var json = JsonSerializer.Serialize(loginModel);
        return SecureStorage.SetAsync(LoginModelKey, json);
    }

    public async Task<LoginModel?> ReadSavedLoginModelAsync()
    {
        var json = await SecureStorage.GetAsync(LoginModelKey);
        if (String.IsNullOrEmpty(json))
            return null;
        return JsonSerializer.Deserialize<LoginModel>(json);
    }
}