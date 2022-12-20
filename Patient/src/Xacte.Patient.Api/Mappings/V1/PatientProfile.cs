using AutoMapper;
using Xacte.Patient.Dto.Api.Patient;
using Xacte.Patient.Dto.Business;

namespace Xacte.Patient.Api.Mappings.V1
{
    /// <summary>
    /// Patient mapper class
    /// </summary>
    public sealed class PatientProfile : Profile
    {
        /// <summary>
        /// Patient mapper implementation
        /// </summary>
        public PatientProfile()
        {
            CreateMap<Data.Entities.Patient, PatientResponse>();
            
            CreateMap<GetPatientRequest, GetPatientRequestModel>();
            CreateMap<CreatePatientRequest, CreatePatientRequestModel>();
            CreateMap<DeletePatientRequest, DeletePatientRequestModel>();
        }
    }
}
