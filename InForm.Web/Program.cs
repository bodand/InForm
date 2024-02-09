using InForm.Web;
using InForm.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var config = builder.Configuration;

builder.Services.AddInFormServer((_, httpClient) =>
{
	httpClient.BaseAddress = new(config["InFormServer:Url"] 
								 ?? builder.HostEnvironment.BaseAddress);
});

await builder.Build().RunAsync();