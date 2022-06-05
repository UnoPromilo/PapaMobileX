using PapaMobileX.App.Shared.Errors;
using PapaMobileX.Shared.Results;

namespace PapaMobileX.App.BusinessLogic.Services.Interfaces;

public interface ISignalRSender
{
    public Task<Result<HubError>> SendMessage(object message);
}