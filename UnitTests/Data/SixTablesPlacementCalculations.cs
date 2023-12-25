﻿using Blazor.Data;
using Blazor.Data.Models;

namespace UnitTests.Data;

public class SixTablesPlacementCalculations
{
    private readonly List<List<Entity>> placements;

    public SixTablesPlacementCalculations()
    {
        SqliteParser parser = new();
        EntityPlacementOrganizer organizer = new();
        List<Entity> entities = parser.SqlScriptToEntities(SqliteScriptTestData.SixTables);
        placements = organizer.CalculateRelativePlacements(entities);
    }
    
    [Fact]
    public void RootsArePlacedCorrectly()
    {
        Assert.Contains(placements[0], ent => ent.Name.Equals("Author"));
        Assert.Contains(placements[0], ent => ent.Name.Equals("Books"));
        Assert.Contains(placements[0], ent => ent.Name.Equals("Categories"));
    }

    [Fact]
    public void SecondRankDependentsArePlacedCorrectly()
    {
        Assert.Contains(placements[1], ent => ent.Name.Equals("PriceOffers"));
        Assert.Contains(placements[1], ent => ent.Name.Equals("Reviews"));
    }

    [Fact]
    public void ThirdRankDependentsArePlacedCorrectly()
    {
        Assert.Contains(placements[1], ent => ent.Name.Equals("BookAuthor"));
        Assert.Contains(placements[1], ent => ent.Name.Equals("BookCategory"));
    }
}