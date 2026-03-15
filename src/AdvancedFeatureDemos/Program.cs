// See https://aka.ms/new-console-template for more information

//Setup Serilog for outputting log items
//Build Config
using AdvancedFeatureDemos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

//Configure logger
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

var connection = configuration.GetConnectionString("DefaultConnection");

//Setup DI
var host = new HostBuilder()
    .UseSerilog()
    .ConfigureServices(services =>
    {
        services.AddTransient<IDemoEngine, DemoEngine>();
        services.AddTransient<IDemoRunner, DemoRunner>();
        services.AddDbContextPool<DemoDbContext>(options =>
            options.UseSqlServer(connection));
    })
    .Build();
//var builder = Host.CreateApplicationBu

using var scope = host.Services.CreateScope();

try
{
    Log.Logger.Information("Starting Demo Runner");
    var runner = scope.ServiceProvider.GetRequiredService<IDemoRunner>();
    await runner.RunDemoAsync();
    Log.Logger.Information("Demo Runner Exited");
}
catch (Exception ex)
{
    Log.Logger.Error(ex, "Unknown error");
}
finally
{
    Log.CloseAndFlush();
}