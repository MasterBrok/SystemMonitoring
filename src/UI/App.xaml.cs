using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Session;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows;
using UI.Services;
using UI.ViewModels;

namespace UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;
        public const string SessionName = "system_monitoring";

        protected override void OnStartup(StartupEventArgs e)
        {

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();
            var main = _serviceProvider.GetRequiredService<MainWindow>();
            main.Show();

        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<Window1>();
            services.AddSingleton<FilterInitilizer>(provider => new FilterInitilizer(Application.Current.Resources));

            var traceSession = new TraceEventSession(App.SessionName)
            {
                BufferSizeMB = 1024,
                StopOnDispose = true
            };
            traceSession.EnableKernelProvider(
        KernelTraceEventParser.Keywords.FileIO |
        KernelTraceEventParser.Keywords.FileIOInit | KernelTraceEventParser.Keywords.NetworkTCPIP
                      );

            services.AddSingleton<TraceEventSession>(traceSession);
            services.AddSingleton<IMonitor, Services.Monitor>();

        }

        private void OnExit(object sender, ExitEventArgs e)
        {
            if (_serviceProvider is IDisposable disposable)
            {
                
                disposable.Dispose();

            }
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
        }
    }

}
