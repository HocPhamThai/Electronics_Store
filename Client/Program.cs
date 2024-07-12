global using System.Net.Http.Json;

global using Electronics_Store.Shared;
global using Electronics_Store.Shared.Responder;
global using Electronics_Store.Shared.User;

global using Electronics_Store.Client;
global using Electronics_Store.Client.Services.Service4Categories_Client;
global using Electronics_Store.Client.Services.Service4Products_Client;
global using Electronics_Store.Client.Services.Service4Authentication_Client;
global using Electronics_Store.Client.Services.Service4Cart_Client;
global using Electronics_Store.Client.Services.Service4Order_Client;
global using Electronics_Store.Client.Authentication;
global using Electronics_Store.Client.Services.Service4Addresses_Client;

global using Microsoft.AspNetCore.Components.Authorization;
global using Microsoft.AspNetCore.Components;

global using MudBlazor.Services;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using Blazored.LocalStorage;
using Electronics_Store.Client.Services.Service4ProductVarieties_Client;
using Syncfusion.Blazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
        builder.Services.AddScoped<IProductsService, ProductsService>();
        builder.Services.AddScoped<ICategoriesService, CategoriesService>();
        builder.Services.AddScoped<ICartService, CartService>();
        builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddScoped<IAddressService, AddressService>();
        builder.Services.AddScoped<IProductVarietyService, ProductVarietyService>();
    builder.Services.AddBlazoredLocalStorage();
    builder.Services.AddOptions();
    builder.Services.AddAuthorizationCore();
        builder.Services.AddScoped<AuthenticationStateProvider, CustomizedAuthenticationStateProvider>();
    builder.Services.AddMudServices();
    builder.Services.AddSyncfusionBlazor();
await builder.Build().RunAsync();