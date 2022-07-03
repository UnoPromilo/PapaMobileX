using Microsoft.Maui.Handlers;
using PapaMobileX.App.Controls;

namespace PapaMobileX.App;

public class VideoImageHandler : ViewHandler<IVideoImage, VideoImageView>
{
    public VideoImageHandler() : base(CustomEntryMapper) { }
    
    public static PropertyMapper<IVideoImage, VideoImageHandler> CustomEntryMapper = new(ViewMapper)
    {
        [nameof(IVideoImage.VideoFrame)] = MapVideoFrame
    };
    
    protected override VideoImageView CreatePlatformView()
    {
        return new VideoImageView(Context);
    }

    private static void MapVideoFrame(VideoImageHandler handler, IVideoImage entry)
    {
        handler.PlatformView?.SetFrame(entry.VideoFrame);
    }
}