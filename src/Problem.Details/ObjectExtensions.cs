using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Problem.Details
{
    internal static class ObjectExtensions
    {
        public static string ToJson(this object obj)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            
            return JsonSerializer.Serialize(obj, options);
        }

        public static string ConvertPropertyCase(this IOptions<JsonOptions> jsonOptions, string property)
        {
            var propNamingPolicy = jsonOptions?.Value?.JsonSerializerOptions.PropertyNamingPolicy;
            return propNamingPolicy?.ConvertName(property) ?? property;
        }
    }
}