using Microsoft.UI.Xaml.Data;

namespace PlaySound.Converters;

public sealed partial class TimeSpanToSecondsConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is TimeSpan time)
        {
            return time.TotalSeconds;
        }

        return value.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is double seconds)
        {
            return TimeSpan.FromSeconds(seconds);
        }

        return value.ToString();
    }
}
