using Blazor.Data.Models;
using Blazor.ViewModels;

namespace Blazor.Data;

public class EntityManager(SqliteParser parser, EntityPlacementOrganizer organizer)
{
    public List<EntityViewModel> Entities { get; set; } = new();

    private const int EntityHeaderHeight = 35;
    private const int EntityAttributeHeight = 25;
    private const int EntitySpacingX = 40;
    private const int EntitySpacingY = 150;
    

    public void GenerateData(string script)
    {
        List<Entity> entities = parser.SqlScriptToEntities(script);

        List<List<Entity>> placements = organizer.CalculateRelativePlacements(entities);

        Entities = ConvertToViewModels(placements);
    }

    private List<EntityViewModel> ConvertToViewModels(List<List<Entity>> placements)
    {
        int x = 0;
        int y = 0;
        List<EntityViewModel> result = new();
        foreach (List<Entity> column in placements)
        {
            foreach (Entity entity in column)
            {
                EntityViewModel evm = ConvertEntity(entity, x, y);
                result.Add(evm);
                x = UpdateXCoordinate(x, entity);
            }

            y += EntitySpacingY;
        }

        return result;
    }

    private static int UpdateXCoordinate(int x, Entity entity)
    {
        x += EntityHeaderHeight;
        x += entity.Attributes.Count * EntityAttributeHeight;
        x += EntitySpacingX;
        return x;
    }

    private static EntityViewModel ConvertEntity(Entity entity, int x, int y)
    {
        EntityViewModel evm = new()
        {
            Entity = entity,
            Xstart = x,
            Ystart = y
        };
        return evm;
    }
}