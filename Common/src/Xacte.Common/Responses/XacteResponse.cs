namespace Xacte.Common.Responses
{
    public class XacteResponse<T> where T : class
    {
        public virtual IList<T> Data { get; set; } = Array.Empty<T>();

        public XacteResponseMeta Meta { get; private set; } = new(isSuccessStatusCode: true);

        public void AddData(IList<T> values)
        {
            foreach (var value in values)
            {
                AddData(value);
            }
        }

        public void AddData(T value)
        {
            if (Data == Array.Empty<T>())
            {
                Data = new List<T>();
            }
            Data.Add(value);
        }

        public void AddConfirmation(string value)
        {
            Meta.Messages.Add(new(value));
        }

        public void AddError(string value)
        {
            Meta.IsSuccessStatusCode = false;
            Meta.Messages.Add(new(value));
        }
    }
}
