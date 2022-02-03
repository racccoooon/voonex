using System.Net;
using FluentBlazorRouter;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using voonex.web;
using voonex.web.Pages;
using voonex.api.client;
using MudBlazor.Services;
using voonex.eventhub;
using voonex.web.Services;
using voonex.web.Settings;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMudServices();
builder.Services.AddTransient<ClientFactory>();
builder.Services.AddOptions<VoonexApiSettings>().Bind(builder.Configuration.GetSection("ApiSettings"));
builder.Services.AddScoped(sp =>
{
    var httpClientHandler = new VoonexHttpClientHandler();
    var httpClient = new HttpClient(httpClientHandler)
    {
        //BaseAddress = new Uri(builder.HostEnvironment.BaseAddress),
    };
    return httpClient;
});
builder.Services.AddEventHub();
builder.Services.AddFluentRouting(rootBuilder =>
{
    rootBuilder.WithPage<Dashboard>("dashboard");
    rootBuilder.WithGroup("server", serverBuilder =>
    {
        serverBuilder.WithPage<CreateServer>("create");
        serverBuilder.WithPage<ServerPage>("{ServerId:guid}");
    });
});

await builder.Build().RunAsync();