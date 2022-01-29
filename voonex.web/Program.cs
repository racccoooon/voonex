using System.Net;
using FluentBlazorRouter;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using voonex.web;
using voonex.web.Pages;
using voonex.api.client;
using MudBlazor.Services;
using voonex.web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMudServices();

builder.Services.AddScoped(sp =>
{
    var httpClientHandler = new VoonexHttpClientHandler();
    var httpClient = new HttpClient(httpClientHandler)
    {
        //BaseAddress = new Uri(builder.HostEnvironment.BaseAddress),
    };
    return httpClient;
});

builder.Services.AddFluentRouting(rootBuilder =>
{
    rootBuilder.WithPage<Dashboard>("dashboard");
    rootBuilder.WithGroup("server", serverBuilder =>
    {
        serverBuilder.WithPage<CreateServer>("create");
    });
});

await builder.Build().RunAsync();