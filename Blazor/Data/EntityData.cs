namespace Blazor.Data;

public class EntityData
{
    public string Name { get; set; } = "Default";
    public List<string> Attributes { get; set; } = new();
    public int Xstart { get; set; }
    public int Ystart { get; set; }
}