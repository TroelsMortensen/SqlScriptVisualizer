namespace Blazor.Data.Models;

public class Attribute
{
    public string Name { get; set; } = "Default";
    public bool IsPrimaryKey { get; set; }
    public List<ForeignKey> ForeignKeys { get; set; } = [];
};