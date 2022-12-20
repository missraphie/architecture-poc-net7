using System.Runtime.Serialization;
using Xacte.Common.Exceptions;
using Xacte.Common.Exceptions.Enums;

namespace Xacte.Patient.Business.Exceptions
{
    [Serializable]
    public class PatientException : XacteBadRequestException
    {
        public new enum Codes
        {
            PatientNotFound = 100,
            PatientInactive = 101,
            PatientInvalidFirstName = 200,
            PatientLastNameAlreadyInUse = 300
        }

        public PatientException()
        {
        }

        public PatientException(string message)
            : base(message)
        {
            Severity = SeverityKind.Error;
        }

        public PatientException(Codes code, SeverityKind severity = SeverityKind.Error)
            : base(string.Empty)
        {
            Code = new BusinessCode(code);
            Severity = severity;
        }

        public PatientException(Codes code, Exception innerException)
            : base(string.Empty, innerException)
        {
            Code = new BusinessCode(code);
            Severity = SeverityKind.Error;
        }

        public PatientException(string message, Exception innerException)
            : base(message, innerException)
        {
            Severity = SeverityKind.Error;
        }

        protected PatientException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Severity = SeverityKind.Error;
        }
    }
}
