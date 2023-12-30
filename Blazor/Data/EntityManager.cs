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

        FkLinks = BuildLinks(Entities);
    }

    private List<FkLink> BuildLinks(List<EntityViewModel> entities)
    {
        List<FkLink> links = new();
        foreach (EntityViewModel entityVm in entities)
        {
            for (var index = 0; index < entityVm.Entity.Attributes.Count; index++)
            {
                Attribute attr = entityVm.Entity.Attributes[index];
                if (attr.ForeignKey == null) continue;

                int sourceY = CalcSourceY(index);
                int sourceX = 0;
                EntityViewModel targetVm = FindTargetEntityViewModel(entities, attr.ForeignKey!.TargetTableName);
                int targetX = Constants.EntityBoxWidth;
                int targetY = CalcTargetY(targetVm, attr.ForeignKey.TargetAttributeName);
                FkLink fkl = new(entityVm, targetVm, (sourceX, sourceY), (targetX, targetY));
                links.Add(fkl);
            }
        }

        return links;
    }

    private static int CalcSourceY(int index)
        => Constants.EntityHeaderHeight + (1 + index) * Constants.EntityAttributeHeight +
           Constants.EntityAttributeHeight / 4; // why magic 4? Thought it would be 2.

    private static int CalcTargetY(EntityViewModel targetVm, string fkAttr)
    {
        Attribute attribute = targetVm.Entity.Attributes.Single(attr => attr.Name.Equals(fkAttr));
        int idx = targetVm.Entity.Attributes.IndexOf(attribute) + 1;
        return Constants.EntityHeaderHeight + idx * Constants.EntityAttributeHeight + Constants.EntityAttributeHeight / 4;
    }

    private static EntityViewModel FindTargetEntityViewModel(List<EntityViewModel> entities, string foreignKeyTargetTableName)
        => entities.Single(evm => evm.Entity.Name.Equals(foreignKeyTargetTableName));


    
}