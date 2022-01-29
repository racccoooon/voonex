using FluentBlazorRouter;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using voonex.web;
using voonex.web.Pages;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMudServices();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddFluentRouting(rootBuilder =>
{
    rootBuilder.WithPage<Dashboard>("dashboard");
    rootBuilder.WithGroup("server", serverBuilder =>
    {
        serverBuilder.WithPage<CreateServer>("create");
    });
});

await builder.Build().RunAsync();