using PapaMobileX.Shared.Results;
using PapaMobileX.Shared.Results.Errors;

namespace PapaMobileX.App.BusinessLogic.Services.Interfaces;

public interface ISignalRConnectionService
{
    Task<Result<Error>> StartConnectionAsync(Uri baseUrl);

    Task StopConnectionAsync();
}