using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mime;
using System.Text.Json;
using Xacte.Common.Exceptions;
using Xacte.Common.Exceptions.Enums;
using Xacte.Common.Responses;
using Xacte.Common.Utils;

namespace Xacte.Common.Hosting.Api.Middlewares
{
    /// <summary>
    /// Error handling middleware
    /// </summary>
    public class XacteErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<XacteErrorHandlingMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="XacteErrorHandlingMiddleware"/> class.
        /// </summary>
        /// <param name="next">A task that represents the completion of request processing.</param>
        /// <param name="logger">Logger concrete implementation.</param>
        public XacteErrorHandlingMiddleware(RequestDelegate next, ILogger<XacteErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Middleware entry point
        /// </summary>
        /// <param name="context">An instance of <seealso cref="HttpContext"/></param>
        /// <returns>A task that represents the completion of request processing.</returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Handles the current exception
        /// </summary>
        /// <param name="context">An instance of <seealso cref="HttpContext"/></param>
        /// <param name="exception">The current <seealso cref="Exception"/> to parse</param>
        /// <returns>A task that represents the completion of request processing.</returns>
        public virtual async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (exception is null)
            {
                return;
            }

            if (exception is AggregateException aggregateException)
            {
                exception = aggregateException.GetBaseException();
            }

            if (exception is XacteException ex)
            {
                if (ex.Code is not null)
                {
                    ////LogContext.PushProperty("BusinessCode", ex.Code.Code);
                }

                if (!ex.HttpStatusCode.HasValue)
                {
                    ex.HttpStatusCode = HttpStatusCode.BadRequest;
                }
            }
            else if (exception is OperationCanceledException)
            {
                ex = new XacteBadRequestException(exception.Message, exception)
                {
                    Code = new BusinessCode(XacteException.Codes.CriticalRuntimeErrorOccurred),
                    Severity = SeverityKind.Error,
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
            }
            else
            {
                // Return fatal error whenever a non applicative exception is thrown
                ex = new XacteException(exception.Message, exception)
                {
                    Code = new BusinessCode(XacteException.Codes.CriticalRuntimeErrorOccurred),
                    Severity = SeverityKind.Critical,
                    HttpStatusCode = HttpStatusCode.InternalServerError
                };
            }

            // Fallback: Defaults to server error
            if (!ex.HttpStatusCode.HasValue)
            {
                ex.HttpStatusCode = HttpStatusCode.InternalServerError;
            }

            await WriteExceptionAsync(context, ex).ConfigureAwait(false);
        }

        private async Task WriteExceptionAsync(HttpContext context, XacteException exception)
        {
            var response = context.Response;
            response.ContentType = MediaTypeNames.Application.Json;

            if (exception.HttpStatusCode.HasValue)
            {
                response.StatusCode = (int)exception.HttpStatusCode.Value;
            }

            switch (exception.Severity)
            {
                case SeverityKind.Information:
                    _logger.LogInformation(exception, "An information exception was raised");
                    break;
                case SeverityKind.Warning:
                case SeverityKind.Retryable:
                    _logger.LogWarning(exception, "A warning exception was raised");
                    break;
                case SeverityKind.Error:
                    _logger.LogError(exception, "An error exception was raised");
                    break;
                case SeverityKind.Critical:
                case SeverityKind.Lethal:
                    _logger.LogCritical(exception, "A critical exception was raised");
                    break;
            }

            XacteException xacteException = exception;
            XacteErrorResponse responseObject = new(new XacteErrorResponseDetail()
            {
                Code = xacteException.Code,
                Status = response.StatusCode,
                ExceptionType = xacteException.ExceptionType,
                Source = xacteException.InnerException is null ? xacteException.Source : xacteException.InnerException.Source,
                Message = xacteException.InnerException is null ? xacteException.Reason : xacteException.InnerException.Message,
                Trace = xacteException.InnerException is null ? xacteException.StackTrace : xacteException.InnerException.StackTrace
            });

            await response.WriteAsync(JsonSerializer.Serialize(responseObject, JsonSerializerUtils.GetJsonSerializerOptions())).ConfigureAwait(false);
        }
    }
}
