using System.Globalization;
using CommunityToolkit.Maui.Converters;

namespace PapaMobileX.App.Converters;

public class InvertedBoolConverter : BaseConverter<bool, bool>
{
    public override bool ConvertFrom(bool value, CultureInfo? culture)
    {
        return !value;
    }

    public override bool ConvertBackTo(bool value, CultureInfo? culture)
    {
        return ConvertFrom(value, culture);
    }
}