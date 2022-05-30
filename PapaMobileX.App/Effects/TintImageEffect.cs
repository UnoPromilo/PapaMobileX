using Microsoft.Maui.Controls.Platform;
using PapaMobileX.App.Effects.Implementations;

namespace PapaMobileX.App.Effects;

public class TintImageEffect : RoutingEffect
{
    public static readonly BindableProperty TintColorProperty
        = BindableProperty.CreateAttached("TintColor", typeof(Color), typeof(TintImageEffect), default(Color),
            propertyChanged: OnTintColorChanged);

    public static Color GetTintColor(BindableObject view)
        => (Color)view.GetValue(TintColorProperty);

    public static void SetTintColor(BindableObject view, Color value)
        => view.SetValue(TintColorProperty, value);

    static void OnTintColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (!(bindable is View view))
            return;

        var oldEffect = view.Effects.FirstOrDefault(e => e is TintImageEffectRouter);

        if (oldEffect != null)
            view.Effects.Remove(oldEffect);

        if (newValue == TintColorProperty.DefaultValue)
            return;

        view.Effects.Add(new TintImageEffectRouter());
    }
          
} 