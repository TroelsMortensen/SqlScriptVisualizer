using Blazor.Data.Models;
using Blazor.Data.Parsers;

namespace UnitTests.Data;

public class VeaScriptTest
{
    private readonly List<Entity> entities;
    
    public VeaScriptTest()
    {
        ISqlParser parser = SqlParserFactory.GetParser("sqlite");
        entities = parser.SqlScriptToEntities(SqliteScriptTestData.EventScript);
    }

    [Fact]
    public void ContainsFourTables()
    {
        Assert.Equal(4, entities.Count);
    }
    
    
}