using System.Collections.ObjectModel;
using UI.Enums;

namespace UI.Models;

public class Counter
{
    public int Count { get; set; }
    internal void Increment()
    {
        Count++;
    }

    internal void Reset()
    {
        Count = 0;
    }
}
