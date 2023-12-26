using Blazor.Data.Models;
using Blazor.ViewModels;

namespace Blazor.Data;

public class EntityManager(SqliteParser parser, EntityPlacementOrganizer organizer)
{

    public List<EntityViewModel> GenerateData(string script)
    {
        List<Entity> entities = parser.SqlScriptToEntities(script);

        List<List<Entity>> placements = organizer.CalculateRelativePlacements(entities);

        List<EntityViewModel> result = ConvertToViewModels(placements);
        return result;
    }

    private List<EntityViewModel> ConvertToViewModels(List<List<Entity>> placements)
    {
        int x = 0;
        List<EntityViewModel> result = new();
        foreach (List<Entity> column in placements)
        {
            int y = 0;
            foreach (Entity entity in column)
            {
                EntityViewModel evm = ConvertEntity(entity, x, y);
                result.Add(evm);
                y = UpdateYCoordinate(y, entity);
            }

            x += Constants.EntitySpacingX + Constants.EntityBoxWidth;
        }

        return result;
    }

    private static int UpdateYCoordinate(int y, Entity entity)
    {
        y += Constants.EntityHeaderHeight;
        y += entity.Attributes.Count * Constants.EntityAttributeHeight;
        y += Constants.EntitySpacingY;
        return y;
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