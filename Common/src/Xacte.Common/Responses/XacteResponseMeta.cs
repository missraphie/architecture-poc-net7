namespace Xacte.Common.Responses
{
    public sealed class XacteResponseMeta
    {
        public XacteResponseMeta(bool isSuccessStatusCode)
        {
            IsSuccessStatusCode = isSuccessStatusCode;
        }

        public ISet<XacteResponseMessage> Messages { get; set; } = new HashSet<XacteResponseMessage>();
        public bool IsSuccessStatusCode { get; set; }
    }
}
