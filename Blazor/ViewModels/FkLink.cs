namespace Blazor.ViewModels;

public class FkLink
{
    public required EntityViewModel Source { get; set; }
    public required EntityViewModel Target { get; set; }
    public required (int x, int y) SourcePoint { get; set; }
    public required (int x, int y) TargetPoint { get; set; }
}