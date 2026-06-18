```
AppMonitor
│
├── App.xaml
│
├── Views
│   ├── MainWindow.xaml
│   ├── DashboardView.xaml
│   ├── EventLogView.xaml
│   ├── ProcessSelectionView.xaml
│   └── SettingsView.xaml
│
├── ViewModels
│   ├── MainViewModel.cs
│   ├── DashboardViewModel.cs
│   ├── EventLogViewModel.cs
│   ├── ProcessSelectionViewModel.cs
│   └── SettingsViewModel.cs
│
├── Models
│   ├── MonitoredProcess.cs
│   ├── FileEvent.cs
│   ├── NetworkEvent.cs
│   ├── ProcessEvent.cs
│   ├── RegistryEvent.cs
│   └── LogEntry.cs
│
├── Services
│   ├── Monitoring
│   │   ├── IMonitorService.cs
│   │   ├── MonitorManager.cs
│   │   ├── FileMonitor.cs
│   │   ├── NetworkMonitor.cs
│   │   ├── ProcessMonitor.cs
│   │   └── RegistryMonitor.cs
│   │
│   ├── Logging
│   │   ├── ILoggerService.cs
│   │   ├── FileLogger.cs
│   │   └── DatabaseLogger.cs
│   │
│   └── Storage
│       ├── SQLiteService.cs
│       └── Repository.cs
│
├── Events
│   ├── EventBus.cs
│   ├── FileCreatedEvent.cs
│   ├── FileModifiedEvent.cs
│   ├── NetworkDetectedEvent.cs
│   └── ProcessStartedEvent.cs
│
├── Helpers
│   ├── RelayCommand.cs
│   ├── ObservableObject.cs
│   └── Extensions.cs
│
└── Database
    └── monitor.db
```