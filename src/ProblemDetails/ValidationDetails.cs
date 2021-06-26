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
            var propNamingPolicy = jsonOptions?.Value?.JsonSerializerOptions.PropertyNamingPolicy;

            Errors = new Dictionary<string, IEnumerable<string>>();
            foreach (var kvp in errors)
            {
                var convertedKey = propNamingPolicy?.ConvertName(kvp.Key) ?? kvp.Key;
                var adjustedMessages = kvp.Value
                    .Select(v => v.Replace($" {kvp.Key} ", $" {convertedKey} "));
                
                Errors.Add(convertedKey, adjustedMessages);
            }
        }

        public Dictionary<string, IEnumerable<string>> Errors { get; }
    }
}