using Blazor.Data;
using Blazor.Data.Models;
using Blazor.Data.Parsers;

namespace UnitTests.Data;

public class TwoTablesPlacementCalculationTests
{
    private readonly List<List<Entity>> placements;

    public TwoTablesPlacementCalculationTests()
    {
        SqliteParser parser = new();
        EntityPlacementOrganizer organizer = new();

        List<Entity> entities = parser.SqlScriptToEntities(SqliteScriptTestData.TwoTables);
        placements = organizer.CalculateRelativePlacements(entities);
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