using System.Globalization;
using System.Windows.Data;

namespace UI.Converters;

public sealed class WindowPercentConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        double size = System.Convert.ToDouble(value);
        double percent = 0.9d;
        return size * percent;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }
}
