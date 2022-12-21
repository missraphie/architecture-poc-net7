using System.Runtime.Serialization;
using Xacte.Common.Exceptions.Enums;

namespace Xacte.Common.Exceptions
{
    [Serializable]
    public class XacteBadRequestException : XacteException
    {
        public XacteBadRequestException()
        {
        }

        public XacteBadRequestException(string message)
            : base(message)
        {
            Severity = SeverityKind.Error;
            HttpStatusCode = System.Net.HttpStatusCode.BadRequest;
        }

        public XacteBadRequestException(string message, Exception innerException)
            : base(message, innerException)
        {
            Severity = SeverityKind.Error;
            HttpStatusCode = System.Net.HttpStatusCode.BadRequest;
        }

        protected XacteBadRequestException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
