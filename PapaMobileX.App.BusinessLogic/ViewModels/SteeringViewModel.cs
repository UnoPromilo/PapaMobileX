using System.ComponentModel;
using PapaMobileX.App.BusinessLogic.Services.Interfaces;
using PapaMobileX.App.BusinessLogic.ViewModels.Abstractions;

namespace PapaMobileX.App.BusinessLogic.ViewModels;

public class SteeringViewModel : BaseViewModel, IDisposable
{
    private readonly IAccelerationControlService _accelerationControlService;
    private readonly IVideoService _videoService;
    private readonly IWheelControlService _wheelControlService;
    private byte[]? _videoFrame = { 0 };

    public SteeringViewModel(IVideoService videoService,
                             IWheelControlService wheelControlService,
                             IAccelerationControlService accelerationControlService)
    {
        _videoService = videoService;
        _wheelControlService = wheelControlService;
        _accelerationControlService = accelerationControlService;
        _videoService.PropertyChanged += VideoServiceOnPropertyChanged;

        _wheelControlService.StartMonitoring();
    }


    public Stream? VideoFrame
    {
        get
        {
            if (_videoFrame is null)
                return null;
            return new MemoryStream(_videoFrame);
        }
    }

    public double Acceleration
    {
        get => throw new NotImplementedException();
        set => _accelerationControlService.UpdateAcceleration(value);
    }

    public bool Break
    {
        get => throw new NotImplementedException();
        set => _accelerationControlService.UpdateBreakPosition(value);
    }


    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _videoService.PropertyChanged -= VideoServiceOnPropertyChanged;
        _wheelControlService.StopMonitoring();
    }

    private void VideoServiceOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        _videoFrame = _videoService.LastFrame;
        OnPropertyChanged(nameof(VideoFrame));
    }
}