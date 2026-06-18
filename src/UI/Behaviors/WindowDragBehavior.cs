
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace UI.Behaviors;

public class WindowDragBehavior : Behavior<UIElement>
{
    private Window? _window;


    [DllImport("user32.dll")]
    private static extern bool SetWindowPos(
        IntPtr hWnd,
        IntPtr hWndInsertAfter,
        int X,
        int Y,
        int cx,
        int cy,
        uint flags);


    private const uint SWP_NOZORDER = 0x0004;
    private const uint SWP_SHOWWINDOW = 0x0040;



    protected override void OnAttached()
    {
        base.OnAttached();

        _window = Window.GetWindow(AssociatedObject);

        if (_window == null)
            return;


        AssociatedObject.MouseLeftButtonDown += MouseDown;
    }



    private void MouseDown(
        object sender,
        MouseButtonEventArgs e)
    {
        if (_window == null)
            return;


        if (_window.WindowState == WindowState.Maximized)
        {
            _window.WindowState = WindowState.Normal;
        }


        _window.DragMove();
    }



    public void Maximize()
    {
        if (_window == null)
            return;


        var handle =
            new System.Windows.Interop.WindowInteropHelper(_window)
            .Handle;


        var area =
            SystemParameters.WorkArea;


        SetWindowPos(
            handle,
            IntPtr.Zero,
            (int)area.Left,
            (int)area.Top,
            (int)area.Width,
            (int)area.Height,
            SWP_NOZORDER | SWP_SHOWWINDOW);
    }



    protected override void OnDetaching()
    {
        AssociatedObject.MouseLeftButtonDown -= MouseDown;

        base.OnDetaching();
    }
}