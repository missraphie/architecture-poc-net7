using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;

namespace Xacte.Common.Hosting.Api.Extensions
{
    /// <summary>
    /// Application builder extension methods
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds Swagger to <see cref="IApplicationBuilder"/>.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add services to.</param>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> to add services to.</param>
        /// <returns>The <see cref="IApplicationBuilder"/> so that additional calls can be chained.</returns>
        public static IApplicationBuilder UseXacteSwagger(this IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            var apiVersionDescriptionProvider = serviceProvider.GetRequiredService<IApiVersionDescriptionProvider>();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                // build a swagger endpoint for each discovered API version
                foreach (var groupName in apiVersionDescriptionProvider.ApiVersionDescriptions.Select(s => s.GroupName))
                {
                    options.SwaggerEndpoint($"/swagger/{groupName}/swagger.json", groupName.ToUpperInvariant());
                }
            });
            return app;
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
    }
}
