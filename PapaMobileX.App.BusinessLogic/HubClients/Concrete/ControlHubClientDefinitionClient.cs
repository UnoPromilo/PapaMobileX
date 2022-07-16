using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using PapaMobileX.App.BusinessLogic.HubClients.Abstraction;
using PapaMobileX.App.BusinessLogic.Services.Interfaces;
using PapaMobileX.Shared.Enums;
using PapaMobileX.Shared.HubDefinitions;

namespace PapaMobileX.App.BusinessLogic.HubClients.Concrete;

public class ControlHubClientDefinitionClient : BaseHubClient<IControlHubServerDefinition>, IControlHubClientDefinition
{
    public ControlHubClientDefinitionClient(ITokenService tokenService,
                                            ILogger<ControlHubClientDefinitionClient> logger) : base(tokenService,
                                                                                                     logger) { }

    protected override string Path => "hubs/control";

    public Task StopConnection(DisconnectReason reason)
    {
        return StopConnectionAsync();
    }

    protected override void Build(Uri serverAddress)
    {
        base.Build(serverAddress);
        InternalHubConnection.On<DisconnectReason>(nameof(StopConnection), StopConnection);
    }
}