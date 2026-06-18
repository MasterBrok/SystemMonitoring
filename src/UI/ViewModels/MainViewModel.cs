using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Channels;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using UI.Commands;
using UI.Controls;
using UI.Enums;
using UI.Events;
using UI.Models;
using UI.Services;
using UI.ViewModels;

namespace UI.ViewModels;

public sealed class MainViewModel : INotifyPropertyChanged
{
    private readonly IMonitor _monitor;
    private readonly FilterInitilizer _filterInitilizer;
    public MainViewModel(IMonitor monitor, FilterInitilizer filterInitilizer)
    {
        _monitor = monitor;
        _filterInitilizer = filterInitilizer;
        Load();
        _monitor.MonitorStatusChanged += MonitorStatusChanged;
        _monitor.EventCountChanged += EventCountChanged;
        StartCommand = new RelayCommand(StartMonitoring);
        StopCommand = new RelayCommand(StopMonitoring);
        CopyRowCommand = new RelayCommand(CopyRow_Selected);
        UpdateProcessesCommand = new RelayCommand(UpdateProcesses);

        Close = new ApplicationCloseCommand(monitor);
        Minimize = new ApplicationMinimizeCommand();
    }

    private void EventCountChanged(object? sender, EventCountChangedEventArgs e)
    {
        App.Current.Dispatcher.BeginInvoke(() => Increase(e.Type));
    }

    private void Load()
    {
        Filters = _filterInitilizer.Initilize().ToList();
    }

    private void MonitorStatusChanged(object sender, MonitorStatus e)
    {
        App.Current.Dispatcher.BeginInvoke(() =>
        {
            if (e == MonitorStatus.Running || e == MonitorStatus.NoStart)
            {
                CanStart = false;
                CanStop = !CanStart;
            }
            else if (e == MonitorStatus.Stop)
            {
                CanStart = true;
                CanStop = false;
            }
        });
    }

    public ICommand StartCommand { get; set; }
    public ICommand StopCommand { get; set; }
    public ICommand CopyRowCommand { get; set; }
    public ICommand UpdateProcessesCommand { get; set; }
    public ICommand Close { get; set; }
    public ICommand Minimize { get; set; }



    public Dictionary<EventType, Counter> Counters { get; } = new()
        {
            { EventType.Created, new Counter() },
            { EventType.Deleted, new Counter()},
            { EventType.Writed, new Counter()},
            { EventType.Readed, new Counter()},
            { EventType.Renamed, new Counter()},
            { EventType.Sent, new Counter()},
            { EventType.Recieved, new Counter()},
            { EventType.Connected, new Counter()},
            { EventType.Other, new Counter()}
        };


    private List<FilterItem> filters;

    public List<FilterItem> Filters
    {
        get { return filters; }
        set
        {
            filters = value;
            OnPropertyChanged();
        }
    }


    public void Increase(EventType type)
    {
        Counters[type].Increment();
        OnPropertyChanged(nameof(Counters));
    }



    private ObservableCollection<ProcessItem> processes;
    public ObservableCollection<ProcessItem> Processes
    {
        get { return processes; }
        set
        {
            processes = value;
            OnPropertyChanged();
        }
    }


    private ObservableCollection<FileRawEventArgs> items = new()
    {
        //new("10000",EventType.Created,DateTime.Now,"D:\\file.json","FileIO/Create"),
        //new("10000",EventType.Deleted,DateTime.Now,"D:\\file.json","FileIO/Delete"),
        //new("10000",EventType.Renamed,DateTime.Now,"D:\\newFile.json","FileIO/Rename"),
        //new("10000",EventType.Writed,DateTime.Now,"D:\\file.json","FileIO/Write"),
        //new("10000",EventType.Readed,DateTime.Now,"D:\\file.json","FileIO/Read"),
        //new("10000",EventType.Sent,DateTime.Now,"127.0.0.1","TCP IP/Send:6000"),
        //new("10000",EventType.Recieved,DateTime.Now,"127.0.0.1","TCP IP/Receive:3000"),
        //new("10000",EventType.Sent,DateTime.Now,"127.0.0.1","UDP IP/Send,:9000"),
        //new("10000",EventType.Recieved,DateTime.Now,"127.0.0.1","UDP IP/Receive:1000"),
    };
    public ObservableCollection<FileRawEventArgs> Items
    {
        get { return items; }
        set
        {
            items = value;
            OnPropertyChanged();
        }
    }

    private ProcessMonitor _processMonitor;



    private ProcessItem _selectedProcess;

    public ProcessItem SelectedProcess
    {
        get { return _selectedProcess; }
        set
        {
            _selectedProcess = value;
            OnPropertyChanged();
            Task.Run(
                    () =>
                    {
                        if (_selectedProcess is not null)
                        {

                            _processMonitor = new(_selectedProcess.Id);
                            ProcessStats = _processMonitor.Stats;
                            using var _ = _processMonitor.Start();
                        }
                    });

        }
    }


    private FileRawEventArgs selectedEvent;

    public FileRawEventArgs SelectedEvent
    {
        get { return selectedEvent; }
        set
        {
            selectedEvent = value;
            OnPropertyChanged();
        }
    }


    private ProcessStats processStats;

    public ProcessStats ProcessStats
    {
        get { return processStats; }
        set
        {
            processStats = value;
            OnPropertyChanged();
        }
    }


    private bool canStart = true;

    public bool CanStart
    {
        get { return canStart; }
        set
        {
            canStart = value;
            OnPropertyChanged();
        }
    }

    private bool canStop;

    public bool CanStop
    {
        get { return canStop; }
        set
        {
            canStop = value;
            OnPropertyChanged();
        }
    }

    private void StartMonitoring()
    {
        try
        {
            if (SelectedProcess is null || filters.Count == 0)
                return;

            items.Clear();
            _monitor.Notify -= _monitor_Notify;
            _monitor.Notify += _monitor_Notify;
            _monitor.Start(SelectedProcess.Id, filters.Where(d => d.IsChecked).Select(d => d.Filter).ToList());
        }
        catch (Exception)
        {

        }
    }
    private void StopMonitoring()
    {
        try
        {
            _monitor.Notify -= _monitor_Notify;
            _monitor.Stop();
            Counters.Values.ToList().ForEach(c => c.Reset());
        }
        catch (Exception)
        {

        }
    }
    private void CopyRow_Selected()
    {
        Clipboard.SetText(JsonSerializer.Serialize(new LogEntry(selectedEvent.Process, selectedEvent.Time, selectedEvent.FileName, selectedEvent.Trace?.EventName)));
    }


    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }


    private void _monitor_Notify(object? sender, Events.FileRawEventArgs e)
    {
        App.Current.Dispatcher.BeginInvoke(() =>
        {
            items.Insert(0, e);
            if (items.Count > 1000)
            {
                items.RemoveAt(items.Count - 1);
            }
        });
    }



    public void UpdateProcesses()
    {
        try
        {
            var prc = Process.GetProcesses()
                .Select(s => new ProcessItem
                {
                    Id = s.Id,
                    Name = s.ProcessName + "," + s.Id
                })
                .ToList();

            if (Processes == null)
            {
                Processes = new ObservableCollection<ProcessItem>(prc);
            }
            else
            {
                foreach (var item in Processes.ToList())
                {
                    if (!prc.Any(d => d.Id == item.Id))
                    {
                        Processes.Remove(item);
                    }
                }


                foreach (var item in prc)
                {
                    if (!Processes.Any(d => d.Id == item.Id))
                    {
                        Processes.Add(item);
                    }
                }

                if (!processes.Any(d => d.Id == -1))
                    Processes.Insert(0, new ProcessItem
                    {
                        Id = -1,
                        Name = "All Processes"
                    });
            }


            OnPropertyChanged(nameof(Processes));
        }
        catch (Exception ex)
        {

        }
    }



}
