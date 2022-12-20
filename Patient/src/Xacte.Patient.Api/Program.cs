using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using Xacte.Common.Hosting.Api.Extensions;
using Xacte.Patient.Business.Services;
using Xacte.Patient.Business.Services.Interfaces;
using Xacte.Patient.Data.Contexts;
using Xacte.Patient.Data.Repositories;
using Xacte.Patient.Data.Repositories.Interfaces;

// NLog: Setup logger
var logger = LogManager.Setup()
                .LoadConfigurationFromAppSettings()
                .GetCurrentClassLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.UseXacteLogger();

    // Add services to the container.
    builder.Services.AddControllers()
        .AddXacteJsonOptions()
        .AddXacteMvcActionFilters();
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    builder.Services.AddHealthChecks();
    builder.Services.AddLocalization();

    builder.Services.AddXacteCoreServices();
    builder.Services.AddXacteVersioning();
    builder.Services.AddXacteResponseCompression();
    builder.Services.AddXacteJsonSerializerOptions();
    builder.Services.AddXacteSwagger(typeof(Program), title: "Patient", description: "Patient management API");

    builder.Services.AddDbContext<PatientDbContext>(options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));

    // DI configurations - business layer
    builder.Services.AddScoped<IPatientService, PatientService>();

    // DI configurations - data layer
    builder.Services.AddScoped<IPatientRepository, PatientRepository>();

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

    app.UseHttpsRedirection();
    app.UseHsts();

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