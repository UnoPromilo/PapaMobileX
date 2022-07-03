using System.ComponentModel;
using PapaMobileX.App.BusinessLogic.Services.Interfaces;
using PapaMobileX.App.BusinessLogic.ViewModels.Abstractions;

namespace PapaMobileX.App.BusinessLogic.ViewModels;

public class SteeringViewModel : BaseViewModel, IDisposable
{
    private readonly IVideoService _videoService;
    private readonly ISteeringService _steeringService;
    private byte[]? _videoFrame = { 0 };
    
    
    public Stream? VideoFrame
    {
        get
        {
            if (_videoFrame is null)
                return null;
            return new MemoryStream(_videoFrame);
        }
    }

    public SteeringViewModel(IVideoService videoService, ISteeringService steeringService)
    {
        _videoService = videoService;
        _steeringService = steeringService;
        _videoService.PropertyChanged += VideoServiceOnPropertyChanged;
        
        _steeringService.StartMonitoring();
    }

    private void VideoServiceOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        _videoFrame = _videoService.LastFrame;
        OnPropertyChanged(nameof(VideoFrame));
    }


    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _videoService.PropertyChanged -= VideoServiceOnPropertyChanged;
        _steeringService.StopMonitoring();
    }
}