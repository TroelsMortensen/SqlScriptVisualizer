using Blazor.Data.Models;
using Blazor.Data.Parsers;
using Blazor.ViewModels;

namespace Blazor.Data;

public class EntityManager(ISqlParser parser)
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