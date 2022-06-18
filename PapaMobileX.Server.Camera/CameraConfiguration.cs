using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PapaMobileX.Server.Camera.Services.Concrete;
using PapaMobileX.Server.Camera.Services.Interfaces;

namespace PapaMobileX.Server.Camera;

public static class CameraConfiguration
{
    public static IServiceCollection ConfigureCamera(this IServiceCollection services)
    {
        services.AddTransient<IVideoCaptureService, VideoCaptureService>();
        services.AddSingleton<IVideoCameraService, VideoCameraService>();
        services.AddSingleton<IHostedService, CameraBackgroundService>();
        services.AddSingleton<IHostedService, StreamBackgroundService>();
        return services;
    }
}