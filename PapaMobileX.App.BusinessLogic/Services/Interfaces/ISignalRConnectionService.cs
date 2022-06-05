using PapaMobileX.App.Shared.Errors;
using PapaMobileX.Shared.Results;

namespace PapaMobileX.App.BusinessLogic.Services.Interfaces;

public interface ISignalRConnectionService
{
    Task<Result<HubError>> StartConnectionAsync(Uri baseUrl);

    Task StopConnectionAsync();
}