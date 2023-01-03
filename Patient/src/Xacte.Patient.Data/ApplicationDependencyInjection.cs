using Microsoft.Extensions.DependencyInjection;
using Xacte.Patient.Data.Repositories;
using Xacte.Patient.Data.Repositories.Interfaces;

namespace Xacte.Patient.Data
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddDataRepositories(this IServiceCollection services)
        {
            services.AddScoped<IPatientRepository, PatientRepository>();
            return services;
        }
    }
}
