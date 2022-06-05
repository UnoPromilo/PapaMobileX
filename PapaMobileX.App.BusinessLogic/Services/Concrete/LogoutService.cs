using PapaMobileX.App.BusinessLogic.Builders.Interfaces;
using PapaMobileX.App.BusinessLogic.Services.Interfaces;

namespace PapaMobileX.App.BusinessLogic.Services.Concrete;

public class LogoutService : ILogoutService
{
    private readonly IHttpClientBuilder _httpClientBuilder;
    private readonly ITokenService _tokenService;

    public LogoutService(ITokenService tokenService, IHttpClientBuilder httpClientBuilder)
    {
        _tokenService = tokenService;
        _httpClientBuilder = httpClientBuilder;
    }

    public void Logout()
    {
        _tokenService.ClearToken();
        _httpClientBuilder.MainHttpClientBaseAddress = null;
    }
}