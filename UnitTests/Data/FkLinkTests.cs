using Blazor.Data;
using Blazor.Data.Parsers;

namespace UnitTests.Data;

public class FkLinkTests
{
    [Fact]
    public void LinksAreGenerated()
    {
        ISqlParser parser = SqlParserFactory.GetParser("sqlite");
        EntityManager em = new(parser);
        em.GenerateData(SqliteScriptTestData.TwoTables);
    }
}