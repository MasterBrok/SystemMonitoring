using System.Windows.Input;

namespace UI.Commands;

public sealed class RelayCommand : ICommand
{
    private Action action;
    public RelayCommand(Action action)
    {
        this.action = action;
    }
    public bool CanExecute(object p) => true;

    public void Execute(object p) => action();


    public event EventHandler CanExecuteChanged;
}
