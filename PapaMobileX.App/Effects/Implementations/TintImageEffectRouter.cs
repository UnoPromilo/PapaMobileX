using System.ComponentModel;
using Microsoft.Maui.Controls.Platform;

namespace PapaMobileX.App.Effects.Implementations;

public partial class TintImageEffectRouter : PlatformEffect
{
    protected override void OnAttached()
    {
        OnAttachedInternal();
    }

    protected override void OnDetached()
    {
        OnDetachedInternal();
    }

    protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
    {
        OnElementPropertyChangedInternal(args);
    }

    private partial void OnAttachedInternal();

    private partial void OnDetachedInternal();

    private partial void OnElementPropertyChangedInternal(PropertyChangedEventArgs args);
}