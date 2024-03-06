namespace UnitTests.Data;

public static class SqliteScriptTestData
{
    public const string TwoTables = @"CREATE TABLE ""TvShows"" (
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

// ###########################

    public const string SixTables = @"CREATE TABLE ""Author"" (
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

// ###########################
    
    public static string EventScript = @"CREATE TABLE ""Guest"" (
    ""Id"" TEXT NOT NULL CONSTRAINT ""PK_Guest"" PRIMARY KEY    
);


CREATE TABLE ""VeaEvent"" (
    ""Id"" TEXT NOT NULL CONSTRAINT ""PK_VeaEvent"" PRIMARY KEY,
    ""MaxGuestNumber"" INTEGER NOT NULL,
    ""Visibility"" TEXT NOT NULL,
    ""Description"" TEXT NOT NULL,
    ""Status"" TEXT NOT NULL,
    ""EndTime"" TEXT NOT NULL,
    ""StartTime"" TEXT NOT NULL,
    ""Title"" TEXT NOT NULL
);


CREATE TABLE ""GuestFk"" (
    ""GuestFk"" TEXT NOT NULL,
    ""EventFk"" TEXT NOT NULL,
    CONSTRAINT ""PK_GuestFk"" PRIMARY KEY (""GuestFk"", ""EventFk""),
    CONSTRAINT ""FK_GuestFk_Guest_GuestFk"" FOREIGN KEY (""GuestFk"") REFERENCES ""Guest"" (""Id"") ON DELETE CASCADE,
    CONSTRAINT ""FK_GuestFk_VeaEvent_EventFk"" FOREIGN KEY (""EventFk"") REFERENCES ""VeaEvent"" (""Id"") ON DELETE CASCADE
);


CREATE TABLE ""Invitation"" (
    ""GuestFk"" TEXT NOT NULL,
    ""EventFk"" TEXT NOT NULL,
    ""status"" TEXT NOT NULL,
    CONSTRAINT ""PK_Invitation"" PRIMARY KEY (""GuestFk"", ""EventFk""),
    CONSTRAINT ""FK_Invitation_Guest_GuestFk"" FOREIGN KEY (""GuestFk"") REFERENCES ""Guest"" (""Id"") ON DELETE CASCADE,
    CONSTRAINT ""FK_Invitation_VeaEvent_EventFk"" FOREIGN KEY (""EventFk"") REFERENCES ""VeaEvent"" (""Id"") ON DELETE CASCADE
);
";
}