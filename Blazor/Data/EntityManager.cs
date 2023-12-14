namespace Blazor.Data;

public class EntityManager
{
    public List<EntityData> Entities { get; set; } = new();

    public void AddEntity(EntityData entity)
    {
        Entities.Add(entity);
    }
}