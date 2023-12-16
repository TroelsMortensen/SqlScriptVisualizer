using Blazor.Data;
using Blazor.Data.Models;
using Attribute = Blazor.Data.Models.Attribute;

namespace UnitTests.Data;

public class SqliteParser6TablesUnitTests
{
    private readonly List<Entity> entities;


    public SqliteParser6TablesUnitTests()
    {
        SqliteParser parser = new();
        entities = parser.SqlScriptToEntities(SixTablesScript);
    }

    [Fact]
    public void ReturnsTwoEntities()
    {
        Assert.Equal(7, entities.Count);

        Assert.Contains(entities, ent => ent.Name.Equals("Author"));
        Assert.Contains(entities, ent => ent.Name.Equals("Books"));
        Assert.Contains(entities, ent => ent.Name.Equals("PriceOffers"));
        Assert.Contains(entities, ent => ent.Name.Equals("Reviews"));
        Assert.Contains(entities, ent => ent.Name.Equals("Categories"));
        Assert.Contains(entities, ent => ent.Name.Equals("BookCategory"));
        Assert.Contains(entities, ent => ent.Name.Equals("BookAuthor"));
    }

    [Theory]
    [InlineData(0,2)]
    [InlineData(1,5)]
    [InlineData(2,1)]
    [InlineData(3,3)]
    [InlineData(4,4)]
    [InlineData(5,5)]
    [InlineData(6,2)]
    public void EntitiesContainsAttributes(int idx, int count)
    {
        Assert.Equal(count, entities[idx].Attributes.Count);
    }

    [Theory]
    [MemberData(nameof(AttributeNames))]
    public void AttributesHaveCorrectName(int idx, List<string> inputNames)
    {
        List<Attribute> attributes = entities[idx].Attributes;
        IEnumerable<string> attrNames = attributes.Select(attr => attr.Name);
        Assert.Equal(inputNames, attrNames);
    }

    public static IEnumerable<object[]> AttributeNames()
    {
        int i = 0;
        yield return new object[] { i++, new List<string>(){"Id", "Name"} };
        yield return new object[] { i++, new List<string>(){"Id", "Title", "PublishDate", "Price", "Publisher"} };
        yield return new object[] { i++, new List<string>(){"Name"} };
        yield return new object[] { i++, new List<string>(){"BookId", "AuthorId", "Order"} };
        yield return new object[] { i++, new List<string>(){"Id", "PromotionalPrice", "PromotionalText", "BookId"} };
        yield return new object[] { i++, new List<string>(){"Id", "Rating", "VoterName", "Comment", "BookId"} };
        yield return new object[] { i++, new List<string>(){"BooksId", "CategoriesName"} };
    }
    
    [Fact]
    public void PrimaryKeyAttributesAreMarkedCorrectly()
    {
        // Assert.True(entities.First().Attributes.First().IsPrimaryKey);
        // Assert.True(entities[1].Attributes.First().IsPrimaryKey);
    }

    [Fact]
    public void OnlyOnePrimaryKeyPerTableWithoutCompositeKey()
    {
        // Assert.Equal(1, entities[0].Attributes.Count(attribute => attribute.IsPrimaryKey));
        // Assert.Equal(1, entities[1].Attributes.Count(attribute => attribute.IsPrimaryKey));
    }

    [Fact]
    public void ForeignKeyAddedToAttributes()
    {
        // List<ForeignKey> foreignKeys = entities[1].Attributes.First(attribute => attribute.Name.Equals("TvShowId")).ForeignKeys;
        // Assert.Single(foreignKeys);
        // Assert.Equal("Id", foreignKeys.First().TargetAttributeName);
        // Assert.Equal("TvShows", foreignKeys.First().TargetTableName);
    }

    [Fact]
    public void CanExtractFkTargetAttributeAndTableNamesWithRegEx()
    {
        // string input =
            // @"    constraint ""fk_episodes_tvshows_tvshowid"" foreign key (""tvshowid"") references ""tvshows"" (""id"") ON DELETE CASCADE";
        // ForeignKey foreignKey = SqliteParser.CreateForeignKey(input);

        // Assert.Equal("tvshows", foreignKey.TargetTableName);
        // Assert.Equal("id", foreignKey.TargetAttributeName);
    }

    [Fact]
    public void CanExtractFkAttributeNameWithRegEx()
    {
        // string input =
            // @"    constraint ""fk_episodes_tvshows_tvshowid"" foreign key (""tvshowid"") references ""tvshows"" (""id"") ON DELETE CASCADE";
        // string fkAttrName = SqliteParser.ExtractFkAttributeName(input);
        // Assert.Equal("tvshowid", fkAttrName);
    }


    private const string SixTablesScript = @"CREATE TABLE ""Author"" (
    ""Id"" INTEGER NOT NULL CONSTRAINT ""PK_Author"" PRIMARY KEY AUTOINCREMENT,
    ""Name"" TEXT NOT NULL
);

CREATE TABLE ""Books"" (
    ""Id"" INTEGER NOT NULL CONSTRAINT ""PK_Books"" PRIMARY KEY AUTOINCREMENT,
    ""Title"" TEXT NOT NULL,
    ""PublishDate"" TEXT NOT NULL,
    ""Price"" TEXT NOT NULL,
    ""Publisher"" TEXT NOT NULL
);

CREATE TABLE ""Categories"" (
    ""Name"" TEXT NOT NULL CONSTRAINT ""PK_Categories"" PRIMARY KEY
);

CREATE TABLE ""BookAuthor"" (
    ""BookId"" INTEGER NOT NULL,
    ""AuthorId"" INTEGER NOT NULL,
    ""Order"" INTEGER NOT NULL,
    CONSTRAINT ""PK_BookAuthor"" PRIMARY KEY (""BookId"", ""AuthorId""),
    CONSTRAINT ""FK_BookAuthor_Author_AuthorId"" FOREIGN KEY (""AuthorId"") REFERENCES ""Author"" (""Id"") ON DELETE CASCADE,
    CONSTRAINT ""FK_BookAuthor_Books_BookId"" FOREIGN KEY (""BookId"") REFERENCES ""Books"" (""Id"") ON DELETE CASCADE
);

CREATE TABLE ""PriceOffers"" (
    ""Id"" INTEGER NOT NULL CONSTRAINT ""PK_PriceOffers"" PRIMARY KEY AUTOINCREMENT,
    ""PromotionalPrice"" TEXT NOT NULL,
    ""PromotionalText"" TEXT NULL,
    ""BookId"" INTEGER NOT NULL,
    CONSTRAINT ""FK_PriceOffers_Books_BookId"" FOREIGN KEY (""BookId"") REFERENCES ""Books"" (""Id"") ON DELETE CASCADE
);

CREATE TABLE ""Reviews"" (
    ""Id"" INTEGER NOT NULL CONSTRAINT ""PK_Reviews"" PRIMARY KEY AUTOINCREMENT,
    ""Rating"" INTEGER NOT NULL,
    ""VoterName"" TEXT NOT NULL,
    ""Comment"" TEXT NOT NULL,
    ""BookId"" INTEGER NOT NULL,
    CONSTRAINT ""FK_Reviews_Books_BookId"" FOREIGN KEY (""BookId"") REFERENCES ""Books"" (""Id"") ON DELETE CASCADE
);

CREATE TABLE ""BookCategory"" (
    ""BooksId"" INTEGER NOT NULL,
    ""CategoriesName"" TEXT NOT NULL,
    CONSTRAINT ""PK_BookCategory"" PRIMARY KEY (""BooksId"", ""CategoriesName""),
    CONSTRAINT ""FK_BookCategory_Books_BooksId"" FOREIGN KEY (""BooksId"") REFERENCES ""Books"" (""Id"") ON DELETE CASCADE,
    CONSTRAINT ""FK_BookCategory_Categories_CategoriesName"" FOREIGN KEY (""CategoriesName"") REFERENCES ""Categories"" (""Name"") ON DELETE CASCADE
);";
}