using System.Net;
using PapaMobileX.App.BusinessLogic.Mappers.Abstraction;
using PapaMobileX.App.BusinessLogic.Models;
using PapaMobileX.App.BusinessLogic.Services.Interfaces;
using PapaMobileX.App.Shared.Errors;
using PapaMobileX.DTOs.Requests;
using PapaMobileX.DTOs.Responses;
using PapaMobileX.Shared.Results;

namespace PapaMobileX.App.BusinessLogic.Services.Concrete;

public class ApiClientService : IApiClientService
{
    private readonly IHttpClientService _httpClientService;
    private readonly IMapper<LoginModel, LoginDTO> _loginDTOMapper;
    private readonly IMapper<LoginResultDTO, LoginResultModel> _loginResultModelMapper;

    public ApiClientService(IHttpClientService httpClientService,
                            IMapper<LoginModel, LoginDTO> loginDTOMapper,
                            IMapper<LoginResultDTO, LoginResultModel> loginResultModelMapper)
    {
        _httpClientService = httpClientService;
        _loginDTOMapper = loginDTOMapper;
        _loginResultModelMapper = loginResultModelMapper;
    }

    public async Task<Result<LoginError, LoginResultModel>> LoginAsync(LoginModel loginModel,
                                                                       CancellationToken cancellationToken = default)
    {
        LoginDTO dto = _loginDTOMapper.Map(loginModel);
        Result<HttpError, LoginResultDTO?> result =
            await _httpClientService.PostAsync<LoginResultDTO>(Constants.ApiEndpoints.Login, dto, cancellationToken);
        if (result.IsFailed)
        {
            if (result.Error.WasCanceled)
                return LoginError.Timeout();
            if (result.Error.HasStatusCode == false)
                return LoginError.ServerNotFound();
            if (result.Error.StatusCode == HttpStatusCode.NotFound)
                return LoginError.WrongCredentials();
            if (result.Error.StatusCode == HttpStatusCode.BadRequest)
                return LoginError.WrongCredentials();

            return LoginError.OtherError();
        }

        LoginResultModel resultModel = _loginResultModelMapper.Map(result.Data!);
        return resultModel;
    }

    public void CancelAllRequests()
    {
        _httpClientService.CancelAllRequests();
    }
}