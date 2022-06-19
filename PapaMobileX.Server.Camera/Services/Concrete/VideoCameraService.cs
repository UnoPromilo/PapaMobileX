using System.Diagnostics;
using Microsoft.Extensions.Logging;
using OpenCvSharp;
using PapaMobileX.Server.Camera.Services.Interfaces;

namespace PapaMobileX.Server.Camera.Services.Concrete;

public class VideoCameraService : IVideoCameraService
{
    private readonly ILogger<VideoCameraService> _logger;
    private readonly IVideoCaptureService _videoCaptureService;
    private VideoCapture _videoCapture = new();
    private readonly Mat _lastFrame = new();
    private readonly Mat _image = new();
    private readonly object _copyLock = new();
    private bool _isInitialized;
    private readonly CancellationTokenSource _cts = new ();
    private CancellationTokenSource _linkedCts;

    private int _framesCounter = 0;
    private Stopwatch _frameStopwatch;

    public event EventHandler? OnFrameReceived;

    public VideoCameraService(ILogger<VideoCameraService> logger, IVideoCaptureService videoCaptureService)
    {
        _logger = logger;
        _videoCaptureService = videoCaptureService;
        _linkedCts = _cts;
        _frameStopwatch = new Stopwatch();
    }
    
    public Task? Initialize()
    {
        if (_isInitialized)
            throw new Exception("Video camera service already initialized");

        Task task = Task.Factory.StartNew(() =>
        {
            _isInitialized = true;
            _frameStopwatch = Stopwatch.StartNew();
            _framesCounter = 0;
            try
            {
                _videoCapture.Dispose();
                _videoCapture = _videoCaptureService.BuildVideoCapture();
                _logger.LogInformation("Video stream started");
                bool isLastFrameReadCorrectly;
                do
                {
                    isLastFrameReadCorrectly = _videoCapture.Read(_lastFrame);
                    if (isLastFrameReadCorrectly == false || _lastFrame.Empty())
                        continue;
                    Copy(_lastFrame, _image);
                    LogFps();
                    OnFrameReceived?.Invoke(this, EventArgs.Empty);
                } while (isLastFrameReadCorrectly && _linkedCts.IsCancellationRequested == false);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error during reading video stream");
                throw;
            }
            finally
            {
                _frameStopwatch.Stop();
                _logger.LogInformation("Video stream ended");
                _isInitialized = false;
            }
        }, TaskCreationOptions.LongRunning);
         return task;
    }

    public void GetFrame(Mat outputMat)
    {
        Copy(_image, outputMat);
    }

    public void AddCancellationToken(CancellationToken cancellationToken)
    { 
        _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, cancellationToken);   
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        _cts.Cancel();
        var internalCts = new CancellationTokenSource();
        internalCts.CancelAfter(1000);
        
        while (_isInitialized && internalCts.IsCancellationRequested == false)
            Thread.Sleep(100);
        
        internalCts.Dispose();
        _cts.Dispose();
        _linkedCts.Dispose();
        _videoCapture.Dispose();
        _lastFrame.Dispose();
        _image.Dispose();
    }

    private void Copy(Mat sourceMat, Mat outputMat)
    {
        lock (_copyLock)
        {
            sourceMat.CopyTo(outputMat);
        }
    }

    private void LogFps()
    {
        _framesCounter++;
        if (_frameStopwatch.Elapsed <= TimeSpan.FromSeconds(10))
            return;
        
        double avgFrameTime = _frameStopwatch.ElapsedMilliseconds * 1d / _framesCounter;
        double avgFps = 1000d / avgFrameTime;
        _logger.LogInformation("FPS: {Fps:F2}", avgFps);
        _frameStopwatch.Restart();
        _framesCounter = 0;
    }
}