namespace UI.Models;

public sealed class ProcessItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    override public string ToString()
    {
        return $"{Name},{Id}";
    }
}
