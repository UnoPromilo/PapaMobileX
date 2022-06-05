using PapaMobileX.App.Shared.Errors;
using PapaMobileX.Shared.Results;
using PapaMobileX.Shared.Results.Errors;

namespace PapaMobileX.App.BusinessLogic.Services.Interfaces;

public interface ISignalRConnectionService
{
    Task<Result<HubError>> StartConnectionAsync(Uri baseUrl);

    Task StopConnectionAsync();
}