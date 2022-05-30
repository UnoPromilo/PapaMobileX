using System.ComponentModel;
using Android.Graphics;
using Android.Widget;
using Microsoft.Maui.Platform;
using PapaMobileX.App.Effects.Implementations;
using Color = Microsoft.Maui.Graphics.Color;

[assembly: ResolutionGroupName("PapaMobileX")]
[assembly: ExportEffect(typeof(TintImageEffectRouter), nameof(TintImageEffectRouter))]

namespace PapaMobileX.App.Effects.Implementations;

public partial class TintImageEffectRouter
{
    private partial void OnAttachedInternal()
    {
        if (Control == null || Element == null)
            return;

        Color? color = TintImageEffect.GetTintColor(Element);

        switch (Control)
        {
            case ImageView image:
                SetImageViewTintColor(image, color);
                break;
        }
    }

    private partial void OnDetachedInternal()
    {
        switch (Control)
        {
            case ImageView image:
                image.ClearColorFilter();
                break;
        }
    }

    private partial void OnElementPropertyChangedInternal(PropertyChangedEventArgs args)
    {
        base.OnElementPropertyChanged(args);

        if (!args.PropertyName!.Equals(TintImageEffect.TintColorProperty.PropertyName))
            return;

        OnAttachedInternal();
    }

    private static void SetImageViewTintColor(ImageView image, Color? color)
    {
        if (color == default(Color))
        {
            image.ClearColorFilter();
        }

        image.SetColorFilter(new PorterDuffColorFilter(color!.ToPlatform(), PorterDuff.Mode.SrcIn!));
    }
}