using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

namespace Xacte.Common.Hosting.Api.Extensions
{
    /// <summary>
    /// Api controller extension methods
    /// </summary>
    public static class MvcBuilderExtensions
    {

        /// <summary>
        /// Adds default mvc controller serializer converter implementations for <see cref = "IMvcBuilder" /> to the<see cref="IServiceCollection"/> with the following<br/>
        /// <see cref="JsonStringEnumConverter"/><br/>
        /// </summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/> to add to.</param>
        /// <returns></returns>
        public static IMvcBuilder AddXacteJsonOptions(this IMvcBuilder builder)
        {
            builder.AddJsonOptions(configure =>
            {
                configure.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            return builder;
        }
    }
}