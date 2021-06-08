using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ProblemDetails
{
    public class ValidationDetails : Details
    {
        // https://github.com/dotnet/aspnetcore/issues/7439#issuecomment-541625844
        public ValidationDetails(int status,
            Dictionary<string, IEnumerable<string>> errors,
            IOptions<JsonOptions> jsonOptions) : base(status)
        {
            Errors = errors.ToDictionary(
                kvp => 
                    jsonOptions?.Value?.JsonSerializerOptions.PropertyNamingPolicy?.ConvertName(kvp.Key) ?? kvp.Key,
                kvp => kvp.Value
            );
        }

        public Dictionary<string, IEnumerable<string>> Errors { get; }
    }
}