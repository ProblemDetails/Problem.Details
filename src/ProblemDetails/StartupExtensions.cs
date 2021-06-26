using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ProblemDetails
{
    public class ConfigureProblemDetails
    {
        public ConfigureProblemDetails MapStatusToTitle(int statusCode, string title)
        {
            Details.MapStatusToTitle(statusCode, title);
            return this;
        }
    }
    
    public static class StartupExtensions
    {
        public static ConfigureProblemDetails AddProblemDetails(this IServiceCollection services)
        {
            services.AddControllers(options =>
                {
                    options.Filters.Add(new HttpResponseExceptionFilter());
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        Dictionary<string, IEnumerable<string>> errors = context.ModelState.ToDictionary(
                            k => k.Key,
                            v => v.Value.Errors
                                .Select(e => e.ErrorMessage));
                        
                        throw new ModelValidationException(errors);
                    };
                });
            
            services.TryAddSingleton<ProblemDetailsMarkerService, ProblemDetailsMarkerService>();
            return new ConfigureProblemDetails();
        }
        
        public static IApplicationBuilder UseProblemDetails(this IApplicationBuilder app)
        {
            var markerService = app.ApplicationServices.GetService<ProblemDetailsMarkerService>();

            if (markerService is null)
            {
                throw new InvalidOperationException(
                    $"Please call {nameof(IServiceCollection)}.{nameof(AddProblemDetails)} in ConfigureServices before adding the middleware.");
            }

            return app.UseMiddleware<ErrorHandlingMiddleware>();
        }
        
        /// <summary>
        /// A marker class used to determine if the required services were added
        /// to the <see cref="IServiceCollection"/> before the middleware is configured.
        /// </summary>
        private class ProblemDetailsMarkerService
        {
        }
    }
}