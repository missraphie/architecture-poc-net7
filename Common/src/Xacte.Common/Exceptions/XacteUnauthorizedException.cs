using System.Runtime.Serialization;
using Xacte.Common.Exceptions.Enums;

namespace Xacte.Common.Exceptions
{
    [Serializable]
    public class XacteUnauthorizedException : XacteException
    {
        public XacteUnauthorizedException()
        {
        }

        public XacteUnauthorizedException(string message)
            : base(message)
        {
            Severity = SeverityKind.Critical;
            HttpStatusCode = System.Net.HttpStatusCode.Unauthorized;
        }

        public XacteUnauthorizedException(string message, Exception innerException)
            : base(message, innerException)
        {
            Severity = SeverityKind.Critical;
            HttpStatusCode = System.Net.HttpStatusCode.Unauthorized;
        }

        protected XacteUnauthorizedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
