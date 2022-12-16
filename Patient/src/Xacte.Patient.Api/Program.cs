using Microsoft.EntityFrameworkCore;
using Xacte.Common.Hosting.Api.Extensions;
using Xacte.Patient.Data.Contexts;

namespace Xacte.Patient.Api
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers()
                .AddXacteJsonOptions();
            builder.Services.AddHealthChecks();

            builder.Services.AddXacteCoreServices();
            builder.Services.AddXacteVersioning();
            builder.Services.AddXacteResponseCompression();
            builder.Services.AddXacteJsonSerializerOptions();
            builder.Services.AddXacteSwagger(typeof(Program), title: "Patient", description: "Patient management API");

            builder.Services.AddDbContext<PatientDbContext>(options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment() || app.Environment.IsLocal())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseXacteSwagger(app.Services);
            app.UseXacteResponseCompression();

            app.UseHttpsRedirection();
            app.UseHsts();

            app.UseAuthorization();

            app.MapControllers();
            app.MapHealthChecks("/health");

            app.Run();
        }
    }
}
