using System.Windows;
using System.Windows.Controls;
using UI.Enums;


namespace UI.Controls
{
    public class MonitorButton : Button
    {
        static MonitorButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MonitorButton), new FrameworkPropertyMetadata(typeof(MonitorButton)));
        }



        public MonitorTemplate MonitorTemplate
        {
            get { return (MonitorTemplate)GetValue(MonitorTemplateProperty); }
            set { SetValue(MonitorTemplateProperty, value); }
        }
        public static readonly DependencyProperty MonitorTemplateProperty =
            DependencyProperty.Register(nameof(MonitorTemplate), typeof(MonitorTemplate), typeof(MonitorButton), new PropertyMetadata(default));


    }
}
