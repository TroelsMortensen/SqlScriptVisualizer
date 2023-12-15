using Blazor.Data;
using Blazor.Data.Models;

namespace UnitTests.Data;

public class SqlParserUnitTests
{
    private readonly SqliteParser parser;

    public SqlParserUnitTests()
    {
        parser = new SqliteParser();
    }

    [Fact]
    public void Parse_ReturnsTwoEntities_GivenTwoTables()
    {
        List<Entity> entities = parser.SqlScriptToEntities(twoTablesScript);
        
        Assert.Equal(2, entities.Count);
        Assert.Equal("TvShows",entities.First().Name);
        Assert.Equal("Episodes",entities[1].Name);
    }

    [Fact]
    public void Parse_EntitiesContainsAttributes_GivenTwoTablesWithAttributes()
    {
    }

    private const string twoTablesScript = @"CREATE TABLE ""TvShows"" (
    ""Id"" INTEGER NOT NULL CONSTRAINT ""PK_TvShows"" PRIMARY KEY AUTOINCREMENT,
    ""Title"" TEXT NOT NULL,
    ""Year"" INTEGER NOT NULL,
    ""Genre"" TEXT NOT NULL
);

CREATE TABLE ""Episodes"" (
    ""Id"" INTEGER NOT NULL CONSTRAINT ""PK_Episodes"" PRIMARY KEY AUTOINCREMENT,
    ""Title"" TEXT NOT NULL,
    ""Runtime"" INTEGER NOT NULL,
    ""SeasonId"" TEXT NOT NULL,
    ""TvShowId"" INTEGER NOT NULL,
    CONSTRAINT ""FK_Episodes_TvShows_TvShowId"" FOREIGN KEY (""TvShowId"") REFERENCES ""TvShows"" (""Id"") ON DELETE CASCADE
);";
}