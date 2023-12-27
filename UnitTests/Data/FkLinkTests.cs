using Blazor.Data;

namespace UnitTests.Data;

public class FkLinkTests
{
    [Fact]
    public void LinksAreGenerated()
    {
        SqliteParser parser = new();
        EntityPlacementOrganizer organizer = new();
        EntityManager em = new(parser, organizer);
        em.GenerateData(SqliteScriptTestData.TwoTables);
    }
}