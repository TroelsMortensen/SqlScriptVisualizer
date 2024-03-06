namespace Blazor.Data.Models;

public class Attribute
{
    public string Name { get; set; } = string.Empty;
    public bool IsPrimaryKey { get; set; }
    public ForeignKey? ForeignKey { get; set; }
};