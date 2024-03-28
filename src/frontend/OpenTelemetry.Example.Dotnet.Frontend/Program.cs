using OpenTelemetry.Example.Dotnet.Frontend.Extensions;
using OpenTelemetry.Example.Dotnet.Frontend.Models;
using OpenTelemetry.Example.Dotnet.Frontend.Services.EventCatalog;
using OpenTelemetry.Example.Dotnet.Frontend.Services.Ordering;
using OpenTelemetry.Example.Dotnet.Frontend.Services.Ordering.InMemory;
using OpenTelemetry.Example.Dotnet.Frontend.Services.ShoppingBasket;

bool isTesting = false;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTelemetry(builder.Configuration, builder.Logging);
// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddSingleton<Settings>();
builder.Services.AddSingleton<IShoppingBasketService, InMemoryShoppingBasketService>();


// Wersja bez po³¹czenia z innymi serwisami

if (isTesting)
{
    builder.Services.AddSingleton<IEventCatalogService, InMemoryEventCatalogService>();
    builder.Services.AddSingleton<IOrderSubmissionService, InMemoryOrderSubmissionService>();
}
else
{
    builder.Services.AddSingleton<IEventCatalogService, EventCatalogService>();
    builder.Services.AddScoped<IOrderSubmissionService, OrderSubmissionService>();
    //builder.Services.AddHttpClient("eventCatalog", config => { config.BaseAddress = new Uri("http://localhost:8002"); });
    //builder.Services.AddHttpClient("ordering", config => { config.BaseAddress = new Uri("http://localhost:8003"); });
    builder.Services.AddHttpClients(builder.Configuration);
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
