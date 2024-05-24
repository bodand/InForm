using InForm.Web;
using InForm.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
var config = builder.Configuration;

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddInFormServer((_, httpClient) =>
{
	Console.WriteLine(config["InFormServer:Url"]);
	httpClient.BaseAddress = new(config["InFormServer:Url"]
								 ?? builder.HostEnvironment.BaseAddress);
});

await builder.Build().RunAsync();