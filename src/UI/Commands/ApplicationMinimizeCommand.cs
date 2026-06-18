using System.Windows;
using System.Windows.Input;

namespace UI.Commands;

public sealed class ApplicationMinimizeCommand : ICommand
{
    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        return Application.Current is not null && Application.Current.MainWindow is not null;
    }

    public void Execute(object? parameter)
    {
        Application.Current.MainWindow.WindowState = WindowState.Minimized;
    }
}
