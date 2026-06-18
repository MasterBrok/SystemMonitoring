using System.Windows;
using System.Windows.Input;
using UI.Services;

namespace UI.Commands;

public sealed class ApplicationCloseCommand : ICommand
{
    public event EventHandler? CanExecuteChanged;
    private readonly IMonitor monitor;

    public ApplicationCloseCommand(IMonitor monitor)
    {
        this.monitor = monitor;
    }

    public bool CanExecute(object? parameter)
    {
        return Application.Current is not null && Application.Current.MainWindow is not null;
    }

    public void Execute(object? parameter)
    {
        try
        {
            monitor.Dispose();
            Application.Current.MainWindow.Close();
        }
        catch (Exception)
        {

        }
    }
}
