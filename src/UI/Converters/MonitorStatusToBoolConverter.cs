using System.Globalization;
using System.Windows.Data;
using UI.Enums;

namespace UI.Converters;

public class MonitorStatusToBoolConverter : IValueConverter
{
    public object Convert(
        object value,
        Type targetType,
        object parameter,
        CultureInfo culture)
    {
        if (value is MonitorStatus status && parameter is MonitorTemplate template)
        {
            if (template == MonitorTemplate.Stop)
            {
                if (status == MonitorStatus.Stop || status == MonitorStatus.NoStart)
                {
                    return false;
                }
                return true;
            }
            
        }
        return true;
    }

    public object ConvertBack(
        object value,
        Type targetType,
        object parameter,
        CultureInfo culture)
    {
        return value;
    }
}