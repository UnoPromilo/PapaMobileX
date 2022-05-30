using PapaMobileX.App.Effects.Implementations;

namespace PapaMobileX.App.Effects;

public class TintImageEffect : RoutingEffect
{
    public static readonly BindableProperty TintColorProperty
        = BindableProperty.CreateAttached("TintColor",
                                          typeof(Color),
                                          typeof(TintImageEffect),
                                          default(Color),
                                          propertyChanged: OnTintColorChanged);

    public static Color? GetTintColor(BindableObject view)
    {
        return (Color)view.GetValue(TintColorProperty);
    }

    public static void SetTintColor(BindableObject view, Color value)
    {
        view.SetValue(TintColorProperty, value);
    }

    private static void OnTintColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (!(bindable is View view))
            return;

        Effect? oldEffect = view.Effects.FirstOrDefault(e => e is TintImageEffectRouter);

        if (oldEffect != null)
            view.Effects.Remove(oldEffect);

        if (newValue == TintColorProperty.DefaultValue)
            return;

        view.Effects.Add(new TintImageEffectRouter());
    }
}