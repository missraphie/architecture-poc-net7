namespace Xacte.Common.Responses
{
    public class XacteErrorResponse
    {
        public XacteErrorResponse(XacteErrorResponseDetail errorResponse)
        {
            Errors = new List<XacteErrorResponseDetail> { errorResponse };
        }

        /// <summary>
        /// List of Xacte errors
        /// </summary>
        /// <remarks>This property is wrapped by a <seealso cref="List{T}"/> to enforce an array representation.</remarks>
        public IList<XacteErrorResponseDetail> Errors { get; set; }

        public XacteResponseMeta Meta { get; private set; } = new(isSuccessStatusCode: false);
    }
}
