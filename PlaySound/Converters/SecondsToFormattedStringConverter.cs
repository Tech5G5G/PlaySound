using Microsoft.UI.Xaml.Data;

namespace PlaySound.Converters;

public sealed partial class SecondsToFormattedStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is double seconds)
        {
            return TimeSpan.FromSeconds(seconds).ToString("g");
        }

        return value.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return value.ToString();
    }
}
