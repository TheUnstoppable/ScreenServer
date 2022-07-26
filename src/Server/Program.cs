global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using ScreenServer.Server.Managers;
global using ScreenServer.Server.Utils;
global using ScreenServer.Shared;
global using Serilog;
global using Serilog.Formatting.Json;
global using System.Net;
global using System.Security.Cryptography;
global using System.Text;
global using ILogger = Serilog.ILogger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(new JsonFormatter(), Path.Combine(builder.Environment.ContentRootPath, "Logs", "screenserver-log-.log"),
    fileSizeLimitBytes: 10 * 1024 * 1024,
    rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
builder.Services.AddSingleton(logger);
builder.Services.AddDbContext<ScreenServerDatabaseContext>(ServiceLifetime.Scoped);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
