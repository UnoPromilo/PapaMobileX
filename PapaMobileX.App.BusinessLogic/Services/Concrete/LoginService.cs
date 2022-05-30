using PapaMobileX.App.BusinessLogic.Builders.Interfaces;
using PapaMobileX.App.BusinessLogic.Models;
using PapaMobileX.App.BusinessLogic.Services.Interfaces;
using PapaMobileX.App.Shared.Errors;
using PapaMobileX.Shared.Results;

namespace PapaMobileX.App.BusinessLogic.Services.Concrete;

public class LoginService : ILoginService
{
    private readonly IApiClientService _apiClientService;
    private readonly IHttpClientBuilder _httpClientBuilder;

    public LoginService(IApiClientService apiClientService, IHttpClientBuilder httpClientBuilder)
    {
        _apiClientService = apiClientService;
        _httpClientBuilder = httpClientBuilder;
    }
    
    public async Task<Result<LoginError>> Login(LoginModel loginModel)
    {
        var baseAddress = TryBuildBaseAddress(loginModel.Address);
        if (baseAddress is null)
            return LoginError.InvalidUriFormat();

        _httpClientBuilder.MainHttpClientBaseAddress = baseAddress;

        var result = await _apiClientService.LoginAsync(loginModel);
        if (result.IsFailed)
        {
            return result.Error;
        }
        
        return Result<LoginError>.Ok();
    }

    private static Uri? TryBuildBaseAddress(string host)
    {
        try
        {
            var hostParts = host.Split(':');
            if (hostParts.Length is > 2 or 0)
            {
                throw new UriFormatException();
            }
            
            UriBuilder builder = new()
            {
                Host = hostParts[0],
                Scheme = Uri.UriSchemeHttps,
            };
            
            if (hostParts.Length == 2)
            {
                Int32.TryParse(hostParts[1], out int port);
                builder.Port = port;
            }
            
            return builder.Uri;
        }
        catch (UriFormatException)
        {
            return null;
        }
    }
}