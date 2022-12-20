using System.Text.Json.Serialization;

namespace Xacte.Common.Exceptions
{
    /// <summary>
    /// Business Code instanciation used to qualify XacteException(s)
    /// </summary>
    public class BusinessCode : IEqualityComparer<BusinessCode>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessCode" /> class.
        /// </summary>
        public BusinessCode()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessCode" /> class.
        /// </summary>
        /// <param name="code">The business error code.</param>
        public BusinessCode(XacteException.Codes code)
            : this(code, Array.Empty<object>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessCode" /> class.
        /// </summary>
        /// <param name="code">The business error code.</param>
        /// <param name="args">The arguments used to qualified the error message.</param>
        public BusinessCode(Enum code, params object[] args)
        {
            Code = code.ToString();
            EnumType = code.GetType().FullName!;
            Args = args;
        }

        /// <summary>
        /// Interal initialization of a new instance of the <see cref="BusinessCode" /> class.
        /// </summary>
        /// <param name="enumName">The tag name of the enum code.</param>
        /// <param name="enumType">The type  of the enum code.</param>
        /// <param name="args">The arguments used to qualified the error message.</param>
        [JsonConstructor]
        internal BusinessCode(string enumName, string enumType, params object[] args)
        {
            Code = enumName;
            EnumType = enumType;
            Args = args ?? Array.Empty<object>();
        }

        [JsonInclude]
        public string Code { get; set; } = string.Empty;

        [JsonInclude]
        public string EnumType { get; set; } = string.Empty;

        [JsonInclude]
        public object[] Args { get; set; } = Array.Empty<object>();

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object of type <paramref name="T" /> to compare.</param>
        /// <param name="y">The second object of type <paramref name="T" /> to compare.</param>
        /// <returns>true if the specified objects are equal; otherwise, false.</returns>
        public bool Equals(BusinessCode? x, BusinessCode? y)
        {
            if (y is null) return false;
            if (ReferenceEquals(this, y)) return true;

            IEqualityComparer<object> comparer = new SequenceItemEqualityComparer();

            return string.Equals(Code, y.Code)
                   && string.Equals(EnumType, y.EnumType)
                   && Args.SequenceEqual(y.Args, comparer);
        }
        public override bool Equals(object? obj)
        {
            return Equals(this, obj as BusinessCode);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object" /> for which a hash code is to be returned.</param>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public int GetHashCode(BusinessCode obj)
        {
            var hashCode = Code?.GetHashCode() ?? 0;
            hashCode = (hashCode * 397) ^ (EnumType?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ (Args?.GetHashCode() ?? 0);
            return hashCode;
        }
        public override int GetHashCode()
        {
            return GetHashCode(this);
        }

        public override string ToString()
        {
            return $"{EnumType}.{Code}";
        }

        private sealed class SequenceItemEqualityComparer : IEqualityComparer<object>
        {
            public new bool Equals(object? x, object? y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x is null) return false;
                if (y is null) return false;
                return GetHashCode(x) == GetHashCode(y);
            }

            public int GetHashCode(object obj)
            {
                unchecked
                {
                    var hashCode = obj?.GetHashCode() ?? 0;
                    return hashCode;
                }
            }
        }
    }
}
