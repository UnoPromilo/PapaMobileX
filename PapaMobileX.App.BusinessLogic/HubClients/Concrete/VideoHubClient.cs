using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using PapaMobileX.App.BusinessLogic.HubClients.Abstraction;
using PapaMobileX.App.BusinessLogic.Services.Interfaces;
using PapaMobileX.App.Shared.Errors;
using PapaMobileX.DTOs.SignalR;
using PapaMobileX.Shared.HubDefinitions;
using PapaMobileX.Shared.Models;
using PapaMobileX.Shared.Results;

namespace PapaMobileX.App.BusinessLogic.HubClients.Concrete;

public class VideoHubClient : BaseHubClient, IVideoHubDefinition
{
    private readonly IVideoService _videoService;

    public VideoHubClient(ITokenService tokenService, ILogger<VideoHubClient> logger, IVideoService videoService) : base(tokenService, logger)
    {
        _videoService = videoService;
    }

    protected override string Path => "hubs/video";

    protected override void Build(Uri serverAddress)
    {
        base.Build(serverAddress);
        InternalHubConnection.On<VideoData>(nameof(Frame), Frame);
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

    public Task Frame(VideoData data)
    {

        return Task.CompletedTask;
    }
}