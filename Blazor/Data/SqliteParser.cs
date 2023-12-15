using Blazor.Data.Models;

namespace Blazor.Data;

public class SqliteParser
{
    private bool isInTable = false;

    public List<Entity> SqlScriptToEntities(string sql)
    {
        List<Entity> result = new();
        string[] strings = sql.Split("\n");
        Entity entity = null!;
        foreach (var line in strings)
        {
            if (IsStartOfTable(line))
            {
                isInTable = true;
                entity = new();
                string entityName = line.Replace("CREATE TABLE", "").Replace("\"", "").Replace("(","").Trim();
                entity.Name = entityName;
            }

            if (IsEndOfTable( line))
            {
                isInTable = false;
                result.Add(entity);
            }
        }

        return result;
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