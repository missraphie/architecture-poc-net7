using AutoMapper;
using Xacte.Patient.Business.Exceptions;
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

        public async Task<CreatePatientResponse> CreateAsync(CreatePatientRequestModel model)
        {
            var response = new CreatePatientResponse();

            var entity = new Data.Entities.Patient
            {
                FirstName = model.FirstName,
                LastName = model.LastName
            };
            entity = await _patientRepository.CreateAsync(entity);

            response.AddData(_mapper.Map<PatientResponse>(entity));
            response.AddConfirmation("Patient created Successful");
            return response;
        }

        public async Task<DeletePatientResponse> DeleteAsync(DeletePatientRequestModel model)
        {
            var response = new DeletePatientResponse();
            if (!await _patientRepository.AnyAsync(model.Guid))
            {
                response.AddError("Patient does not exist");
                return response;
            }

            await _patientRepository.DeleteAsync(model.Guid);

            response.AddConfirmation("Patient deleted successfully");
            return response;
        }

        public async Task<GetPatientResponse> GetAsync(GetPatientRequestModel model)
        {
            var response = new GetPatientResponse();
            if (!await _patientRepository.AnyAsync(model.Guid))
            {
                //response.AddError("Patient does not exist");
                //return response;
                throw new PatientException(PatientException.Codes.PatientNotFound);
            }

            var patient = await _patientRepository.GetAsync(model.Guid);

            response.AddData(_mapper.Map<PatientResponse>(patient));
            response.AddConfirmation("Get Successful");
            return response;
        }

        public async Task<GetPatientsResponse> GetAsync()
        {
            var patients = await _patientRepository.GetAsync();

            var response = new GetPatientsResponse();
            response.AddData(patients.Select(_mapper.Map<PatientResponse>).ToList());
            response.AddConfirmation("Get Successful");
            return response;
        }
    }
}
