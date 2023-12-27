using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazor;
using Blazor.Data;
using Blazor.Data.Models;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<EntityManager>();
builder.Services.AddScoped<SqliteParser>();
builder.Services.AddScoped<EntityPlacementOrganizer>();
builder.Services.AddScoped<RedrawEventPublisher>();

await builder.Build().RunAsync();