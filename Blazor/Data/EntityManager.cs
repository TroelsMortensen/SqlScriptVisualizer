using System.Reflection.Metadata;
using Blazor.Data.Models;
using Blazor.ViewModels;
using Attribute = Blazor.Data.Models.Attribute;

namespace Blazor.Data;

public class EntityManager(SqliteParser parser)
{
    public List<EntityViewModel> Entities { get; set; } = new();
    public List<FkLink> FkLinks { get; set; } = new();

    public void GenerateData(string script)
    {
        List<Entity> entities = parser.SqlScriptToEntities(script);

        List<List<Entity>> placements = new EntityPlacementOrganizer().CalculateRelativePlacements(entities);

        Entities = new EntityToVmConverter().Convert(placements);

        FkLinks = new FkLinkBuilder().Build(Entities);
    }
}