using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Session;
using System.Data.Common;
using System.Threading.Channels;
using UI.Enums;
using UI.Events;
using UI.Services;
namespace UI.Services;

public sealed class Monitor : IMonitor, IDisposable
{
    private TraceEventSession _session;
    private Channel<FileRawEventArgs> _queue;

    private Task? _worker;
    private Task? _etwTask;

    private CancellationTokenSource? _cts;

    private bool _started;

    public event EventHandler<FileRawEventArgs>? Notify;
    public event EventHandler<EventCountChangedEventArgs>? EventCountChanged;

    public event EventHandler<MonitorStatus>? MonitorStatusChanged;
    private HashSet<FilterMonitorType> _enabledFilters;
    public Monitor(TraceEventSession session)
    {
        _session = session;
    }
    public void Dispose()
    {
        try
        {
            _cts?.Cancel();
            _session?.Dispose();
            _queue?.Writer.TryComplete();
        }
        catch (Exception)
        {
        }
    }

    public void Start(int processId, List<FilterMonitorType> filters)
    {
        try
        {
            if (_started)
                return;


            _cts = new CancellationTokenSource();
            _queue = Channel.CreateBounded<FileRawEventArgs>(new BoundedChannelOptions(5000)
            {
                FullMode = BoundedChannelFullMode.Wait,
            });
            //_session = new TraceEventSession(App.SessionName)
            //{
            //    BufferSizeMB = 1024,
            //    StopOnDispose = true
            //};
          //  _session.EnableKernelProvider(
          //KernelTraceEventParser.Keywords.FileIO |
          //KernelTraceEventParser.Keywords.FileIOInit | KernelTraceEventParser.Keywords.NetworkTCPIP
          //              );

            _enabledFilters = filters.ToHashSet();
            EnabelFileKernel(processId);
            EnableNetworkKernel(processId);

            _worker = Task.Run(async () =>
            {
                await foreach (var item in _queue.Reader.ReadAllAsync())
                {
                    try
                    {
                        OnNotify(item);

                    }
                    catch (Exception)
                    {

                    }
                }

            }, _cts.Token);


            _etwTask = Task.Run(() =>
            {
                try
                {
                    _session.Source.Process();
                }
                catch (Exception ex)
                {
                    // log
                }
            });

            _started = true;
            MonitorStatusChanged?.Invoke(this, MonitorStatus.Running);

        }
        catch (Exception)
        {
        }
    }



    public void Stop()
    {
        try
        {
            if (!_started)
                return;

            _cts?.Cancel();
            //_session?.Dispose();
            _queue?.Writer.TryComplete();
        
        }
        catch (Exception)
        {

        }
        finally
        {
            _started = false;
            MonitorStatusChanged?.Invoke(this, MonitorStatus.Stop);
        }
    }
    private bool IsAllowed(EventType type)
    {
        return type switch
        {
            EventType.Readed => _enabledFilters.Contains(FilterMonitorType.Read),
            EventType.Writed => _enabledFilters.Contains(FilterMonitorType.Write),
            EventType.Created => _enabledFilters.Contains(FilterMonitorType.Create),
            EventType.Deleted => _enabledFilters.Contains(FilterMonitorType.Delete),
            EventType.Renamed => _enabledFilters.Contains(FilterMonitorType.Rename),
            EventType.Sent => _enabledFilters.Contains(FilterMonitorType.Send),
            EventType.Recieved => _enabledFilters.Contains(FilterMonitorType.Receive),
            _ => false
        };
    }
    private void PushFileEvent(
        int processId,
        int dataProcessId,
        EventType type,
        DateTime timestamp,
        string fileName,
        string detail,
        TraceEvent data)
    {
        if (processId != -1 && dataProcessId != processId)
            return;

        EventCountChanged?.Invoke(this, new EventCountChangedEventArgs(type));

        if (!IsAllowed(type))
            return;


        _queue.Writer.TryWrite(
            new FileRawEventArgs(
                dataProcessId.ToString(),
                type,
                timestamp,
                fileName,
                detail + ',' + data.EventName,
                data));
    }
    private void EnabelFileKernel(int processId)
    {
        _session.Source.Kernel.FileIOCreate += data =>
        {
            PushFileEvent(
                processId,
                data.ProcessID,
                EventType.Created,
                data.TimeStamp,
                data.FileName,
                data.EventName,
                data);
        };

        _session.Source.Kernel.FileIOWrite += data =>
        {
            PushFileEvent(
                processId,
                data.ProcessID,
                EventType.Writed,
                data.TimeStamp,
                data.FileName,
                data.EventName,
                data);
        };

        _session.Source.Kernel.FileIORename += data =>
        {
            PushFileEvent(
                processId,
                data.ProcessID,
                EventType.Renamed,
                data.TimeStamp,
                data.FileName,
                data.EventName,
                data);
        };


        _session.Source.Kernel.FileIODelete += data =>
        {
            PushFileEvent(
                processId,
                data.ProcessID,
                EventType.Deleted,
                data.TimeStamp,
                data.FileName,
                data.EventName,
                data);
        };


        _session.Source.Kernel.FileIORead += data =>
        {
            PushFileEvent(
                processId,
                data.ProcessID,
                EventType.Readed,
                data.TimeStamp,
                data.FileName,
                data.EventName,
                data);
        };
    }

    public void EnableNetworkKernel(int processId)
    {
        _session.Source.Kernel.TcpIpSend += data =>
        {
            PushFileEvent(
               processId,
               data.ProcessID,
               EventType.Sent,
               data.TimeStamp,
               data.daddr.ToString(),
               $"{data.size}byte:{data.dport}",
               data);

            PushFileEvent(
              processId,
              data.ProcessID,
              EventType.Sent,
              data.TimeStamp,
              data.saddr.ToString(),
              $"{data.size}byte:{data.sport}",
              data);

        };
        _session.Source.Kernel.TcpIpRecv += data =>
        {
            PushFileEvent(
                processId,
                data.ProcessID,
                EventType.Recieved,
                data.TimeStamp,
                data.daddr.ToString(),
                $"{data.size}byte:{data.dport}",
                data);
        };


        _session.Source.Kernel.UdpIpSend += data =>
        {
            PushFileEvent(
               processId,
               data.ProcessID,
               EventType.Sent,
               data.TimeStamp,
               data.daddr.ToString(),
               $"{data.size}byte:{data.dport}",
               data);

            PushFileEvent(
              processId,
              data.ProcessID,
              EventType.Sent,
              data.TimeStamp,
              data.saddr.ToString(),
              $"{data.size}byte:{data.sport}",
              data);
        };

        _session.Source.Kernel.UdpIpRecv += data =>
        {
            PushFileEvent(
                processId,
                data.ProcessID,
                EventType.Recieved,
                data.TimeStamp,
                data.daddr.ToString(),
                $"{data.size}byte:{data.dport}",
                data);
        };


    }
    private void OnNotify(FileRawEventArgs e) { Notify?.Invoke(this, e); }
}