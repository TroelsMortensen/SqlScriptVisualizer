namespace Blazor.ViewModels;

public class FkLink
{
    public (int x, int y) SourcePoint => (relativeSourcePoint.x + source.Xstart, relativeSourcePoint.y + source.Ystart);
    public (int x, int y) TargetPoint => (relativeTargetPoint.x + target.Xstart, relativeTargetPoint.y + target.Ystart);
    
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