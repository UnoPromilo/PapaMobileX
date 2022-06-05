using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using PapaMobileX.App.BusinessLogic.Services.Interfaces;
using PapaMobileX.App.Shared.Errors;
using PapaMobileX.Shared.Results;

namespace PapaMobileX.App.BusinessLogic.HubClients.Abstraction;

public abstract class BaseHubClient : IHubClient
{
    private const int StartRetryCount = 3;
    private const int DelayBetweenRetrySeconds = 3;
    protected HubConnection? InternalHubConnection { get; private set; }

    private readonly ITokenService _tokenService;
    private readonly ILogger<BaseHubClient> _logger;

    protected BaseHubClient(ITokenService tokenService, ILogger<BaseHubClient> logger)
    {
        _tokenService = tokenService;
        _logger = logger;
    }
    protected abstract string Path { get; }
    public abstract bool CanSupportMessage(object message);

    public virtual Task<Result<HubError>> SendMessage(object dto)
    {
        if (CanSupportMessage(dto) == false)
            return Task.FromResult(Result<HubError>.Failed(HubError.UnsupportedMessageType()));


        if (InternalHubConnection is null)
            return Task.FromResult(Result<HubError>.Failed(HubError.HubInactive()));

        return Task.FromResult(Result<HubError>.Ok());
    }

    [MemberNotNull(nameof(InternalHubConnection))]
    protected virtual void Build(Uri serverAddress)
    {
        InternalHubConnection = new HubConnectionBuilder()
                                .WithUrl($"{serverAddress}{Path}",
                                         options =>
                                         {
                                             options.AccessTokenProvider = () => Task.FromResult(_tokenService.Token);
                                         })
                                .WithAutomaticReconnect(new SignalrRetryPolicy())
                                .Build();
    }

    public async Task<Result<HubError>> StartConnectionAsync(Uri serverAddress)
    {
        if (InternalHubConnection is null)
            Build(serverAddress);
        
        if (InternalHubConnection.State != HubConnectionState.Disconnected)
            return HubError.HubAlreadyStarted();

        for (var i = 0; i < StartRetryCount; i++)
        {
            try
            {
                await InternalHubConnection.StartAsync();
                if (InternalHubConnection.State == HubConnectionState.Connected)
                    return Result<HubError>.Ok();

                await Task.Delay(DelayBetweenRetrySeconds);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
        return HubError.ProblemDuringStartupAttempt();
    }

    public async Task<Result<HubError>> StopConnectionAsync()
    {
        if (InternalHubConnection is null || InternalHubConnection.State == HubConnectionState.Disconnected)
            return HubError.ProblemDuringStopAttempt();

        await InternalHubConnection.StopAsync();
        await InternalHubConnection.DisposeAsync();
        InternalHubConnection = null;
        return Result<HubError>.Ok();
    }

    public bool IsRunning => InternalHubConnection?.State is not (HubConnectionState.Disconnected or null);

    private class SignalrRetryPolicy : IRetryPolicy
    {
        public TimeSpan? NextRetryDelay(RetryContext retryContext)
        {

            if (retryContext.PreviousRetryCount > 3)
                return TimeSpan.FromSeconds(10);

            return TimeSpan.FromSeconds(DelayBetweenRetrySeconds * retryContext.PreviousRetryCount);
        }
    }
}