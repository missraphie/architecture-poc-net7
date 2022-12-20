namespace Xacte.Common.Responses
{
    public sealed class XacteResponseMessage
    {
        public XacteResponseMessage(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}
