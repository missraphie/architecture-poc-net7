using Xacte.Common.Hosting.Api.Extensions;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
using Xacte.ApiGateway.Aggregators;

namespace Xacte.ApiGateway
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var env = builder.Environment.EnvironmentName;

            // Add ocelot
#pragma warning disable ASP0013 // Suggest switching from using Configure methods to WebApplicationBuilder.Configuration
            builder.Host.ConfigureHostConfiguration(hostConfig =>
            {
                hostConfig
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", true, true)
                   .AddJsonFile($"appsettings.{env}.json", true, true)
                   .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
                   .AddJsonFile($"ocelot.{env}.json", optional: false, reloadOnChange: true)
                   .AddEnvironmentVariables();
            });
#pragma warning restore ASP0013 // Suggest switching from using Configure methods to WebApplicationBuilder.Configuration

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
    }
}