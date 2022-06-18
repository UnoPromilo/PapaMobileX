using Microsoft.Extensions.Configuration;
using OpenCvSharp;
using PapaMobileX.Server.Camera.Services.Interfaces;

namespace PapaMobileX.Server.Camera.Services.Concrete;

public class VideoCaptureService : IVideoCaptureService
{
    private readonly IConfiguration _configuration;

    public VideoCaptureService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public VideoCapture BuildVideoCapture()
    {
        if (_configuration.GetValue<bool>(Constants.UseGStreamerKey))
        {
            var connectionString = _configuration.GetValue<string>(Constants.GStreamerStringKey);
            return new VideoCapture(connectionString, VideoCaptureAPIs.GSTREAMER);
        }

        return VideoCapture.FromCamera(0);
    }
}