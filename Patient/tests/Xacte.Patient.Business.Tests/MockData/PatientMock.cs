using Xacte.Patient.Dto.Api.Patient;
using Xacte.Patient.Dto.Business;

namespace Xacte.Patient.Business.Tests.MockData
{
    public static class PatientMock
    {
        public static Data.Entities.Patient GetPatient(Action<Data.Entities.Patient>? updateObjAction = null)
        {
            var request = new Data.Entities.Patient
            {
                Guid = Guid.NewGuid(),
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString()
            };

            if (updateObjAction is not null)
            {
                updateObjAction(request);
            }
            return request;
        }

        public static CreatePatientRequestModel GetCreatePatientRequestModel(Action<CreatePatientRequestModel>? updateObjAction = null)
        {
            var request = new CreatePatientRequestModel
            {
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
            };

            if (updateObjAction is not null)
            {
                updateObjAction(request);
            }
            return request;
        }

        public static DeletePatientRequestModel GetDeletePatientRequestModel(Action<DeletePatientRequestModel>? updateObjAction = null)
        {
            var request = new DeletePatientRequestModel
            {
                Guid = Guid.NewGuid()
            };

            if (updateObjAction is not null)
            {
                updateObjAction(request);
            }
            return request;
        }

        public static GetPatientRequestModel GetGetPatientRequestModel(Action<GetPatientRequestModel>? updateObjAction = null)
        {
            var request = new GetPatientRequestModel
            {
                Guid = Guid.NewGuid()
            };

            if (updateObjAction is not null)
            {
                updateObjAction(request);
            }
            return request;
        }

        public static PatientResponse GetPatientResponse(Action<PatientResponse>? updateObjAction = null)
        {
            var request = new PatientResponse
            {
                Guid = Guid.NewGuid(),
            };

            if (updateObjAction is not null)
            {
                updateObjAction(request);
            }
            return request;
        }
    }
}
