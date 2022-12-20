using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using Xacte.Common.Exceptions;
using Xacte.Patient.Business.Exceptions;

namespace Xacte.Patient.Api.Filters
{
    internal sealed class PatientExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is XacteException cbException)
            {
                ExceptionHelper.ApplyHttpStatus(cbException);
            }

            context.ExceptionDispatchInfo!.Throw();
        }
    }

    internal static class ExceptionHelper
    {
        public static void ApplyHttpStatus(XacteException exception)
        {
            switch (exception)
            {
                case PatientException ex:
                    ApplyProfileServiceException(ex);
                    break;
            }
        }

        private static void ApplyProfileServiceException(PatientException exception)
        {
            _ = Enum.TryParse<PatientException.Codes>(exception.Code.Code, out var code);
            switch (code)
            {
                case PatientException.Codes.PatientInactive:
                case PatientException.Codes.PatientInvalidFirstName:
                    exception.HttpStatusCode = HttpStatusCode.BadRequest;
                    break;
                case PatientException.Codes.PatientNotFound:
                    exception.HttpStatusCode = HttpStatusCode.NotFound;
                    break;
                case PatientException.Codes.PatientLastNameAlreadyInUse:
                    exception.HttpStatusCode = HttpStatusCode.Conflict;
                    break;
                default:
                    exception.HttpStatusCode = HttpStatusCode.InternalServerError;
                    break;
            }
        }
    }

}
