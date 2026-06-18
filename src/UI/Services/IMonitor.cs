using UI.Enums;
using UI.Events;
using UI.Models;

namespace UI.Services;

public interface IMonitor  : IDisposable
{
    void Start(int processId, List<FilterMonitorType> filters);
    void Stop();
    event EventHandler<FileRawEventArgs>? Notify;
    event EventHandler<MonitorStatus>? MonitorStatusChanged;
    event EventHandler<EventCountChangedEventArgs>? EventCountChanged;

}
