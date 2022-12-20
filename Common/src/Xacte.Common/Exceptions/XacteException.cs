using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xacte.Common.Exceptions.Enums;
using Xacte.Common.Exceptions.Extensions;
using Xacte.Common.Exceptions.Helpers;

namespace Xacte.Common.Exceptions
{
    /// <summary>
    /// Xacte Exception custom object
    /// </summary>
    [Serializable]
    public class XacteException : Exception
    {
        [NonSerialized]
        private BusinessCode _code = new();
        [NonSerialized]
        private string _rawReason = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="XacteException"/> class.
        /// </summary>
        public XacteException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XacteException"/> class.
        /// </summary>
        /// <param name="message">A message that desXactebes the error.</param>
        [JsonConstructor]
        public XacteException(string message)
            : base(message)
        {
            Init(GetType(), new(), message);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XacteException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the rawReason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public XacteException(string message, Exception innerException)
            : base(message, innerException)
        {
            var ex = this.Flatten<XacteException>().FirstOrDefault(e => e.Code?.Code is not null);
            if (ex is null)
            {
                Init(GetType(), new(), string.Empty);
            }
            else
            {
                var localized = Localize(ex.Code);
                Init(innerException.GetType(), ex.Code, localized);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XacteException"/> class.
        /// </summary>
        protected XacteException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public enum Codes
        {
            CriticalRuntimeErrorOccurred
        }

        /// <summary>
        /// Gets the formated rawReason.
        /// </summary>
        public string Reason { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the Localized message.
        /// </summary>
        /// <value>
        /// The L7D message.
        /// </value>
        //[JsonProperty]
        private string RawReason
        {
            get => _rawReason;
            set
            {
                _rawReason = value;
                FormatReason();
            }
        }

        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        //[JsonProperty]
        public BusinessCode Code
        {
            get => _code;
            set
            {
                _code = value;
                RawReason = Localize(_code);
            }
        }

        /// <summary>
        /// Gets or sets the exception type.
        /// </summary>
        public string ExceptionType { get; private set; } = string.Empty;

        /// <summary>
        /// Gets or sets the error's severity.
        /// </summary>
        public SeverityKind Severity { get; set; }

        /// <summary>
        /// Gets or sets the HTTP status code that will override default one
        /// </summary>
        //[JsonProperty]
        public HttpStatusCode? HttpStatusCode { get; set; }

        /// <summary>
        /// Gets the Localized and formatted message.
        /// </summary>
        /// <value>
        /// The formatted L7D message.
        /// </value>
        private void FormatReason()
        {
            if (Code != null && !string.IsNullOrEmpty(RawReason) && Code.Args.Any())
            {
                Reason = string.Format(RawReason, Code.Args);
            }
            else
            {
                Reason = RawReason;
            }
        }

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        /// <param name="exceptionType">Type of the original exception.</param>
        /// <param name="code">The code.</param>
        /// <param name="reason">The user message.</param>
        private void Init(Type exceptionType, BusinessCode code, string? reason)
        {
            ExceptionType = exceptionType.Name;
            Code = code;

            if (!string.IsNullOrWhiteSpace(reason))
            {
                RawReason = reason;
            }

            Severity = SeverityKind.Error;
            HttpStatusCode = null;
        }

        /// <summary>
        /// Localizes the specified code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        private static string Localize(BusinessCode? code)
        {
            try
            {
                return code is not null
                    ? ExceptionHelper.GetMessage(CultureInfo.CurrentCulture, code.EnumType, code.Code)
                    : string.Empty;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Unable to Localize error code {code}. {e.Message}");
                return string.Empty;
            }
        }
    }

    [Serializable]
    public class XacteBadRequestException : XacteException
    {
        public XacteBadRequestException()
        {
        }

        public XacteBadRequestException(string message)
            : base(message)
        {
            Severity = SeverityKind.Error;
            HttpStatusCode = System.Net.HttpStatusCode.BadRequest;
        }

        public XacteBadRequestException(string message, Exception innerException)
            : base(message, innerException)
        {
            Severity = SeverityKind.Error;
            HttpStatusCode = System.Net.HttpStatusCode.BadRequest;
        }

        protected XacteBadRequestException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
