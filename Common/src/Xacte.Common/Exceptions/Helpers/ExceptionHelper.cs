using System.Globalization;
using System.Reflection;
using System.Resources;

namespace Xacte.Common.Exceptions.Helpers
{
    internal static class ExceptionHelper
    {
        /// <summary>
        ///     The Type => RessourceManagers cache
        /// </summary>
        private static readonly Dictionary<Type, ResourceManager> ResourceManagerCache = new();
        private static readonly ResourceManager BaseResourceManager = new(typeof(XacteException));

        /// <summary>
        /// Gets the message associate to a given code.
        /// </summary>
        /// <param name="culture">The culture.</param>
        /// <param name="ressourceType">Type of the ressource.</param>
        /// <param name="ressourceTag">The ressource tag.</param>
        /// <returns>System.String.</returns>

        internal static string GetMessage(CultureInfo culture, string ressourceType, string ressourceTag)
        {
            // Load from the resource file
            return Lookup(culture, ressourceType, ressourceTag);
        }

        /// <summary>
        ///     Lookups the specified type within the resource manager.
        /// </summary>
        private static string Lookup(CultureInfo culture, string ressourceType, string ressourceTag)
        {
            if (ressourceTag is null || ressourceType is null)
            {
                return string.Empty;
            }

            // note: the following does not guard against exceptions that do not subclass
            // our base classes [done elsewhere], however it will not crash either

            // Try to locate the ressource type, case insensitive, and do not throw on error

            var type = Type.GetType(ressourceType, ResolveAssembly, ResolveType!, false, true);

            // It embeded type move up in the code structure till we reach the declaring class
            while (type is not null && !type.IsClass && type != typeof(object))
            {
                type = type.DeclaringType;
            }

            // Load ressources and move up in the Exception hierarchy 
            while (type != typeof(Exception) && type is not null)
            {
                var rm = LoadResourceManager(type);

                // ReSharper disable once EmptyGeneralCatchClause
                try
                {
                    var propertyValue = rm.GetString(ressourceTag, culture);
                    if (propertyValue is not null)
                    {
                        return propertyValue;
                    }
                }
                catch
                {
                    // Ignored.
                }

                // walk the inheritance chain for 'type':
                type = type.GetTypeInfo().BaseType;
            }

            return string.Empty;
        }

        private static Type? ResolveType(Assembly assembly, string typeName, bool ignoreCase)
        {
            if (assembly is not null)
            {
                return assembly.GetType(typeName, ignoreCase);
            }

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            return assemblies.Select(ass => ass.GetType(typeName, false, ignoreCase)).FirstOrDefault(type => type is not null);
        }

        private static Assembly? ResolveAssembly(AssemblyName assemblyName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            return assemblies.FirstOrDefault(ass => ass.GetName().Equals(assemblyName));
        }

        /// <summary>
        /// Add a "derived" Exception resource file that contains localized versions of error codes.
        /// </summary>
        /// <param name="type">The class type associated with that resources.</param>
        /// <returns>The resource manager associated with the given type.</returns>
        private static ResourceManager LoadResourceManager(Type type)
        {
            var manager = BaseResourceManager;

            if (type is null)
            {
                return manager;
            }

            lock (ResourceManagerCache)
            {
                if (ResourceManagerCache.TryGetValue(type, out var value))
                {
                    return value;
                }
            }

            // ReSharper disable once EmptyGeneralCatchClause
            try
            {
                manager = new ResourceManager(type);
            }
            catch
            {
                // ignored intentionally: if the exception resource manager is absent,
                // we are still in a supported configuration
            }

            lock (ResourceManagerCache)
            {
                ResourceManagerCache.Add(type, manager);
            }
            return manager;
        }
    }
}
