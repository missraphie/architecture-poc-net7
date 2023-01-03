using Microsoft.Extensions.DependencyInjection;
using Xacte.Patient.Business.Services;
using Xacte.Patient.Business.Services.Interfaces;

namespace Xacte.Patient.Business
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<IPatientService, PatientService>();
            return services;
        }
    }
}
