using PapaMobileX.App.BusinessLogic.Models;
using PapaMobileX.App.Shared.Errors;
using PapaMobileX.Shared.Results;

namespace PapaMobileX.App.BusinessLogic.Services.Interfaces;

public interface IApiClientService
{
    Task<Result<LoginError, LoginResultModel>> LoginAsync(LoginModel loginModel,
                                                          CancellationToken cancellationToken = default);

    Task<Result<HttpError>> TestConnectionAsync(CancellationToken cancellationToken = default);
    
    void CancelAllRequests();
}