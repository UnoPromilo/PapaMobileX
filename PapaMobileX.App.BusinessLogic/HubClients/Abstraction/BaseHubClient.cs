using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using PapaMobileX.App.BusinessLogic.Services.Interfaces;
using PapaMobileX.App.Shared.Errors;
using PapaMobileX.Shared.Results;

namespace PapaMobileX.App.BusinessLogic.HubClients.Abstraction;

public abstract class BaseHubClient<T> : BaseHubClient
{
    private readonly ReadOnlyDictionary<Type, string> _messageTypeToHubMethodName;

    protected BaseHubClient(ITokenService tokenService, ILogger<BaseHubClient<T>> logger) : base(tokenService, logger)
    {
        Dictionary<Type, string> messageTypeToHubMethodName = new();

        foreach (MethodInfo method in typeof(T).GetMethods())
        {
            ParameterInfo[] parameters = method.GetParameters();
            if (parameters.Length != 1)
                throw new Exception("Unsupported count of parameters");

            if (messageTypeToHubMethodName.ContainsKey(parameters[0].ParameterType))
                throw new Exception("Parameter type have to be unique");

            messageTypeToHubMethodName.Add(parameters[0].ParameterType, method.Name);
        }

        _messageTypeToHubMethodName = new ReadOnlyDictionary<Type, string>(messageTypeToHubMethodName);
    }

    public override async Task<Result<HubError>> SendMessage(object dto)
    {
        if (CanSupportMessage(dto) == false)
            return Result<HubError>.Failed(HubError.UnsupportedMessageType());

        if (InternalHubConnection is null)
            return Result<HubError>.Failed(HubError.HubInactive());

        await InternalHubConnection!.SendAsync(_messageTypeToHubMethodName[dto.GetType()], dto);
        return Result<HubError>.Ok();
    }

    public override bool CanSupportMessage(object message)
    {
        return _messageTypeToHubMethodName.ContainsKey(message.GetType());
    }
}

public abstract class BaseHubClient : IHubClient
{
    private const int StartRetryCount = 3;
    private const int DelayBetweenRetrySeconds = 3;
    private readonly ILogger<BaseHubClient> _logger;

    private readonly ITokenService _tokenService;

    protected BaseHubClient(ITokenService tokenService, ILogger<BaseHubClient> logger)
    {
        _tokenService = tokenService;
        _logger = logger;
    }

    protected HubConnection? InternalHubConnection { get; private set; }
    protected abstract string Path { get; }

    public abstract bool CanSupportMessage(object message);

    public abstract Task<Result<HubError>> SendMessage(object dto);

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
                _logger.LogError(ex, "{ErrorMessage}", ex.Message);
            }
        }

        return HubError.ProblemDuringStartupAttempt();
    }

    public async Task<Result<HubError>> StopConnectionAsync()
    {
        if (InternalHubConnection is null || (InternalHubConnection.State == HubConnectionState.Disconnected))
            return HubError.ProblemDuringStopAttempt();

        await InternalHubConnection.StopAsync();
        await InternalHubConnection.DisposeAsync();
        InternalHubConnection = null;
        return Result<HubError>.Ok();
    }

    public bool IsRunning => InternalHubConnection?.State is not (HubConnectionState.Disconnected or null);

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