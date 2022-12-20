using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Xacte.Common.Responses;

namespace Xacte.Common.Hosting.Api.ActionFilters
{
    /// <summary>
    /// Action Filter to validate the model of a request.
    /// If the model has validation attributes (i.e.: Required, Range),
    /// this filter is able to validate the model, add error messages,
    /// and return a BaseMessage response.
    /// </summary>
    public sealed class ModelValidationActionFilter : IActionFilter
    {
        /// <summary>
        /// Method callend before the endpoint is executed
        /// </summary>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errorMessages = GetErrorMessages(context);
                var result = CreateActionResultForErrorMessages(errorMessages);
                context.Result = result;
            }
        }

        /// <summary>
        /// Method called after the endpoint is executed
        /// </summary>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No Action Required
        }

        private static Dictionary<string, List<string>> GetErrorMessages(ActionExecutingContext context)
        {
            var errorKeys = new Dictionary<string, List<string>>();
            foreach (var modelStateEntry in context.ModelState.Where(w => w.Value is not null && w.Value.Errors.Any()))
            {
                var errorValues = new List<string>();
                foreach (var modelStateEntryError in modelStateEntry.Value!.Errors)
                {
                    errorValues.Add(modelStateEntryError.ErrorMessage);
                }
                errorKeys.Add(modelStateEntry.Key, errorValues);
            }
            return errorKeys;
        }

        private static IActionResult CreateActionResultForErrorMessages(Dictionary<string, List<string>> errorMessages)
        {
            var baseMessage = CreateBaseMessage(errorMessages);
            var result = new UnprocessableEntityObjectResult(baseMessage);
            return result;
        }

        private static XacteModelValidationResponse CreateBaseMessage(Dictionary<string, List<string>> errorMessages)
        {
            var result = new XacteModelValidationResponse(errorMessages);
            result.Meta.Messages.Add(new("One or more validation errors occured."));
            return result;
        }
    }
}
