using System.ComponentModel;
using Microsoft.Extensions.Hosting;
using PapaMobileX.Server.Camera.Services.Interfaces;

namespace PapaMobileX.Server.Camera.Services.Concrete;

public class CameraBackgroundService : BackgroundService
{
    private readonly IVideoCameraService _cameraService;
    private Task? _workerTask;

    public CameraBackgroundService(IVideoCameraService cameraService)
    {
        _cameraService = cameraService;
    }
    
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _workerTask = _cameraService.Initialize();
        return base.StartAsync(cancellationToken);
    }

    protected override Task? ExecuteAsync(CancellationToken stoppingToken)
    {
        _cameraService.AddCancellationToken(stoppingToken);
        return _workerTask;
    }

    public override void Dispose()
    {
        _cameraService.Dispose();
        base.Dispose();
    }
}