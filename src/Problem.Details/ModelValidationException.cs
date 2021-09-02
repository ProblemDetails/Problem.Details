using System;
using System.Collections.Generic;

namespace Problem.Details
{
    public class ModelValidationException : Exception
    {
        public IReadOnlyDictionary<string, string[]> FieldErrors { get; }
        
        public ModelValidationException(Dictionary<string, string[]> fieldErrors)
        {
            FieldErrors = fieldErrors;
        }
    }
}