using System;

namespace Problem.Details.IntegrationTests.Common
{
    public static class Extensions
    {
        public static string TruncateUntil(this string text, string needle) =>
            text.Substring(0, text.IndexOf(needle, StringComparison.Ordinal));
    }
}