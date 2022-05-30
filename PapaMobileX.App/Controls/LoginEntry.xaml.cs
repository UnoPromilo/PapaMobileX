using PapaMobileX.App.Effects;

namespace PapaMobileX.App.Controls;

public partial class LoginEntry : Border
{
    public static readonly BindableProperty TintColorProperty
        = BindableProperty.CreateAttached(nameof(TintColor), typeof(Color), typeof(LoginEntry), default(Color),
            propertyChanged: OnColorChanged);
    
    public static readonly BindableProperty IconSourceProperty
        = BindableProperty.CreateAttached(nameof(IconSource), typeof(ImageSource), typeof(LoginEntry), default(ImageSource),
            propertyChanged: OnIconChanged);
    
    public static readonly BindableProperty PlaceholderProperty
        = BindableProperty.CreateAttached(nameof(Placeholder), typeof(string), typeof(LoginEntry), default(string),
            propertyChanged: OnPlaceholderChanged);
    
    public static readonly BindableProperty IsPasswordProperty
        = BindableProperty.CreateAttached(nameof(IsPassword), typeof(bool), typeof(LoginEntry), default(bool),
            propertyChanged: OnIsPasswordChanged);

    public static readonly BindableProperty TextProperty
        = BindableProperty.CreateAttached(nameof(Text), typeof(string), typeof(LoginEntry), default(string), BindingMode.TwoWay, 
            propertyChanged: OnTextChanged);
    
    public static readonly BindableProperty KeyboardProperty
        = BindableProperty.CreateAttached(nameof(Keyboard), typeof(Keyboard), typeof(LoginEntry), Keyboard.Default, 
            propertyChanged: OnKeyboardChanged);
    
    public Color TintColor
    {
        get => (Color) GetValue(TintColorProperty);
        set => SetValue(TintColorProperty, value);
    }
    
    public ImageSource IconSource
    {
        get => (ImageSource) GetValue(IconSourceProperty);
        set => SetValue(IconSourceProperty, value);
    }
    
    public string Placeholder
    {
        get => (string) GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }
    
    public bool IsPassword
    {
        get => (bool) GetValue(IsPasswordProperty);
        set => SetValue(IsPasswordProperty, value);
    }
    
    public string Text
    {
        get => (string) GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
    
    public Keyboard Keyboard
    {
        get => (Keyboard) GetValue(KeyboardProperty);
        set => SetValue(KeyboardProperty, value);
    }

    public LoginEntry()
    {
        InitializeComponent();
    }

    private static void OnColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (!(bindable is LoginEntry loginEntry))
            return;

        var color = newValue as Color;
        loginEntry.SetColor(color);
    }

    private static void OnIconChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (!(bindable is LoginEntry loginEntry))
            return;

        loginEntry.SetIcon((ImageSource)newValue);
    } 
    
    private static void OnPlaceholderChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (!(bindable is LoginEntry loginEntry))
            return;

        loginEntry.SetPlaceholder((string)newValue);
    } 
    
    private static void OnIsPasswordChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (!(bindable is LoginEntry loginEntry))
            return;

        loginEntry.SetIsPassword((bool)newValue);
    } 
    
    private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (!(bindable is LoginEntry loginEntry))
            return;

        loginEntry.SetText((string)newValue);
    } 
    
    private static void OnKeyboardChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (!(bindable is LoginEntry loginEntry))
            return;

        loginEntry.SetKeyboard((Keyboard)newValue);
    } 
    
    private void SetColor(Color color)
    {
        var brush = new SolidColorBrush(color);
        Shadow.Brush = brush;
        Stroke = brush;
        TintImageEffect.SetTintColor(Icon, color);
    }
    
    private void SetIcon(ImageSource image)
    {
        Icon.Source = image;
    }
    
    private void SetPlaceholder(string placeholder)
    {
        InternalEntry.Placeholder = placeholder;
    }
    
    private void SetIsPassword(bool isPassword)
    {
        InternalEntry.IsPassword = isPassword;
    }
    
    private void SetText(string text)
    {
        InternalEntry.Text = text;
    }
    
    private void SetKeyboard(Keyboard keyboard)
    {
        InternalEntry.Keyboard = keyboard;
    }
}