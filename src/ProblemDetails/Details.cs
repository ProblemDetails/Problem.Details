using System.Collections.Generic;

namespace ProblemDetails
{
    public class Details
    {
        public static readonly Dictionary<int, string> StatusToTitle = new()
        {
            { 400, "One or more validation errors occurred." }
        };
        
        public Details(int status)
        {
            Status = status;
        }

        public string Type => $"https://httpstatuses.com/{Status}";

        public string Title => StatusToTitle.ContainsKey(Status) ? StatusToTitle[Status] : $"Error {Status}";
        
        public int Status { get; set; }
    }
}