using AutoMapper;
using Xacte.Patient.Dto.Api.Patient;
using Xacte.Patient.Dto.Business;

namespace Xacte.Patient.Api.Mappings.V1
{
    public sealed class PatientProfile : Profile
    {
        public PatientProfile()
        {
            CreateMap<GetPatientRequest, GetPatientRequestModel>();
            CreateMap<Data.Entities.Patient, PatientResponse>();
        }
    }
}
