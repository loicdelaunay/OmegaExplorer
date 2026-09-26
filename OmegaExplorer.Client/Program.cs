#region

using Blazor.SubtleCrypto;
using Blazored.LocalStorage;
using BlazorStyled;
using Howler.Blazor.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Extensions;
using MudBlazor.Services;
using MudExtensions.Services;
using OmegaExplorer.Client;
using OmegaExplorer.Client.Components.Maps;
using OmegaExplorer.Client.Components.Maps.Drivers;
using OmegaExplorer.Client.Managers;
using OmegaExplorer.Client.Providers.Hubs;
using OmegaExplorer.Client.Services;
using OmegaExplorer.Client.Services.UserPreference;
using OmegaExplorer.Client.Utilities.API;
using OmegaExplorer.Client.Utilities.Configuration;
using Serilog;
using Serilog.Events;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using Toolbelt.Blazor.I18nText;

#endregion

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);

// Configurer Serilog
Log.Logger = new LoggerConfiguration()
             .MinimumLevel.Information()
             .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
             .Enrich.WithProperty("ApplicationName", "OmegaExplorer.Client")
             .WriteTo.BrowserConsole()
             .CreateLogger();

// Add SERILOG to the Service Container
builder.Logging.AddSerilog(dispose: true);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Configuration.GetSection(ConfigurationApplication.API).Bind(ConfigurationApplication.ConfigurationApi);

builder.Services.AddI18nText(
                             options => options.PersistenceLevel = PersistanceLevel.Cookie);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<JavascriptProvider>();
builder.Services.AddScoped<IUserPreference, UserPreference>();
builder.Services.AddScoped<IDynamicMapMemory, DynamicMapMemory>();

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});
builder.Services.AddBlazorStyled();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddSubtleCrypto(opt =>
                                     opt.Key = ConfigurationApplication.ConfigurationApi.Secret //Use another key
                                );

//https://www.mudex.org/readme#installation
builder.Services.AddMudServicesWithExtensions();

//https://codebeam-mudextensions.pages.dev/
builder.Services.AddMudExtensions();

//Enable modules
builder.Services.AddMudBlazorScrollManager();
builder.Services.AddScrollManagerExtended();

builder.Services.AddScoped<IHowl, Howl>();
builder.Services.AddScoped<IHowlGlobal, HowlGlobal>();

DynamicCardStackDriverManager.Initialize();
await DynamicMapDriverManager.InitializeAsync();

Console.WriteLine($"[OmegaExplorer] : Connecting to API at {ConfigurationApplication.ConfigurationApi.Url}");
await ApiManager.Initialize(ConfigurationApplication.ConfigurationApi.Url);

// Add cycle hub websocket connectivity
builder.Services.AddSingleton<CycleHubProvider>();
builder.Services.AddSingleton<CycleProcessHubProvider>();
builder.Services.AddSingleton<NotificationHubProvider>();

await builder.Build().RunAsync();