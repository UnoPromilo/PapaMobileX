using System.Globalization;
using CommunityToolkit.Maui.Converters;

namespace PapaMobileX.App.Converters;

public class StreamToImageSourceConverter : BaseConverter<Stream?, ImageSource?>
{
    public override ImageSource? ConvertFrom(Stream? value, CultureInfo? culture)
    {
        if (value is null)
            return null;
        
        return ImageSource.FromStream(() => value);
    }

    public override Stream? ConvertBackTo(ImageSource? value, CultureInfo? culture)
    {
        throw new NotImplementedException();
    }
}