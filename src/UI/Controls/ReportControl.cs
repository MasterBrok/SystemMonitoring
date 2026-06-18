using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace UI.Controls
{
   
    public class ReportControl : Control
    {
        static ReportControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ReportControl), new FrameworkPropertyMetadata(typeof(ReportControl)));
        }

        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(ImageSource), typeof(ReportControl), new PropertyMetadata(default));





        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(ReportControl), new PropertyMetadata(default));



        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(string), typeof(ReportControl), new PropertyMetadata(default));



    }
}
