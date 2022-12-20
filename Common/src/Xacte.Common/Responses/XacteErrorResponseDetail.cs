using Xacte.Common.Exceptions;

namespace Xacte.Common.Responses
{
    public sealed class XacteErrorResponseDetail
    {
        public BusinessCode? Code { get; set; }
        public int Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public string ExceptionType { get; set; } = string.Empty;
        public string? Source { get; set; }
        public string? Trace { get; set; }
    }
}
