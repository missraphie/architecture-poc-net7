using System.Runtime.Serialization;
using Xacte.Common.Exceptions.Enums;

namespace Xacte.Common.Exceptions
{
    [Serializable]
    public class XacteForbiddenException : XacteException
    {
        public XacteForbiddenException()
        {
        }

        public XacteForbiddenException(string message)
            : base(message)
        {
            Severity = SeverityKind.Critical;
            HttpStatusCode = System.Net.HttpStatusCode.Forbidden;
        }

        public XacteForbiddenException(string message, Exception innerException)
            : base(message, innerException)
        {
            Severity = SeverityKind.Critical;
            HttpStatusCode = System.Net.HttpStatusCode.Forbidden;
        }

        protected XacteForbiddenException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
