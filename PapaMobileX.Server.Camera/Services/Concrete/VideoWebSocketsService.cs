using System.Collections.Concurrent;
using System.Net.WebSockets;
using PapaMobileX.Server.Camera.Services.Interfaces;

namespace PapaMobileX.Server.Camera.Services.Concrete;

public class VideoWebSocketsService : IVideoWebSocketsService
{
    private readonly ManualResetEvent _newFrameEvent = new(false);
    private readonly SemaphoreSlim _frameAccessSemaphore = new(1);
    private byte[]? _lastFrame;

    public async Task AddConnection(WebSocket webSocket)
    {
        while (webSocket.CloseStatus.HasValue == false)
        {
            _newFrameEvent.WaitOne();
            _newFrameEvent.Reset();
            if (webSocket.State == WebSocketState.Open)
            {
                await _frameAccessSemaphore.WaitAsync();
                var buffer = new ArraySegment<byte>(_lastFrame!);
                await webSocket.SendAsync(buffer, WebSocketMessageType.Binary, WebSocketMessageFlags.EndOfMessage, CancellationToken.None);
                _frameAccessSemaphore.Release();
            }
        }

        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, String.Empty, CancellationToken.None);
    }

    public async Task SendNewFrame(byte[] frame)
    {
        await _frameAccessSemaphore.WaitAsync();
        _lastFrame = frame;
        _frameAccessSemaphore.Release();
        _newFrameEvent.Set();
    }
}