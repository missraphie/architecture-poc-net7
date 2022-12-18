using AutoMapper;
using Xacte.Patient.Business.Services.Interfaces;
using Xacte.Patient.Data.Repositories.Interfaces;
using Xacte.Patient.Dto.Api.Patient;
using Xacte.Patient.Dto.Business;

namespace Xacte.Patient.Business.Services
{
    public sealed class PatientService : IPatientService
    {
        private readonly IMapper _mapper;
        private readonly IPatientRepository _patientRepository;

        public PatientService(IMapper mapper, IPatientRepository patientRepository)
        {
            _mapper = mapper;
            _patientRepository = patientRepository;
        }

        public async Task<GetPatientResponse> GetPatientAsync(GetPatientRequestModel model)
        {
            var patient = await _patientRepository.GetAsync(model.Guid);

            var response = new GetPatientResponse();
            response.AddData(_mapper.Map<PatientResponse>(patient));
            response.AddConfirmation("Get Patient Successful");
            return response;
        }

        public async Task<GetPatientsResponse> GetPatientsAsync()
        {
            var patients = await _patientRepository.GetAsync();

            var response = new GetPatientsResponse();
            response.AddData(patients.Select(_mapper.Map<PatientResponse>).ToList());
            response.AddConfirmation("You got a patient");
            return response;
        }
    }
}
