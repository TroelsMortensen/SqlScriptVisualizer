﻿using Blazor.Data;
using Blazor.Data.Models;
using Blazor.Data.Parsers;
using Blazor.ViewModels;

namespace UnitTests.Data;

public class SomeWierdErrorReproductionTest
{
    [Fact]
    public void GeneratingDataTwiceDoesNotDoubleEntityCount()
    {
        ISqlParser parser = SqlParserFactory.GetParser("sqlite");

        EntityManager em = new(parser);
        em.GenerateData(SqliteScriptTestData.TwoTables);

        Assert.Equal(2, em.Entities.Count);

        em.GenerateData(SqliteScriptTestData.TwoTables);

        Assert.Equal(2, em.Entities.Count);

        // List<Entity> entities = parser.SqlScriptToEntities(SqliteScriptTestData.TwoTables);
        // placements = organizer.CalculateRelativePlacements(entities);
    }
}