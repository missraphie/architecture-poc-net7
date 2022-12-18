namespace Xacte.Common.Response
{
    public abstract class XacteResponse<T> where T : class
    {
        public abstract T Data { get; set; }

        public bool IsSuccess { get; private set; } = true;

        public ISet<XacteResponseMessage> Messages { get; set; } = new HashSet<XacteResponseMessage>();

        public void AddData(T data)
        {
            Data = data;
        }

        public void AddConfirmation(string value)
        {
            Messages.Add(new(0, value));
        }

        public void AddError(string value)
        {
            IsSuccess = false;
            Messages.Add(new(0, value));
        }
    }
}
