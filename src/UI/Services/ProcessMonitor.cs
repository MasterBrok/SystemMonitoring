using System.Diagnostics;
using UI.Models;

namespace UI.Services;

public class ProcessMonitor
{
    private readonly int _pid;
    private readonly Process _process;

    private TimeSpan _lastCpu;
    private DateTime _lastTime;

    public ProcessStats Stats { get; } = new();


    public ProcessMonitor(int pid)
    {
        _pid = pid;
        _process = Process.GetProcessById(pid);

        _lastCpu = _process.TotalProcessorTime;
        _lastTime = DateTime.UtcNow;
    }


    public async Task Start()
    {
        while (!_process.HasExited)
        {
            UpdateCpu();
            UpdateMemory();

            await Task.Delay(1000);
        }
    }


    private void UpdateCpu()
    {
        var nowCpu = _process.TotalProcessorTime;
        var now = DateTime.UtcNow;


        var cpuMs =
            (nowCpu - _lastCpu).TotalMilliseconds;


        var totalMs =
            (now - _lastTime).TotalMilliseconds;


        if (totalMs > 0)
        {
            Stats.CPU =
                cpuMs /
                (totalMs * Environment.ProcessorCount)
                * 100;
        }


        _lastCpu = nowCpu;
        _lastTime = now;
    }


    private void UpdateMemory()
    {
        _process.Refresh();
        Stats.Memory =
            _process.WorkingSet64 / 1024 / 1024;
    }
}