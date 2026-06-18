using Microsoft.Diagnostics.Tracing;
using System.ComponentModel.DataAnnotations.Schema;
using UI.Enums;

namespace UI.Events;

[Serializable]
public sealed class FileRawEventArgs : EventArgs
{
    public FileRawEventArgs(string process, EventType action, DateTime time, string value, string details, TraceEvent? innerTrace = null)
    {
        EventType = action;
        Time = time;
        FileName = value;
        Detail = details;
        Process = process;
        Trace = innerTrace;
    }
    public FileRawEventArgs()
    {

    }
    public EventType EventType { get; set; }
    public DateTime Time { get; set; }
    public string FileName { get; set; }
    public string Detail { get; set; }
    public string Process { get; set; }

    [NotMapped]
    public TraceEvent? Trace { get; }
}
public sealed class EventCountChangedEventArgs : EventArgs
{
    public EventCountChangedEventArgs(EventType type)
    {
        Type = type;
    }
    public EventType Type { get; set; }
}