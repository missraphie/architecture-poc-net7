using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using Xacte.Common.Hosting.Api.ActionFilters;

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

        /// <summary>
        /// Adds default mvc action filters. Current filters:<br/>
        /// <see cref="ModelValidationActionFilter"/><br/>
        /// </summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/> to add to.</param>
        /// <returns></returns>
        public static IMvcBuilder AddXacteMvcActionFilters(this IMvcBuilder builder)
        {
            // Web API controllers have a filter that automatically validate the model
            // Because we are creating our own, we want to disable the default filter.
            builder.ConfigureApiBehaviorOptions(opt => opt.SuppressModelStateInvalidFilter = true);

            // When adding filters, order matter.
            builder
                // This must be the first filter, because once a filter returns a failure,
                // the next filter is not executed. This filter relies on the 
                // "executed" method and does nothing on the "executing".
                .AddMvcOptions(opt => opt.Filters.Add<ModelValidationActionFilter>());
            return builder;
        }
    }
}