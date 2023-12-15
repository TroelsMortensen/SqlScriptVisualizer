﻿using Blazor.Data.Models;

namespace Blazor.Data;

public class SqliteParser(EntityManager entityManager)
{
    public void SqliteToEntities(string sql)
    {
        string[] strings = sql.Split("\n");
        Entity entity = null!;
        bool isInTable = false;
        foreach (var line in strings)
        {
            if ("CREATE TABLE".Equals(line))
            {
                entity = new();
                string entityName = line.Replace("CREATE TABLE", "").Replace("\"", "").Trim();
                
            }
        }
    }
}