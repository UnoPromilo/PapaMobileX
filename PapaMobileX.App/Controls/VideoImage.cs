using System.IO;
using Microsoft.Maui.Controls;

namespace PapaMobileX.App.Controls;

public class VideoImage : View, IVideoImage
{
    public static BindableProperty VideoFrameProperty = BindableProperty.Create(nameof(VideoFrame), typeof(Stream), typeof(VideoImage));
    
    public Stream? VideoFrame
    {
        get => (Stream?)GetValue(VideoFrameProperty);
        set => SetValue(VideoFrameProperty, value);
    }
}