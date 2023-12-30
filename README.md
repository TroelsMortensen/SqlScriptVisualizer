# Sql Script Visualizer

## Purpose:
This project is supposed to visualize the database generated by EFC without having to actually generate either migration or database.

It currently works only for SQLite.

You can use the following CLI command to generate the SQL script, which would create the database according to your DbContext:

`dotnet ef dbcontext script`

This command will output the script to the console. Copy "most of it", i.e. the create table statements, 
but leave the creation of indices, and inserting seed data. It may not work correctly, if these statements are included. I haven't tested properly.

Copy/paste the script into the text area in the web page, click the button, and see the diagram generated on the right.\
You can drag individual entities around by dragging the entity header. You can drag the entire diagram, by grabbing the canvas.


### Test data
As an example, below are two SQLite scripts, which you can try out.

This simple 2-table script.

```sql
CREATE TABLE "TvShows" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_TvShows" PRIMARY KEY AUTOINCREMENT,
    "Title" TEXT NOT NULL,
    "Year" INTEGER NOT NULL,
    "Genre" TEXT NOT NULL
);

CREATE TABLE "Episodes" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Episodes" PRIMARY KEY AUTOINCREMENT,
    "Title" TEXT NOT NULL,
    "Runtime" INTEGER NOT NULL,
    "SeasonId" TEXT NOT NULL,
    "TvShowId" INTEGER NOT NULL,
    CONSTRAINT "FK_Episodes_TvShows_TvShowId" FOREIGN KEY ("TvShowId") REFERENCES "TvShows" ("Id") ON DELETE CASCADE
);
```

Or a slightly more elaborate 7-table script:

```sql
CREATE TABLE "Author" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Author" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL
);


CREATE TABLE "Books" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Books" PRIMARY KEY AUTOINCREMENT,
    "Title" TEXT NOT NULL,
    "PublishDate" TEXT NOT NULL,
    "Price" TEXT NOT NULL,
    "Publisher" TEXT NOT NULL
);


CREATE TABLE "Categories" (
    "Name" TEXT NOT NULL CONSTRAINT "PK_Categories" PRIMARY KEY
);


CREATE TABLE "BookAuthor" (
    "BookId" INTEGER NOT NULL,
    "AuthorId" INTEGER NOT NULL,
    "Order" INTEGER NOT NULL,
    CONSTRAINT "PK_BookAuthor" PRIMARY KEY ("BookId", "AuthorId"),
    CONSTRAINT "FK_BookAuthor_Author_AuthorId" FOREIGN KEY ("AuthorId") REFERENCES "Author" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_BookAuthor_Books_BookId" FOREIGN KEY ("BookId") REFERENCES "Books" ("Id") ON DELETE CASCADE
);


CREATE TABLE "PriceOffers" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_PriceOffers" PRIMARY KEY AUTOINCREMENT,
    "PromotionalPrice" TEXT NOT NULL,
    "PromotionalText" TEXT NULL,
    "BookId" INTEGER NOT NULL,
    CONSTRAINT "FK_PriceOffers_Books_BookId" FOREIGN KEY ("BookId") REFERENCES "Books" ("Id") ON DELETE CASCADE
);


CREATE TABLE "Reviews" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Reviews" PRIMARY KEY AUTOINCREMENT,
    "Rating" INTEGER NOT NULL,
    "VoterName" TEXT NOT NULL,
    "Comment" TEXT NOT NULL,
    "BookId" INTEGER NOT NULL,
    CONSTRAINT "FK_Reviews_Books_BookId" FOREIGN KEY ("BookId") REFERENCES "Books" ("Id") ON DELETE CASCADE
);


CREATE TABLE "BookCategory" (
    "BooksId" INTEGER NOT NULL,
    "CategoriesName" TEXT NOT NULL,
    CONSTRAINT "PK_BookCategory" PRIMARY KEY ("BooksId", "CategoriesName"),
    CONSTRAINT "FK_BookCategory_Books_BooksId" FOREIGN KEY ("BooksId") REFERENCES "Books" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_BookCategory_Categories_CategoriesName" FOREIGN KEY ("CategoriesName") REFERENCES "Categories" ("Name") ON DELETE CASCADE
);
```

## Updates:

* Can now drag entire diagram, by grabbing the checkered background.
* Diagram is now shown, seemingly correctly.


#### Note about GitHub pages deployment

Hosting on GitHub pages was setup using [this source](https://swimburger.net/blog/dotnet/how-to-deploy-aspnet-blazor-webassembly-to-github-pages).

Together with this:

"
I had the same issue after creating another template from a working project. To fix it I had to change Workflow permission through Repo -> Settings -> Actions -> General Set: Read and write permissions Check: Allow Github Actions to create and approve pull requests
"

From [here](https://github.com/actions/checkout/issues/417), the comment from bhismafarhan.
