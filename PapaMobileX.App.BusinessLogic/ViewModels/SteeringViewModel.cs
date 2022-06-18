using System.ComponentModel;
using PapaMobileX.App.BusinessLogic.Services.Interfaces;
using PapaMobileX.App.BusinessLogic.ViewModels.Abstractions;

namespace PapaMobileX.App.BusinessLogic.ViewModels;

public class SteeringViewModel : BaseViewModel, IDisposable
{
    private readonly IVideoService _videoService;
    private MemoryStream _videoFrame;

    public Stream? VideoFrame
    {
        get => _videoFrame;
    }

    public SteeringViewModel(IVideoService videoService)
    {
        _videoService = videoService;
        _videoService.PropertyChanged += VideoServiceOnPropertyChanged;
        _videoFrame = new MemoryStream();
    }

    private void VideoServiceOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(_videoService.LastFrame))
        {
            _videoService.LastFrame!.CopyTo(_videoFrame);
            OnPropertyChanged(nameof(VideoFrame));
        }
    }


    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _videoService.PropertyChanged -= VideoServiceOnPropertyChanged;
    }
}