using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.IO.Compression;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xacte.Common.Hosting.Api.Configurations;
using Xacte.Common.Hosting.Api.ConfigureOptions;
using Xacte.Common.Hosting.Api.OperationFilters;
using Xacte.Common.Services;

namespace Xacte.Common.Hosting.Api.Extensions
{
    /// <summary>
    /// Api extension methods for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a default implementation for Xacte Core Services to the <see cref="IServiceCollection"/> with the following<br/>
        /// AddMemoryCache<br/>
        /// AddHttpContextAccessor<br/>
        /// <see cref="CurrentUserService"/>
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add services to.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddXacteCoreServices(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            return services;
        }

        /// <summary>
        /// Adds default serializer converter implementations for <see cref="JsonSerializer"/> to the <see cref="IServiceCollection"/> with the following<br/>
        /// <see cref="JsonStringEnumConverter"/><br/>
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <param name="options">Provides options to be used with <see cref="JsonSerializer"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddXacteJsonSerializerOptions(this IServiceCollection services, Action<JsonSerializerOptions> options = null)
        {
            services.AddSingleton(sp => new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = {
                    new JsonStringEnumConverter()
                }
            });

            if (options != null)
            {
                services.Configure<JsonSerializerOptions>(cfg => options?.Invoke(cfg));
            }
            return services;
        }

        /// <summary>
        /// Adds a default implementation for Xacte API versioning to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        /// <remarks>https://github.com/dotnet/aspnet-api-versioning/wiki/Swashbuckle-Integration</remarks>
        public static IServiceCollection AddXacteVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                options.ReportApiVersions = true;
            });
            services.AddVersionedApiExplorer(options =>
            {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v'major[.minor][-status]"
                options.GroupNameFormat = "'v'VVV";

                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
                options.SubstituteApiVersionInUrl = true;
            });
            return services;
        }

        /// <summary>
        /// Adds a default implementation for Swagger to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <param name="containerType">Program class type.</param>
        /// <param name="description">Description of the API.</param>
        /// <param name="title">Title of the API.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddXacteSwagger(this IServiceCollection services, Type containerType, string title, string description)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>((sp) =>
            {
                return new ConfigureSwaggerOptions(sp.GetRequiredService<IApiVersionDescriptionProvider>())
                {
                    Title = title,
                    Description = description
                };
            });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                // add a custom operation filter which sets default values
                options.OperationFilter<SwaggerDefaultValues>();

                // integrate xml comments
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = containerType.GetTypeInfo().Assembly.GetName().Name + ".xml";
                var xmlCommentsFilePath = Path.Combine(basePath, fileName);
                if (File.Exists(xmlCommentsFilePath))
                {
                    options.IncludeXmlComments(xmlCommentsFilePath);
                }
            });
            return services;
        }

        /// <summary>
        /// Adds response compression to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <param name="compressionProviders">List of requested compression providers, defaults to Brotli and Gzip.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddXacteResponseCompression(this IServiceCollection services, CompressionProviders compressionProviders = CompressionProviders.Brotli | CompressionProviders.Gzip)
        {
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;

                if (compressionProviders.HasFlag(CompressionProviders.Brotli))
                {
                    options.Providers.Add<BrotliCompressionProvider>();
                }
                if (compressionProviders.HasFlag(CompressionProviders.Gzip))
                {
                    options.Providers.Add<GzipCompressionProvider>();
                }
            });
            if (compressionProviders.HasFlag(CompressionProviders.Brotli))
            {
                services.Configure<BrotliCompressionProviderOptions>(options =>
                {
                    options.Level = CompressionLevel.Fastest;
                });
            }
            if (compressionProviders.HasFlag(CompressionProviders.Gzip))
            {
                services.Configure<GzipCompressionProviderOptions>(options =>
                {
                    options.Level = CompressionLevel.SmallestSize;
                });
            }
            return services;
        }
    }
}
