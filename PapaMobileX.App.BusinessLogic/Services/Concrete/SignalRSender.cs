using PapaMobileX.App.BusinessLogic.HubClients.Abstraction;
using PapaMobileX.App.BusinessLogic.Services.Interfaces;
using PapaMobileX.App.Shared.Errors;
using PapaMobileX.Shared.Results;

namespace PapaMobileX.App.BusinessLogic.Services.Concrete;

public class SignalRSender : ISignalRSender
{
    private readonly IEnumerable<IHubClient> _hubClients;

    public SignalRSender(IEnumerable<IHubClient> hubClients)
    {
        _hubClients = hubClients;
    }

    public async Task<Result<HubError>> SendMessage(object message)
    {
        Result<HubError>[] results = await Task.WhenAll(_hubClients
                                                        .Where(c => c.CanSupportMessage(message))
                                                        .Select(c => c.SendMessage(message)));
        if (results.Any() == false)
            return HubError.UnsupportedMessageType();

        if (results.Any(r => r.IsFailed))
            return results.First(r => r.IsFailed);

        return Result<HubError>.Ok();
    }
}