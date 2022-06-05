using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using PapaMobileX.App.BusinessLogic.HubClients.Abstraction;
using PapaMobileX.App.BusinessLogic.Services.Interfaces;
using PapaMobileX.App.Shared.Errors;
using PapaMobileX.DTOs.SignalR;
using PapaMobileX.Shared.Results;

namespace PapaMobileX.App.BusinessLogic.HubClients.Concrete;

public class TestHubClient : BaseHubClient
{
    public TestHubClient(ITokenService tokenService, ILogger<TestHubClient> logger) : base(tokenService, logger) { }

    protected override string Path => "hubs/test";

    protected override void Build(Uri serverAddress)
    {
        base.Build(serverAddress);
        InternalHubConnection.On<TestDTO>(nameof(ReceiveMessage), ReceiveMessage);
    }

    public override bool CanSupportMessage(object message)
    {
        return message is TestDTO;
    }

    public override async Task<Result<HubError>> SendMessage(object dto)
    {
        Result<HubError> baseResult = await base.SendMessage(dto);
        if (baseResult.IsFailed)
            return baseResult;

        await InternalHubConnection!.SendAsync("SendMessage", dto);
        return Result<HubError>.Ok();
    }

    private Task ReceiveMessage(TestDTO testDTO)
    {
        //TODO
        return Task.CompletedTask;
    }
}