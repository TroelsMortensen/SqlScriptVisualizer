using Blazor.Data.Models;

namespace Blazor.Data;

public class EntityPlacementOrganizer
{
    public List<List<Entity>> CalculateRelativePlacements(List<Entity> entities)
    {
        List<Entity> unplacedEntities = entities.ToList();
        List<List<Entity>> placedEntities = new();

        PlaceAllRootEntities(unplacedEntities, placedEntities);

        while (unplacedEntities.Count != 0)
        {
            Entity entityToPlace = FindNextEntityToPlace(unplacedEntities, placedEntities);

            List<string> namesOfFkTargetTables = GetNamesOfFkTargetTables(entityToPlace);
            int maxIdx = CalculatePlacementIndex(placedEntities, namesOfFkTargetTables);

            EnsureListExists(placedEntities, maxIdx);

            TransferEntity(placedEntities, maxIdx, entityToPlace, unplacedEntities);
        }

        return placedEntities;
    }

    private static void PlaceAllRootEntities(List<Entity> unplaced, List<List<Entity>> placed)
    {
        List<Entity> entitiesWithNoFks = FindEntitiesWithNoFks(unplaced);
        unplaced.RemoveAll(ent => entitiesWithNoFks.Contains(ent));
        placed.Add(new());
        placed[0].AddRange(entitiesWithNoFks);
    }

    private static List<Entity> FindEntitiesWithNoFks(List<Entity> unplaced)
        => unplaced.Where(ent => ent.Attributes.All(attr => attr.ForeignKey == null)).ToList();


    private static void TransferEntity(List<List<Entity>> placed, int maxIdx, Entity entityToPlace, List<Entity> unplaced)
    {
        placed[maxIdx].Add(entityToPlace);
        unplaced.Remove(entityToPlace);
    }

    private static void EnsureListExists(List<List<Entity>> placed, int maxIdx)
    {
        if (placed.Count <= maxIdx)
        {
            placed.Add(new());
        }
    }

    private static int CalculatePlacementIndex(List<List<Entity>> placed, List<string> namesOfFkTargetTables)
    {
        int maxIdx = 0;
        foreach (List<Entity> list in placed)
        {
            if (ColumnListContainsTargetEntity(namesOfFkTargetTables, list))
            {
                maxIdx++;
            }
            else break;
        }

        return maxIdx;
    }

    private static bool ColumnListContainsTargetEntity(IEnumerable<string> namesOfFkTargetTables, List<Entity> list)
        => list.Any(ent => namesOfFkTargetTables.Contains(ent.Name));


    private static List<string> GetNamesOfFkTargetTables(Entity entityToPlace)
        => entityToPlace
            .Attributes
            .Where(attr => attr.ForeignKey != null)
            .Select(attr => attr.ForeignKey!.TargetTableName).ToList();

    private static Entity FindNextEntityToPlace(List<Entity> unplaced, List<List<Entity>> placed)
    {
        foreach (Entity entity in unplaced)
        {
            List<string> foreignKeyNames = GetAttributeNamesOfAllForeignKeysOfEntity(entity);

            List<string> namesOfPlacedTables = GetNamesOfPlacedTables(placed);

            bool allFkTargetsArePlaced = AllTargetTablesArePlaced(foreignKeyNames, namesOfPlacedTables);

            if (allFkTargetsArePlaced)
            {
                return entity;
            }
        }

        throw new Exception("There must always be a next entity to place!");
    }

    private static bool AllTargetTablesArePlaced(List<string> foreignKeyNames, List<string> namesOfPlacedTables)
        => foreignKeyNames.All(namesOfPlacedTables.Contains);

    private static List<string> GetNamesOfPlacedTables(List<List<Entity>> placed)
        => placed
            .SelectMany(list => list)
            .Select(ent => ent.Name)
            .ToList();

    private static List<string> GetAttributeNamesOfAllForeignKeysOfEntity(Entity entity)
        => entity
            .Attributes
            .Where(attr => attr.ForeignKey != null)
            .Select(attr => attr.ForeignKey!.TargetTableName)
            .ToList();
}