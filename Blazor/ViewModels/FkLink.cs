namespace Blazor.ViewModels;

public class FkLink
{
    public (int x, int y) SourcePoint => (relativeSourcePoint.x + source.X, relativeSourcePoint.y + source.Y);
    public (int x, int y) TargetPoint => (relativeTargetPoint.x + target.X, relativeTargetPoint.y + target.Y);
    
    private readonly EntityViewModel source;
    private readonly EntityViewModel target;
    private readonly (int x, int y) relativeSourcePoint;
    private readonly (int x, int y) relativeTargetPoint;

    public FkLink(EntityViewModel source, EntityViewModel target, (int x, int y) relativeSourcePoint, (int x, int y) relativeTargetPoint)
    {
        this.source = source;
        this.target = target;
        this.relativeSourcePoint = relativeSourcePoint;
        this.relativeTargetPoint = relativeTargetPoint;
    }
}