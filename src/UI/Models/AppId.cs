using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace UI.Models;

public struct AppId(int processId)
{
    public readonly int ProcessId = processId;
    public bool Equals(int pId)
    {
        if (pId == 0)
            return false;
        return pId == ProcessId;
    }
}
