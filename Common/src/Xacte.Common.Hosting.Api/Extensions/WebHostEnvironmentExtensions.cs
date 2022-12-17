using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Xacte.Common.Hosting.Api.Extensions
{
    /// <summary>
    /// WebHostEnvironment extension methods
    /// </summary>
    public static class WebHostEnvironmentExtensions
    {
        /// <summary>
        /// Checks if the current host environment name is Local.
        /// </summary>
        /// <param name="hostEnvironment">An instance of <see cref="IHostEnvironment"/>.</param>
        /// <returns>True if the environment name is Local, otherwise false.</returns>
        public static bool IsLocal(this IWebHostEnvironment hostEnvironment)
        {
            return "Local".Equals(hostEnvironment.EnvironmentName, StringComparison.OrdinalIgnoreCase);
        }
    }
}
