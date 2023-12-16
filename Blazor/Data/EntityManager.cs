using Blazor.ViewModels;

namespace Blazor.Data;

public class EntityManager(SqliteParser parser)
{
    public List<EntityViewModel> Entities { get; set; } = new();


    public void GenerateData(string script)
    {
        List<EntityViewModel> entityViewModels = parser.SqlScriptToEntities(script)
            .Select(ent => new EntityViewModel()
            {
                Entity = ent
            }).ToList();
        CalculatePlacements(entityViewModels);
    }
}