using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using Xacte.Common.Hosting.Api.Extensions;
using Xacte.Patient.Business;
using Xacte.Patient.Data;
using Xacte.Patient.Data.Contexts;

// NLog: Setup logger
var logger = LogManager.Setup()
                .LoadConfigurationFromAppSettings()
                .GetCurrentClassLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.UseXacteLogger();

    // Add services to the container.
    builder.Services
        .AddControllers()
            .AddXacteJsonOptions()
            .AddXacteMvcActionFilters();

    builder.Services
        .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
        .AddLocalization()
        .AddDbContext<PatientContext>(options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));
    builder.Services.AddHealthChecks();

    builder.Services
        .AddXacteCoreServices()
        .AddXacteVersioning()
        .AddXacteResponseCompression()
        .AddXacteJsonSerializerOptions()
        .AddXacteSwagger(typeof(Program), title: "Patient", description: "Patient management API");

    // DI configurations - business layer + data layer
    builder.Services
        .AddBusinessServices()
        .AddDataRepositories();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment() || app.Environment.IsLocal())
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseXacteErrorHandling();
    app.UseXacteSwagger(app.Services);
    app.UseXacteRequestLocalization();
    app.UseXacteResponseCompression();
    app.UseXacteSecureConnection();

    app.UseAuthorization();

    app.MapControllers();
    app.MapHealthChecks("/health");

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