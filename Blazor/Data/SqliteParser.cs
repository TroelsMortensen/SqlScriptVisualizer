using System.Text.RegularExpressions;
using Blazor.Data.Models;
using Attribute = Blazor.Data.Models.Attribute;

namespace Blazor.Data;

public class SqliteParser
{
    private bool isInTable = false;
    List<Entity> result = new();
    Entity entity = null!;

    public List<Entity> SqlScriptToEntities(string sql)
    {
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

    private void AddCompositePrimaryKey(string line)
    {
        if (!line.Trim().StartsWith("constraint", StringComparison.OrdinalIgnoreCase) ||
            !line.Contains("primary key", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        line = line.Remove(0, line.IndexOf('('))
            .Replace("(", "")
            .Replace("),", "")
            .Replace("\r", "")
            .Replace("\"", "");
        string[] pkAttrNames = line.Split(",");
        foreach (string name in pkAttrNames)
        {
            string trimmedName = name.Trim();
            entity.Attributes
                .Single(attr => attr.Name.Equals(trimmedName))
                .IsPrimaryKey = true;
        }
    }

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
    {
        Attribute attribute = entity.Attributes.Single(attr => attr.Name.Equals(attrName));
        attribute.ForeignKeys.Add(fk);
    }

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
        string pattern = "foreign key \\(\"(\\w+)\"\\) references \\\"(\\w+)\\\" \\(\\\"(\\w+)\\\"\\)";
        Regex r = new(pattern, RegexOptions.IgnoreCase);
        Match match = r.Match(line);
        GroupCollection groupCollection = match.Groups;
        string targetTable = groupCollection[2].Value;
        string targetAttr = groupCollection[3].Value;
        ForeignKey fk = new()
        {
            TargetAttributeName = targetAttr,
            TargetTableName = targetTable
        };
        return fk;
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

        Attribute attr = new();
        attr.Name = ExtractAttributeName(line);
        attr.IsPrimaryKey = ComputeIsPrimaryKey(line);
        entity.Attributes.Add(attr);
    }


    private static bool ComputeIsPrimaryKey(string line)
    {
        return line.Contains("primary key", StringComparison.OrdinalIgnoreCase);
    }

    private void HandleEndOfTable(string line)
    {
        if (!IsEndOfTable(line))
        {
            return;
        }

        isInTable = false;
        result.Add(entity);
    }

    private bool IsAttributeLine(string line)
    {
        return isInTable && !line.Trim().StartsWith("constraint", StringComparison.OrdinalIgnoreCase);
    }

    private static string ExtractAttributeName(string line)
    {
        return line.Trim().Split(" ").First().Replace("\"", "");
    }

    private void AddEntityNameToEntity(string line)
    {
        entity.Name = line.Replace("CREATE TABLE", "").Replace("\"", "").Replace("(", "").Trim();
    }

    private bool IsEndOfTable(string line)
    {
        return isInTable && line.Trim().Contains(");");
    }

    private bool IsStartOfTable(string line)
    {
        return line.Contains("CREATE TABLE") && !isInTable;
    }
}