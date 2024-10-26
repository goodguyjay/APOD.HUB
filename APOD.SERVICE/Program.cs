using APOD.SERVICE.Core;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.Configure<RouteOptions>(options =>
{
    options.SetParameterPolicy<RegexInlineRouteConstraint>("regex");
});

builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.SetMinimumLevel(LogLevel.Information);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "NASA APOD Wallpaper API", Version = "v1" });
});

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();

logger.LogInformation("Starting...");

var downloadFolder = ConfigurationReader.ReadSetting("DownloadFolder");

if (!Directory.Exists(downloadFolder))
{
    Directory.CreateDirectory(downloadFolder);
}

logger.LogInformation($"Images will be downloaded to: {downloadFolder}");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "NASA APOD Wallpaper API V1");
    });
}

app.MapGet("/", () => "NASA APOD Wallpaper API is running.");

app.Run();
