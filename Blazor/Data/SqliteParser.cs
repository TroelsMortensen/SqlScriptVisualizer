using Blazor.Data.Models;
using Attribute = Blazor.Data.Models.Attribute;

namespace Blazor.Data;

public class SqliteParser
{
    private bool isInTable = false;
    List<Entity> result = new();

    public List<Entity> SqlScriptToEntities(string sql)
    {
        string[] strings = sql.Split("\n");
        Entity entity = null!;
        foreach (var line in strings)
        {
            HandleEndOfTable(line, result, entity);

            HandleAttribute(line, entity);

            entity = HandleStartOfTable(line, entity);
        }

        return result;
    }

    private Entity HandleStartOfTable(string line, Entity entity)
    {
        if (IsStartOfTable(line))
        {
            isInTable = true;
            entity = new();
            string entityName = ExtractEntityName(line);
            entity.Name = entityName;
        }

        return entity;
    }

    private void HandleAttribute(string line, Entity entity)
    {
        if (IsAttributeLine(line))
        {
            Attribute attr = new();
            attr.Name = ExtractAttributeName(line);
            entity.Attributes.Add(attr);
        }
    }

    private void HandleEndOfTable(string line, List<Entity> result, Entity entity)
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