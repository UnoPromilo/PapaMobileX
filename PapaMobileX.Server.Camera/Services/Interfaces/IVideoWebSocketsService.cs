using System.Net.WebSockets;

namespace PapaMobileX.Server.Camera.Services.Interfaces;

public interface IVideoWebSocketsService
{
    Task AddConnection(WebSocket webSocket);

    Task SendNewFrame(byte[] frame);
}