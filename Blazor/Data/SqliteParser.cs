namespace Blazor.Data;

public class SqliteParser(EntityManager entityManager)
{
    public void SqliteToEntities(string sql)
    {
        string[] strings = sql.Split("\n");
        EntityData data = new();
        foreach (var line in strings)
        {
            if ("CREATE TABLE".Equals(line))
            {
                string entityName = line.Replace("CREATE TABLE", "").Replace("\"", "").Trim();
                Console.WriteLine($"name {entityName}");
                
            }
        }
    }
}