using Blazor.Data.Parsers;

namespace UnitTests;

public class SqlParserFactory
{
    public static ISqlParser GetParser(string parser)
    {
        return new SqliteParser();
    }
}