using PapaMobileX.App.BusinessLogic.HubClients.Abstraction;
using PapaMobileX.App.BusinessLogic.Services.Interfaces;
using PapaMobileX.App.Shared.Errors;
using PapaMobileX.Shared.Results;

namespace PapaMobileX.App.BusinessLogic.Services.Concrete;

public class SignalRConnectionService : ISignalRConnectionService
{
    private readonly IEnumerable<IHubClient> _hubClients;

    public SignalRConnectionService(IEnumerable<IHubClient> hubClients)
    {
        _hubClients = hubClients;
    }

    public async Task<Result<HubError>> StartConnectionAsync(Uri baseUrl)
    {
        if (_hubClients.Any(c => c.IsRunning))
            await StopConnectionAsync();

        Result<HubError>[] results =
            await Task.WhenAll(_hubClients.Select(client => client.StartConnectionAsync(baseUrl)));

        if (results.All(result => result.IsSuccess))
            return Result<HubError>.Ok();

        await StopConnectionAsync();
        return results.First(r => r.IsFailed);
    }

    public async Task StopConnectionAsync()
    {
        await Task.WhenAll(_hubClients
                           .Where(c => c.IsRunning == false)
                           .Select(c => c.StopConnectionAsync()));
    }
}