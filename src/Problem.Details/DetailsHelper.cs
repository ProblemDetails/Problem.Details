using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Problem.Details
{
    public static class DetailsHelper
    {
        private static readonly Dictionary<HttpStatusCode, string> StatusToTitleMapping = new();
        
        private static readonly Dictionary<Type, HttpStatusCode> ExceptionToStatusMapping = new();

        public static bool ShowErrorDetails { get; private set; }
     
        /// <summary>
        /// Map of custom titles
        /// </summary>
        public static void MapStatusToTitle(HttpStatusCode statusCode, string title) =>
            StatusToTitleMapping[statusCode] = title;
        
        /// <summary>
        /// Map of custom exception
        /// </summary>
        public static void MapException<TException>(HttpStatusCode statusCode) =>
            ExceptionToStatusMapping[typeof(TException)] = statusCode;

        /// <summary>
        /// Display exception details in <see cref="ProblemDetails"/>
        /// </summary>
        /// <param name="show"></param>
        public static void SetShowErrorDetails(bool show) =>
            ShowErrorDetails = show;

        /// <summary>
        /// Adjust property casing based on the JsonSerializerOptions.PropertyNamingPolicy setting
        /// </summary>
        internal static Dictionary<string, string[]> GetAdjustedValidationErrors(
            IOptions<JsonOptions> jsonOptions,
            IReadOnlyDictionary<string, string[]> propertyErrors)
        {
            var res = new Dictionary<string, string[]>();

            foreach (var (property, errors) in propertyErrors)
            {
                var adjustedProperty = jsonOptions.ConvertPropertyCase(property);
                var adjustedMessages = errors.Select(v => v.Replace($" {property} ", $" {adjustedProperty} ")).ToArray();
                res.Add(adjustedProperty, adjustedMessages);
            }
            return res;
        }

        internal static ProblemDetails FillDetails(ProblemDetails details, HttpStatusCode code)
        {
            details.Type = $"https://httpstatuses.com/{(int)code}";
            details.Title = GetTitle(code);
            details.Status = (int)code;
            return details;
        }
        
        internal static ProblemDetails CreateProblemDetails(HttpStatusCode code) =>
            FillDetails(new ProblemDetails(), code);
        
        internal static ProblemDetails CreateProblemDetails(Exception exception)
        {
            var type = exception.GetType();
            if (ExceptionToStatusMapping.TryGetValue(type, out var code))
                return CreateProblemDetails(code);
            
            return FillDetails(new ProblemDetails(), HttpStatusCode.InternalServerError);
        }

        private static string GetTitle(HttpStatusCode code)
        {
            if (StatusToTitleMapping.ContainsKey(code))
                return StatusToTitleMapping[code];

            var humanizedStatus = string.Join(" ", Regex.Split(code.ToString(), @"(?<!^)(?=[A-Z](?![A-Z]|$))"));
            return $"Error: {humanizedStatus}";
        }
    }
}