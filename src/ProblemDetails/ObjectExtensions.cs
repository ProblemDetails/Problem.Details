using System.Text.Json;

namespace ProblemDetails
{
    internal static class ObjectExtensions
    {
        public static string ToJson(this object obj)
        {
            return JsonSerializer.Serialize(obj,
                new JsonSerializerOptions(JsonSerializerDefaults.Web));
        }
    }
}