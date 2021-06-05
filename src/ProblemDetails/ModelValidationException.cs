using System;
using System.Collections.Generic;

namespace ProblemDetails
{
    public class ModelValidationException : Exception
    {
        public Dictionary<string, IEnumerable<string>> FieldErrors { get; } 
        
        public ModelValidationException(Dictionary<string, IEnumerable<string>> fieldErrors)
        {
            FieldErrors = fieldErrors;
        }
    }
}