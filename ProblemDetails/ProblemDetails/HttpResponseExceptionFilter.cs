using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ProblemDetails
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order => int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new ObjectResult(new
                {
                    title = "One or more validation errors occurred.",
                    status = StatusCodes.Status400BadRequest,
                    errors = context.ModelState.ToDictionary(
                        k => k.Key, 
                        v => v.Value.Errors
                            .Select(e => e.ErrorMessage))
                })
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
                context.Canceled = true;
            }

            // if (context.Exception is HttpResponseException exception)
            // {
            //     context.Result = new ObjectResult(exception.Value)
            //     {
            //         StatusCode = exception.Status,
            //     };
            //     context.ExceptionHandled = true;
            // }
        }
    }
}