using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Problem.Details
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context,
            IOptions<JsonOptions> jsonOptions,
            ILogger<ErrorHandlingMiddleware> logger)
        {
            try
            {
                await _next(context);
            }
            catch (ModelValidationException ex)
            { 
                var adjustedErrors = DetailsHelper.GetAdjustedValidationErrors(jsonOptions, ex.FieldErrors);
                var validationDetails = new ValidationProblemDetails(adjustedErrors);
                DetailsHelper.FillDetails(validationDetails, HttpStatusCode.BadRequest);

                await HandleProblemAsync(context, validationDetails);
                return;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception handled");
                var details = DetailsHelper.CreateProblemDetails(ex);

                if (DetailsHelper.ShowErrorDetails)
                {
                    var propertyName = jsonOptions.ConvertPropertyCase("Exception");
                    details.Extensions.Add(propertyName, ex.ToString());
                }

                await HandleProblemAsync(context, details); 
                return;
            }

            if (context.Response.StatusCode > 400)
            {
                var details = DetailsHelper.CreateProblemDetails((HttpStatusCode)context.Response.StatusCode);
                await HandleProblemAsync(context, details);
            }
        }
        
        private static async Task HandleProblemAsync(HttpContext context, ProblemDetails details)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = details.Status ?? 500;
            await context.Response.WriteAsync(details.ToJson());
            await context.Response.CompleteAsync();
        }
    }
}