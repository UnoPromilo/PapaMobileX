namespace PapaMobileX.App.Controls;

public class AccelerationSlider : Slider
{
    public AccelerationSlider()
    {
        ModifyEntry();
    }

    void ModifyEntry()
    {
        Microsoft.Maui.Handlers.SliderHandler.Mapper.AppendToMapping("AccelerationThumb", (handler, view) =>
        {
            #if ANDROID
            handler.PlatformView.SetThumb(AndroidX.Core.Content.ContextCompat.GetDrawable(handler.PlatformView.Context, Resource.Drawable.acceleration_thumb));
            handler.PlatformView.ProgressDrawable = AndroidX.Core.Content.ContextCompat.GetDrawable(handler.PlatformView.Context, Resource.Drawable.acceleration_style);
            handler.PlatformView.SplitTrack = false;
            #elif iOS
            throw new NotImplementedException();
            #endif
        });
    }
}