using OpenCvSharp;

namespace PapaMobileX.Server.Camera.Services.Interfaces;

public interface IVideoCameraService : IDisposable
{
    Task? Initialize();

    void GetFrame(Mat outputMat);

    void AddCancellationToken(CancellationToken cancellationToken);

    event EventHandler? OnFrameReceived;
}