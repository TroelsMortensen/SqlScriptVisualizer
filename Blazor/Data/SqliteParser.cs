using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using Blazor.Data.Models;
using Attribute = Blazor.Data.Models.Attribute;

namespace Blazor.Data;

public partial class SqliteParser
{
    private bool isInTable = false;
    List<Entity> result = new();
    Entity entity = null!;

    public List<Entity> SqlScriptToEntities(string sql)
    {
        ResetState();
        string[] strings = sql.Split("\n");
        foreach (var line in strings)
        {
            HandleEndOfTable(line);

            HandleAttribute(line);

            AddForeignKey(line);

            AddCompositePrimaryKey(line);

            HandleStartOfTable(line);
        }

        return result;
    }

    private void ResetState()
    {
        isInTable = false;
        result = new();
        entity = null!;
    }

    private void AddCompositePrimaryKey(string line)
    {
        if (IsNotPrimaryKeyConstraintDefinition(line))
        {
            return;
        }

        line = ExtractCommaSeparatedListOfAttributeNames(line);
        IEnumerable<string> pkAttrNames = TrimAllNames(line);
        foreach (string name in pkAttrNames)
        {
            entity.Attributes
                .Single(attr => attr.Name.Equals(name))
                .IsPrimaryKey = true;
        }
    }

    private static bool IsNotPrimaryKeyConstraintDefinition(string line)
        => !line.Trim().StartsWith("constraint", StringComparison.OrdinalIgnoreCase) ||
           !line.Contains("primary key", StringComparison.OrdinalIgnoreCase);


    private static IEnumerable<string> TrimAllNames(string line)
        => line.Split(",").Select(s => s.Trim());

    private static string ExtractCommaSeparatedListOfAttributeNames(string line)
        => line
            .Remove(0, line.IndexOf('('))
            .Replace("(", "")
            .Replace("),", "")
            .Replace("\r", "")
            .Replace("\"", "");

    private void AddForeignKey(string line)
    {
        if (!line.Contains("foreign key", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        string attrName = ExtractFkAttributeName(line);
        ForeignKey fk = CreateForeignKey(line);
        AddForeignKeyToAttribute(attrName, fk);
    }

    private void AddForeignKeyToAttribute(string attrName, ForeignKey fk)
        => entity.Attributes.Single(attr => attr.Name.Equals(attrName)).ForeignKey = fk;

    public static string ExtractFkAttributeName(string line)
    {
        string pattern = "foreign key \\(\"(\\w+)\"\\) references";
        Regex r = new(pattern, RegexOptions.IgnoreCase);
        Match match = r.Match(line);
        GroupCollection groupCollection = match.Groups;
        return groupCollection[1].Value;
    }

    public static ForeignKey CreateForeignKey(string line)
    {
        (string Table, string Attr) data = ExtractTargetTableAndAttribute(line);
        ForeignKey fk = new()
        {
            TargetAttributeName = data.Attr,
            TargetTableName = data.Table
        };
        return fk;
    }

    private static (string Table, string Attr) ExtractTargetTableAndAttribute(string line)
    {
        string pattern = "foreign key \\(\"(\\w+)\"\\) references \\\"(\\w+)\\\" \\(\\\"(\\w+)\\\"\\)";
        Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
        Match match = r.Match(line);
        GroupCollection groupCollection = match.Groups;
        return (groupCollection[2].Value, groupCollection[3].Value);
    }

    private void HandleStartOfTable(string line)
    {
        if (!IsStartOfTable(line))
        {
            return;
        }

        isInTable = true;
        entity = new();
        AddEntityNameToEntity(line);
    }

    private void HandleAttribute(string line)
    {
        if (!IsAttributeLine(line))
        {
            return;
        }

        Attribute attr = new()
        {
            Name = ExtractAttributeName(line),
            IsPrimaryKey = ComputeIsPrimaryKey(line)
        };
        entity.Attributes.Add(attr);
    }


    private static bool ComputeIsPrimaryKey(string line)
        => line.Contains("primary key", StringComparison.OrdinalIgnoreCase);

    private void HandleEndOfTable(string line)
    {
        if (!IsEndOfTable(line))
        {
            return;
        }

        isInTable = false;
        result.Add(entity);
        entity = null!;
    }

    private bool IsAttributeLine(string line)
        => isInTable && !line.Trim().StartsWith("constraint", StringComparison.OrdinalIgnoreCase);

    private static string ExtractAttributeName(string line)
        => line.Trim().Split(" ").First().Replace("\"", "");

    private void AddEntityNameToEntity(string line)
        => entity.Name = line.Replace("CREATE TABLE", "").Replace("\"", "").Replace("(", "").Trim();

    private bool IsEndOfTable(string line)
        => isInTable && line.Trim().Contains(");");

    private bool IsStartOfTable(string line)
        => line.Contains("CREATE TABLE") && !isInTable;
    [GeneratedRegex("foreign key \\(\"(\\w+)\"\\) references \\\"(\\w+)\\\" \\(\\\"(\\w+)\\\"\\)", RegexOptions.IgnoreCase, "en-GB")]
    private static partial Regex MyRegex();
}