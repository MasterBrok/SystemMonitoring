using System.Windows;
using System.Windows.Media;
using UI.Controls;
using UI.Enums;
using UI.Models;

namespace UI.Services;

public sealed class FilterInitilizer
{
    private readonly ResourceDictionary _resource;
    const string BackgroundKeyFormat = "Brush.Background.Data.{0}";
    const string ForegroundKeyFormat = "Brush.Foreground.Data.{0}";
    const string IconKeyFormat = "{0}";
    const string PrimaryBrushKeyFormat = "Brush.Data.{0}";

    public FilterInitilizer(ResourceDictionary resource)
    {
        _resource = resource;
    }
    public IEnumerable<FilterItem> Initilize()
    {
        var filters = (FilterMonitorType[])Enum.GetValues(typeof(FilterMonitorType));

        foreach (var filter in filters)
        {
            var bgFind = _resource[string.Format(BackgroundKeyFormat, filter)] as Brush;
            var fgFind = _resource[string.Format(ForegroundKeyFormat, filter)] as Brush;
            var iconFind = _resource[string.Format(IconKeyFormat, filter)] as ImageSource;
            var primaryBrushFind = _resource[string.Format(PrimaryBrushKeyFormat, filter)] as Brush;
            yield return new FilterItem(filter.ToString(), filter, bgFind, fgFind, primaryBrushFind, iconFind);
        }

    }

}
