using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenCvSharp;
using PapaMobileX.Server.Camera.Services.Interfaces;

namespace PapaMobileX.Server.Camera.Services.Concrete;

public class StreamBackgroundService : BackgroundService
{
    private readonly IVideoCameraService _videoCameraService;
    private readonly ILogger<StreamBackgroundService> _logger;
    private readonly IVideoWebSocketsService _videoWebSocketsService;
    private readonly ManualResetEvent _frameReceivedEvent;
    private readonly ImageEncodingParam[] _imageEncodingParams = { new(ImwriteFlags.JpegQuality, 50) };


    public StreamBackgroundService(IVideoCameraService videoCameraService,
                                   ILogger<StreamBackgroundService> logger,
                                   IVideoWebSocketsService videoWebSocketsService)
    {
        _frameReceivedEvent = new ManualResetEvent(false);
        _videoCameraService = videoCameraService;
        _logger = logger;
        _videoWebSocketsService = videoWebSocketsService;
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
        Task task = Task.Factory.StartNew(() =>
        {
            while (cancellationToken.IsCancellationRequested == false)
            {
                try
                {
                    _frameReceivedEvent.WaitOne();
                    _frameReceivedEvent.Reset();

                    using Mat mat = new();
                    _videoCameraService.GetFrame(mat);
                    var bytes = mat.ImEncode(".jpg", _imageEncodingParams);
                    _videoWebSocketsService.SendNewFrame(bytes);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error while streaming video");
                }
            }
        }, TaskCreationOptions.LongRunning);
        return task;
    }
}