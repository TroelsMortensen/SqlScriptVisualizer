using System.Text.RegularExpressions;
using Blazor.Data.Models;
using Attribute = Blazor.Data.Models.Attribute;

namespace Blazor.Data.Parsers;

public abstract class BaseParser : ISqlParser
{
    private bool isInTable = false;
    List<Entity> result = new();
    Entity entity = null!;

    public List<Entity> SqlScriptToEntities(string sql)
    {
        ResetState();
        string[] strings = sql.Split("\n");
        foreach (var line in strings)
        {
            HandleEndOfTable(line);

            HandleAttribute(line);

            AddForeignKey(line);

            AddCompositePrimaryKey(line);

            HandleStartOfTable(line);
        }

        return result;
    }

    private void ResetState()
    {
        isInTable = false;
        result = new();
        entity = null!;
    }

    private void AddCompositePrimaryKey(string line)
    {
        if (IsNotPrimaryKeyConstraintDefinition_DialectSpecific(line))
        {
            return;
        }

        line = ExtractCommaSeparatedListOfAttributeNames_DialectSpecific(line);
        IEnumerable<string> pkAttrNames = TrimAllNames(line);
        foreach (string name in pkAttrNames)
        {
            entity.Attributes
                .Single(attr => attr.Name.Equals(name))
                .IsPrimaryKey = true;
        }
    }


    private static IEnumerable<string> TrimAllNames(string line)
        => line.Split(",").Select(s => s.Trim());


    private void AddForeignKey(string line)
    {
        if (IsNotForeignKeyLine_DialectSpecific(line))
        {
            return;
        }

        string attrName = ExtractFkAttributeName_DialectSpecific(line);
        ForeignKey fk = CreateForeignKey(line);
        AddForeignKeyToAttribute(attrName, fk);
    }


    private void AddForeignKeyToAttribute(string attrName, ForeignKey fk)
        => entity.Attributes.Single(attr => attr.Name.Equals(attrName)).ForeignKey = fk;


    private ForeignKey CreateForeignKey(string line)
    {
        (string Table, string Attr) data = ExtractTargetTableAndAttribute_DialectSpecific(line);
        ForeignKey fk = new()
        {
            TargetAttributeName = data.Attr,
            TargetTableName = data.Table
        };
        return fk;
    }


    private void HandleStartOfTable(string line)
    {
        if (!IsStartOfTable_DialectSpecific(line))
        {
            return;
        }

        isInTable = true;
        entity = new();
        entity.Name = GetEntityName_DialectSpecific(line);
    }

    private void HandleAttribute(string line)
    {
        if (!IsAttributeLine(line))
        {
            return;
        }

        Attribute attr = new()
        {
            Name = ExtractAttributeName_DialectSpecific(line),
            IsPrimaryKey = ComputeIsPrimaryKey_DialectSpecific(line)
        };
        entity.Attributes.Add(attr);
    }


    private void HandleEndOfTable(string line)
    {
        if (!IsEndOfTable(line))
        {
            return;
        }

        isInTable = false;
        result.Add(entity);
        entity = null!;
    }

    private bool IsAttributeLine(string line)
        => isInTable && IsAttributeLine_DialectSpecific(line);

    private bool IsEndOfTable(string line)
        => isInTable && IsEndOfTable_DialectSpecific(line);

    private bool IsStartOfTable_DialectSpecific(string line)
        => IsCreateTableDefinition_DialectSpecific(line) && !isInTable;

    protected abstract bool ComputeIsPrimaryKey_DialectSpecific(string line);

    protected abstract bool IsAttributeLine_DialectSpecific(string line);


    protected abstract string ExtractAttributeName_DialectSpecific(string line);

    protected abstract string GetEntityName_DialectSpecific(string line);


    protected abstract bool IsEndOfTable_DialectSpecific(string line);


    protected abstract bool IsCreateTableDefinition_DialectSpecific(string line);

    protected abstract (string Table, string Attr) ExtractTargetTableAndAttribute_DialectSpecific(string line);

    protected abstract bool IsNotForeignKeyLine_DialectSpecific(string line);

    protected abstract string ExtractFkAttributeName_DialectSpecific(string line);

    protected abstract string ExtractCommaSeparatedListOfAttributeNames_DialectSpecific(string line);

    protected abstract bool IsNotPrimaryKeyConstraintDefinition_DialectSpecific(string line);
}