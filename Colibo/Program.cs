using Colibo.Components;
using Colibo.Data.AZURE_Context;
using Colibo.Data.XML_Context;
using Colibo.Service.Json_Service;
using Colibo.Service.MergedData_Service;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<IContext_Xml, Xml_Context>();
builder.Services.AddSingleton<IContext_Azure, AzureAD_Context>();
builder.Services.AddSingleton<IMergedData_Service, MergedData_Service>();
builder.Services.AddSingleton<IJsonData_Service, JsonData_Service>();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();


Log.Information("Starting web application");

builder.Host.UseSerilog(); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error", createScopeForErrors: true);
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();


app.Run();
