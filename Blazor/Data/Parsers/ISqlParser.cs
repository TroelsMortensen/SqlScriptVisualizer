using Blazor.Data.Models;

namespace Blazor.Data.Parsers;

public interface ISqlParser
{
    List<Entity> SqlScriptToEntities(string sql);
}