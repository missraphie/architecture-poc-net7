namespace Xacte.Common.Responses
{
    public class XacteModelValidationResponse
    {
        public XacteModelValidationResponse(Dictionary<string, List<string>> errors)
        {
            Errors = new List<Dictionary<string, List<string>>> { errors };
        }

        /// <summary>
        /// List of all validation errors
        /// </summary>
        /// <remarks>This property is wrapped by a <seealso cref="List{T}"/> to enforce an array representation.</remarks>
        public IList<Dictionary<string, List<string>>> Errors { get; private set; }

        public XacteResponseMeta Meta { get; private set; } = new(isSuccessStatusCode: false);
    }
}
