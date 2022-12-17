using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Xacte.Common.Hosting.Api.Extensions
{
    /// <summary>
    /// WebApplicationBuilder extension methods
    /// </summary>
    public static class WebApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds a default implementation of NLog to the <see cref="WebApplicationBuilder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="WebApplicationBuilder"/> to add logging to.</param>
        /// <returns>The <see cref="WebApplicationBuilder"/> so that additional calls can be chained.</returns>
        public static WebApplicationBuilder UseXacteLogger(this WebApplicationBuilder builder)
        {
            builder.Logging.ClearProviders();
            builder.Host.UseNLog();
            return builder;
        }
    }
}
