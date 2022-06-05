using Microsoft.AspNetCore.SignalR.Client;
using PapaMobileX.App.Shared.Errors;
using PapaMobileX.Shared.Results;

namespace PapaMobileX.App.BusinessLogic.HubClients.Abstraction;

public interface IHubClient
{
    bool CanSupportMessage(object message);

    public Task<Result<HubError>> SendMessage(object dto);

    Task<Result<HubError>> StartConnectionAsync(Uri serverAddress);

    Task<Result<HubError>> StopConnectionAsync();
    
    bool IsRunning { get; }
}