using NLog;
using NLog.Web;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
using Xacte.ApiGateway.Aggregators;
using Xacte.Common.Hosting.Api.Extensions;

// NLog: Setup logger
var logger = LogManager.Setup()
                .LoadConfigurationFromAppSettings()
                .GetCurrentClassLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.UseXacteLogger();

    // Add ocelot
    var environment = builder.Environment.EnvironmentName;
    builder.Configuration.AddConfiguration(new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{environment}.json", optional: true)
        .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"ocelot.{environment}.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables().Build());

    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddOcelot(builder.Configuration)
        .AddCacheManager(x =>
        {
            x.WithDictionaryHandle();
        })
        .AddPolly()
        .AddSingletonDefinedAggregator<PatientAggregator>();

    builder.Services.AddXacteResponseCompression();
    builder.Services.AddXacteJsonSerializerOptions();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment() || app.Environment.IsLocal())
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseXacteResponseCompression();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.UseOcelot();

    app.Run();
}
catch (Exception exception)
{
    // NLog: Catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // NLog: Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    LogManager.Shutdown();
}