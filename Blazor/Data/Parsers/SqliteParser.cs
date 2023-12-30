using System.Text.RegularExpressions;

namespace Blazor.Data.Parsers;

public class SqliteParser : BaseParser
{
    protected override bool IsNotPrimaryKeyConstraintDefinition_DialectSpecific(string line)
        => !line.Trim().StartsWith("constraint", StringComparison.OrdinalIgnoreCase) ||
           !line.Contains("primary key", StringComparison.OrdinalIgnoreCase);

    protected override string ExtractCommaSeparatedListOfAttributeNames_DialectSpecific(string line)
        => line
            .Remove(0, line.IndexOf('('))
            .Replace("(", "")
            .Replace("),", "")
            .Replace("\r", "")
            .Replace("\"", "");

    protected override bool IsNotForeignKeyLine_DialectSpecific(string line)
        => !line.Contains("foreign key", StringComparison.OrdinalIgnoreCase);

    protected override string ExtractFkAttributeName_DialectSpecific(string line)
    {
        string pattern = "foreign key \\(\"(\\w+)\"\\) references";
        Regex r = new(pattern, RegexOptions.IgnoreCase);
        Match match = r.Match(line);
        GroupCollection groupCollection = match.Groups;
        return groupCollection[1].Value;
    }

    protected override (string Table, string Attr) ExtractTargetTableAndAttribute_DialectSpecific(string line)
    {
        GroupCollection groupCollection =
            new Regex("foreign key \\(\"(\\w+)\"\\) references \\\"(\\w+)\\\" \\(\\\"(\\w+)\\\"\\)", RegexOptions.IgnoreCase)
                .Match(line)
                .Groups;
        return (groupCollection[2].Value, groupCollection[3].Value);
    }


    protected override bool ComputeIsPrimaryKey_DialectSpecific(string line)
        => line.Contains("primary key", StringComparison.OrdinalIgnoreCase);

    protected override bool IsAttributeLine_DialectSpecific(string line)
        => !line.Trim().StartsWith("constraint", StringComparison.OrdinalIgnoreCase);

    protected override string ExtractAttributeName_DialectSpecific(string line)
        => line.Trim().Split(" ").First().Replace("\"", "");

    protected override string GetEntityName_DialectSpecific(string line)
        => line.Replace("CREATE TABLE", "").Replace("\"", "").Replace("(", "").Trim();

    protected override bool IsEndOfTable_DialectSpecific(string line)
        => line.Trim().Contains(");");

    protected override bool IsCreateTableDefinition_DialectSpecific(string line)
        => line.Contains("CREATE TABLE");
}