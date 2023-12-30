using System.Text.RegularExpressions;
using Blazor.Data;
using Blazor.Data.Models;
using Blazor.Data.Parsers;

namespace UnitTests.Data;

public class SqliteParser2TablesUnitTests
{
    private readonly List<Entity> entities;
    
    
    public SqliteParser2TablesUnitTests()
    {
        SqliteParser parser = new();
        entities = parser.SqlScriptToEntities(SqliteScriptTestData.TwoTables);
    }

    [Fact]
    public void ReturnsTwoEntities()
    {
        Assert.Equal(2, entities.Count);
        Assert.Equal("TvShows", entities.First().Name);
        Assert.Equal("Episodes", entities[1].Name);
    }

    [Fact]
    public void EntitiesContainsAttributes()
    {
        Assert.Equal(4, entities.First().Attributes.Count);
        Assert.Equal(5, entities[1].Attributes.Count);
    }

    [Fact]
    public void AttributesHaveCorrectName()
    {
        Assert.Equal("Id", entities[0].Attributes[0].Name);
        Assert.Equal("Title", entities[0].Attributes[1].Name);
        Assert.Equal("Year", entities[0].Attributes[2].Name);
        Assert.Equal("Genre", entities[0].Attributes[3].Name);

        Assert.Equal("Id", entities[1].Attributes[0].Name);
        Assert.Equal("Title", entities[1].Attributes[1].Name);
        Assert.Equal("Runtime", entities[1].Attributes[2].Name);
        Assert.Equal("SeasonId", entities[1].Attributes[3].Name);
        Assert.Equal("TvShowId", entities[1].Attributes[4].Name);
    }

    [Fact]
    public void PrimaryKeyAttributesAreMarkedCorrectly()
    {
        Assert.True(entities.First().Attributes.First().IsPrimaryKey);
        Assert.True(entities[1].Attributes.First().IsPrimaryKey);
    }

    [Fact]
    public void OnlyOnePrimaryKeyPerTableWithoutCompositeKey()
    {
        Assert.Equal(1, entities[0].Attributes.Count(attribute => attribute.IsPrimaryKey));
        Assert.Equal(1, entities[1].Attributes.Count(attribute => attribute.IsPrimaryKey));
    }

    [Fact]
    public void ForeignKeyAddedToAttributes()
    {
        ForeignKey foreignKey = entities[1].Attributes.First(attribute => attribute.Name.Equals("TvShowId")).ForeignKey!;
        // Assert.Single(foreignKey);
        Assert.Equal("Id", foreignKey.TargetAttributeName);
        Assert.Equal("TvShows", foreignKey.TargetTableName);
    }

    [Fact]
    public void CanExtractFkTargetAttributeAndTableNamesWithRegEx()
    {
        string input =
            @"    constraint ""fk_episodes_tvshows_tvshowid"" foreign key (""tvshowid"") references ""tvshows"" (""id"") ON DELETE CASCADE";
        ForeignKey foreignKey = SqliteParser.CreateForeignKey(input);

        Assert.Equal("tvshows", foreignKey.TargetTableName);
        Assert.Equal("id", foreignKey.TargetAttributeName);
    }

    [Fact]
    public void CanExtractFkAttributeNameWithRegEx()
    {
        string input =
            @"    constraint ""fk_episodes_tvshows_tvshowid"" foreign key (""tvshowid"") references ""tvshows"" (""id"") ON DELETE CASCADE";
        string fkAttrName = SqliteParser.ExtractFkAttributeName(input);
        Assert.Equal("tvshowid", fkAttrName);
    }



}