using Blazor.Data;

namespace UnitTests.Data;

public class SqlParserUnitTests
{
    [Fact]
    public void Parse_ReturnsTwoEntities_GivenTwoTables()
    {
        SqliteParser parser = new();
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