using Blazor.Data;
using Blazor.Data.Models;

namespace UnitTests.Data;

public class TwoTablesPlacementCalculationTests
{
    private List<List<Entity>> placements;
    private readonly List<Entity> entities;

    public TwoTablesPlacementCalculationTests()
    {
        SqliteParser parser = new();
        EntityManager entityManager = new(parser);
        entities = parser.SqlScriptToEntities(SqliteScriptTestData.TwoTables);
        placements = entityManager.CalculateRelativePlacements(entities);
    }

    [Fact]
    public void TablesArePlaced()
    {
        Assert.Single(placements[0]);
        Assert.Single(placements[1]);
    }

    [Fact]
    public void TablesArePlacedInCorrectColumns()
    {
        Assert.Contains(placements[0], ent => ent.Name.Equals("TvShows"));
        Assert.Contains(placements[1], ent => ent.Name.Equals("Episodes"));
    }
}