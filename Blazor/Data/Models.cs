namespace Blazor.Data;

public record Entity(string Name, List<Attribute> Attributes);

public record Attribute(string Name, bool IsPrimaryKey, List<ForeignKey> ForeignKeys);

public record ForeignKey(string TargetTableName, string TargetAttributeName);
