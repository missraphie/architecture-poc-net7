namespace Xacte.Common.Response
{
    public sealed class XacteResponseMessage
    {
        public XacteResponseMessage(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public int Code { get; set; }
        public string Message { get; set; }
    }
}
