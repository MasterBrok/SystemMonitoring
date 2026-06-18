namespace UI.Models;

public class LogEntry
{
    public LogEntry(string processId, DateTime timeStamp, string fileName, string eventName)
    {
        ProcessId = processId;
        TimeStamp = timeStamp;
        FileName = fileName;
        EventName = eventName;
    }

    public string ProcessId { get; set; } = string.Empty;
    public DateTime TimeStamp { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string EventName { get; set; } = string.Empty;
}