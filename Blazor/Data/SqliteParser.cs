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

            AddAttribute(line);

            HandleStartOfTable(line);
        }

        return result;
    }

    private void HandleStartOfTable(string line)
    {
        if (IsStartOfTable(line))
        {
            isInTable = true;
            entity = new();
            string entityName = ExtractEntityName(line);
            entity.Name = entityName;
        }

    }

    private void AddAttribute(string line)
    {
        if (IsAttributeLine(line))
        {
            Attribute attr = new();
            attr.Name = ExtractAttributeName(line);
            attr.IsPrimaryKey = ComputeIsPrimaryKey(line);
            entity.Attributes.Add(attr);
        }
    }

    private bool ComputeIsPrimaryKey(string line)
    {
        return line.Contains("primary key", StringComparison.OrdinalIgnoreCase);
    }

    private void HandleEndOfTable(string line)
    {
        if (IsEndOfTable(line))
        {
            isInTable = false;
            result.Add(entity);
        }
    }

    private bool IsAttributeLine(string line)
    {
        return isInTable && !line.Trim().StartsWith("constraint", StringComparison.OrdinalIgnoreCase);
    }

    private string ExtractAttributeName(string line)
    {
        return line.Trim().Split(" ").First().Replace("\"", "");
    }

    private static string ExtractEntityName(string line)
    {
        return line.Replace("CREATE TABLE", "").Replace("\"", "").Replace("(", "").Trim();
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