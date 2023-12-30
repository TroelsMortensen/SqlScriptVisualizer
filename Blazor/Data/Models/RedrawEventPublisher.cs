using Microsoft.AspNetCore.Components;

namespace Blazor.Data.Models;

public class RedrawEventPublisher
{
    public Action OnDragEntity { get; set; }
    public Action<(double x, double y)> OnMoveCanvas { get; set; }

    public void RaiseRedrawEvent()
    {
        OnDragEntity?.Invoke();
    }

    public void MoveCanves((double x, double y) diff)
    {
        OnMoveCanvas?.Invoke(diff);
    }
}