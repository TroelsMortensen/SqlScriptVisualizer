using Blazor.Data;
using Blazor.Data.Parsers;

namespace UnitTests.Data;

public class FkLinkTests
{
    [Fact]
    public void LinksAreGenerated()
    {
        SqliteParser parser = new();
        EntityManager em = new(parser);
        em.GenerateData(SqliteScriptTestData.TwoTables);
    }
}