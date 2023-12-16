using System.Collections;
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
    [InlineData(0, 2)]
    [InlineData(1, 5)]
    [InlineData(2, 1)]
    [InlineData(3, 3)]
    [InlineData(4, 4)]
    [InlineData(5, 5)]
    [InlineData(6, 2)]
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
        yield return new object[] { i++, new List<string>() { "Id", "Name" } };
        yield return new object[] { i++, new List<string>() { "Id", "Title", "PublishDate", "Price", "Publisher" } };
        yield return new object[] { i++, new List<string>() { "Name" } };
        yield return new object[] { i++, new List<string>() { "BookId", "AuthorId", "Order" } };
        yield return new object[] { i++, new List<string>() { "Id", "PromotionalPrice", "PromotionalText", "BookId" } };
        yield return new object[] { i++, new List<string>() { "Id", "Rating", "VoterName", "Comment", "BookId" } };
        yield return new object[] { i++, new List<string>() { "BooksId", "CategoriesName" } };
    }

    [Theory]
    [MemberData(nameof(PrimaryKeyAttrs))]
    public void PrimaryKeyAttributesAreMarkedCorrectly(int idx, List<string> pkAttrNames)
    {
        bool allArePk = entities[idx]
            .Attributes
            .Where(attr => pkAttrNames.Contains(attr.Name))
            .All(attr => attr.IsPrimaryKey);

        Assert.True(allArePk);
    }

    public static IEnumerable<object[]> PrimaryKeyAttrs()
    {
        int i = 0;
        yield return new object[] { i++, new List<string>() { "Id" } };
        yield return new object[] { i++, new List<string>() { "Id" } };
        yield return new object[] { i++, new List<string>() { "Name" } };
        yield return new object[] { i++, new List<string>() { "BookId", "AuthorId" } };
        yield return new object[] { i++, new List<string>() { "Id" } };
        yield return new object[] { i++, new List<string>() { "Id" } };
        yield return new object[] { i++, new List<string>() { "BooksId", "CategoriesName" } };
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    [InlineData(2, 1)]
    [InlineData(3, 2)]
    [InlineData(4, 1)]
    [InlineData(5, 1)]
    [InlineData(6, 2)]
    public void NumberOfPrimaryKeysAreCorrect(int idx, int count)
    {
        Assert.Equal(count, entities[idx].Attributes.Count(attr => attr.IsPrimaryKey));
    }

    [Theory]
    [InlineData(3, "BookId", "Id", "Books")]
    [InlineData(3, "AuthorId", "Id", "Author")]
    [InlineData(4, "BookId", "Id", "Books")]
    [InlineData(5, "BookId", "Id", "Books")]
    [InlineData(6, "BooksId", "Id", "Books")]
    [InlineData(6, "CategoriesName", "Name", "Categories")]
    public void ForeignKeyAddedToAttributes(int idx, string attributeName, string targetAttributeName, string targetTableName)
    {
        Entity entity = entities[idx]; // BookAuthor table
        ForeignKey fk = entity.Attributes.Single(attr => attr.Name.Equals(attributeName)).ForeignKey!;
        Assert.Equal(targetAttributeName, fk.TargetAttributeName);
        Assert.Equal(targetTableName, fk.TargetTableName);
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