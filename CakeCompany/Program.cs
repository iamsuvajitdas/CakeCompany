// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using CakeCompany.Service.Configuration;
using CakeCompany.Service;


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information, rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("Starting service...");

    var builder = Host.CreateDefaultBuilder(args);
    builder.UseSerilog();

    builder.ConfigureServices(services => services.AddServices());

    var app = builder.Build();
    app.Services.GetService<Executor>().Execute();
}
catch(Exception ex)
{
    Log.Fatal(ex, "Exception occured in application building");
}
finally
{
    Log.Information("Exiting service...");
}
