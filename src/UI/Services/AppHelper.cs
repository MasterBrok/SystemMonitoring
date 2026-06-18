using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
namespace UI.Services;

public static class AppHelper
{
    const int GWL_STYLE = -16;
    const uint WS_CAPTION = 0x00C00000;
    const uint WS_THICKFRAME = 0x00040000;

    [DllImport("user32.dll")]
    static extern int GetWindowLong(
        IntPtr hWnd,
        int nIndex);


    [DllImport("user32.dll")]
    static extern int SetWindowLong(
        IntPtr hWnd,
        int nIndex,
        int dwNewLong);



    public static void FixMaximize(Window window)
    {
        var handle =
            new WindowInteropHelper(window).Handle;


        int style =
            GetWindowLong(handle, GWL_STYLE);


        SetWindowLong(
            handle,
            GWL_STYLE,
            style |
            (int)WS_THICKFRAME |
            (int)WS_CAPTION);
    }

    public static string FileEventInfo(this TraceEvent trace)
    {
        string text =
            $"System Monitoring{Environment.NewLine}" +
            $"-------------------------{Environment.NewLine}" +
            $"Task ID : {trace.TaskGuid}{Environment.NewLine}" +
            $"Task Name : {trace.TaskName}{Environment.NewLine}" +
            $"Event Name : {trace.EventName}{Environment.NewLine}" +
            $"Event Index : {trace.EventIndex}{Environment.NewLine}" +
            $"-------------------------";

        return text;
    }
}