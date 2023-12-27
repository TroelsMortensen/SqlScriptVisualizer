using Microsoft.AspNetCore.Components;

namespace Blazor.Data.Models;

public class RedrawEventPublisher
{
    public Action OnRedraw { get; set; }

    public void RaiseRedrawEvent()
    {
        OnRedraw?.Invoke();
    }
}