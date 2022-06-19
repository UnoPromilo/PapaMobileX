using System.ComponentModel;
using PapaMobileX.App.BusinessLogic.Services.Interfaces;
using PapaMobileX.App.BusinessLogic.ViewModels.Abstractions;

namespace PapaMobileX.App.BusinessLogic.ViewModels;

public class SteeringViewModel : BaseViewModel, IDisposable
{
    private readonly IVideoService _videoService;
    private byte[]? _videoFrame = { 0 };
    private const int FpsLimit = 10;
    private const int MaxFpsValue = 30;
    private int _frameCounter;
    
    
    public Stream VideoFrame
    {
        get
        {
            if (_videoFrame is null)
                return null;
            return new MemoryStream(_videoFrame);
        }
    }

    public SteeringViewModel(IVideoService videoService)
    {
        _videoService = videoService;
        _videoService.PropertyChanged += VideoServiceOnPropertyChanged;
    }

    private void VideoServiceOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        //BLEEEE, but performance
        if (_frameCounter > MaxFpsValue / FpsLimit)
        {
            _videoFrame = _videoService.LastFrame;
            OnPropertyChanged(nameof(VideoFrame));
            _frameCounter = 0;
        }
        _frameCounter++;
    }


    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _videoService.PropertyChanged -= VideoServiceOnPropertyChanged;
    }
}