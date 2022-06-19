using System.ComponentModel;
using System.Net.WebSockets;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using PapaMobileX.App.BusinessLogic.Services.Interfaces;
using PapaMobileX.Shared.Results;
using PapaMobileX.Shared.Results.Errors;
using Websocket.Client;

namespace PapaMobileX.App.BusinessLogic.Services.Concrete;

public class VideoService : IVideoService
{
    private readonly ILogger<VideoService> _logger;
    private byte[]? _lastFrame;
    private WebsocketClient? _clientWebSocket;
    private const string VideoEndpointPattern = "wss://{0}/video";
    private readonly IList<IDisposable> _subscriptions;

    public VideoService(ILogger<VideoService> logger)
    {
        _logger = logger;
        _subscriptions = new List<IDisposable>();
    }

    public byte[]? LastFrame
    {
        get => _lastFrame;
        private set => SetField(ref _lastFrame, value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public async Task<Result<Error>> StartConnectionAsync(Uri baseUrl)
    {
        _clientWebSocket = new WebsocketClient(GetConnectionString(baseUrl.Host));
        ConfigureWebsocketClient(_clientWebSocket);
        try
        {
            await _clientWebSocket.StartOrFail();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed during starting video connection");
            Result<Error>.Failed(new Error("Failed during starting video connection"));
        }

        return Result<Error>.Ok();
    }

    public async Task StopConnection()
    {
        await _clientWebSocket!.Stop(WebSocketCloseStatus.NormalClosure, String.Empty);
        _ = _subscriptions.Select(s =>
        {
            s.Dispose();
            return s;
        });
        _subscriptions.Clear();
        _clientWebSocket.Dispose();
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    private Uri GetConnectionString(string host)
    {
        var connectionString =  String.Format(VideoEndpointPattern, host);
        return new Uri(connectionString);
    }

    private void ConfigureWebsocketClient(WebsocketClient client)
    {
        _subscriptions.Add(client
                          .ReconnectionHappened
                          .Subscribe(info =>
                                         _logger.LogInformation("Reconnection happened, type: {Type}", info.Type)));
        
        _subscriptions.Add(client
                          .MessageReceived
                          .ObserveOn(TaskPoolScheduler.Default)
                          .Subscribe(MessageReceived));

    }

    private void MessageReceived(ResponseMessage message)
    {
        LastFrame = message.Binary;
    }
}