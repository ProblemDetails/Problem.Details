using System.Text.Json;

namespace ProblemDetails
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
    }
}