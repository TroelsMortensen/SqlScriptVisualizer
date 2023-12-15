namespace Blazor.Data.Models;

public class ForeignKey
{
    public string TargetTableName { get; set; } = null!;
    public string TargetAttributeName { get; set; } = null!;
}