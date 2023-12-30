using Blazor.Data.Models;
using Blazor.ViewModels;

namespace Blazor.Data;

public class EntityToVmConverter
{
    public List<EntityViewModel> Convert(List<List<Entity>> placements)
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
        => y + Constants.EntityHeaderHeight +
           entity.Attributes.Count * Constants.EntityAttributeHeight +
           Constants.EntitySpacingY;

    private static EntityViewModel ConvertEntity(Entity entity, int x, int y)
        => new()
        {
            Entity = entity,
            X = x + 15,
            Y = y + 15
        };
}