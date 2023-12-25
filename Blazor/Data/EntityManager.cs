using Blazor.Data.Models;
using Blazor.ViewModels;

namespace Blazor.Data;

public class EntityManager(SqliteParser parser, EntityPlacementOrganizer organizer)
{
    public List<EntityViewModel> Entities { get; set; } = new();


    public void GenerateData(string script)
    {
        List<Entity> entities = parser.SqlScriptToEntities(script);

        List<List<Entity>> placements = organizer.CalculateRelativePlacements(entities);

        Entities = ConvertToViewModels(placements);
    }

    private List<EntityViewModel> ConvertToViewModels(List<List<Entity>> placements)
    {
        throw new NotImplementedException();
    }

    

    
}