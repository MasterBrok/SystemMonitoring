using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UI.Models;

public class ProcessStats : INotifyPropertyChanged
{
    private double _cpu;
    private double _memory;
    
    public double CPU
    {
        get => _cpu;
        set
        {
            _cpu = value;
            OnPropertyChanged();
        }
    }

    public double Memory
    {
        get => _memory;
        set
        {
            _memory = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(
        [CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(
            this,
            new(name));
    }
}