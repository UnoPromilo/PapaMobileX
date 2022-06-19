using System.Net.WebSockets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PapaMobileX.Server.Camera.Services.Interfaces;

namespace PapaMobileX.Server.Controllers;

[ApiController]
[Route("video")]
public class VideoController : ControllerBase
{
    private readonly IVideoWebSocketsService _videoWebSocketsService;

    public VideoController(IVideoWebSocketsService videoWebSocketsService)
    {
        _videoWebSocketsService = videoWebSocketsService;
    }
    
    [HttpGet(Name = "GetVideo")]
    public async Task GetAsync()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            await _videoWebSocketsService.AddConnection(webSocket);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}