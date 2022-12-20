namespace Xacte.Common.Exceptions.Extensions
{
    public static class ExceptionExtensions
    {
        public static IEnumerable<T> Flatten<T>(this Exception ex) where T : Exception
        {
            var list = new List<Exception>();
            
            if (ex is not T) return list.Cast<T>();

            list.Add(ex);

            if (ex.InnerException is not null)
            {
                list.AddRange(ex.InnerException.Flatten<T>());
            }

            var aggregate = ex as AggregateException;
            if (aggregate is not null)
            {
                foreach (var inner in aggregate.InnerExceptions.Where(e => e is T).Cast<T>())
                {
                    list.AddRange(inner.Flatten<T>());
                }
            }
            return list.Cast<T>();
        }
    }
}
