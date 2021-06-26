using System.Collections.Generic;

namespace ProblemDetails
{
    public class Details
    {
        private static readonly Dictionary<int, string> StatusToTitleMappings = new()
        {
            { 400, "One or more validation errors occurred." }
        };
        
        public Details(int status)
        {
            Status = status;
        }

        public string Type => $"https://httpstatuses.com/{Status}";

        public string Title =>
            StatusToTitleMappings.ContainsKey(Status)
                ? StatusToTitleMappings[Status]
                : $"Error {Status}";
        
        public int Status { get; set; }

        public static void MapStatusToTitle(int statusCode, string title)
        {
            if (StatusToTitleMappings.ContainsKey(statusCode)) 
                StatusToTitleMappings[statusCode] = title;
            else
                StatusToTitleMappings.Add(statusCode, title);

        }
    }
}