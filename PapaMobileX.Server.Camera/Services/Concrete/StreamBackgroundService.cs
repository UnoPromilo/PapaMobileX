using System.Diagnostics;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenCvSharp;
using PapaMobileX.Server.Camera.Services.Interfaces;
using PapaMobileX.Server.SignalR.Hubs;
using PapaMobileX.Shared.HubDefinitions;
using PapaMobileX.Shared.Models;

namespace PapaMobileX.Server.Camera.Services.Concrete;

public class StreamBackgroundService : BackgroundService
{
    private readonly IVideoCameraService _videoCameraService;
    private readonly ILogger<StreamBackgroundService> _logger;
    private readonly IHubContext<VideoHub, IVideoHubDefinition> _hubContext;
    private readonly ManualResetEvent _frameReceivedEvent;
    private readonly ImageEncodingParam[] _imageEncodingParams = { new(ImwriteFlags.JpegQuality, 50) };


    public StreamBackgroundService(IVideoCameraService videoCameraService, ILogger<StreamBackgroundService> logger, IHubContext<VideoHub, IVideoHubDefinition> hubContext)
    {
        _frameReceivedEvent = new ManualResetEvent(false);
        _videoCameraService = videoCameraService;
        _logger = logger;
        _hubContext = hubContext;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _videoCameraService.OnFrameReceived += VideoCameraServiceOnOnFrameReceived;
        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _videoCameraService.OnFrameReceived -= VideoCameraServiceOnOnFrameReceived;
        return base.StopAsync(cancellationToken);
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return StreamVideo(stoppingToken);
    }

    private void VideoCameraServiceOnOnFrameReceived(object? sender, EventArgs e)
    {
        _frameReceivedEvent.Set();
    }
    
    private Task StreamVideo(CancellationToken cancellationToken)
    {
        var task = new Task(() =>
        {
            while (cancellationToken.IsCancellationRequested == false)
            {
                try
                {
                    _frameReceivedEvent.WaitOne();
                    using Mat mat = new();
                    _videoCameraService.GetFrame(mat);
                    var ms = mat.ImEncode(".jpg", _imageEncodingParams);
                    var data = new VideoData
                    {
                        Data = Convert.ToBase64String(ms)
                    };
                    _hubContext.Clients.All.Frame(data);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error while streaming video");
                }
            }
        }, TaskCreationOptions.LongRunning);
        task.Start();
        return task;
    }
}