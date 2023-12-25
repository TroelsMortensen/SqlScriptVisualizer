using Blazor.Data;
using Blazor.Data.Models;
using Blazor.ViewModels;

namespace UnitTests.Data;

public class SomeWierdErrorReproductionTest
{
    [Fact]
    public void GeneratingDataTwiceDoesNotDoubleEntityCount()
    {
        SqliteParser parser = new();
        EntityPlacementOrganizer organizer = new();
        EntityManager em = new(parser, organizer);
        List<EntityViewModel> entityViewModels = em.GenerateData(SqliteScriptTestData.TwoTables);

        Assert.Equal(2, entityViewModels.Count);
        
        List<EntityViewModel> entityViewModels2 = em.GenerateData(SqliteScriptTestData.TwoTables);

        Assert.Equal(2, entityViewModels2.Count);
        
        // List<Entity> entities = parser.SqlScriptToEntities(SqliteScriptTestData.TwoTables);
        // placements = organizer.CalculateRelativePlacements(entities);
    }
}