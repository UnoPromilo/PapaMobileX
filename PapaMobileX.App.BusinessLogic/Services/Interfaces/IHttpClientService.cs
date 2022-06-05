using PapaMobileX.App.Shared.Errors;
using PapaMobileX.Shared.Results;

namespace PapaMobileX.App.BusinessLogic.Services.Interfaces;

public interface IHttpClientService
{
    Task<Result<HttpError, T?>> GetAsync<T>(string resource, CancellationToken cancellationToken = default);

    Task<Result<HttpError, T?>> PostAsync<T>(string resource,
                                             object body,
                                             CancellationToken cancellationToken = default);

    Task<Result<HttpError>> PostAsync(string resource, object body, CancellationToken cancellationToken = default);

    void CancelAllRequests();

    Task<Result<HttpError>> GetAsync(string resource, CancellationToken cancellationToken = default);
}