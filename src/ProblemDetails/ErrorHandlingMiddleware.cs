using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ProblemDetails
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
                ValidationDetails details = new(StatusCodes.Status400BadRequest, ex.FieldErrors, jsonOptions);
                await HandleProblemAsync(context, HttpStatusCode.BadRequest, details.ToJson());
                return;
            }
            catch (Exception ex) //TODO: dev error details
            {
                logger.LogError(ex, "Exception handled");
                Details details = new(StatusCodes.Status500InternalServerError);
                await HandleProblemAsync(context, HttpStatusCode.InternalServerError, details.ToJson());
                return;
            }

            if (context.Response.StatusCode > 400)
            {
                Details details = new(context.Response.StatusCode);
                await HandleProblemAsync(context, context.Response.StatusCode, details.ToJson());
            }
        }

        private static Task HandleProblemAsync(HttpContext context, HttpStatusCode statusCode, string body) => 
            HandleProblemAsync(context, (int) statusCode, body);

        private static async Task HandleProblemAsync(HttpContext context, int statusCode, string body)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync(body);
            await context.Response.CompleteAsync();
        }
    }
}