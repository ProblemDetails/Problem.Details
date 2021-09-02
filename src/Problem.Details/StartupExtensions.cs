using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Problem.Details
{
    public class ConfigureProblemDetails
    {
        public ConfigureProblemDetails MapStatusToTitle(HttpStatusCode code, string title)
        {
            DetailsHelper.MapStatusToTitle(code, title);
            return this;
        }

        /// <summary>
        /// Map you custom exception against a <see cref="HttpStatusCode"/>
        /// </summary>
        public ConfigureProblemDetails MapException<TException>(HttpStatusCode code)
            where TException: Exception
        {
            DetailsHelper.MapException<TException>(code);
            return this;
        }

        /// <summary>
        /// Adds detailed exception to <see cref="ProblemDetails"/> extensions list.
        /// Not recommended for production
        /// <remarks>default: false</remarks>
        /// </summary>
        public ConfigureProblemDetails ShowErrorDetails(bool show)
        {
            DetailsHelper.SetShowErrorDetails(show);
            return this;
        }
    }

    public static class StartupExtensions
    {
        public static IServiceCollection AddProblemDetails(this IServiceCollection services)
        {
            services.AddControllers(options =>
                {
                    options.Filters.Add(new HttpResponseExceptionFilter());
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var errors = context.ModelState.ToDictionary(
                            k => k.Key,
                            v => v.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                        );

                        throw new ModelValidationException(errors);
                    };
                });

            services.TryAddSingleton<ProblemDetailsMarkerService, ProblemDetailsMarkerService>();
            return services;
        }

        public static IApplicationBuilder UseProblemDetails(this IApplicationBuilder app, Action<ConfigureProblemDetails> configure = null)
        {
            var configurator = new ConfigureProblemDetails();
            configure?.Invoke(configurator);

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