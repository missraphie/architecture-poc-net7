using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Xacte.Common.Hosting.Api.Middlewares;

namespace Xacte.Common.Hosting.Api.Extensions
{
    /// <summary>
    /// Application builder extension methods
    /// </summary>
    public static class ApplicationBuilderExtensions
    {

        /// <summary>
        /// Adds custom error handling middleware to <see cref="IApplicationBuilder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/> to add services to.</param>
        /// <returns>The <see cref="IApplicationBuilder"/> so that additional calls can be chained.</returns>
        public static IApplicationBuilder UseXacteErrorHandling(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<XacteErrorHandlingMiddleware>();
            return builder;
        }

        /// <summary>
        /// Adds request localization to <see cref="IApplicationBuilder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/> to add services to.</param>
        /// <returns>The <see cref="IApplicationBuilder"/> so that additional calls can be chained.</returns>
        /// <remarks>Currently supported languages are fr-CA and en-CA, defaults to fr-CA</remarks>
        public static IApplicationBuilder UseXacteRequestLocalization(this IApplicationBuilder builder)
        {
            var supportedCultures = new[] { "fr-CA", "en-CA" };
            var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);
            builder.UseRequestLocalization(localizationOptions);
            return builder;
        }

        /// <summary>
        /// Adds response compression to <see cref="IApplicationBuilder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/> to add services to.</param>
        /// <returns>The <see cref="IApplicationBuilder"/> so that additional calls can be chained.</returns>
        public static IApplicationBuilder UseXacteResponseCompression(this IApplicationBuilder builder)
        {
            builder.UseResponseCompression();
            return builder;
        }

        /// <summary>
        /// Adds secure connection protocols to <see cref="WebApplication"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/> to add services to.</param>
        /// <returns>The <see cref="IApplicationBuilder"/> so that additional calls can be chained.</returns>
        public static IApplicationBuilder UseXacteSecureConnection(this WebApplication builder)
        {
            if (!builder.Environment.IsLocal())
            {
                builder.UseHttpsRedirection();
                builder.UseHsts();
            }
            return builder;
        }

        /// <summary>
        /// Adds Swagger to <see cref="IApplicationBuilder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/> to add services to.</param>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> to add services to.</param>
        /// <returns>The <see cref="IApplicationBuilder"/> so that additional calls can be chained.</returns>
        public static IApplicationBuilder UseXacteSwagger(this IApplicationBuilder builder, IServiceProvider serviceProvider)
        {
            var apiVersionDescriptionProvider = serviceProvider.GetRequiredService<IApiVersionDescriptionProvider>();

            builder.UseSwagger();
            builder.UseSwaggerUI(options =>
            {
                // build a swagger endpoint for each discovered API version
                foreach (var groupName in apiVersionDescriptionProvider.ApiVersionDescriptions.Select(s => s.GroupName))
                {
                    options.SwaggerEndpoint($"/swagger/{groupName}/swagger.json", groupName.ToUpperInvariant());
                }
            });
            return builder;
        }
    }
}
