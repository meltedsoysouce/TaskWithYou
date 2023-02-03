using BlazorStrap;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TaskWithYou.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// BlazorStrap
builder.Services.AddBlazorStrap();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });


// ViewModel
//builder.Services.AddScoped<TaskWithYou.Client.ViewModels.Content.IViewModelFactory,
//                           TaskWithYou.Client.ViewModels.Content.ViewModelFactory>();

await builder.Build().RunAsync();
