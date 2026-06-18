using System.Windows.Media;
using UI.Enums;

namespace UI.Models;

public sealed class FilterItem
{
    public FilterItem()
    {
    }

    public FilterItem(string text,FilterMonitorType filter,Brush background, Brush foreground, Brush primaryBrush, ImageSource icon)
    {
        Text = text;
        Filter = filter;
        Background = background;
        Foreground = foreground;
        PrimaryBrush = primaryBrush;
        Icon = icon;
    }
    public string Text { get; set; }
    public bool IsChecked { get; set; }
    public FilterMonitorType Filter { get; set; }
    public Brush Background { get; set; }
    public Brush Foreground { get; set; }
    public Brush PrimaryBrush { get; set; }
    public ImageSource Icon { get; set; }
}