using System.Runtime.Serialization;
using Xacte.Common.Exceptions.Enums;

namespace Xacte.Common.Exceptions
{
    [Serializable]
    public class XacteNotFoundException : XacteException
    {
        public XacteNotFoundException()
        {
        }

        public XacteNotFoundException(string message)
            : base(message)
        {
            Severity = SeverityKind.Error;
            HttpStatusCode = System.Net.HttpStatusCode.NotFound;
        }

        public XacteNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
            Severity = SeverityKind.Error;
            HttpStatusCode = System.Net.HttpStatusCode.NotFound;
        }

        protected XacteNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
