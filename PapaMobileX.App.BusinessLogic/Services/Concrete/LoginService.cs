using PapaMobileX.App.BusinessLogic.Builders.Interfaces;
using PapaMobileX.App.BusinessLogic.Models;
using PapaMobileX.App.BusinessLogic.Services.Interfaces;
using PapaMobileX.App.Shared.Errors;
using PapaMobileX.Shared.Results;
using PapaMobileX.Shared.Results.Errors;

namespace PapaMobileX.App.BusinessLogic.Services.Concrete;

public class LoginService : ILoginService
{
    private readonly IApiClientService _apiClientService;
    private readonly IHttpClientBuilder _httpClientBuilder;
    private readonly ISignalRConnectionService _signalRConnectionService;
    private readonly IVideoService _videoService;
    private readonly ITokenService _tokenService;

    public LoginService(IApiClientService apiClientService,
                        IHttpClientBuilder httpClientBuilder,
                        ITokenService tokenService,
                        ISignalRConnectionService signalRConnectionService,
                        IVideoService videoService)
    {
        _apiClientService = apiClientService;
        _httpClientBuilder = httpClientBuilder;
        _tokenService = tokenService;
        _signalRConnectionService = signalRConnectionService;
        _videoService = videoService;
    }

    public async Task<Result<LoginError>> Login(LoginModel loginModel)
    {
        Uri? baseAddress = TryBuildBaseAddress(loginModel.Address);
        if (baseAddress is null)
            return LoginError.InvalidUriFormat();

        _httpClientBuilder.MainHttpClientBaseAddress = baseAddress;

        Result<LoginError, LoginResultModel> result = await _apiClientService.LoginAsync(loginModel);
        if (result.IsFailed)
            return result.Error;

        _tokenService.SaveToken(result.Data.Token);
        return await InitializeConnectionAsync();
    }

    public async Task<Result<LoginError>> InitializeConnectionAsync()
    {
        Result<HubError> signalRResult =
            await _signalRConnectionService.StartConnectionAsync(_httpClientBuilder.MainHttpClientBaseAddress!);
        if (signalRResult.IsFailed)
            return LoginError.OtherError();

        Result<Error> videoResult =
            await _videoService.StartConnectionAsync(_httpClientBuilder.MainHttpClientBaseAddress!);
        if (videoResult.IsFailed)
            return LoginError.OtherError();

        return Result<LoginError>.Ok();
    }

    private static Uri? TryBuildBaseAddress(string host)
    {
        try
        {
            string[] hostParts = host.Split(':');
            if (hostParts.Length is > 2 or 0)
                throw new UriFormatException();

            UriBuilder builder = new()
            {
                Host = hostParts[0],
                Scheme = Uri.UriSchemeHttps
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