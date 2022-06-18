using OpenCvSharp;

namespace PapaMobileX.Server.Camera.Services.Interfaces;

public interface IVideoCaptureService
{
    VideoCapture BuildVideoCapture();
}